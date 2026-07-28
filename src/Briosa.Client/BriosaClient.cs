using Briosa.Core.V1Alpha1;
using Grpc.Core;
using Grpc.Net.Client;
using TargetProtocol = Briosa.Sa.V2026_1_0529_7.V1Alpha1;

namespace Briosa.Client;

/// <summary>
/// Thin asynchronous wrapper around generated Briosa gRPC clients.
/// It performs no operation retries.
/// </summary>
public sealed class BriosaClient : IDisposable
{
    private readonly BriosaClientOptions _options;
    private readonly GrpcChannel _channel;
    private readonly DiscoveryService.DiscoveryServiceClient _discovery;
    private readonly TargetProtocol.FileOperations.FileOperationsClient _fileOperations;

    /// <summary>Creates a client for one Briosa endpoint.</summary>
    public BriosaClient(BriosaClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        _options = options;
        _channel = GrpcChannel.ForAddress(options.Address);
        _discovery = new DiscoveryService.DiscoveryServiceClient(_channel);
        _fileOperations =
            new TargetProtocol.FileOperations.FileOperationsClient(_channel);
    }

    /// <summary>
    /// Reads current server/discovery state and verifies the package's exact protocol identity.
    /// </summary>
    public async Task<BriosaServerSnapshot> GetServerSnapshotAsync(
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        var deadline = ResolveDeadline(timeout);
        try
        {
            var serverInfo = await _discovery.GetServerInfoAsync(
                    new GetServerInfoRequest(),
                    deadline: deadline,
                    cancellationToken: cancellationToken)
                .ResponseAsync.ConfigureAwait(false);
            var capabilities = await _discovery.ListCapabilitiesAsync(
                    new ListCapabilitiesRequest(),
                    deadline: deadline,
                    cancellationToken: cancellationToken)
                .ResponseAsync.ConfigureAwait(false);

            BriosaProtocolCompatibility.Validate(serverInfo, capabilities);
            return new BriosaServerSnapshot(serverInfo, capabilities);
        }
        catch (RpcException exception)
        {
            throw BriosaCallException.FromRpcException(exception);
        }
    }

    /// <summary>Executes the exact-target Get Working Directory operation once.</summary>
    public async Task<TargetProtocol.GetWorkingDirectoryResult>
        GetWorkingDirectoryAsync(
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default)
    {
        try
        {
            return await _fileOperations.GetWorkingDirectoryAsync(
                    new TargetProtocol.GetWorkingDirectoryRequest(),
                    deadline: ResolveDeadline(timeout),
                    cancellationToken: cancellationToken)
                .ResponseAsync.ConfigureAwait(false);
        }
        catch (RpcException exception)
        {
            throw BriosaCallException.FromRpcException(exception);
        }
    }

    /// <inheritdoc />
    public void Dispose() => _channel.Dispose();

    private DateTime ResolveDeadline(TimeSpan? timeout)
    {
        var effectiveTimeout = timeout ?? _options.DefaultTimeout;
        if (effectiveTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeout),
                "The timeout must be positive.");
        }

        return DateTime.UtcNow.Add(effectiveTimeout);
    }
}
