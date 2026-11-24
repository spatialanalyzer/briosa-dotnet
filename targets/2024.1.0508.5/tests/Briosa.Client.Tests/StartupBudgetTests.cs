#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using Google.Protobuf;
using Grpc.Core;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

/// <summary>
/// Pins the client startup budget against the server's default lifecycle bounds
/// (spatialanalyzer/briosa main 0c7ff7a; #298 and #315), pins that budget expiry is
/// reported as startup-timeout whichever phase it interrupts, and pins that lifecycle
/// calls carry no client-imposed per-call deadline.
/// </summary>
public sealed partial class StartupBudgetTests
{
    // Server defaults, identical in both targets.
    // Briosa:Worker:StartupTimeout: worker launch, COM activation, Ready, ConnectEx, and
    // the application association after a successful readiness probe.
    private static readonly TimeSpan ServerStartupTimeout = TimeSpan.FromSeconds(10);

    // Briosa:Worker:ReadinessProbeTimeout: the private readiness probe inside Connect.
    private static readonly TimeSpan ServerReadinessProbeTimeout = TimeSpan.FromSeconds(30);

    // Briosa:SpatialAnalyzer:ApplicationStartupTimeout: LaunchSpatialAnalyzer's window wait.
    private static readonly TimeSpan ServerApplicationStartupTimeout = TimeSpan.FromSeconds(30);

    // Fixed heartbeat timeout: Connect's liveness recheck after the association.
    private static readonly TimeSpan ServerHeartbeatTimeout = TimeSpan.FromSeconds(5);

    // Fixed worker shutdown bound: forced cleanup before a failed Connect is reported.
    private static readonly TimeSpan ServerWorkerShutdownTimeout = TimeSpan.FromSeconds(5);

    // Server host startup, process observations, discovery, and round trips.
    private static readonly TimeSpan Margin = TimeSpan.FromSeconds(20);

    [Fact]
    public void DefaultStartupBudgetIsTwoMinutes()
    {
        Assert.Equal(TimeSpan.FromMinutes(2), new BriosaStartOptions().StartupTimeout);
        Assert.Equal(TimeSpan.FromMinutes(2), BriosaStartOptions.Default.StartupTimeout);
    }

    [Fact]
    public void DefaultStartupBudgetCoversServerWorstCaseWithMargin()
    {
        var startSdk = ServerStartupTimeout;
        var launchApplication = ServerApplicationStartupTimeout;
        var connect = ServerStartupTimeout + ServerReadinessProbeTimeout +
            ServerStartupTimeout + ServerHeartbeatTimeout + ServerWorkerShutdownTimeout;
        var serverWorstCase = startSdk + launchApplication + connect;

        Assert.Equal(TimeSpan.FromSeconds(100), serverWorstCase);
        Assert.True(
            BriosaStartOptions.Default.StartupTimeout >= serverWorstCase + Margin,
            $"The default startup budget must cover the server's {serverWorstCase} worst case plus {Margin}.");
    }

    [Fact]
    public async Task StartupPhasesShareOneOverallBudget()
    {
        var transport = new RecordingTransport();
        await using var client = CreateClient(new BriosaClientOptions(), transport);

        await client.StartAsync();

        Assert.Equal(
            ["snapshot", "start-sdk", "launch-sa", "connect-sdk", "snapshot"],
            transport.Calls.Select(call => call.Name));
        var budget = transport.Calls[0].Token;
        Assert.True(budget.CanBeCanceled);
        Assert.All(transport.Calls, call => Assert.Equal(budget, call.Token));
    }

    [Theory]
    [InlineData("snapshot")]
    [InlineData("start-sdk")]
    [InlineData("launch-sa")]
    [InlineData("connect-sdk")]
    public async Task BudgetExpiryDuringAnyStartupRpcIsAStartupTimeout(string phase)
    {
        var transport = new RecordingTransport { HangAt = phase };
        await using var client = CreateClient(new BriosaClientOptions(), transport);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(
            () => client.StartAsync(new BriosaStartOptions
            {
                StartupTimeout = TimeSpan.FromMilliseconds(100),
            }));

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(phase, transport.Calls[^1].Name);
    }

