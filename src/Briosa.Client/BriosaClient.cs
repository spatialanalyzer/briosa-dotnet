using Grpc.Core;

namespace Briosa;

/// <summary>
/// Owns one local Briosa server session and provides typed lifecycle and MP
/// operations for its exact SpatialAnalyzer target.
/// </summary>
public sealed class BriosaClient : IAsyncDisposable
{
    private static readonly TimeSpan ServerProbeDelay = TimeSpan.FromMilliseconds(50);

    private readonly BriosaClientOptions _options;
    private readonly IBriosaServerLauncher _serverLauncher;
    private readonly IClientTransportFactory _transportFactory;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private ClientSession? _session;
    private Task? _startTask;
    private Task? _stopTask;
    private bool _disposed;

    /// <summary>Creates a dormant client. No process is started by construction.</summary>
    public BriosaClient(BriosaClientOptions? options = null)
        : this(
            options ?? new BriosaClientOptions(),
            new BriosaServerLauncher(),
            new GrpcClientTransportFactory())
    {
    }

    internal BriosaClient(
        BriosaClientOptions options,
        IBriosaServerLauncher serverLauncher,
        IClientTransportFactory transportFactory)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(serverLauncher);
        ArgumentNullException.ThrowIfNull(transportFactory);
        options.Validate();
        _options = options;
        _serverLauncher = serverLauncher;
        _transportFactory = transportFactory;
    }

    /// <summary>Starts the ordinary ready-for-MP local session.</summary>
    public Task StartAsync(CancellationToken cancellationToken = default) =>
        StartAsync(BriosaStartOptions.Default, cancellationToken);

    /// <summary>Starts a local session and performs the selected lifecycle phases.</summary>
    public async Task StartAsync(
        BriosaStartOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        Task startTask;
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            ThrowIfDisposed();
            if (_session?.StartupCompleted == true)
            {
                return;
            }

            if (_startTask is not null)
            {
                startTask = _startTask;
            }
            else if (_session is not null)
            {
                throw new BriosaLifecycleException("startup-partially-completed");
            }
            else if (_stopTask is not null)
            {
                throw new BriosaLifecycleException("client-stop-in-progress");
            }
            else
            {
                startTask = StartCoreAsync(options);
                _startTask = startTask;
                _ = startTask.ContinueWith(
                    static task => _ = task.Exception,
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnFaulted |
                        TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
            }
        }
        finally
        {
            _gate.Release();
        }

        await startTask.WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Returns an identity-validated snapshot of the owned server.</summary>
    public Task<BriosaServerSnapshot> GetServerSnapshotAsync(
        CancellationToken cancellationToken = default) =>
        RunLifecycleAsync(
            async (session, token) =>
            {
                try
                {
                    var response = await session.Transport
                        .GetServerSnapshotAsync(token)
                        .ConfigureAwait(false);
                    var snapshot = ProtocolMapping.MapSnapshot(
                        response.ServerInfo,
                        response.Capabilities);
                    session.UpdateSnapshot(snapshot);
                    return snapshot;
                }
                catch (RpcException exception)
                {
                    throw ProtocolMapping.MapRpcException(exception, token);
                }
            },
            cancellationToken);

    /// <summary>Reads current SpatialAnalyzer application and ownership state.</summary>
    public Task<SpatialAnalyzerLifecycleState> GetSpatialAnalyzerStateAsync(
        CancellationToken cancellationToken = default) =>
        RunLifecycleAsync(
            async (session, token) =>
            {
                try
                {
                    var state = ProtocolMapping.MapApplicationState(
                        await session.Transport.GetApplicationStateAsync(token)
                            .ConfigureAwait(false));
                    session.ApplicationState = state;
                    return state;
                }
                catch (RpcException exception)
                {
                    throw ProtocolMapping.MapRpcException(exception, token);
                }
            },
            cancellationToken);

    /// <summary>Launches a new exact-target SpatialAnalyzer application.</summary>
    public Task<SpatialAnalyzerLifecycleState> LaunchSpatialAnalyzerAsync(
        CancellationToken cancellationToken = default) =>
        LaunchSpatialAnalyzerAsync(new SpatialAnalyzerLaunchOptions(), cancellationToken);

    /// <summary>Launches SpatialAnalyzer with approved, structured launch inputs.</summary>
    public Task<SpatialAnalyzerLifecycleState> LaunchSpatialAnalyzerAsync(
        SpatialAnalyzerLaunchOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RunLifecycleAsync(
            async (session, token) =>
            {
                try
                {
                    var state = ProtocolMapping.MapApplicationState(
                        await session.Transport.LaunchApplicationAsync(options, token)
                            .ConfigureAwait(false));
                    session.ApplicationState = state;
                    return state;
                }
                catch (RpcException exception)
                {
                    throw ProtocolMapping.MapRpcException(exception, token);
                }
            },
            cancellationToken);
    }

    /// <summary>Closes only the SpatialAnalyzer application owned by this server.</summary>
    public Task<SpatialAnalyzerLifecycleState> CloseOwnedSpatialAnalyzerAsync(
        CancellationToken cancellationToken = default) =>
        RunLifecycleAsync(
            async (session, token) =>
            {
                var current = await EnsureApplicationStateAsync(session, token)
                    .ConfigureAwait(false);
                var generation = RequireGeneration(
                    current.ApplicationGeneration,
                    "application-generation-unavailable");
                try
                {
                    var state = ProtocolMapping.MapApplicationState(
                        await session.Transport.CloseApplicationAsync(generation, token)
                            .ConfigureAwait(false));
                    session.ApplicationState = state;
                    return state;
                }
                catch (RpcException exception)
                {
                    throw ProtocolMapping.MapRpcException(exception, token, current);
                }
            },
            cancellationToken);

    /// <summary>Reads SDK process, connection, readiness, and recovery state.</summary>
    public Task<SpatialAnalyzerSdkLifecycleState> GetSpatialAnalyzerSdkStateAsync(
        CancellationToken cancellationToken = default) =>
        RunLifecycleAsync(
            async (session, token) =>
            {
                try
                {
                    var state = ProtocolMapping.MapSdkState(
                        await session.Transport.GetSdkStateAsync(token)
                            .ConfigureAwait(false));
                    session.UpdateSdkState(state);
                    return state;
                }
                catch (RpcException exception)
                {
                    throw ProtocolMapping.MapRpcException(
                        exception,
                        token,
                        session.ApplicationState);
                }
            },
            cancellationToken);

    /// <summary>Starts a new disconnected SDK generation.</summary>
    public Task<SpatialAnalyzerSdkLifecycleState> StartSpatialAnalyzerSdkAsync(
        CancellationToken cancellationToken = default) =>
        RunLifecycleAsync(
            async (session, token) =>
            {
                try
                {
                    var state = ProtocolMapping.MapSdkState(
                        await session.Transport.StartSdkAsync(token)
                            .ConfigureAwait(false));
                    session.UpdateSdkState(state);
                    return state;
                }
                catch (RpcException exception)
                {
                    throw ProtocolMapping.MapRpcException(
                        exception,
                        token,
                        session.ApplicationState);
                }
            },
            cancellationToken);

    /// <summary>Connects the current SDK generation and proves MP readiness.</summary>
    public Task<SpatialAnalyzerSdkLifecycleState> ConnectToSpatialAnalyzerAsync(
        CancellationToken cancellationToken = default) =>
        ConnectSdkAsync(reconnect: false, cancellationToken);

    /// <summary>Retries connection and readiness on the current healthy SDK generation.</summary>
    public Task<SpatialAnalyzerSdkLifecycleState> ReconnectToSpatialAnalyzerAsync(
        CancellationToken cancellationToken = default) =>
        ConnectSdkAsync(reconnect: true, cancellationToken);

    /// <summary>Stops the current SDK generation without closing SpatialAnalyzer.</summary>
    public Task<SpatialAnalyzerSdkLifecycleState> StopSpatialAnalyzerSdkAsync(
        CancellationToken cancellationToken = default) =>
        RunSdkGenerationTransitionAsync(
            static (transport, generation, token) =>
                transport.StopSdkAsync(generation, token),
            cancellationToken);

    /// <summary>Replaces a faulted SDK generation without replaying MP work.</summary>
    public Task<SpatialAnalyzerSdkLifecycleState> RecoverSpatialAnalyzerSdkAsync(
        CancellationToken cancellationToken = default) =>
        RunSdkGenerationTransitionAsync(
            static (transport, generation, token) =>
                transport.RecoverSdkAsync(generation, token),
            cancellationToken);

    /// <summary>Executes Get Working Directory once. The client never retries it.</summary>
    public async Task<string> GetWorkingDirectoryAsync(
        CancellationToken cancellationToken = default)
    {
        ClientSession session;
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            ThrowIfDisposed();
            if (_startTask is not null)
            {
                throw new BriosaLifecycleException("client-start-in-progress");
            }

            session = RequireStartedSession();
            if (!session.TryEnterCommand())
            {
                throw new BriosaLifecycleException("mp-command-admission-closed");
            }
        }
        finally
        {
            _gate.Release();
        }

        try
        {
            return await session.Transport.GetWorkingDirectoryAsync(
                    _options.CommandTimeout,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        catch (RpcException exception)
        {
            throw ProtocolMapping.MapRpcException(
                exception,
                cancellationToken,
                session.ApplicationState);
        }
        finally
        {
            session.ExitCommand();
        }
    }

    /// <summary>Ends this client-owned server session. SpatialAnalyzer remains running.</summary>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        Task stopTask;
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            ThrowIfDisposed();
            stopTask = _stopTask ??= StopCoreAsync(_startTask);
        }
        finally
        {
            _gate.Release();
        }

        await stopTask.WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        Task stopTask;
        await _gate.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            stopTask = _stopTask ??= StopCoreAsync(_startTask);
        }
        finally
        {
            _gate.Release();
        }

        await stopTask.ConfigureAwait(false);
        _gate.Dispose();
    }

    private async Task StartCoreAsync(BriosaStartOptions options)
    {
        IOwnedBriosaServer? server = null;
        IClientTransport? transport = null;
        ClientSession? session = null;
        using var startupCts = new CancellationTokenSource(options.StartupTimeout);
        var token = startupCts.Token;

        try
        {
            server = await _serverLauncher.LaunchAsync(token).ConfigureAwait(false);
            transport = _transportFactory.Create(server.Address);
            var snapshot = await WaitForServerAsync(server, transport, token)
                .ConfigureAwait(false);
            session = new ClientSession(server, transport, snapshot);

            await _gate.WaitAsync(token).ConfigureAwait(false);
            try
            {
                ThrowIfDisposed();
                _session = session;
            }
            finally
            {
                _gate.Release();
            }

            server = null;
            transport = null;

            if (options.StartSpatialAnalyzerSdk)
            {
                session.UpdateSdkState(ProtocolMapping.MapSdkState(
                    await session.Transport.StartSdkAsync(token)
                        .ConfigureAwait(false)));
            }

            if (options.LaunchSpatialAnalyzer)
            {
                session.ApplicationState = ProtocolMapping.MapApplicationState(
                    await session.Transport.LaunchApplicationAsync(options.LaunchOptions, token)
                        .ConfigureAwait(false));
            }

            if (options.ConnectToSpatialAnalyzer)
            {
                var sdk = await EnsureSdkStateAsync(session, token).ConfigureAwait(false);
                var generation = RequireGeneration(
                    sdk.SdkGeneration,
                    "sdk-generation-unavailable");
                session.UpdateSdkState(ProtocolMapping.MapSdkState(
                    await session.Transport.ConnectSdkAsync(
                            generation,
                            reconnect: false,
                            token)
                        .ConfigureAwait(false)));
                var refreshed = await session.Transport.GetServerSnapshotAsync(token)
                    .ConfigureAwait(false);
                session.UpdateSnapshot(ProtocolMapping.MapSnapshot(
                    refreshed.ServerInfo,
                    refreshed.Capabilities));
                if (!session.Snapshot.ReadyForMp || session.SdkState?.ReadyForMp != true)
                {
                    throw new BriosaProtocolException("startup-readiness-not-established");
                }
            }

            session.PublishStartup();
        }
        catch (RpcException exception) when (session is not null)
        {
            throw ProtocolMapping.MapRpcException(
                exception,
                token,
                session.ApplicationState);
        }
        catch (OperationCanceledException exception) when (startupCts.IsCancellationRequested)
        {
            throw new BriosaStartupException("startup-timeout", exception);
        }
        finally
        {
            if (session is null)
            {
                if (transport is not null)
                {
                    await transport.DisposeAsync().ConfigureAwait(false);
                }

                if (server is not null)
                {
                    await server.DisposeAsync().ConfigureAwait(false);
                }
            }

            await _gate.WaitAsync().ConfigureAwait(false);
            try
            {
                _startTask = null;
            }
            finally
            {
                _gate.Release();
            }
        }
    }

    private static async Task<BriosaServerSnapshot> WaitForServerAsync(
        IOwnedBriosaServer server,
        IClientTransport transport,
        CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (server.HasExited)
            {
                throw new BriosaStartupException("server-process-exited");
            }

            try
            {
                var response = await transport.GetServerSnapshotAsync(cancellationToken)
                    .ConfigureAwait(false);
                return ProtocolMapping.MapSnapshot(
                    response.ServerInfo,
                    response.Capabilities);
            }
            catch (RpcException exception) when (
                exception.StatusCode == StatusCode.Unavailable)
            {
                await Task.Delay(ServerProbeDelay, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (RpcException exception)
            {
                throw ProtocolMapping.MapRpcException(exception, cancellationToken);
            }
        }
    }

    private Task<SpatialAnalyzerSdkLifecycleState> ConnectSdkAsync(
        bool reconnect,
        CancellationToken cancellationToken) =>
        RunLifecycleAsync(
            async (session, token) =>
            {
                var current = await EnsureSdkStateAsync(session, token)
                    .ConfigureAwait(false);
                var generation = RequireGeneration(
                    current.SdkGeneration,
                    "sdk-generation-unavailable");
                try
                {
                    var state = ProtocolMapping.MapSdkState(
                        await session.Transport.ConnectSdkAsync(
                                generation,
                                reconnect,
                                token)
                            .ConfigureAwait(false));
                    session.UpdateSdkState(state);
                    return state;
                }
                catch (RpcException exception)
                {
                    throw ProtocolMapping.MapRpcException(
                        exception,
                        token,
                        session.ApplicationState);
                }
            },
            cancellationToken);

    private Task<SpatialAnalyzerSdkLifecycleState> RunSdkGenerationTransitionAsync(
        Func<IClientTransport, int, CancellationToken, Task<Briosa.Client.Transport.SpatialAnalyzerSdkLifecycleState>> transition,
        CancellationToken cancellationToken) =>
        RunLifecycleAsync(
            async (session, token) =>
            {
                var current = await EnsureSdkStateAsync(session, token)
                    .ConfigureAwait(false);
                var generation = RequireGeneration(
                    current.SdkGeneration,
                    "sdk-generation-unavailable");
                try
                {
                    var state = ProtocolMapping.MapSdkState(
                        await transition(session.Transport, generation, token)
                            .ConfigureAwait(false));
                    session.UpdateSdkState(state);
                    return state;
                }
                catch (RpcException exception)
                {
                    throw ProtocolMapping.MapRpcException(
                        exception,
                        token,
                        session.ApplicationState);
                }
            },
            cancellationToken);

    private async Task<T> RunLifecycleAsync<T>(
        Func<ClientSession, CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            ThrowIfDisposed();
            if (_startTask is not null)
            {
                throw new BriosaLifecycleException("client-start-in-progress");
            }

            if (_stopTask is not null)
            {
                throw new BriosaLifecycleException("client-stop-in-progress");
            }

            return await operation(RequireStartedSession(), cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            _gate.Release();
        }
    }

    private static async Task<SpatialAnalyzerLifecycleState> EnsureApplicationStateAsync(
        ClientSession session,
        CancellationToken cancellationToken)
    {
        if (session.ApplicationState is not null)
        {
            return session.ApplicationState;
        }

        var state = ProtocolMapping.MapApplicationState(
            await session.Transport.GetApplicationStateAsync(cancellationToken)
                .ConfigureAwait(false));
        session.ApplicationState = state;
        return state;
    }

    private static async Task<SpatialAnalyzerSdkLifecycleState> EnsureSdkStateAsync(
        ClientSession session,
        CancellationToken cancellationToken)
    {
        if (session.SdkState is not null)
        {
            return session.SdkState;
        }

        var state = ProtocolMapping.MapSdkState(
            await session.Transport.GetSdkStateAsync(cancellationToken)
                .ConfigureAwait(false));
        session.UpdateSdkState(state);
        return state;
    }

    private async Task StopCoreAsync(Task? pendingStart)
    {
        ClientSession? session = null;
        try
        {
            if (pendingStart is not null)
            {
                try
                {
                    await pendingStart.ConfigureAwait(false);
                }
                catch (BriosaException)
                {
                }
            }

            await _gate.WaitAsync().ConfigureAwait(false);
            try
            {
                session = _session;
                _session = null;
                session?.CloseCommandAdmission();
            }
            finally
            {
                _gate.Release();
            }

            if (session is null)
            {
                return;
            }

            await session.WaitForCommandsAsync().ConfigureAwait(false);
            await StopSdkBestEffortAsync(session).ConfigureAwait(false);
            await session.Transport.DisposeAsync().ConfigureAwait(false);
            await session.Server.DisposeAsync().ConfigureAwait(false);
        }
        finally
        {
            await _gate.WaitAsync().ConfigureAwait(false);
            try
            {
                _stopTask = null;
            }
            finally
            {
                _gate.Release();
            }
        }
    }

    private static async Task StopSdkBestEffortAsync(ClientSession session)
    {
        try
        {
            var state = await EnsureSdkStateAsync(session, CancellationToken.None)
                .ConfigureAwait(false);
            if (state.SdkGeneration is { } generation &&
                state.SdkState != SpatialAnalyzerSdkState.Stopped)
            {
                await session.Transport.StopSdkAsync(generation, CancellationToken.None)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception exception) when (
            exception is BriosaException or RpcException or ObjectDisposedException)
        {
        }
    }

    private ClientSession RequireStartedSession() =>
        _session ?? throw new BriosaLifecycleException("client-not-started");

    private static int RequireGeneration(int? generation, string diagnosticCode) =>
        generation is > 0
            ? generation.Value
            : throw new BriosaLifecycleException(diagnosticCode);

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    private sealed class ClientSession(
        IOwnedBriosaServer server,
        IClientTransport transport,
        BriosaServerSnapshot snapshot)
    {
        private readonly object _commandSync = new();
        private readonly TaskCompletionSource _commandsDrained = new(
            TaskCreationOptions.RunContinuationsAsynchronously);
        private bool _commandAdmissionOpen;
        private int _activeCommands;

        public IOwnedBriosaServer Server { get; } = server;
        public IClientTransport Transport { get; } = transport;
        public BriosaServerSnapshot Snapshot { get; set; } = snapshot;
        public SpatialAnalyzerLifecycleState? ApplicationState { get; set; }
        public SpatialAnalyzerSdkLifecycleState? SdkState { get; private set; }
        public bool StartupCompleted { get; private set; }

        public void UpdateSdkState(SpatialAnalyzerSdkLifecycleState state)
        {
            SdkState = state;
            lock (_commandSync)
            {
                RefreshCommandAdmission();
            }
        }

        public void UpdateSnapshot(BriosaServerSnapshot snapshot)
        {
            Snapshot = snapshot;
            lock (_commandSync)
            {
                RefreshCommandAdmission();
            }
        }

        public void PublishStartup()
        {
            StartupCompleted = true;
            lock (_commandSync)
            {
                RefreshCommandAdmission();
            }
        }

        private void RefreshCommandAdmission() =>
            _commandAdmissionOpen =
                Snapshot.ReadyForMp && SdkState?.ReadyForMp == true;

        public bool TryEnterCommand()
        {
            lock (_commandSync)
            {
                if (!_commandAdmissionOpen)
                {
                    return false;
                }

                _activeCommands++;
                return true;
            }
        }

        public void ExitCommand()
        {
            lock (_commandSync)
            {
                _activeCommands--;
                if (_activeCommands == 0 && !_commandAdmissionOpen)
                {
                    _commandsDrained.TrySetResult();
                }
            }
        }

        public void CloseCommandAdmission()
        {
            lock (_commandSync)
            {
                _commandAdmissionOpen = false;
                if (_activeCommands == 0)
                {
                    _commandsDrained.TrySetResult();
                }
            }
        }

        public Task WaitForCommandsAsync()
        {
            lock (_commandSync)
            {
                return _activeCommands == 0
                    ? Task.CompletedTask
                    : _commandsDrained.Task;
            }
        }
    }
}
