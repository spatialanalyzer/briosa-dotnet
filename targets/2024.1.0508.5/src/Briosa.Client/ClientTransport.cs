using Grpc.Core;
using Grpc.Net.Client;
using Google.Protobuf;
using Transport = Briosa.Client.Transport;

namespace Briosa;

internal interface IClientTransport : IAsyncDisposable
{
    // Runs after a lifecycle request is built and immediately before its RPC is
    // submitted. A startup whose server published lifecycle bounds sets it to its
    // budget check while it runs, so nothing is submitted past the deadline; it is
    // null otherwise. Test doubles that ignore it keep the default.
    Action? BeforeSubmit { get => null; set { } }

    Task<(Transport.GetServerInfoResponse ServerInfo, Transport.ListCapabilitiesResponse Capabilities)>
        GetServerSnapshotAsync(CancellationToken cancellationToken);

    Task<Transport.ListCapabilitiesResponse> ListCapabilitiesAsync(
        bool includeDenied,
        CancellationToken cancellationToken);

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

    Task<Transport.GetWorkingDirectoryResult> GetWorkingDirectoryAsync(
        TimeSpan? timeout,
        CancellationToken cancellationToken);

    Task<IMessage> InvokeOperationAsync(
        string service,
        string rpc,
        IMessage request,
        MessageParser responseParser,
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
    private readonly CallInvoker _callInvoker;

    public GrpcClientTransport(Uri address)
    {
        _channel = GrpcChannel.ForAddress(address);
        _discovery = new(_channel);
        _application = new(_channel);
        _sdk = new(_channel);
        _fileOperations = new(_channel);
        _callInvoker = _channel.CreateCallInvoker();
    }

    public Action? BeforeSubmit { get; set; }

    public async Task<(Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse)>
        GetServerSnapshotAsync(CancellationToken cancellationToken)
    {
        var request = new Transport.GetServerInfoRequest();
        BeforeSubmit?.Invoke();
        var serverInfo = await _discovery.GetServerInfoAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false);
        // ListCapabilitiesAsync checks again, between the two discovery RPCs.
        var capabilities = await ListCapabilitiesAsync(includeDenied: false, cancellationToken)
            .ConfigureAwait(false);
        return (serverInfo, capabilities);
    }

    public async Task<Transport.ListCapabilitiesResponse> ListCapabilitiesAsync(
        bool includeDenied,
        CancellationToken cancellationToken)
    {
        var request = new Transport.ListCapabilitiesRequest { IncludeDenied = includeDenied };
        BeforeSubmit?.Invoke();
        return await _discovery.ListCapabilitiesAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false);
    }

    public async Task<Transport.SpatialAnalyzerLifecycleState> GetApplicationStateAsync(
        CancellationToken cancellationToken)
    {
        var request = new Transport.GetSpatialAnalyzerStateRequest();
        BeforeSubmit?.Invoke();
        return (await _application.GetSpatialAnalyzerStateAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

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

        // Every option is read; nothing between this check and the RPC.
        BeforeSubmit?.Invoke();
        return (await _application.LaunchSpatialAnalyzerAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.SpatialAnalyzerLifecycleState> CloseApplicationAsync(
        int expectedGeneration,
        CancellationToken cancellationToken)
    {
        var request = new Transport.CloseOwnedSpatialAnalyzerRequest
        {
            ExpectedApplicationGeneration = expectedGeneration,
        };
        BeforeSubmit?.Invoke();
        return (await _application.CloseOwnedSpatialAnalyzerAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> GetSdkStateAsync(
        CancellationToken cancellationToken)
    {
        var request = new Transport.GetSpatialAnalyzerSdkStateRequest();
        BeforeSubmit?.Invoke();
        return (await _sdk.GetSpatialAnalyzerSdkStateAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> StartSdkAsync(
        CancellationToken cancellationToken)
    {
        var request = new Transport.StartSpatialAnalyzerSdkRequest();
        BeforeSubmit?.Invoke();
        return (await _sdk.StartSpatialAnalyzerSdkAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> ConnectSdkAsync(
        int expectedGeneration,
        bool reconnect,
        CancellationToken cancellationToken)
    {
        if (reconnect)
        {
            var reconnectRequest = new Transport.ReconnectToSpatialAnalyzerRequest
            {
                ExpectedSdkGeneration = expectedGeneration,
            };
            BeforeSubmit?.Invoke();
            return (await _sdk.ReconnectToSpatialAnalyzerAsync(
                    reconnectRequest,
                    cancellationToken: cancellationToken)
                .ResponseAsync.ConfigureAwait(false)).State;
        }

        var request = new Transport.ConnectToSpatialAnalyzerRequest
        {
            ExpectedSdkGeneration = expectedGeneration,
        };
        BeforeSubmit?.Invoke();
        return (await _sdk.ConnectToSpatialAnalyzerAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> StopSdkAsync(
        int expectedGeneration,
        CancellationToken cancellationToken)
    {
        var request = new Transport.StopSpatialAnalyzerSdkRequest
        {
            ExpectedSdkGeneration = expectedGeneration,
        };
        BeforeSubmit?.Invoke();
        return (await _sdk.StopSpatialAnalyzerSdkAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.SpatialAnalyzerSdkLifecycleState> RecoverSdkAsync(
        int expectedGeneration,
        CancellationToken cancellationToken)
    {
        var request = new Transport.RecoverSpatialAnalyzerSdkRequest
        {
            ExpectedSdkGeneration = expectedGeneration,
            Mode = Transport.SpatialAnalyzerSdkRecoveryMode.ReplaceWithoutReplay,
        };
        BeforeSubmit?.Invoke();
        return (await _sdk.RecoverSpatialAnalyzerSdkAsync(
                request,
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false)).State;
    }

    public async Task<Transport.GetWorkingDirectoryResult> GetWorkingDirectoryAsync(
        TimeSpan? timeout,
        CancellationToken cancellationToken) =>
        await _fileOperations.GetWorkingDirectoryAsync(
                new Transport.GetWorkingDirectoryRequest(),
                deadline: timeout is null ? null : DateTime.UtcNow.Add(timeout.Value),
                cancellationToken: cancellationToken)
            .ResponseAsync.ConfigureAwait(false);

    public async Task<IMessage> InvokeOperationAsync(
        string service,
        string rpc,
        IMessage request,
        MessageParser responseParser,
        TimeSpan? timeout,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(service);
        ArgumentException.ThrowIfNullOrWhiteSpace(rpc);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(responseParser);

        var method = new Method<IMessage, IMessage>(
            MethodType.Unary,
            service,
            rpc,
            Marshallers.Create<IMessage>(
                static value => value.ToByteArray(),
                static _ => throw new NotSupportedException(
                    "Briosa request deserialization is server-owned.")),
            Marshallers.Create<IMessage>(
                static value => value.ToByteArray(),
                responseParser.ParseFrom));
        var options = new CallOptions(
            deadline: timeout is null ? null : DateTime.UtcNow.Add(timeout.Value),
            cancellationToken: cancellationToken);
        return await _callInvoker.AsyncUnaryCall(method, null, options, request)
            .ResponseAsync.ConfigureAwait(false);
    }

    public ValueTask DisposeAsync()
    {
        _channel.Dispose();
        return ValueTask.CompletedTask;
    }
}
