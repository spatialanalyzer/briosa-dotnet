using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

/// <summary>
/// The records that client 0.5.1 extended keep the equality, hashing, and text
/// of client 0.5.0: the 0.5.1 members take no part in equality or hashing, and
/// appear in <c>ToString()</c> only when the server sent them. A consumer built
/// against 0.5.0 that rebuilds a value from its 0.5.0 members therefore still
/// finds it equal, for example in a <see cref="HashSet{T}"/>.
/// </summary>
public sealed class RecordCompatibilityTests
{
    private static readonly OperationRiskFlag[] Flags = [OperationRiskFlag.FilesystemRead];

    [Fact]
    public void ACapabilityComparesByItsZeroFiveZeroMembersOnly()
    {
        var plain = Capability();
        var described = plain with
        {
            RequestOptions = [],
            PolicyDenial = new PolicyDenialDetails { Rule = PolicyDenialRule.Override },
        };

        Assert.Equal(plain, described);
        Assert.True(plain == described);
        Assert.Equal(plain.GetHashCode(), described.GetHashCode());
        Assert.Contains(Capability(), new HashSet<BriosaOperationCapability> { described });
        Assert.NotEqual(plain, plain with { Admission = OperationAdmission.DeniedOverride });
        Assert.NotEqual(plain, plain with { RiskFlags = [OperationRiskFlag.FilesystemRead] });
        Assert.NotEqual(plain, plain with { OperationId = "file_operations.other" });
    }

    [Fact]
    public void ACapabilityPrintsAsInZeroFiveZeroUntilTheServerSendsMore()
    {
        var plain = Capability();

        Assert.Equal(
            "BriosaOperationCapability { OperationId = file_operations.get_working_directory, " +
            "GrpcService = briosa.FileOperations, Rpc = GetWorkingDirectory, " +
            "FullyQualifiedMethod = /briosa.FileOperations/GetWorkingDirectory, " +
            $"RiskFlags = {Flags}, DurationClass = Quick, ValidationStatus = NoRecordedGap, Admission = Admitted }}",
            plain.ToString());
        Assert.EndsWith(
            "Admission = Admitted, PolicyDenial = PolicyDenialDetails { Rule = Override, DeniedFlag = , " +
            "AdmissionProfile = , RequestOptionField =  }, RequestOptions = Briosa.OperationRequestOption[] }",
            (plain with
            {
                PolicyDenial = new PolicyDenialDetails { Rule = PolicyDenialRule.Override },
                RequestOptions = Array.Empty<OperationRequestOption>(),
            }).ToString(),
            StringComparison.Ordinal);
    }

    [Fact]
    public void ASnapshotComparesByItsZeroFiveZeroMembersOnly()
    {
        var plain = Snapshot();
        var published = plain with
        {
            LifecycleBounds = new LifecycleBounds { StartupWithLaunch = TimeSpan.FromSeconds(100) },
            ConfiguredInteractionMode = ConfiguredSpatialAnalyzerInteractionMode.Silent,
        };

        Assert.Equal(plain, published);
        Assert.Equal(plain.GetHashCode(), published.GetHashCode());
        Assert.Contains(plain, new HashSet<BriosaServerSnapshot> { published });
        Assert.NotEqual(plain, plain with { ReadyForMp = false });
        Assert.NotEqual(plain, plain with { PolicyFingerprint = "sha256:" + new string('e', 64) });
    }

    [Fact]
    public void ASnapshotPrintsAsInZeroFiveZeroUntilTheServerSendsMore()
    {
        var plain = Snapshot();

        Assert.Equal(
            "BriosaServerSnapshot { BriosaVersion = 0.10.0, SourceRevision = " + new string('a', 40) + ", " +
            "ProtocolPackage = briosa, SpatialAnalyzerTarget = " + Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget + ", " +
            "ReadyForMp = True, WorkerState = Ready, ConnectionState = Connected, " +
            "ExecutionReadinessState = ExecutionReady, TargetIsolationMode = SingleTenant, " +
            "AdmissionProfile = standard, PolicyFingerprint = sha256:" + new string('b', 64) + ", " +
            $"Operations = {plain.Operations} }}",
            plain.ToString());
        var published = (plain with
        {
            LifecycleBounds = new LifecycleBounds(),
            ConfiguredInteractionMode = ConfiguredSpatialAnalyzerInteractionMode.Manual,
        }).ToString();
        Assert.StartsWith(plain.ToString()[..^2], published, StringComparison.Ordinal);
        Assert.Contains(", LifecycleBounds = LifecycleBounds { StartSdk = ", published, StringComparison.Ordinal);
        Assert.EndsWith(", ConfiguredInteractionMode = Manual }", published, StringComparison.Ordinal);
    }

