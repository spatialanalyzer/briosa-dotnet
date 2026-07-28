using System.Diagnostics.CodeAnalysis;
using Briosa.Core.V1Alpha1;
using Google.Protobuf;
using Grpc.Core;

namespace Briosa.Client;

/// <summary>Represents a failed Briosa gRPC call and its optional typed operation detail.</summary>
[SuppressMessage(
    "Design",
    "CA1032:Implement standard exception constructors",
    Justification = "Instances require a gRPC status and parsed operation detail.")]
public sealed class BriosaCallException : Exception
{
    private const string ErrorTrailerName = "briosa-operation-error-bin";

    private BriosaCallException(
        RpcException innerException,
        OperationError? operationError,
        bool operationErrorMalformed)
        : base(
            $"Briosa call failed with gRPC status {innerException.StatusCode}.",
            innerException)
    {
        StatusCode = innerException.StatusCode;
        OperationError = operationError;
        OperationErrorMalformed = operationErrorMalformed;
    }

    /// <summary>Gets the canonical gRPC status.</summary>
    public StatusCode StatusCode { get; }

    /// <summary>Gets the typed, value-free operation detail when Briosa supplied one.</summary>
    public OperationError? OperationError { get; }

    /// <summary>Gets whether the binary operation detail was present but malformed.</summary>
    public bool OperationErrorMalformed { get; }

    /// <summary>Gets whether execution started but its outcome is unknown.</summary>
    public bool CompletionUnknown =>
        OperationError?.ExecutionDisposition ==
        ExecutionDisposition.StartedOutcomeUnknown;

    /// <summary>
    /// Gets whether an ambiguous call requires reconciliation before any manual replay.
    /// The client never automatically replays operations.
    /// </summary>
    public bool ReconciliationRequired =>
        CompletionUnknown &&
        OperationError?.ReplayGuidance ==
            ReplayGuidance.ReconcileBeforeReplay;

    internal static BriosaCallException FromRpcException(RpcException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var detail = exception.Trailers.FirstOrDefault(
            entry => entry.Key == ErrorTrailerName);
        if (detail is null)
        {
            return new BriosaCallException(
                exception,
                operationError: null,
                operationErrorMalformed: false);
        }

        try
        {
            return new BriosaCallException(
                exception,
                OperationError.Parser.ParseFrom(detail.ValueBytes),
                operationErrorMalformed: false);
        }
        catch (InvalidProtocolBufferException)
        {
            return new BriosaCallException(
                exception,
                operationError: null,
                operationErrorMalformed: true);
        }
    }
}
