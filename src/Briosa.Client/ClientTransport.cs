using Grpc.Core;
using Grpc.Net.Client;
using Transport = Briosa.Client.Transport;

namespace Briosa;

internal interface IClientTransport : IAsyncDisposable
{
    Task<(Transport.GetServerInfoResponse ServerInfo, Transport.ListCapabilitiesResponse Capabilities)>
        GetServerSnapshotAsync(CancellationToken cancellationToken);

    Task<Transport.SpatialAnalyzerLifecycleState> GetApplicationStateAsync(
        CancellationToken cancellationToken);

    Task<Transport.SpatialAnalyzerLifecycleState> LaunchApplicationAsync(
        SpatialAnalyzerLaunchOptions options,
        CancellationToken cancellationToken);

    Task<Transport.SpatialAnalyzerLifecycleState> CloseApplicationAsync(
        int expectedGeneration,
        CancellationToken cancellationToken);

    Task<Transport.SpatialAnalyzerSdkLifecycleState> GetSdkStateAsync(
        CancellationToken cancellationToken);

    Task<Transport.SpatialAnalyzerSdkLifecycleState> StartSdkAsync(
        CancellationToken cancellationToken);

    Task<Transport.SpatialAnalyzerSdkLifecycleState> ConnectSdkAsync(
        int expectedGeneration,
        bool reconnect,
        CancellationToken cancellationToken);

    Task<Transport.SpatialAnalyzerSdkLifecycleState> StopSdkAsync(
        int expectedGeneration,
        CancellationToken cancellationToken);

    Task<Transport.SpatialAnalyzerSdkLifecycleState> RecoverSdkAsync(
        int expectedGeneration,
        CancellationToken cancellationToken);

    Task<string> GetWorkingDirectoryAsync(
        TimeSpan? timeout,
        CancellationToken cancellationToken);
}

internal interface IClientTransportFactory
{
    IClientTransport Create(Uri address);
}

internal sealed class GrpcClientTransportFactory : IClientTransportFactory
{
    public IClientTransport Create(Uri address) => new GrpcClientTransport(address);
}

internal sealed class GrpcClientTransport : IClientTransport
{
    private readonly GrpcChannel _channel;
    private readonly Transport.DiscoveryService.DiscoveryServiceClient _discovery;
    private readonly Transport.SpatialAnalyzerLifecycle.SpatialAnalyzerLifecycleClient _application;
    private readonly Transport.SpatialAnalyzerSdkLifecycle.SpatialAnalyzerSdkLifecycleClient _sdk;
    private readonly Transport.FileOperations.FileOperationsClient _fileOperations;

    public GrpcClientTransport(Uri address)
    {
        _channel = GrpcChannel.ForAddress(address);
        _discovery = new(_channel);
        _application = new(_channel);
        _sdk = new(_channel);
        _fileOperations = new(_channel);
    }

    public async Task<(Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse)>
        GetServerSnapshotAsync(CancellationToken cancellationToken)
    {
        var serverInfo = await _discovery.GetServerInfoAsync(
                new Transport.GetServerInfoRequest(),
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false);
        var capabilities = await _discovery.ListCapabilitiesAsync(
                new Transport.ListCapabilitiesRequest(),
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false);
        return (serverInfo, capabilities);
    }

    public async Task<Transport.SpatialAnalyzerLifecycleState> GetApplicationStateAsync(
        CancellationToken cancellationToken) =>
        (await _application.GetSpatialAnalyzerStateAsync(
                new Transport.GetSpatialAnalyzerStateRequest(),
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;

    public async Task<Transport.SpatialAnalyzerLifecycleState> LaunchApplicationAsync(
        SpatialAnalyzerLaunchOptions options,
        CancellationToken cancellationToken)
    {
        var request = new Transport.LaunchSpatialAnalyzerRequest
        {
            StartMinimized = options.StartMinimized,
        };
        if (options.JobFilePath is not null)
        {
            request.JobFilePath = options.JobFilePath;
        }
        else if (options.QuickStartInstrumentName is not null)
        {
            request.QuickStartInstrumentName = options.QuickStartInstrumentName;
        }

        return (await _application.LaunchSpatialAnalyzerAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.SpatialAnalyzerLifecycleState> CloseApplicationAsync(
        int expectedGeneration,
        CancellationToken cancellationToken) =>
        (await _application.CloseOwnedSpatialAnalyzerAsync(
                new Transport.CloseOwnedSpatialAnalyzerRequest
                {
                    ExpectedApplicationGeneration = expectedGeneration,
                },
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> GetSdkStateAsync(
        CancellationToken cancellationToken) =>
        (await _sdk.GetSpatialAnalyzerSdkStateAsync(
                new Transport.GetSpatialAnalyzerSdkStateRequest(),
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> StartSdkAsync(
        CancellationToken cancellationToken) =>
        (await _sdk.StartSpatialAnalyzerSdkAsync(
                new Transport.StartSpatialAnalyzerSdkRequest(),
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> ConnectSdkAsync(
        int expectedGeneration,
        bool reconnect,
        CancellationToken cancellationToken)
    {
        if (reconnect)
        {
            return (await _sdk.ReconnectToSpatialAnalyzerAsync(
                    new Transport.ReconnectToSpatialAnalyzerRequest
                    {
                        ExpectedSdkGeneration = expectedGeneration,
                    },
                    cancellationToken: cancellationToken)
                .ResponseAsync.ConfigureAwait(false)).State;
        }

        return (await _sdk.ConnectToSpatialAnalyzerAsync(
                new Transport.ConnectToSpatialAnalyzerRequest
                {
                    ExpectedSdkGeneration = expectedGeneration,
                },
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> StopSdkAsync(
        int expectedGeneration,
        CancellationToken cancellationToken) =>
        (await _sdk.StopSpatialAnalyzerSdkAsync(
                new Transport.StopSpatialAnalyzerSdkRequest
                {
                    ExpectedSdkGeneration = expectedGeneration,
                },
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> RecoverSdkAsync(
        int expectedGeneration,
        CancellationToken cancellationToken) =>
        (await _sdk.RecoverSpatialAnalyzerSdkAsync(
                new Transport.RecoverSpatialAnalyzerSdkRequest
                {
                    ExpectedSdkGeneration = expectedGeneration,
                    Mode = Transport.SpatialAnalyzerSdkRecoveryMode.ReplaceWithoutReplay,
                },
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;

    public async Task<string> GetWorkingDirectoryAsync(
        TimeSpan? timeout,
        CancellationToken cancellationToken)
    {
        var response = await _fileOperations.GetWorkingDirectoryAsync(
                new Transport.GetWorkingDirectoryRequest(),
                deadline: timeout is null ? null : DateTime.UtcNow.Add(timeout.Value),
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false);
        if (!response.HasDirectory)
        {
            throw new BriosaProtocolException("working-directory-missing");
        }

        return response.Directory;
    }

    public ValueTask DisposeAsync()
    {
        _channel.Dispose();
        return ValueTask.CompletedTask;
    }
}
