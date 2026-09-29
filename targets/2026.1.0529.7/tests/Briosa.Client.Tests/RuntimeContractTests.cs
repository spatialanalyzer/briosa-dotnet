using Google.Protobuf;
using Grpc.Core;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

public sealed class RuntimeContractTests
{
    [Fact]
    public void OverloadPreservesNotStartedAndIndependentReplayGuidance()
    {
        var detail = new Transport.OperationError
        {
            OperationId = "variables.set_double_variable",
            Kind = Transport.OperationFailureKind.Overloaded,
            DiagnosticCode = "worker-admission-full",
            ExecutionDisposition = Transport.ExecutionDisposition.NotStarted,
            RecoveryGuidance = Transport.RecoveryGuidance.None,
            ReplayGuidance = Transport.ReplayGuidance.MayReplay,
            ReplaySafety = Transport.ReplaySafety.Unknown,
        };
        var failure = new RpcException(
            new Status(StatusCode.ResourceExhausted, "ignored"),
            new Metadata { { "briosa-operation-error-bin", detail.ToByteArray() } });

        var mapped = Assert.IsType<BriosaOperationException>(
            ProtocolMapping.MapRpcException(failure, CancellationToken.None));

        Assert.Equal(OperationFailureKind.Overloaded, mapped.Kind);
        Assert.Equal(ExecutionDisposition.NotStarted, mapped.ExecutionDisposition);
        Assert.Equal(RecoveryGuidance.None, mapped.RecoveryGuidance);
        Assert.Equal(ReplayGuidance.MayReplay, mapped.ReplayGuidance);
        Assert.Equal(ReplaySafety.Unknown, mapped.ReplaySafety);
        Assert.Same(failure, mapped.InnerException);
    }

    [Theory]
    [InlineData(1u, false)]
    [InlineData(2u, true)]
    [InlineData(3u, false)]
    public void SelectionRequiresCurrentContractMajor(uint major, bool expected) =>
        Assert.Equal(expected, ServerSelectionPolicy.Compatible(major, 0, "0.9.0", "source"));

    [Fact]
    public void LegacyBootstrapDoesNotBypassMajorTwo() =>
        Assert.False(ServerSelectionPolicy.Compatible(
            0, 0, ServerSelectionPolicy.LegacyVersion, ServerSelectionPolicy.LegacyRevision));

    [Fact]
    public void StoppingIsPreservedInDiscovery()
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
            Compatibility = new Transport.CompatibilityContract { Major = 2 },
            WorkerState = Transport.WorkerRuntimeState.Stopping,
            TargetIsolationMode = Transport.TargetIsolationMode.SingleTenant,
        };
        var capabilities = new Transport.ListCapabilitiesResponse
        {
            ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
            SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
        };
        Assert.Equal(WorkerRuntimeState.Stopping, ProtocolMapping.MapSnapshot(server, capabilities).WorkerState);
        server.Compatibility.Major = 1;
        Assert.Throws<BriosaCompatibilityException>(() => ProtocolMapping.MapSnapshot(server, capabilities));
    }
}