    // The reviewed scenario: a consumer built against 0.5.0 rebuilds a value that
    // a Server 0.10.1 reply produced from its 0.5.0 members and finds it equal.
    [Fact]
    public void ValuesRebuiltFromTheirZeroFiveZeroMembersStayEqual()
    {
        var (server, capabilities) = Discovery();
        server.LifecycleBounds = new Transport.LifecycleBounds { StartupWithLaunchMilliseconds = 100_000 };
        server.ConfiguredSpatialAnalyzerInteractionMode = Transport.ConfiguredSpatialAnalyzerInteractionMode.Silent;
        capabilities.Operations[0].RequestOptions = new Transport.OperationRequestOptions();

        var snapshot = ProtocolMapping.MapSnapshot(server, capabilities);
        var capability = Assert.Single(snapshot.Operations);
        var rebuiltCapability = new BriosaOperationCapability
        {
            OperationId = capability.OperationId,
            GrpcService = capability.GrpcService,
            Rpc = capability.Rpc,
            FullyQualifiedMethod = capability.FullyQualifiedMethod,
            RiskFlags = capability.RiskFlags,
            DurationClass = capability.DurationClass,
            ValidationStatus = capability.ValidationStatus,
            Admission = capability.Admission,
        };
        var rebuiltSnapshot = new BriosaServerSnapshot
        {
            BriosaVersion = snapshot.BriosaVersion,
            SourceRevision = snapshot.SourceRevision,
            ProtocolPackage = snapshot.ProtocolPackage,
            SpatialAnalyzerTarget = snapshot.SpatialAnalyzerTarget,
            ReadyForMp = snapshot.ReadyForMp,
            WorkerState = snapshot.WorkerState,
            ConnectionState = snapshot.ConnectionState,
            ExecutionReadinessState = snapshot.ExecutionReadinessState,
            TargetIsolationMode = snapshot.TargetIsolationMode,
            AdmissionProfile = snapshot.AdmissionProfile,
            PolicyFingerprint = snapshot.PolicyFingerprint,
            Operations = snapshot.Operations,
        };

        Assert.NotNull(capability.RequestOptions);
        Assert.NotNull(snapshot.LifecycleBounds);
        Assert.Equal(rebuiltCapability, capability);
        Assert.Equal(rebuiltCapability.GetHashCode(), capability.GetHashCode());
        Assert.Contains(rebuiltCapability, new HashSet<BriosaOperationCapability> { capability });
        Assert.Equal(rebuiltSnapshot, snapshot);
        Assert.Equal(rebuiltSnapshot.GetHashCode(), snapshot.GetHashCode());
        Assert.Contains(rebuiltSnapshot, new HashSet<BriosaServerSnapshot> { snapshot });
    }

    // A Server 0.10.0 reply prints exactly as client 0.5.0 printed it.
    [Fact]
    public void AServer0100ReplyPrintsWithoutTheNewMembers()
    {
        var (server, capabilities) = Discovery();

        var snapshot = ProtocolMapping.MapSnapshot(server, capabilities);
        var capability = Assert.Single(snapshot.Operations);

        Assert.DoesNotContain("LifecycleBounds", snapshot.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("ConfiguredInteractionMode", snapshot.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("RequestOptions", capability.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("PolicyDenial", capability.ToString(), StringComparison.Ordinal);
        Assert.EndsWith($"Operations = {snapshot.Operations} }}", snapshot.ToString(), StringComparison.Ordinal);
        Assert.EndsWith("Admission = Admitted }", capability.ToString(), StringComparison.Ordinal);
    }

    private static BriosaOperationCapability Capability() => new()
    {
        OperationId = "file_operations.get_working_directory",
        GrpcService = "briosa.FileOperations",
        Rpc = "GetWorkingDirectory",
        FullyQualifiedMethod = "/briosa.FileOperations/GetWorkingDirectory",
        RiskFlags = Flags,
        DurationClass = OperationDurationClass.Quick,
        ValidationStatus = OperationValidationStatus.NoRecordedGap,
        Admission = OperationAdmission.Admitted,
    };

    private static BriosaServerSnapshot Snapshot() => new()
    {
        BriosaVersion = "0.10.0",
        SourceRevision = new string('a', 40),
        ProtocolPackage = "briosa",
        SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
        ReadyForMp = true,
        WorkerState = WorkerRuntimeState.Ready,
        ConnectionState = SpatialAnalyzerConnectionState.Connected,
        ExecutionReadinessState = SpatialAnalyzerExecutionReadinessState.ExecutionReady,
        TargetIsolationMode = TargetIsolationMode.SingleTenant,
        AdmissionProfile = "standard",
        PolicyFingerprint = "sha256:" + new string('b', 64),
        Operations = [Capability()],
    };

    private static (Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse) Discovery()
    {
        var server = new Transport.GetServerInfoResponse
        {
            Version = new Transport.VersionCoordinates
            {
                BriosaVersion = "0.10.0",
                SourceRevision = new string('a', 40),
                ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
                SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
            },
            Compatibility = new Transport.CompatibilityContract { Major = 3 },
            TargetIsolationMode = Transport.TargetIsolationMode.SingleTenant,
        };
        var capabilities = new Transport.ListCapabilitiesResponse
        {
            ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
            SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
            AdmissionProfile = "standard",
            PolicyFingerprint = "sha256:" + new string('b', 64),
        };
        capabilities.Operations.Add(new Transport.OperationCapability
        {
            OperationId = "file_operations.get_working_directory",
            GrpcService = "briosa.FileOperations",
            Rpc = "GetWorkingDirectory",
            FullyQualifiedMethod = "/briosa.FileOperations/GetWorkingDirectory",
            DurationClass = Transport.OperationDurationClass.Quick,
            ValidationStatus = Transport.OperationValidationStatus.NoRecordedGap,
            Admission = Transport.OperationAdmission.Admitted,
        });
        return (server, capabilities);
    }
}
