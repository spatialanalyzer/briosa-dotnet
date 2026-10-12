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
public sealed class StartupBudgetTests
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

    private static BriosaClient CreateClient(
        BriosaClientOptions options,
        RecordingTransport transport) =>
        new(options, new RecordingLauncher(), new RecordingTransportFactory(transport));

    private sealed record RecordedCall(string Name, CancellationToken Token);

    private sealed class RecordingLauncher : IBriosaServerLauncher
    {
        public Task<IOwnedBriosaServer> LaunchAsync(
            BriosaLoggingOptions? logging,
            CancellationToken cancellationToken) =>
            Task.FromResult<IOwnedBriosaServer>(new RecordingServer());
    }

    private sealed class RecordingServer : IOwnedBriosaServer
    {
        public Uri Address { get; } = new("http://127.0.0.1:49152");
        public bool HasExited => false;
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
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

        public List<RecordedCall> Calls { get; } = [];

        // The RPC that never answers. Like Grpc.Net.Client, it then fails with
        // StatusCode.Cancelled once its token is cancelled.
        public string? HangAt { get; init; }

        private async Task RecordAsync(string name, CancellationToken cancellationToken)
        {
            Calls.Add(new(name, cancellationToken));
            if (name != HangAt)
            {
                return;
            }

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
            await RecordAsync("snapshot", cancellationToken).ConfigureAwait(false);
            return Snapshot(_connected);
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
            return Application(running: true);
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
            return Sdk();
        }

        public async Task<Transport.SpatialAnalyzerSdkLifecycleState> StartSdkAsync(
            CancellationToken cancellationToken)
        {
            await RecordAsync("start-sdk", cancellationToken).ConfigureAwait(false);
            _sdkGeneration++;
            _connected = false;
            return Sdk();
        }

        public async Task<Transport.SpatialAnalyzerSdkLifecycleState> ConnectSdkAsync(
            int expectedGeneration,
            bool reconnect,
            CancellationToken cancellationToken)
        {
            await RecordAsync(reconnect ? "reconnect-sdk" : "connect-sdk", cancellationToken).ConfigureAwait(false);
            _connected = true;
            return Sdk();
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

        public Task<string> GetWorkingDirectoryAsync(
            TimeSpan? timeout,
            CancellationToken cancellationToken) =>
            throw new NotSupportedException();

        public Task<IMessage> InvokeOperationAsync(
            string service,
            string rpc,
            IMessage request,
            MessageParser responseParser,
            TimeSpan? timeout,
            CancellationToken cancellationToken) =>
            throw new NotSupportedException();

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

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
                Compatibility = new Transport.CompatibilityContract { Major = 2 },
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
