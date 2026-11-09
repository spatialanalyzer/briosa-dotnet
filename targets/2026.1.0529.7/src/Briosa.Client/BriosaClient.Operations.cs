using Google.Protobuf;
using Grpc.Core;

namespace Briosa;

public sealed partial class BriosaClient
{
    internal async Task<TResult> InvokeOperationAsync<TResult>(
        string service,
        string rpc,
        IMessage request,
        MessageParser responseParser,
        Action<MpCompletion>? onMpCompletion,
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
            // Validated for every call. The observer runs once, before the outputs
            // are mapped, so a caller learns how the MP command completed even if
            // mapping then fails.
            var completion = ProtocolMapping.MapMpCompletion(response);
            onMpCompletion?.Invoke(completion);
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

    internal async Task InvokeOperationAsync(
        string service,
        string rpc,
        IMessage request,
        MessageParser responseParser,
        Action<MpCompletion>? onMpCompletion,
        CancellationToken cancellationToken)
    {
        _ = await InvokeOperationAsync<NoOperationResult>(
                service,
                rpc,
                request,
                responseParser,
                onMpCompletion,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private sealed class NoOperationResult;
}
