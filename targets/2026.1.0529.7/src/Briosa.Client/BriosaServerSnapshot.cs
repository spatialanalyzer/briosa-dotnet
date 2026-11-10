#pragma warning disable CS1591 // Public contract is documented in briosa-docs.

using System.Diagnostics.CodeAnalysis;

namespace Briosa;

/// <summary>One detached, identity-validated discovery snapshot.</summary>
public sealed record BriosaServerSnapshot
{
    public required string BriosaVersion { get; init; }
    public required string SourceRevision { get; init; }
    public required string ProtocolPackage { get; init; }
    public required string SpatialAnalyzerTarget { get; init; }
    public bool ReadyForMp { get; init; }
    public WorkerRuntimeState WorkerState { get; init; }
    public SpatialAnalyzerConnectionState ConnectionState { get; init; }
    public SpatialAnalyzerExecutionReadinessState ExecutionReadinessState { get; init; }
    public TargetIsolationMode TargetIsolationMode { get; init; }

    /// <summary>
    /// Gets the server's resolved named admission profile: <c>read-only</c>,
    /// <c>standard</c>, <c>device</c>, or <c>full</c>.
    /// </summary>
    public string AdmissionProfile { get; init; } = string.Empty;

    /// <summary>
    /// Gets the value-free fingerprint of every admission input. It is opaque:
    /// compare it only for equality, for example to tell whether a replacement
    /// generation runs under the same policy.
    /// </summary>
    public string PolicyFingerprint { get; init; } = string.Empty;

    /// <summary>Gets the operations the active policy admits, ordered by operation ID.</summary>
    public required IReadOnlyList<BriosaOperationCapability> Operations { get; init; }

    public bool Supports(string fullyQualifiedMethod) =>
        Operations.Any(item => string.Equals(
            item.FullyQualifiedMethod,
            fullyQualifiedMethod,
            StringComparison.Ordinal));
}

/// <summary>
/// The diagnostic listing of the operations the server's active policy denies,
/// with the policy it was taken under. Denied entries never become callable and
/// never change the admitted operations of a <see cref="BriosaServerSnapshot"/>.
/// </summary>
public sealed record BriosaDeniedOperationListing
{
    /// <summary>Gets the resolved named admission profile the listing was taken under.</summary>
    public required string AdmissionProfile { get; init; }

    /// <summary>
    /// Gets the opaque fingerprint of the policy the listing was taken under.
    /// Compare it only for equality, for example with
    /// <see cref="BriosaServerSnapshot.PolicyFingerprint"/>.
    /// </summary>
    public required string PolicyFingerprint { get; init; }

    /// <summary>
    /// Gets each denied operation, with the precedence step that denied it in
    /// <see cref="BriosaOperationCapability.Admission"/>.
    /// </summary>
    public required IReadOnlyList<BriosaOperationCapability> Operations { get; init; }
}

/// <summary>One operation's reviewed capability row, as the server reports it.</summary>
public sealed record BriosaOperationCapability
{
    public required string OperationId { get; init; }
    public required string GrpcService { get; init; }
    public required string Rpc { get; init; }
    public required string FullyQualifiedMethod { get; init; }

    /// <summary>
    /// Gets the reviewed risk flags in declaration order. An empty list means that
    /// no reviewed risk applies. A request option can add operator UI at call time,
    /// and the server then decides; the client never pre-validates options.
    /// </summary>
    public IReadOnlyList<OperationRiskFlag> RiskFlags { get; init; } = [];

    /// <summary>Gets the reviewed duration class, which selects the server execution budget.</summary>
    public OperationDurationClass DurationClass { get; init; }

    /// <summary>Gets the recorded validation gap. It is informational and never affects admission.</summary>
    public OperationValidationStatus ValidationStatus { get; init; }

    /// <summary>
    /// Gets the active policy's static decision for this row, exactly as the server
    /// reports it. The client never recomputes admission.
    /// </summary>
    public OperationAdmission Admission { get; init; }
}

/// <summary>The reviewed risk vocabulary. A later protocol value maps to <see cref="Unknown"/>.</summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Mirrors the server's risk-flag vocabulary; each capability lists its flags, so this is not a [Flags] enum.")]
public enum OperationRiskFlag
{
    /// <summary>A value that this client does not know.</summary>
    Unknown = -1,
    Unspecified = 0,
    FilesystemMetadata = 1,
    FilesystemRead = 2,
    FilesystemWrite = 3,
    FilesystemDelete = 4,
    Destructive = 5,
    CodeExecution = 6,
    PhysicalMotion = 7,
    DeviceSession = 8,
    DeviceConfig = 9,
    InteractiveUi = 10,
    ExternalIo = 11,
}

/// <summary>The reviewed execution-duration class. Unspecified means unreviewed.</summary>
public enum OperationDurationClass
{
    /// <summary>A value that this client does not know.</summary>
    Unknown = -1,
    Unspecified = 0,
    Quick = 1,
    LongRunning = 2,
    Interactive = 3,
}

/// <summary>The recorded validation gap. Unspecified means unreviewed.</summary>
public enum OperationValidationStatus
{
    /// <summary>A value that this client does not know.</summary>
    Unknown = -1,
    Unspecified = 0,
    NoRecordedGap = 1,
    FixturePending = 2,
    AtRiskUnvalidated = 3,
}

/// <summary>The active policy's static decision for an operation's reviewed row.</summary>
public enum OperationAdmission
{
    /// <summary>A value that this client does not know.</summary>
    Unknown = -1,
    Unspecified = 0,
    Admitted = 1,

    /// <summary>The profile, widened by allowed flags, does not admit every risk flag, or the read-only effect filter excludes the operation.</summary>
    DeniedProfile = 2,

    /// <summary>A risk flag of the operation is set to deny.</summary>
    DeniedFlag = 3,

    /// <summary>A per-operation override is set to deny.</summary>
    DeniedOverride = 4,

    /// <summary>An exclusive workflow, which no setting admits in the current isolation mode.</summary>
    DeniedExclusive = 5,

    /// <summary>The operation's classification metadata is not reviewed.</summary>
    DeniedUnreviewed = 6,
}

public enum WorkerRuntimeState
{
    Unspecified,
    Stopped,
    Starting,
    Ready,
    Degraded,
    Stopping,
}

public enum TargetIsolationMode
{
    Unspecified,
    SingleTenant,
    LeaseIsolated,
}
