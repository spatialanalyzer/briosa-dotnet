using Google.Protobuf;
using Grpc.Core;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

/// <summary>
/// Typed denial detail and request options from Server 0.10.1
/// (spatialanalyzer/briosa#397). Server 0.10.0 sends none of it, so its absence
/// must leave every mapping exactly as client 0.5.0 made it.
/// </summary>
public sealed class PolicyDenialTests
{
    private const string OperationTrailer = "briosa-operation-error-bin";

    [Fact]
    public void APolicyDenialCarriesItsTypedDetail()
    {
        var error = PolicyDenied();
        error.PolicyDenial = new Transport.PolicyDenialDetails
        {
            Rule = Transport.PolicyDenialRule.Flag,
            DeniedFlag = "interactive_ui",
            RequestOptionField = "show_results_dialog",
        };

        var mapped = Map(error);

        Assert.Equal(OperationFailureKind.PolicyDenied, mapped.Kind);
        var detail = Assert.IsType<PolicyDenialDetails>(mapped.PolicyDenial);
        Assert.Equal(PolicyDenialRule.Flag, detail.Rule);
        Assert.Equal(OperationRiskFlag.InteractiveUi, detail.DeniedFlag);
        Assert.Equal("show_results_dialog", detail.RequestOptionField);
        Assert.Null(detail.AdmissionProfile);
    }

    [Fact]
    public void AProfileDenialNamesTheProfileAndNoFlag()
    {
        var error = PolicyDenied();
        error.PolicyDenial = new Transport.PolicyDenialDetails
        {
            Rule = Transport.PolicyDenialRule.Profile,
            AdmissionProfile = "standard",
        };

        var detail = Assert.IsType<PolicyDenialDetails>(Map(error).PolicyDenial);

        Assert.Equal(PolicyDenialRule.Profile, detail.Rule);
        Assert.Equal("standard", detail.AdmissionProfile);
        Assert.Null(detail.DeniedFlag);
        Assert.Null(detail.RequestOptionField);
    }

    // Server 0.10.0: no detail, and the failure is exactly what 0.5.0 reported.
    [Fact]
    public void WithoutDetailThePolicyDenialIsUnchanged()
    {
        var withDetail = PolicyDenied();
        withDetail.PolicyDenial = new Transport.PolicyDenialDetails { Rule = Transport.PolicyDenialRule.Override };

        var bare = Map(PolicyDenied());
        var detailed = Map(withDetail);

        Assert.Null(bare.PolicyDenial);
        AssertSameFailure(bare, detailed);
    }

    // When the enclosing message decodes, an unknown rule, an unknown flag name,
    // or detail inconsistent with its rule or kind never changes the failure.
    [Theory]
    [InlineData(42, "physical_motion", null)]
    [InlineData(2, "teleportation", null)]
    [InlineData(2, "unspecified", null)]
    [InlineData(2, "PHYSICAL_MOTION", null)]
    [InlineData(2, null, null)]
    [InlineData(1, "physical_motion", null)]
    [InlineData(0, null, "standard")]
    public void UnrecognizedOrInconsistentDetailNeverChangesTheFailure(
        int rule,
        string? deniedFlag,
        string? profile)
    {
        var error = PolicyDenied();
        error.PolicyDenial = new Transport.PolicyDenialDetails { Rule = (Transport.PolicyDenialRule)rule };
        if (deniedFlag is not null)
        {
            error.PolicyDenial.DeniedFlag = deniedFlag;
        }

        if (profile is not null)
        {
            error.PolicyDenial.AdmissionProfile = profile;
        }

        var mapped = Map(error);

        AssertSameFailure(Map(PolicyDenied()), mapped);
        var detail = Assert.IsType<PolicyDenialDetails>(mapped.PolicyDenial);
        Assert.Equal(
            rule switch { 0 => PolicyDenialRule.Unspecified, 1 => PolicyDenialRule.Profile, 2 => PolicyDenialRule.Flag, _ => PolicyDenialRule.Unknown },
            detail.Rule);
        Assert.Equal(
            deniedFlag switch { null => null, "physical_motion" => OperationRiskFlag.PhysicalMotion, _ => OperationRiskFlag.Unknown },
            detail.DeniedFlag);
    }

