using Grpc.Core;

namespace Briosa.Client.Tests;

// Cross-client rule C5 (spatialanalyzer/briosa#363): a transport failure without
// a typed trailer gets the code `transport-` plus the gRPC status name in lower
// kebab case, as in briosa-js and briosa-py. Cancellation keeps its
// language-native form.
public sealed class TransportFailureTests
{
    [Theory]
    [InlineData(StatusCode.OK, "transport-ok")]
    [InlineData(StatusCode.Unknown, "transport-unknown")]
    [InlineData(StatusCode.InvalidArgument, "transport-invalid-argument")]
    [InlineData(StatusCode.DeadlineExceeded, "transport-deadline-exceeded")]
    [InlineData(StatusCode.NotFound, "transport-not-found")]
    [InlineData(StatusCode.AlreadyExists, "transport-already-exists")]
    [InlineData(StatusCode.PermissionDenied, "transport-permission-denied")]
    [InlineData(StatusCode.ResourceExhausted, "transport-resource-exhausted")]
    [InlineData(StatusCode.FailedPrecondition, "transport-failed-precondition")]
    [InlineData(StatusCode.Aborted, "transport-aborted")]
    [InlineData(StatusCode.OutOfRange, "transport-out-of-range")]
    [InlineData(StatusCode.Unimplemented, "transport-unimplemented")]
    [InlineData(StatusCode.Internal, "transport-internal")]
    [InlineData(StatusCode.Unavailable, "transport-unavailable")]
    [InlineData(StatusCode.DataLoss, "transport-data-loss")]
    [InlineData(StatusCode.Unauthenticated, "transport-unauthenticated")]
    [InlineData((StatusCode)99, "transport-failure")]
    public void StatusWithoutATypedTrailerMapsToItsTransportCode(StatusCode status, string expected)
    {
        var failure = new RpcException(new Status(status, "detail text is never parsed"));

        var mapped = Assert.IsType<BriosaTransportException>(
            ProtocolMapping.MapRpcException(failure, CancellationToken.None));

        Assert.Equal(expected, mapped.DiagnosticCode);
        Assert.Same(failure, mapped.InnerException);
    }

    // A status that gRPC adds later needs a reviewed name, not a guess.
    [Fact]
    public void EveryDefinedStatusExceptCancelledHasItsOwnTransportCode()
    {
        var codes = Enum.GetValues<StatusCode>()
            .Where(status => status != StatusCode.Cancelled)
            .Select(status => Assert.IsType<BriosaTransportException>(ProtocolMapping.MapRpcException(
                new RpcException(new Status(status, string.Empty)),
                CancellationToken.None)).DiagnosticCode)
            .ToArray();

        Assert.All(codes, code => Assert.Matches("^transport-[a-z]+(-[a-z]+)*$", code));
        Assert.DoesNotContain("transport-failure", codes);
        Assert.Equal(codes.Length, codes.Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void CancelledStaysLanguageNativeCancellation()
    {
        var failure = new RpcException(new Status(StatusCode.Cancelled, string.Empty));

        Assert.IsType<OperationCanceledException>(
            ProtocolMapping.MapRpcException(failure, CancellationToken.None));
    }
}
