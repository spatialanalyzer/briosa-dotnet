#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using Google.Protobuf;
using Grpc.Core;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

public sealed class ClientContractTests
{
    [Fact]
    public void ProtocolIdentityMatchesMergedLifecycleArtifact()
    {
        Assert.Equal(
            "briosa-protocol-0.2.0-lifecycle-sa-2026.1.0529.7",
            Transport.BriosaProtocolIdentity.ArtifactName);
        Assert.Equal("briosa", Transport.BriosaProtocolIdentity.ProtocolPackage);
        Assert.Equal(
            "standard-protobuf-grpc",
            Transport.BriosaProtocolIdentity.ClientGenerationContract);
        Assert.Equal(
            "bd19e8f32a8bd717e6cf2ec2aea93b68b8c39c11",
            Transport.BriosaProtocolIdentity.SourceRevision);
        Assert.Equal(
            "2026.1.0529.7",
            Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget);
    }

    [Fact]
    public void ConstructionIsDormantAndValidatesOnlyClientOptions()
    {
        var launcher = new FakeServerLauncher();

        _ = new BriosaClient(
            new BriosaClientOptions(),
            launcher,
            new FakeTransportFactory(new FakeTransport()));

        Assert.Equal(0, launcher.LaunchCount);
        Assert.Throws<ArgumentOutOfRangeException>(() => new BriosaClient(
            new BriosaClientOptions { CommandTimeout = TimeSpan.Zero },
            launcher,
            new FakeTransportFactory(new FakeTransport())));
    }

    [Fact]
    public void StartOptionsRejectUnsafeOrContradictoryCombinations()
    {
        Assert.Throws<ArgumentException>(() => new BriosaStartOptions
        {
            StartSpatialAnalyzerSdk = false,
        }.Validate());
        Assert.Throws<ArgumentException>(() => new BriosaStartOptions
        {
            LaunchSpatialAnalyzer = false,
            LaunchOptions = new SpatialAnalyzerLaunchOptions
            {
                StartMinimized = true,
            },
        }.Validate());
        Assert.Throws<ArgumentException>(() => new SpatialAnalyzerLaunchOptions
        {
            JobFilePath = @"C:\Jobs\A.xit64",
            QuickStartInstrumentName = "Tracker",
        }.Validate());
    }

    [Fact]
    public async Task DefaultStartupRunsOrderedLifecycleAndStopLeavesApplicationRunning()
    {
        var transport = new FakeTransport();
        var launcher = new FakeServerLauncher();
        await using var client = CreateClient(launcher, transport);

        await client.StartAsync();
        var directory = await client.GetWorkingDirectoryAsync();
        await client.StopAsync();

        Assert.Equal("C:\\Working", directory);
        Assert.Equal(
            ["snapshot", "start-sdk", "launch-sa", "connect-sdk", "snapshot", "get-working-directory", "stop-sdk"],
            transport.Calls);
        Assert.Equal(1, launcher.LaunchCount);
        Assert.True(launcher.Server.Disposed);
        Assert.Equal(0, transport.CloseApplicationCount);
    }