    // Only a policy denial names a policy step: detail that arrives with another
    // kind is not shown, and that failure is otherwise exactly as without it.
    [Fact]
    public void DetailOnAnotherFailureKindIsNotShown()
    {
        var bare = PolicyDenied();
        bare.Kind = Transport.OperationFailureKind.Validation;
        bare.DiagnosticCode = "request-validation-failed";
        var detailed = bare.Clone();
        detailed.PolicyDenial = new Transport.PolicyDenialDetails
        {
            Rule = Transport.PolicyDenialRule.Flag,
            DeniedFlag = "physical_motion",
        };

        var mapped = Map(detailed);

        Assert.Equal(OperationFailureKind.Validation, mapped.Kind);
        Assert.Null(mapped.PolicyDenial);
        AssertSameFailure(Map(bare), mapped);
    }

    // A value a server sends never reaches the public detail as text: an
    // unrecognized flag name is only Unknown, and the record's text never
    // carries it.
    [Fact]
    public void AnUnrecognizedFlagNameIsNotCopiedIntoTheDetail()
    {
        var error = PolicyDenied();
        error.PolicyDenial = new Transport.PolicyDenialDetails
        {
            Rule = Transport.PolicyDenialRule.Flag,
            DeniedFlag = "review/test-marker",
        };

        var detail = Assert.IsType<PolicyDenialDetails>(Map(error).PolicyDenial);

        Assert.Equal(OperationRiskFlag.Unknown, detail.DeniedFlag);
        Assert.DoesNotContain("review/test-marker", detail.ToString(), StringComparison.Ordinal);
        Assert.Null(typeof(PolicyDenialDetails).GetProperty("DeniedFlagName"));
    }

    // Fields a later server adds inside the detail are skipped.
    [Fact]
    public void LaterFieldsInsideTheDetailAreIgnored()
    {
        var error = PolicyDenied();
        error.PolicyDenial = new Transport.PolicyDenialDetails { Rule = Transport.PolicyDenialRule.Exclusive };
        var detailBytes = error.PolicyDenial.ToByteArray()
            .Concat(new byte[] { 0x2A, 0x03, (byte)'n', (byte)'e', (byte)'w' })
            .ToArray();
        error.PolicyDenial = null;
        var bytes = error.ToByteArray()
            .Concat(new byte[] { 0x5A, (byte)detailBytes.Length })
            .Concat(detailBytes)
            .ToArray();

        var mapped = Assert.IsType<BriosaOperationException>(ProtocolMapping.MapRpcException(
            new RpcException(new Status(StatusCode.PermissionDenied, "ignored"), new Metadata { { OperationTrailer, bytes } }),
            CancellationToken.None));

        Assert.Equal(OperationFailureKind.PolicyDenied, mapped.Kind);
        Assert.Equal(PolicyDenialRule.Exclusive, mapped.PolicyDenial?.Rule);
    }

    // If the enclosing message cannot be decoded, the existing malformed-message
    // handling applies.
    [Fact]
    public void AnUndecodableTrailerIsStillAMalformedTypedError()
    {
        var bytes = PolicyDenied().ToByteArray().Concat(new byte[] { 0x5A, 0x7F, 0x08 }).ToArray();

        var failure = Assert.Throws<BriosaProtocolException>(() => ProtocolMapping.MapRpcException(
            new RpcException(new Status(StatusCode.PermissionDenied, "ignored"), new Metadata { { OperationTrailer, bytes } }),
            CancellationToken.None));

        Assert.Equal("typed-error-malformed", failure.DiagnosticCode);
    }

