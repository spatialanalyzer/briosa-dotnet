namespace Briosa.Client.Tests;

/// <summary>
/// The gRPC transport runs its submission check after it builds each lifecycle
/// request and before it sends anything, so a refused submission never reaches
/// the network. The address below accepts no connection, and no call gets that far.
/// </summary>
public sealed class TransportSubmissionTests
{
    private static readonly Uri Nowhere = new("http://127.0.0.1:9");

    public static TheoryData<string> LifecycleCalls() => new()
    {
        "snapshot", "list-capabilities", "get-sa-state", "launch-sa", "close-sa",
        "get-sdk-state", "start-sdk", "connect-sdk", "reconnect-sdk", "stop-sdk", "recover-sdk",
    };

    [Theory]
    [MemberData(nameof(LifecycleCalls))]
    public async Task EveryLifecycleCallIsCheckedBeforeItIsSent(string call)
    {
        var transport = new GrpcClientTransport(Nowhere);
        try
        {
            var checks = 0;
            transport.BeforeSubmit = () =>
            {
                checks++;
                throw new OperationCanceledException("refused");
            };

            var refused = await Assert.ThrowsAsync<OperationCanceledException>(() => Invoke(transport, call))
                .ConfigureAwait(true);

            Assert.Equal("refused", refused.Message);
            Assert.Equal(1, checks);
        }
        finally
        {
            await transport.DisposeAsync().ConfigureAwait(true);
        }
    }

    [Fact]
    public async Task TheCheckIsOffUntilAStartupSetsIt()
    {
        IClientTransport fake = new NoCheckTransport();
        var transport = new GrpcClientTransport(Nowhere);
        try
        {
            Assert.Null(transport.BeforeSubmit);
            fake.BeforeSubmit = () => { };
            Assert.Null(fake.BeforeSubmit);
        }
        finally
        {
            await transport.DisposeAsync().ConfigureAwait(true);
            await fake.DisposeAsync().ConfigureAwait(true);
        }
    }

    private static Task Invoke(GrpcClientTransport transport, string call) => call switch
    {
        "snapshot" => transport.GetServerSnapshotAsync(CancellationToken.None),
        "list-capabilities" => transport.ListCapabilitiesAsync(includeDenied: true, CancellationToken.None),
        "get-sa-state" => transport.GetApplicationStateAsync(CancellationToken.None),
        "launch-sa" => transport.LaunchApplicationAsync(new SpatialAnalyzerLaunchOptions(), CancellationToken.None),
        "close-sa" => transport.CloseApplicationAsync(1, CancellationToken.None),
        "get-sdk-state" => transport.GetSdkStateAsync(CancellationToken.None),
        "start-sdk" => transport.StartSdkAsync(CancellationToken.None),
        "connect-sdk" => transport.ConnectSdkAsync(1, reconnect: false, CancellationToken.None),
        "reconnect-sdk" => transport.ConnectSdkAsync(1, reconnect: true, CancellationToken.None),
        "stop-sdk" => transport.StopSdkAsync(1, CancellationToken.None),
        "recover-sdk" => transport.RecoverSdkAsync(1, CancellationToken.None),
        _ => throw new ArgumentOutOfRangeException(nameof(call)),
    };

    // A transport that keeps the interface's default check: always none.
    private sealed class NoCheckTransport : IClientTransport
    {
        public Task<(Client.Transport.GetServerInfoResponse ServerInfo, Client.Transport.ListCapabilitiesResponse Capabilities)> GetServerSnapshotAsync(CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.ListCapabilitiesResponse> ListCapabilitiesAsync(bool includeDenied, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.SpatialAnalyzerLifecycleState> GetApplicationStateAsync(CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.SpatialAnalyzerLifecycleState> LaunchApplicationAsync(SpatialAnalyzerLaunchOptions options, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.SpatialAnalyzerLifecycleState> CloseApplicationAsync(int expectedGeneration, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.SpatialAnalyzerSdkLifecycleState> GetSdkStateAsync(CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.SpatialAnalyzerSdkLifecycleState> StartSdkAsync(CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.SpatialAnalyzerSdkLifecycleState> ConnectSdkAsync(int expectedGeneration, bool reconnect, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.SpatialAnalyzerSdkLifecycleState> StopSdkAsync(int expectedGeneration, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.SpatialAnalyzerSdkLifecycleState> RecoverSdkAsync(int expectedGeneration, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Client.Transport.GetWorkingDirectoryResult> GetWorkingDirectoryAsync(TimeSpan? timeout, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<Google.Protobuf.IMessage> InvokeOperationAsync(string service, string rpc, Google.Protobuf.IMessage request, Google.Protobuf.MessageParser responseParser, TimeSpan? timeout, CancellationToken cancellationToken) => throw new NotSupportedException();
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
