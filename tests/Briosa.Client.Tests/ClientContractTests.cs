using Briosa.Core.V1Alpha1;
using Google.Protobuf;
using Grpc.Core;

namespace Briosa.Client.Tests;

public sealed class ClientContractTests
{
    [Fact]
    public void ProtocolIdentityMatchesPinnedArtifact()
    {
        Assert.Equal(
            "briosa-protocol-0.2.0-dev.2-sa-2026.1.0529.7-catalog-5",
            BriosaProtocolIdentity.ArtifactName);
        Assert.Equal(
            "4ce33ac6ecc9db382e870aa2c005f90a25128ad863fcf007c855d00470ea3e39",
            BriosaProtocolIdentity.ArtifactSha256);
        Assert.Equal("0.2.0-dev.2", BriosaProtocolIdentity.BriosaVersion);
        Assert.Equal(
            "1a0714345981592b37e26a90ffc4db0de32fe388",
            BriosaProtocolIdentity.SourceRevision);
        Assert.Equal(
            "briosa.sa.v2026_1_0529_7.v1alpha1",
            BriosaProtocolIdentity.TargetProtocolPackage);
        Assert.Equal("2026.1.0529.7", BriosaProtocolIdentity.SpatialAnalyzerTarget);
        Assert.Equal("5", BriosaProtocolIdentity.CatalogRevision);
    }

    [Fact]
    public void CompatibilityAcceptsExactProtocolAndCatalogIdentity()
    {
        var (serverInfo, capabilities) = MatchingIdentity();

        BriosaProtocolCompatibility.Validate(serverInfo, capabilities);
    }

    [Fact]
    public void CompatibilityRejectsCatalogDrift()
    {
        var (serverInfo, capabilities) = MatchingIdentity();
        capabilities.CatalogRevision = "different";

        var exception = Assert.Throws<BriosaCompatibilityException>(
            () => BriosaProtocolCompatibility.Validate(serverInfo, capabilities));

        Assert.Equal("capability-catalog-revision-mismatch", exception.DiagnosticCode);
    }

    [Fact]
    public void TypedErrorPreservesUnknownCompletionAndReconciliation()
    {
        var detail = new OperationError
        {
            OperationId = "conformance.mutating_operation",
            Kind = OperationFailureKind.WorkerWatchdogTimeout,
            DiagnosticCode = "worker-execution-watchdog-timeout",
            ExecutionDisposition = ExecutionDisposition.StartedOutcomeUnknown,
            RecoveryGuidance = RecoveryGuidance.WorkerReplacement,
            ReplayGuidance = ReplayGuidance.ReconcileBeforeReplay,
            ReplaySafety = ReplaySafety.Unsafe
        };
        var trailers = new Metadata
        {
            { "briosa-operation-error-bin", detail.ToByteArray() }
        };
        var rpcException = new RpcException(
            new Status(StatusCode.Unavailable, "not parsed"),
            trailers);

        var exception = BriosaCallException.FromRpcException(rpcException);

        Assert.Equal(StatusCode.Unavailable, exception.StatusCode);
        Assert.Equal(detail, exception.OperationError);
        Assert.True(exception.CompletionUnknown);
        Assert.True(exception.ReconciliationRequired);
        Assert.False(exception.OperationErrorMalformed);
    }

    [Fact]
    public void MalformedTypedErrorIsReportedWithoutParsingStatusText()
    {
        var trailers = new Metadata
        {
            { "briosa-operation-error-bin", new byte[] { 0xff } }
        };
        var rpcException = new RpcException(
            new Status(StatusCode.DataLoss, "untrusted detail"),
            trailers);

        var exception = BriosaCallException.FromRpcException(rpcException);

        Assert.Equal(StatusCode.DataLoss, exception.StatusCode);
        Assert.Null(exception.OperationError);
        Assert.True(exception.OperationErrorMalformed);
        Assert.DoesNotContain(
            "untrusted detail",
            exception.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public void OptionsRejectNonHttpTransportAndNonPositiveTimeout()
    {
        var invalidScheme = new BriosaClientOptions
        {
            Address = new Uri("ftp://localhost")
        };
        var invalidTimeout = new BriosaClientOptions
        {
            Address = new Uri("http://localhost"),
            DefaultTimeout = TimeSpan.Zero
        };

        Assert.Throws<ArgumentException>(invalidScheme.Validate);
        Assert.Throws<ArgumentOutOfRangeException>(invalidTimeout.Validate);
    }

    private static (
        GetServerInfoResponse ServerInfo,
        ListCapabilitiesResponse Capabilities) MatchingIdentity() =>
        (
            new GetServerInfoResponse
            {
                Version = new VersionCoordinates
                {
                    CoreProtocolPackage =
                        BriosaProtocolIdentity.CoreProtocolPackage,
                    SpatialAnalyzerTarget =
                        BriosaProtocolIdentity.SpatialAnalyzerTarget,
                    TargetProtocolPackage =
                        BriosaProtocolIdentity.TargetProtocolPackage,
                    CatalogRevision =
                        BriosaProtocolIdentity.CatalogRevision
                }
            },
            new ListCapabilitiesResponse
            {
                CatalogId = BriosaProtocolIdentity.CatalogId,
                CatalogRevision = BriosaProtocolIdentity.CatalogRevision,
                SpatialAnalyzerTarget =
                    BriosaProtocolIdentity.SpatialAnalyzerTarget,
                TargetProtocolPackage =
                    BriosaProtocolIdentity.TargetProtocolPackage
            });
}