    // The one helper maps every flag of the pinned protocol by its lowercase
    // value-name suffix, and nothing else.
    [Fact]
    public void EveryProtocolRiskFlagNameMapsToItsPublicFlag()
    {
        var flags = Enum.GetValues<Transport.OperationRiskFlag>()
            .Where(flag => flag != Transport.OperationRiskFlag.Unspecified)
            .ToArray();
        Assert.Equal(11, flags.Length);
        foreach (var flag in flags)
        {
            var name = Transport.DiscoveryReflection.Descriptor.EnumTypes
                .Single(type => type.Name == nameof(Transport.OperationRiskFlag))
                .FindValueByNumber((int)flag).Name["OPERATION_RISK_FLAG_".Length..];
            var lower = string.Concat(name.Select(c => char.IsAsciiLetterUpper(c) ? (char)(c + 32) : c));

            Assert.Equal(flag.ToString(), ProtocolMapping.MapDeniedFlag(lower).ToString());
            Assert.Equal(OperationRiskFlag.Unknown, ProtocolMapping.MapDeniedFlag(name));
        }

        Assert.Equal(OperationRiskFlag.FilesystemWrite, ProtocolMapping.MapDeniedFlag("filesystem_write"));
        Assert.Equal(OperationRiskFlag.Unknown, ProtocolMapping.MapDeniedFlag(string.Empty));
        Assert.Equal(OperationRiskFlag.Unknown, ProtocolMapping.MapDeniedFlag("unspecified"));
        Assert.Equal(OperationRiskFlag.Unknown, ProtocolMapping.MapDeniedFlag(" physical_motion"));
    }

    [Fact]
    public void ADeniedCapabilityCarriesItsDetail()
    {
        var (_, capabilities) = Discovery();
        var denied = Capability(Transport.OperationAdmission.DeniedOverride);
        denied.PolicyDenial = new Transport.PolicyDenialDetails { Rule = Transport.PolicyDenialRule.Override };
        capabilities.Operations.Add(denied);

        var entry = Assert.Single(ProtocolMapping.MapDeniedCapabilities(capabilities).Operations);

        Assert.Equal(OperationAdmission.DeniedOverride, entry.Admission);
        Assert.Equal(PolicyDenialRule.Override, entry.PolicyDenial?.Rule);
    }

    // Admission is the server's value; an inconsistent or unknown detail never
    // changes it, and never makes a denied operation look admitted.
    [Theory]
    [InlineData(Transport.OperationAdmission.Admitted, Transport.PolicyDenialRule.Flag, OperationAdmission.Admitted)]
    [InlineData(Transport.OperationAdmission.DeniedProfile, (Transport.PolicyDenialRule)42, OperationAdmission.DeniedProfile)]
    [InlineData(Transport.OperationAdmission.DeniedFlag, Transport.PolicyDenialRule.Override, OperationAdmission.DeniedFlag)]
    public void ACapabilityDetailNeverChangesAdmission(
        Transport.OperationAdmission admission,
        Transport.PolicyDenialRule rule,
        OperationAdmission expected)
    {
        var (server, capabilities) = Discovery();
        var capability = Capability(admission);
        capability.PolicyDenial = new Transport.PolicyDenialDetails { Rule = rule, DeniedFlag = "not_a_flag" };
        capabilities.Operations.Add(capability);

        var mapped = Assert.Single(ProtocolMapping.MapSnapshot(server, capabilities).Operations);

        Assert.Equal(expected, mapped.Admission);
        Assert.Equal(OperationRiskFlag.Unknown, mapped.PolicyDenial?.DeniedFlag);
    }

    // Server 0.10.0 does not describe request options: null, "not described".
    // Server 0.10.1 sends a present, empty list when none apply.
    [Fact]
    public void AbsentRequestOptionsStayDistinctFromNone()
    {
        var (server, capabilities) = Discovery();
        capabilities.Operations.Add(Capability(Transport.OperationAdmission.Admitted));
        var described = Capability(Transport.OperationAdmission.Admitted);
        described.OperationId = "file_operations.get_working_directory_described";
        described.RequestOptions = new Transport.OperationRequestOptions();
        capabilities.Operations.Add(described);

        var operations = ProtocolMapping.MapSnapshot(server, capabilities).Operations;

        Assert.Null(operations[0].RequestOptions);
        Assert.Null(operations[0].PolicyDenial);
        Assert.NotNull(operations[1].RequestOptions);
        Assert.Empty(operations[1].RequestOptions!);
    }