    [Fact]
    public async Task CallerCancellationDuringStartupStaysCancellation()
    {
        var transport = new RecordingTransport { HangAt = "connect-sdk" };
        await using var client = CreateClient(new BriosaClientOptions(), transport);
        using var caller = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));

        var exception = await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => client.StartAsync(
                new BriosaStartOptions { StartupTimeout = TimeSpan.FromSeconds(1) },
                caller.Token));

        Assert.IsNotType<BriosaStartupException>(exception);
        Assert.Equal(caller.Token, exception.CancellationToken);
    }

    [Fact]
    public async Task LifecycleCallsCarryNoClientDeadline()
    {
        var transport = new RecordingTransport();
        await using var client = CreateClient(
            new BriosaClientOptions { CommandTimeout = TimeSpan.FromSeconds(1) },
            transport);
        await client.StartAsync();
        transport.Calls.Clear();

        _ = await client.GetServerSnapshotAsync();
        _ = await client.GetSpatialAnalyzerStateAsync();
        _ = await client.GetSpatialAnalyzerSdkStateAsync();
        _ = await client.ReconnectToSpatialAnalyzerAsync();
        _ = await client.RecoverSpatialAnalyzerSdkAsync();
        _ = await client.ConnectToSpatialAnalyzerAsync();
        _ = await client.StopSpatialAnalyzerSdkAsync();
        _ = await client.StartSpatialAnalyzerSdkAsync();
        _ = await client.LaunchSpatialAnalyzerAsync();
        _ = await client.CloseOwnedSpatialAnalyzerAsync();

        Assert.Equal(
            [
                "snapshot", "get-sa-state", "get-sdk-state", "reconnect-sdk", "recover-sdk",
                "connect-sdk", "stop-sdk", "start-sdk", "launch-sa", "close-sa",
            ],
            transport.Calls.Select(call => call.Name));
        Assert.All(transport.Calls, call => Assert.False(call.Token.CanBeCanceled));
    }

    // Server 0.10.0 publishes no lifecycle bounds: the omitted budget stays two
    // minutes and the startup RPCs are exactly those of client 0.5.0.
    [Fact]
    public async Task WithoutPublishedBoundsTheOmittedBudgetStaysTwoMinutes()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport { HangAt = "connect-sdk", Clock = clock };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var start = client.StartAsync();
        var hang = await transport.Reached.Task.ConfigureAwait(true);

        Assert.Equal(
            ["snapshot", "start-sdk", "launch-sa", "connect-sdk"],
            transport.Calls.Select(call => call.Name));
        await AssertExpiresAtAsync(clock, start, TimeSpan.FromMinutes(2), hang);
    }

    [Fact]
    public async Task TheOmittedBudgetAdoptsThePublishedAllowanceAtTheFirstSnapshot()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            HangAt = "connect-sdk",
            Clock = clock,
            SnapshotAt = [TimeSpan.FromSeconds(10)],
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var start = client.StartAsync();
        var hang = await transport.Reached.Task.ConfigureAwait(true);

        await AssertExpiresAtAsync(clock, start, TimeSpan.FromSeconds(320), hang);
    }

    [Fact]
    public async Task AnExplicitTwoMinuteBudgetIsNotAdopted()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            HangAt = "connect-sdk",
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var start = client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromMinutes(2) });
        var hang = await transport.Reached.Task.ConfigureAwait(true);

        await AssertExpiresAtAsync(clock, start, TimeSpan.FromMinutes(2), hang);
    }

    // A snapshot validated at 121 s, before the bootstrap timer callback has run,
    // never extends the budget for a 300 s allowance, and no further phase starts.
    [Fact]
    public async Task LateDiscoveryExpiresWithoutAnotherPhase()
    {
        var clock = new ManualTimeProvider();
        var launcher = new RecordingLauncher();
        var transport = new RecordingTransport
        {
            Clock = clock,
            SnapshotAt = [TimeSpan.FromSeconds(121)],
            FireTimersAtSnapshot = false,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock, launcher);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => client.StartAsync());

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(["snapshot"], transport.Calls.Select(call => call.Name));
        Assert.True(launcher.Server.Disposed);
    }

    // The same late discovery, when the server publishes bounds without the
    // allowance the selected phases need: the bootstrap deadline still holds.
    [Fact]
    public async Task LateDiscoveryWithPartialBoundsExpiresWithoutAnotherPhase()
    {
        var clock = new ManualTimeProvider();
        var launcher = new RecordingLauncher();
        var partial = ServerBounds(startupWithLaunch: 300_000);
        partial.ClearStartupWithLaunchMilliseconds();
        Assert.True(partial.HasStartSdkMilliseconds);
        var transport = new RecordingTransport
        {
            Clock = clock,
            SnapshotAt = [TimeSpan.FromSeconds(121)],
            FireTimersAtSnapshot = false,
            Bounds = [partial],
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock, launcher);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => client.StartAsync());

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(["snapshot"], transport.Calls.Select(call => call.Name));
        Assert.True(launcher.Server.Disposed);
    }

    // Server 0.10.0 publishes no bounds: the same late discovery proceeds until
    // the bootstrap timer itself fires, exactly as client 0.5.0 did.
    [Fact]
    public async Task LateDiscoveryWithoutBoundsLeavesTheBootstrapTimerInCharge()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            HangAt = "start-sdk",
            Clock = clock,
            SnapshotAt = [TimeSpan.FromSeconds(121)],
            FireTimersAtSnapshot = false,
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var start = client.StartAsync();
        var hang = await transport.Reached.Task.ConfigureAwait(true);

        Assert.False(hang.IsCancellationRequested);
        clock.Advance(TimeSpan.Zero);
        Assert.True(hang.IsCancellationRequested);
        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => start).ConfigureAwait(true);
        Assert.Equal("startup-timeout", exception.DiagnosticCode);
    }

    // The refresh after Connect never restarts or re-adopts the budget.
    [Fact]
    public async Task ThePostConnectRefreshKeepsTheAdoptedDeadline()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            HangAt = "snapshot",
            HangAtOccurrence = 2,
            Clock = clock,
            SnapshotAt = [TimeSpan.Zero, TimeSpan.FromSeconds(100)],
            Bounds = [ServerBounds(startupWithLaunch: 150_000), ServerBounds(startupWithLaunch: 900_000)],
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var start = client.StartAsync();
        var hang = await transport.Reached.Task.ConfigureAwait(true);

        await AssertExpiresAtAsync(clock, start, TimeSpan.FromSeconds(170), hang);
    }

    [Fact]
    public async Task CallerCancellationStaysIndependentOfTheAdoptedBudget()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            HangAt = "connect-sdk",
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);
        using var caller = new CancellationTokenSource();

        var start = client.StartAsync(caller.Token);
        var hang = await transport.Reached.Task.ConfigureAwait(true);
        await caller.CancelAsync();

        var exception = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => start);
        Assert.Equal(caller.Token, exception.CancellationToken);
        Assert.False(hang.IsCancellationRequested);
        clock.Advance(TimeSpan.FromSeconds(320));
        Assert.True(hang.IsCancellationRequested);
    }

    private static async Task AssertExpiresAtAsync(
        ManualTimeProvider clock,
        Task start,
        TimeSpan deadline,
        CancellationToken budget)
    {
        clock.Advance(deadline - clock.Elapsed - TimeSpan.FromTicks(1));
        Assert.False(budget.IsCancellationRequested, $"The budget expired before {deadline}.");
        Assert.False(start.IsCompleted);
        clock.Advance(TimeSpan.FromTicks(1));
        Assert.True(budget.IsCancellationRequested, $"The budget did not expire at {deadline}.");
        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => start).ConfigureAwait(true);
        Assert.Equal("startup-timeout", exception.DiagnosticCode);
    }

    private static Transport.LifecycleBounds ServerBounds(uint startupWithLaunch) => new()
    {
        StartSdkMilliseconds = 45_000,
        LaunchSpatialAnalyzerMilliseconds = 30_000,
        ConnectSpatialAnalyzerMilliseconds = 60_000,
        RecoverSdkMilliseconds = 55_000,
        StopSdkMilliseconds = 15_000,
        CloseOwnedSpatialAnalyzerMilliseconds = 30_000,
        StartupWithLaunchMilliseconds = startupWithLaunch,
        StartupWithoutLaunchMilliseconds = 70_000,
        HostGracefulShutdown = new Transport.GracefulShutdownBound { FiniteMilliseconds = 105_000 },
    };

    private static BriosaClient CreateClient(
        BriosaClientOptions options,
        RecordingTransport transport,
        TimeProvider? timeProvider = null,
        RecordingLauncher? launcher = null) =>
        new(options, launcher ?? new RecordingLauncher(), new RecordingTransportFactory(transport), timeProvider);

    private sealed record RecordedCall(string Name, CancellationToken Token);

    private sealed class RecordingLauncher : IBriosaServerLauncher
    {
        public RecordingServer Server { get; } = new();

        public Task<IOwnedBriosaServer> LaunchAsync(
            BriosaLoggingOptions? logging,
            CancellationToken cancellationToken) =>
            Task.FromResult<IOwnedBriosaServer>(Server);
    }

    private sealed class RecordingServer : IOwnedBriosaServer
    {
        public Uri Address { get; } = new("http://127.0.0.1:49152");
        public bool HasExited => false;
        public bool Disposed { get; private set; }

        // Thrown by DisposeAsync after the close is recorded.
        public Exception? DisposeFailure { get; set; }

        public ValueTask DisposeAsync()
        {
            Disposed = true;
            return DisposeFailure is null ? ValueTask.CompletedTask : ValueTask.FromException(DisposeFailure);
        }
    }

    private sealed class RecordingTransportFactory(RecordingTransport transport)
        : IClientTransportFactory
    {
        public IClientTransport Create(Uri address) => transport;
    }

    private sealed class RecordingTransport : IClientTransport
    {
        private int _sdkGeneration;
        private bool _connected;
        private int _snapshots;

        public List<RecordedCall> Calls { get; } = [];

        // The RPC that never answers. Like Grpc.Net.Client, it then fails with
        // StatusCode.Cancelled once its token is cancelled.
        public string? HangAt { get; init; }

        // Which call of HangAt hangs, counting from 1.
        public int HangAtOccurrence { get; init; } = 1;

        // Receives the hanging call's token once that call is reached.
        public TaskCompletionSource<CancellationToken> Reached { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public ManualTimeProvider? Clock { get; init; }

        // The clock reading at which each snapshot call is observed, by call order.
        public TimeSpan[] SnapshotAt { get; init; } = [];

        // Whether moving the clock to a snapshot's reading runs the timers that
        // fall due. False models a timer callback that has not run yet.
        public bool FireTimersAtSnapshot { get; init; } = true;

        // The lifecycle bounds each snapshot publishes, by call order. A missing
        // entry publishes none, as Server 0.10.0 does.
        public Transport.LifecycleBounds?[] Bounds { get; init; } = [];

        // Runs inside each call, after it is recorded and before it answers, with
        // the call's name and occurrence (from 1): a test moves the clock or holds
        // the call there. The fake then answers whatever the token says, as a
        // reply already on its way would.
        public Func<string, int, Task>? DuringCall { get; init; }

        public bool Disposed { get; private set; }

        // The startup's submission check, which the real transport runs after it
        // builds a request and immediately before the RPC; this fake runs it at
        // the same point, so a refused submission is never recorded.
        public Action? BeforeSubmit { get; set; }

        // Runs while a call's request is built, before the submission check, with
        // the call's name and the occurrence it would have.
        public Action<string, int>? WhileBuildingRequest { get; init; }

        // Replaces the application state that Launch returns.
        public Transport.SpatialAnalyzerLifecycleState? LaunchReply { get; init; }

        // How many capability listings were submitted after server information.
        public int CapabilitySubmissions { get; private set; }

        // How many MP commands reached this transport.
        public int MpCalls { get; private set; }

        // Calls whose reply arrives with its State submessage absent, as the
        // generated transport then returns null.
        public string[] MissingStateFor { get; init; } = [];

        // Shapes each snapshot's server information, by call order from 0.
        public Action<int, Transport.GetServerInfoResponse>? ShapeServerInfo { get; init; }

        // How many capabilities each connected snapshot lists.
        public int ConnectedCapabilityCount { get; init; }

        // Thrown by DisposeAsync after the close is recorded.
        public Exception? DisposeFailure { get; set; }

        // Runs once a snapshot's reply is built, just before it is returned, with
        // the snapshot's index from 0: the moment the reply arrives.
        public Action<int>? BeforeAnswer { get; init; }

        private async Task RecordAsync(string name, CancellationToken cancellationToken)
        {
            WhileBuildingRequest?.Invoke(name, Calls.Count(call => call.Name == name) + 1);
            BeforeSubmit?.Invoke();
            Calls.Add(new(name, cancellationToken));
            var occurrence = Calls.Count(call => call.Name == name);
            if (DuringCall is not null)
            {
                await DuringCall(name, occurrence).ConfigureAwait(false);
            }

            if (name != HangAt || occurrence != HangAtOccurrence)
            {
                return;
            }

            Reached.TrySetResult(cancellationToken);

            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Call canceled by the client."));
            }
        }

        public async Task<(Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse)>
            GetServerSnapshotAsync(CancellationToken cancellationToken)
        {
            var index = _snapshots++;
            if (Clock is not null && index < SnapshotAt.Length && SnapshotAt[index] > Clock.Elapsed)
            {
                var by = SnapshotAt[index] - Clock.Elapsed;
                if (FireTimersAtSnapshot)
                {
                    Clock.Advance(by);
                }
                else
                {
                    Clock.AdvanceWithoutFiringTimers(by);
                }
            }

            await RecordAsync("snapshot", cancellationToken).ConfigureAwait(false);
            // Server information has answered; the capability listing is the
            // second discovery RPC, checked on its own.
            BeforeSubmit?.Invoke();
            CapabilitySubmissions++;
            var (server, capabilities) = Snapshot(_connected);
            if (index < Bounds.Length && Bounds[index] is { } bounds)
            {
                server.LifecycleBounds = bounds;
            }

            ShapeServerInfo?.Invoke(index, server);
            if (_connected && ConnectedCapabilityCount > 0)
            {
                for (var count = 0; count < ConnectedCapabilityCount; count++)
                {
                    capabilities.Operations.Add(new Transport.OperationCapability
                    {
                        OperationId = "file_operations.get_working_directory",
                        GrpcService = "briosa.FileOperations",
                        Rpc = "GetWorkingDirectory",
                        FullyQualifiedMethod = "/briosa.FileOperations/GetWorkingDirectory",
                        DurationClass = Transport.OperationDurationClass.Quick,
                        ValidationStatus = Transport.OperationValidationStatus.NoRecordedGap,
                        Admission = Transport.OperationAdmission.Admitted,
                    });
                }
            }

            BeforeAnswer?.Invoke(index);
            return (server, capabilities);
        }

        public async Task<Transport.SpatialAnalyzerLifecycleState> GetApplicationStateAsync(
            CancellationToken cancellationToken)
        {
            await RecordAsync("get-sa-state", cancellationToken).ConfigureAwait(false);
            return Application(running: true);
        }

        public async Task<Transport.SpatialAnalyzerLifecycleState> LaunchApplicationAsync(
            SpatialAnalyzerLaunchOptions options,
            CancellationToken cancellationToken)
        {
            await RecordAsync("launch-sa", cancellationToken).ConfigureAwait(false);
            return MissingStateFor.Contains("launch-sa") ? null! : LaunchReply ?? Application(running: true);
        }

        public async Task<Transport.SpatialAnalyzerLifecycleState> CloseApplicationAsync(
            int expectedGeneration,
            CancellationToken cancellationToken)
        {
            await RecordAsync("close-sa", cancellationToken).ConfigureAwait(false);
            return Application(running: false);
        }

        public async Task<Transport.SpatialAnalyzerSdkLifecycleState> GetSdkStateAsync(
            CancellationToken cancellationToken)
        {
            await RecordAsync("get-sdk-state", cancellationToken).ConfigureAwait(false);
            return MissingStateFor.Contains("get-sdk-state") ? null! : Sdk();
        }

        public async Task<Transport.SpatialAnalyzerSdkLifecycleState> StartSdkAsync(
            CancellationToken cancellationToken)
        {
            await RecordAsync("start-sdk", cancellationToken).ConfigureAwait(false);
            _sdkGeneration++;
            _connected = false;
            return MissingStateFor.Contains("start-sdk") ? null! : Sdk();
        }

        public async Task<Transport.SpatialAnalyzerSdkLifecycleState> ConnectSdkAsync(
            int expectedGeneration,
            bool reconnect,
            CancellationToken cancellationToken)
        {
            var name = reconnect ? "reconnect-sdk" : "connect-sdk";
            await RecordAsync(name, cancellationToken).ConfigureAwait(false);
            _connected = true;
            return MissingStateFor.Contains(name) ? null! : Sdk();
        }

        public async Task<Transport.SpatialAnalyzerSdkLifecycleState> StopSdkAsync(
            int expectedGeneration,
            CancellationToken cancellationToken)
        {
            await RecordAsync("stop-sdk", cancellationToken).ConfigureAwait(false);
            _sdkGeneration = 0;
            _connected = false;
            return Sdk();
        }

        public async Task<Transport.SpatialAnalyzerSdkLifecycleState> RecoverSdkAsync(
            int expectedGeneration,
            CancellationToken cancellationToken)
        {
            await RecordAsync("recover-sdk", cancellationToken).ConfigureAwait(false);
            _sdkGeneration++;
            _connected = false;
            return Sdk();
        }

        public Task<Transport.ListCapabilitiesResponse> ListCapabilitiesAsync(
            bool includeDenied,
            CancellationToken cancellationToken) =>
            throw new NotSupportedException();

        public Task<Transport.GetWorkingDirectoryResult> GetWorkingDirectoryAsync(
            TimeSpan? timeout,
            CancellationToken cancellationToken)
        {
            MpCalls++;
            throw new NotSupportedException();
        }

        public Task<IMessage> InvokeOperationAsync(
            string service,
            string rpc,
            IMessage request,
            MessageParser responseParser,
            TimeSpan? timeout,
            CancellationToken cancellationToken)
        {
            MpCalls++;
            throw new NotSupportedException();
        }

        public ValueTask DisposeAsync()
        {
            Disposed = true;
            return DisposeFailure is null ? ValueTask.CompletedTask : ValueTask.FromException(DisposeFailure);
        }

        private static Transport.SpatialAnalyzerLifecycleState Application(bool running)
        {
            var state = new Transport.SpatialAnalyzerLifecycleState
            {
                StateRevision = 1,
                ApplicationState = running
                    ? Transport.SpatialAnalyzerApplicationState.Running
                    : Transport.SpatialAnalyzerApplicationState.NotRunning,
                Ownership = running
                    ? Transport.SpatialAnalyzerOwnership.ServerLaunched
                    : Transport.SpatialAnalyzerOwnership.None,
            };
            if (running)
            {
                state.ApplicationGeneration = 1;
            }

            return state;
        }

        private Transport.SpatialAnalyzerSdkLifecycleState Sdk()
        {
            var state = new Transport.SpatialAnalyzerSdkLifecycleState
            {
                StateRevision = 1,
                SdkState = _sdkGeneration == 0
                    ? Transport.SpatialAnalyzerSdkState.Stopped
                    : _connected
                        ? Transport.SpatialAnalyzerSdkState.Ready
                        : Transport.SpatialAnalyzerSdkState.Running,
                ConnectionState = _connected
                    ? Transport.SpatialAnalyzerConnectionState.Connected
                    : Transport.SpatialAnalyzerConnectionState.Disconnected,
                ExecutionReadinessState = _connected
                    ? Transport.SpatialAnalyzerExecutionReadinessState.ExecutionReady
                    : Transport.SpatialAnalyzerExecutionReadinessState.Unverified,
                ReadyForMp = _connected,
                RecoveryState = Transport.SpatialAnalyzerSdkRecoveryState.NotRequired,
            };
            if (_sdkGeneration > 0)
            {
                state.SdkGeneration = _sdkGeneration;
            }

            return state;
        }

        private static (Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse)
            Snapshot(bool ready)
        {
            var server = new Transport.GetServerInfoResponse
            {
                Version = new Transport.VersionCoordinates
                {
                    BriosaVersion = Transport.BriosaProtocolIdentity.BriosaVersion,
                    SourceRevision = Transport.BriosaProtocolIdentity.SourceRevision,
                    ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
                    SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
                },
                WorkerState = Transport.WorkerRuntimeState.Ready,
                SpatialAnalyzerConnectionState = ready
                    ? Transport.SpatialAnalyzerConnectionState.Connected
                    : Transport.SpatialAnalyzerConnectionState.Disconnected,
                SpatialAnalyzerExecutionReadinessState = ready
                    ? Transport.SpatialAnalyzerExecutionReadinessState.ExecutionReady
                    : Transport.SpatialAnalyzerExecutionReadinessState.Unverified,
                ReadyForMp = ready,
                TargetIsolationMode = Transport.TargetIsolationMode.SingleTenant,
                Compatibility = new Transport.CompatibilityContract { Major = 3 },
            };
            var capabilities = new Transport.ListCapabilitiesResponse
            {
                ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
                SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
            };
            return (server, capabilities);
        }
    }
}