    [Fact]
    public async Task ControlPlaneOnlyStartupDoesNotCreateSdkOrApplication()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);

        await client.StartAsync(new BriosaStartOptions
        {
            StartSpatialAnalyzerSdk = false,
            LaunchSpatialAnalyzer = false,
            ConnectToSpatialAnalyzer = false,
        });
        var snapshot = await client.GetServerSnapshotAsync();

        Assert.False(snapshot.ReadyForMp);
        await Assert.ThrowsAsync<BriosaLifecycleException>(
            () => client.GetWorkingDirectoryAsync());
        Assert.Equal(["snapshot", "snapshot"], transport.Calls);
    }

    [Fact]
    public async Task PostServerLifecycleFailurePreservesDiagnosticControlPlane()
    {
        var transport = new FakeTransport
        {
            LaunchFailure = ApplicationLifecycleFailure(),
        };
        var launcher = new FakeServerLauncher();
        await using var client = CreateClient(launcher, transport);

        var failure = await Assert.ThrowsAsync<BriosaSpatialAnalyzerException>(
            () => client.StartAsync());
        var state = await client.GetSpatialAnalyzerStateAsync();

        Assert.Equal(SpatialAnalyzerLifecycleFailureKind.LaunchFailed, failure.Kind);
        Assert.Equal("sa-launch-failed", failure.DiagnosticCode);
        Assert.Equal(SpatialAnalyzerApplicationState.NotRunning, state.ApplicationState);
        Assert.False(launcher.Server.Disposed);

        await client.StopAsync();
        Assert.True(launcher.Server.Disposed);
    }

    [Fact]
    public async Task FailedFinalReadinessDoesNotPublishMpAdmission()
    {
        var transport = new FakeTransport { PublishReadySnapshot = false };
        await using var client = CreateClient(new FakeServerLauncher(), transport);

        await Assert.ThrowsAsync<BriosaProtocolException>(() => client.StartAsync());
        Assert.True((await client.GetSpatialAnalyzerSdkStateAsync()).ReadyForMp);
        await Assert.ThrowsAsync<BriosaLifecycleException>(
            () => client.GetWorkingDirectoryAsync());
    }

    [Fact]
    public async Task ConcurrentStartCallsShareOneOwnedServer()
    {
        var transport = new FakeTransport();
        var launcher = new FakeServerLauncher();
        await using var client = CreateClient(launcher, transport);

        await Task.WhenAll(client.StartAsync(), client.StartAsync());

        Assert.Equal(1, launcher.LaunchCount);
        Assert.Equal(1, transport.Calls.Count(item => item == "start-sdk"));
    }

    [Fact]
    public async Task GenerationGuardsAreRetainedAndSuppliedAutomatically()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();

        await client.ReconnectToSpatialAnalyzerAsync();
        await client.StopSpatialAnalyzerSdkAsync();
        await client.StartSpatialAnalyzerSdkAsync();
        await client.CloseOwnedSpatialAnalyzerAsync();

        Assert.Equal([1, 1], transport.ConnectGenerations);
        Assert.Equal([1], transport.StopGenerations.Take(1));
        Assert.Equal([2], transport.CloseGenerations);
    }

    [Fact]
    public void SdkIncidentPreservesStableStringOperationId()
    {
        var mapped = ProtocolMapping.MapSdkState(new Transport.SpatialAnalyzerSdkLifecycleState
        {
            SdkState = Transport.SpatialAnalyzerSdkState.Faulted,
            ConnectionState = Transport.SpatialAnalyzerConnectionState.Faulted,
            ExecutionReadinessState = Transport.SpatialAnalyzerExecutionReadinessState.OperatorRecoveryRequired,
            RecoveryState = Transport.SpatialAnalyzerSdkRecoveryState.RecoveryAvailable,
            LastIncident = new Transport.SpatialAnalyzerSdkIncident
            {
                SdkGeneration = 4,
                TerminationKind = Transport.SpatialAnalyzerSdkTerminationKind.WatchdogTerminated,
                OperationId = "file_operations.get_working_directory",
            },
        });

        Assert.Equal(
            "file_operations.get_working_directory",
            mapped.LastIncident?.OperationId);
    }

    private static BriosaClient CreateClient(
        FakeServerLauncher launcher,
        FakeTransport transport) =>
        new(
            new BriosaClientOptions(),
            launcher,
            new FakeTransportFactory(transport));

    private static RpcException ApplicationLifecycleFailure()
    {
        var detail = new Transport.SpatialAnalyzerLifecycleError
        {
            Rpc = "LaunchSpatialAnalyzer",
            Kind = Transport.SpatialAnalyzerLifecycleFailureKind.LaunchFailed,
            DiagnosticCode = "sa-launch-failed",
            RecoveryGuidance = Transport.LifecycleRecoveryGuidance.CorrectEnvironment,
            State = FakeTransport.ApplicationNotRunning(),
        };
        return new RpcException(
            new Status(StatusCode.FailedPrecondition, "not parsed"),
            new Metadata
            {
                { "briosa-spatial-analyzer-lifecycle-error-bin", detail.ToByteArray() },
            });
    }

    private sealed class FakeServerLauncher : IBriosaServerLauncher
    {
        public int LaunchCount { get; private set; }
        public FakeOwnedServer Server { get; } = new();

        public Task<IOwnedBriosaServer> LaunchAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            LaunchCount++;
            return Task.FromResult<IOwnedBriosaServer>(Server);
        }
    }

    private sealed class FakeOwnedServer : IOwnedBriosaServer
    {
        public Uri Address { get; } = new("http://127.0.0.1:49152");
        public bool HasExited => false;
        public bool Disposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            Disposed = true;
            return ValueTask.CompletedTask;
        }
    }

    private sealed class FakeTransportFactory(FakeTransport transport)
        : IClientTransportFactory
    {
        public IClientTransport Create(Uri address) => transport;
    }

    private sealed class FakeTransport : IClientTransport
    {
        private int _sdkGeneration;
        private bool _connected;

        public List<string> Calls { get; } = [];
        public List<int> ConnectGenerations { get; } = [];
        public List<int> StopGenerations { get; } = [];
        public List<int> CloseGenerations { get; } = [];
        public int CloseApplicationCount { get; private set; }
        public RpcException? LaunchFailure { get; init; }
        public bool PublishReadySnapshot { get; init; } = true;

        public Task<(Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse)>
            GetServerSnapshotAsync(CancellationToken cancellationToken)
        {
            Calls.Add("snapshot");
            return Task.FromResult(MatchingSnapshot(
                _connected && PublishReadySnapshot));
        }

        public Task<Transport.SpatialAnalyzerLifecycleState> GetApplicationStateAsync(
            CancellationToken cancellationToken)
        {
            Calls.Add("get-sa-state");
            return Task.FromResult(ApplicationNotRunning());
        }

        public Task<Transport.SpatialAnalyzerLifecycleState> LaunchApplicationAsync(
            SpatialAnalyzerLaunchOptions options,
            CancellationToken cancellationToken)
        {
            Calls.Add("launch-sa");
            if (LaunchFailure is not null)
            {
                throw LaunchFailure;
            }

            return Task.FromResult(ApplicationRunning());
        }

        public Task<Transport.SpatialAnalyzerLifecycleState> CloseApplicationAsync(
            int expectedGeneration,
            CancellationToken cancellationToken)
        {
            Calls.Add("close-sa");
            CloseApplicationCount++;
            CloseGenerations.Add(expectedGeneration);
            return Task.FromResult(ApplicationNotRunning());
        }

        public Task<Transport.SpatialAnalyzerSdkLifecycleState> GetSdkStateAsync(
            CancellationToken cancellationToken)
        {
            Calls.Add("get-sdk-state");
            return Task.FromResult(SdkState());
        }

        public Task<Transport.SpatialAnalyzerSdkLifecycleState> StartSdkAsync(
            CancellationToken cancellationToken)
        {
            Calls.Add("start-sdk");
            _sdkGeneration++;
            _connected = false;
            return Task.FromResult(SdkState());
        }

        public Task<Transport.SpatialAnalyzerSdkLifecycleState> ConnectSdkAsync(
            int expectedGeneration,
            bool reconnect,
            CancellationToken cancellationToken)
        {
            Calls.Add(reconnect ? "reconnect-sdk" : "connect-sdk");
            ConnectGenerations.Add(expectedGeneration);
            _connected = true;
            return Task.FromResult(SdkState());
        }

        public Task<Transport.SpatialAnalyzerSdkLifecycleState> StopSdkAsync(
            int expectedGeneration,
            CancellationToken cancellationToken)
        {
            Calls.Add("stop-sdk");
            StopGenerations.Add(expectedGeneration);
            _connected = false;
            _sdkGeneration = 0;
            return Task.FromResult(SdkState());
        }

        public Task<Transport.SpatialAnalyzerSdkLifecycleState> RecoverSdkAsync(
            int expectedGeneration,
            CancellationToken cancellationToken)
        {
            Calls.Add("recover-sdk");
            _sdkGeneration++;
            _connected = false;
            return Task.FromResult(SdkState());
        }

        public Task<string> GetWorkingDirectoryAsync(
            TimeSpan? timeout,
            CancellationToken cancellationToken)
        {
            Calls.Add("get-working-directory");
            return Task.FromResult("C:\\Working");
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        public static Transport.SpatialAnalyzerLifecycleState ApplicationNotRunning() =>
            new()
            {
                StateRevision = 1,
                ApplicationState = Transport.SpatialAnalyzerApplicationState.NotRunning,
                Ownership = Transport.SpatialAnalyzerOwnership.None,
            };

        private static Transport.SpatialAnalyzerLifecycleState ApplicationRunning() =>
            new()
            {
                StateRevision = 2,
                ApplicationState = Transport.SpatialAnalyzerApplicationState.Running,
                Ownership = Transport.SpatialAnalyzerOwnership.ServerLaunched,
                ApplicationGeneration = 2,
            };

        private Transport.SpatialAnalyzerSdkLifecycleState SdkState()
        {
            var state = new Transport.SpatialAnalyzerSdkLifecycleState
            {
                StateRevision = 3,
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

        private static (
            Transport.GetServerInfoResponse,
            Transport.ListCapabilitiesResponse) MatchingSnapshot(bool ready)
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
            };
            var capabilities = new Transport.ListCapabilitiesResponse
            {
                ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
                SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
            };
            capabilities.Operations.Add(new Transport.OperationCapability
            {
                OperationId = "file_operations.get_working_directory",
                GrpcService = "briosa.FileOperations",
                Rpc = "GetWorkingDirectory",
                FullyQualifiedMethod = "/briosa.FileOperations/GetWorkingDirectory",
            });
            return (server, capabilities);
        }
    }
}
