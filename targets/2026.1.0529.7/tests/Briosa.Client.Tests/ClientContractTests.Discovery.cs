#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

// Discovery at contract major 3 (spatialanalyzer/briosa#314): the admission
// policy and each capability's reviewed classification, and a diagnostic
// listing of denied operations.
public sealed partial class ClientContractTests
{
    [Fact]
    public void SnapshotExposesTheAdmissionPolicyAndCapabilityClassification()
    {
        var (server, capabilities) = DiscoveryResponses();
        capabilities.AdmissionProfile = "standard";
        capabilities.PolicyFingerprint = "sha256:" + new string('a', 64);
        var operation = Capability(
            "file_operations.import_ascii_file",
            Transport.OperationAdmission.Admitted,
            Transport.OperationRiskFlag.FilesystemRead,
            Transport.OperationRiskFlag.InteractiveUi);
        operation.DurationClass = Transport.OperationDurationClass.LongRunning;
        operation.ValidationStatus = Transport.OperationValidationStatus.FixturePending;
        capabilities.Operations.Add(operation);

        var snapshot = ProtocolMapping.MapSnapshot(server, capabilities);

        Assert.Equal("standard", snapshot.AdmissionProfile);
        Assert.Equal(capabilities.PolicyFingerprint, snapshot.PolicyFingerprint);
        var capability = Assert.Single(snapshot.Operations);
        Assert.Equal(
            [OperationRiskFlag.FilesystemRead, OperationRiskFlag.InteractiveUi],
            capability.RiskFlags);
        Assert.Equal(OperationDurationClass.LongRunning, capability.DurationClass);
        Assert.Equal(OperationValidationStatus.FixturePending, capability.ValidationStatus);
        Assert.Equal(OperationAdmission.Admitted, capability.Admission);
    }

    [Fact]
    public void EmptyRiskFlagsMeanThatNoReviewedRiskApplies()
    {
        var (server, capabilities) = DiscoveryResponses();
        capabilities.Operations.Add(Capability(
            "file_operations.get_working_directory",
            Transport.OperationAdmission.Admitted));

        var capability = Assert.Single(ProtocolMapping.MapSnapshot(server, capabilities).Operations);

        Assert.Empty(capability.RiskFlags);
        Assert.Equal(OperationDurationClass.Quick, capability.DurationClass);
        Assert.Equal(OperationValidationStatus.NoRecordedGap, capability.ValidationStatus);
    }

    // A value added in a later revision never fails discovery and is never
    // folded into a known value.
    [Fact]
    public void UnknownClassificationValuesMapToADistinctUnknownMember()
    {
        var (server, capabilities) = DiscoveryResponses();
        var operation = Capability(
            "construction_operations.delete_objects",
            (Transport.OperationAdmission)42,
            (Transport.OperationRiskFlag)42,
            Transport.OperationRiskFlag.Destructive);
        operation.DurationClass = (Transport.OperationDurationClass)42;
        operation.ValidationStatus = (Transport.OperationValidationStatus)42;
        capabilities.Operations.Add(operation);

        var capability = Assert.Single(ProtocolMapping.MapSnapshot(server, capabilities).Operations);

        Assert.Equal([OperationRiskFlag.Unknown, OperationRiskFlag.Destructive], capability.RiskFlags);
        Assert.Equal(OperationDurationClass.Unknown, capability.DurationClass);
        Assert.Equal(OperationValidationStatus.Unknown, capability.ValidationStatus);
        Assert.Equal(OperationAdmission.Unknown, capability.Admission);
    }

    [Fact]
    public async Task DeniedOperationsComeOnlyFromTheDiagnosticListing()
    {
        var transport = new FakeTransport();
        transport.DeniedOperations.Add(Capability(
            "utility_operations.delete_folder",
            Transport.OperationAdmission.DeniedProfile,
            Transport.OperationRiskFlag.FilesystemDelete));
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();

        var denied = await client.GetDeniedOperationsAsync();

        var entry = Assert.Single(denied);
        Assert.Equal("utility_operations.delete_folder", entry.OperationId);
        Assert.Equal(OperationAdmission.DeniedProfile, entry.Admission);
        Assert.Equal([OperationRiskFlag.FilesystemDelete], entry.RiskFlags);
        Assert.Contains("list-capabilities:include-denied", transport.Calls);
        var snapshot = await client.GetServerSnapshotAsync();
        Assert.False(snapshot.Supports("/briosa.UtilityOperations/DeleteFolder"));
        Assert.All(snapshot.Operations, item => Assert.Equal(OperationAdmission.Admitted, item.Admission));
        Assert.Equal("standard", snapshot.AdmissionProfile);
    }

    [Fact]
    public async Task DeniedListingFromAnotherTargetIsRejected()
    {
        var transport = new FakeTransport { DeniedListingTarget = "1900.1.0000.0" };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();

        var failure = await Assert.ThrowsAsync<BriosaCompatibilityException>(
            () => client.GetDeniedOperationsAsync());

        Assert.Equal("capability-sa-target-mismatch", failure.DiagnosticCode);
    }

    private static (Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse) DiscoveryResponses() =>
        (new Transport.GetServerInfoResponse
        {
            Version = new Transport.VersionCoordinates
            {
                BriosaVersion = Transport.BriosaProtocolIdentity.BriosaVersion,
                SourceRevision = Transport.BriosaProtocolIdentity.SourceRevision,
                ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
                SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
            },
            Compatibility = new Transport.CompatibilityContract { Major = 3 },
            TargetIsolationMode = Transport.TargetIsolationMode.SingleTenant,
        },
        new Transport.ListCapabilitiesResponse
        {
            ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
            SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
            AdmissionProfile = "standard",
            PolicyFingerprint = "sha256:" + new string('b', 64),
        });

    private static Transport.OperationCapability Capability(
        string operationId,
        Transport.OperationAdmission admission,
        params Transport.OperationRiskFlag[] riskFlags)
    {
        var (service, rpc) = operationId switch
        {
            "file_operations.get_working_directory" => ("briosa.FileOperations", "GetWorkingDirectory"),
            "file_operations.import_ascii_file" => ("briosa.FileOperations", "ImportAsciiFile"),
            "construction_operations.delete_objects" => ("briosa.ConstructionOperations", "DeleteObjects"),
            "utility_operations.delete_folder" => ("briosa.UtilityOperations", "DeleteFolder"),
            _ => throw new ArgumentOutOfRangeException(nameof(operationId)),
        };
        var capability = new Transport.OperationCapability
        {
            OperationId = operationId,
            GrpcService = service,
            Rpc = rpc,
            FullyQualifiedMethod = $"/{service}/{rpc}",
            DurationClass = Transport.OperationDurationClass.Quick,
            ValidationStatus = Transport.OperationValidationStatus.NoRecordedGap,
            Admission = admission,
        };
        capability.RiskFlags.Add(riskFlags);
        return capability;
    }
}
