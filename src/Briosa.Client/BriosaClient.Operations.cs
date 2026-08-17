using Google.Protobuf;
using Grpc.Core;

namespace Briosa;

public sealed partial class BriosaClient
{
    private async Task<TResult> InvokeOperationAsync<TResult>(
        string service,
        string rpc,
        IMessage request,
        MessageParser responseParser,
        CancellationToken cancellationToken)
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
            var response = await session.Transport.InvokeOperationAsync(
                    service,
                    rpc,
                    request,
                    responseParser,
                    _options.CommandTimeout,
                    cancellationToken)
                .ConfigureAwait(false);
            return OperationProtocolMapper.MapResponse<TResult>(response);
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

    private async Task InvokeOperationAsync(
        string service,
        string rpc,
        IMessage request,
        MessageParser responseParser,
        CancellationToken cancellationToken)
    {
        _ = await InvokeOperationAsync<NoOperationResult>(
                service,
                rpc,
                request,
                responseParser,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private sealed class NoOperationResult;
}