    [Fact]
    public void RequestOptionsCarryTheirReviewedDescription()
    {
        var (server, capabilities) = Discovery();
        var capability = Capability(Transport.OperationAdmission.Admitted);
        var dialog = new Transport.OperationRequestOption
        {
            FieldName = "show_results_dialog",
            Effect = Transport.OperationRequestOptionEffect.InteractiveUi,
            Condition = Transport.OperationRequestOptionCondition.WhenTrue,
            Omission = Transport.OperationRequestOptionOmission.DoesNotActivate,
            Admission = Transport.OperationAdmission.DeniedProfile,
            PolicyDenial = new Transport.PolicyDenialDetails
            {
                Rule = Transport.PolicyDenialRule.Profile,
                AdmissionProfile = "standard",
                RequestOptionField = "show_results_dialog",
            },
        };
        var mode = new Transport.OperationRequestOption
        {
            FieldName = "measurement_mode",
            Effect = Transport.OperationRequestOptionEffect.BackgroundWork,
            Condition = Transport.OperationRequestOptionCondition.WhenOneOf,
            Omission = Transport.OperationRequestOptionOmission.Required,
            Admission = Transport.OperationAdmission.DeniedExclusive,
        };
        mode.EnablingEnumValues.Add(["MEASUREMENT_MODE_CONTINUOUS", "MEASUREMENT_MODE_STABLE"]);
        capability.RequestOptions = new Transport.OperationRequestOptions();
        capability.RequestOptions.Options.Add([dialog, mode]);
        capabilities.Operations.Add(capability);

        var mapped = Assert.Single(ProtocolMapping.MapSnapshot(server, capabilities).Operations);

        Assert.Equal(OperationAdmission.Admitted, mapped.Admission);
        Assert.Null(mapped.PolicyDenial);
        Assert.Collection(
            mapped.RequestOptions!,
            option =>
            {
                Assert.Equal("show_results_dialog", option.FieldName);
                Assert.Equal(OperationRequestOptionEffect.InteractiveUi, option.Effect);
                Assert.Equal(OperationRequestOptionCondition.WhenTrue, option.Condition);
                Assert.Equal(OperationRequestOptionOmission.DoesNotActivate, option.Omission);
                Assert.Empty(option.EnablingEnumValues);
                Assert.Equal(OperationAdmission.DeniedProfile, option.Admission);
                Assert.Equal(PolicyDenialRule.Profile, option.PolicyDenial?.Rule);
                Assert.Equal("standard", option.PolicyDenial?.AdmissionProfile);
                Assert.Equal("show_results_dialog", option.PolicyDenial?.RequestOptionField);
            },
            option =>
            {
                Assert.Equal("measurement_mode", option.FieldName);
                Assert.Equal(OperationRequestOptionEffect.BackgroundWork, option.Effect);
                Assert.Equal(OperationRequestOptionCondition.WhenOneOf, option.Condition);
                Assert.Equal(OperationRequestOptionOmission.Required, option.Omission);
                Assert.Equal(["MEASUREMENT_MODE_CONTINUOUS", "MEASUREMENT_MODE_STABLE"], option.EnablingEnumValues);
                Assert.Equal(OperationAdmission.DeniedExclusive, option.Admission);
                Assert.Null(option.PolicyDenial);
            });
    }

    // Unknown effect, condition, omission, and admission values are unknown
    // metadata: never permission, never a discovery failure.
    [Fact]
    public void UnknownRequestOptionValuesAreUnknownMetadata()
    {
        var (server, capabilities) = Discovery();
        var capability = Capability(Transport.OperationAdmission.Admitted);
        capability.RequestOptions = new Transport.OperationRequestOptions();
        capability.RequestOptions.Options.Add(new Transport.OperationRequestOption
        {
            FieldName = "future_option",
            Effect = (Transport.OperationRequestOptionEffect)42,
            Condition = (Transport.OperationRequestOptionCondition)42,
            Omission = (Transport.OperationRequestOptionOmission)42,
            Admission = (Transport.OperationAdmission)42,
            PolicyDenial = new Transport.PolicyDenialDetails { Rule = (Transport.PolicyDenialRule)42 },
        });
        capabilities.Operations.Add(capability);

        var mapped = Assert.Single(ProtocolMapping.MapSnapshot(server, capabilities).Operations);
        var option = Assert.Single(mapped.RequestOptions!);

        Assert.Equal(OperationAdmission.Admitted, mapped.Admission);
        Assert.Equal(OperationRequestOptionEffect.Unknown, option.Effect);
        Assert.Equal(OperationRequestOptionCondition.Unknown, option.Condition);
        Assert.Equal(OperationRequestOptionOmission.Unknown, option.Omission);
        Assert.Equal(OperationAdmission.Unknown, option.Admission);
        Assert.Equal(PolicyDenialRule.Unknown, option.PolicyDenial?.Rule);
    }

    private static void AssertSameFailure(BriosaOperationException expected, BriosaOperationException actual)
    {
        Assert.Equal(expected.OperationId, actual.OperationId);
        Assert.Equal(expected.Kind, actual.Kind);
        Assert.Equal(expected.DiagnosticCode, actual.DiagnosticCode);
        Assert.Equal(expected.ExecutionDisposition, actual.ExecutionDisposition);
        Assert.Equal(expected.RecoveryGuidance, actual.RecoveryGuidance);
        Assert.Equal(expected.ReplayGuidance, actual.ReplayGuidance);
        Assert.Equal(expected.ReplaySafety, actual.ReplaySafety);
        Assert.Equal(expected.MpExecution, actual.MpExecution);
        Assert.Equal(expected.Message, actual.Message);
    }

    private static BriosaOperationException Map(Transport.OperationError error) =>
        Assert.IsType<BriosaOperationException>(ProtocolMapping.MapRpcException(
            new RpcException(
                new Status(StatusCode.PermissionDenied, "ignored"),
                new Metadata { { OperationTrailer, error.ToByteArray() } }),
            CancellationToken.None));

    private static Transport.OperationError PolicyDenied() => new()
    {
        OperationId = "file_operations.get_working_directory",
        Kind = Transport.OperationFailureKind.PolicyDenied,
        DiagnosticCode = "operation-policy-denied",
        ExecutionDisposition = Transport.ExecutionDisposition.NotStarted,
        RecoveryGuidance = Transport.RecoveryGuidance.None,
        ReplayGuidance = Transport.ReplayGuidance.DoNotReplay,
        ReplaySafety = Transport.ReplaySafety.Safe,
    };

    private static Transport.OperationCapability Capability(Transport.OperationAdmission admission) => new()
    {
        OperationId = "file_operations.get_working_directory",
        GrpcService = "briosa.FileOperations",
        Rpc = "GetWorkingDirectory",
        FullyQualifiedMethod = "/briosa.FileOperations/GetWorkingDirectory",
        DurationClass = Transport.OperationDurationClass.Quick,
        ValidationStatus = Transport.OperationValidationStatus.NoRecordedGap,
        Admission = admission,
    };

    private static (Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse) Discovery() =>
        (new Transport.GetServerInfoResponse
        {
            Version = new Transport.VersionCoordinates
            {
                BriosaVersion = "0.10.1",
                SourceRevision = new string('a', 40),
                ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
                SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
            },
            Compatibility = new Transport.CompatibilityContract { Major = 3, Revision = 1 },
            TargetIsolationMode = Transport.TargetIsolationMode.SingleTenant,
        },
        new Transport.ListCapabilitiesResponse
        {
            ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
            SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
            AdmissionProfile = "standard",
            PolicyFingerprint = "sha256:" + new string('b', 64),
        });
}
