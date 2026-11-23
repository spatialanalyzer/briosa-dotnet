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

    /// <summary>
    /// Gets the lifecycle wait allowances that the server computes from its own
    /// configuration, or <see langword="null"/> when the server publishes none.
    /// </summary>
    /// <remarks>
    /// Server 0.10.1 and later publish them; Server 0.10.0 does not, and the
    /// client then keeps its fixed defaults. Each allowance covers only the
    /// server's own work. None includes transport time, and none promises that a
    /// response arrives or that a process exits within it. With Server 0.10.1 and
    /// later, a startup whose <see cref="BriosaStartOptions.StartupTimeout"/> is
    /// left unset sizes its budget from these allowances.
    /// </remarks>
    public LifecycleBounds? LifecycleBounds { get; init; }

    /// <summary>
    /// Gets the SpatialAnalyzer interaction mode that the server is configured to
    /// set after each verified Connect. This is the server's configuration, never
    /// the mode SpatialAnalyzer is using now: SpatialAnalyzer has no getter for
    /// it, and an operator or a Set Interaction Mode call can change it at any
    /// time.
    /// </summary>
    /// <remarks>
    /// Server 0.10.1 and later report it. Against Server 0.10.0, and for a value
    /// this client does not recognize, it is
    /// <see cref="ConfiguredSpatialAnalyzerInteractionMode.Unknown"/>.
    /// </remarks>
    public ConfiguredSpatialAnalyzerInteractionMode ConfiguredInteractionMode { get; init; }

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
    /// <see cref="BriosaOperationCapability.Admission"/>. With Server 0.10.1 and
    /// later, <see cref="BriosaOperationCapability.PolicyDenial"/> also carries
    /// the typed denial detail.
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

    /// <summary>
    /// Gets which policy step denied this operation's reviewed row, or
    /// <see langword="null"/> when the server sent no detail.
    /// </summary>
    /// <remarks>
    /// Server 0.10.1 and later send it exactly when <see cref="Admission"/> is a
    /// denied value; Server 0.10.0 never sends it. It is informational: an
    /// absent, inconsistent, or unrecognized detail never changes
    /// <see cref="Admission"/>.
    /// </remarks>
    public PolicyDenialDetails? PolicyDenial { get; init; }

    /// <summary>
    /// Gets the operation's reviewed request options, or <see langword="null"/>
    /// when the server did not describe them.
    /// </summary>
    /// <remarks>
    /// Server 0.10.1 and later describe them for every operation, with an empty
    /// list when no request option applies. Server 0.10.0 does not, so the value
    /// is <see langword="null"/>, which means "not described", never "none
    /// apply". The list order carries no meaning. The client never pre-validates
    /// a request from this description; the server decides each request.
    /// </remarks>
    public IReadOnlyList<OperationRequestOption>? RequestOptions { get; init; }
}

/// <summary>
/// One reviewed request option: a request field whose value turns on operator UI
/// or leaves work running after the call returns. Server 0.10.1 and later
/// describe them in discovery.
/// </summary>
/// <remarks>
/// The description never permits anything. The server decides each actual
/// request, with every option it turns on taken together, and can deny an
/// admitted operation for its options with a typed
/// <see cref="OperationFailureKind.PolicyDenied"/> failure. The client never
/// pre-validates a request from this description.
/// </remarks>
public sealed record OperationRequestOption
{
    /// <summary>Gets the option's field name in the operation's protobuf request message.</summary>
    public required string FieldName { get; init; }

    /// <summary>Gets what the option turns on.</summary>
    public OperationRequestOptionEffect Effect { get; init; }

    /// <summary>Gets which values of the field turn the effect on.</summary>
    public OperationRequestOptionCondition Condition { get; init; }

    /// <summary>
    /// Gets, for <see cref="OperationRequestOptionCondition.WhenOneOf"/>, the
    /// protobuf enum value names that turn the effect on. Any enum number the
    /// server does not recognize turns it on too. Empty for every other condition.
    /// </summary>
    public IReadOnlyList<string> EnablingEnumValues { get; init; } = [];

    /// <summary>Gets what omitting the field does.</summary>
    public OperationRequestOptionOmission Omission { get; init; }

    /// <summary>
    /// Gets what the active policy decides for a request that turns on this
    /// option alone, with every other request option inactive. It answers that
    /// one question only.
    /// </summary>
    public OperationAdmission Admission { get; init; }

    /// <summary>
    /// Gets which policy step denies such a request, or <see langword="null"/>
    /// when the server sent no detail. The server sends it exactly when
    /// <see cref="Admission"/> is a denied value.
    /// </summary>
    public PolicyDenialDetails? PolicyDenial { get; init; }
}

/// <summary>What a request option turns on. A later protocol value maps to <see cref="Unknown"/>.</summary>
public enum OperationRequestOptionEffect
{
    /// <summary>A value that this client does not know. It is unknown metadata, never permission.</summary>
    Unknown = -1,
    Unspecified = 0,

    /// <summary>The request opens or blocks on operator UI: it gains the interactive UI risk flag and the interactive duration class.</summary>
    InteractiveUi = 1,

    /// <summary>The call returns while device or operator work keeps running: the request is an exclusive workflow.</summary>
    BackgroundWork = 2,
}

/// <summary>Which values of a request option's field turn its effect on. A later protocol value maps to <see cref="Unknown"/>.</summary>
public enum OperationRequestOptionCondition
{
    /// <summary>A value that this client does not know. It is unknown metadata, never permission.</summary>
    Unknown = -1,
    Unspecified = 0,

    /// <summary>A Boolean field set to true.</summary>
    WhenTrue = 1,

    /// <summary>A Boolean field set to false.</summary>
    WhenFalse = 2,

    /// <summary>A message field that is present.</summary>
    WhenPresent = 3,

    /// <summary>A string field that is present and not empty.</summary>
    WhenNonEmpty = 4,

    /// <summary>An enum field set to one of <see cref="OperationRequestOption.EnablingEnumValues"/>, or to any number the server does not recognize.</summary>
    WhenOneOf = 5,
}

/// <summary>What omitting a request option's field does. A later protocol value maps to <see cref="Unknown"/>.</summary>
public enum OperationRequestOptionOmission
{
    /// <summary>A value that this client does not know. It is unknown metadata, never permission.</summary>
    Unknown = -1,
    Unspecified = 0,

    /// <summary>Omitting the field does not turn the effect on.</summary>
    DoesNotActivate = 1,

    /// <summary>Omitting the field turns the effect on.</summary>
    Activates = 2,

    /// <summary>The request mapping requires the field: once mapping is reached, omitting it fails validation before worker dispatch.</summary>
    Required = 3,
}

/// <summary>
/// The lifecycle wait allowances that Server 0.10.1 and later compute from their
/// own effective configuration. An allowance that the server did not send, or
/// sent as zero, is <see langword="null"/>: unavailable, never unbounded.
/// </summary>
/// <remarks>
/// Each allowance covers only the server's own work, from the event its server
/// documentation names. None includes transport or network time, and none
/// promises that a response reaches the caller or that any process exits within
/// it. A missing allowance affects only its own purpose: the others stay usable.
/// </remarks>
public sealed record LifecycleBounds
{
    /// <summary>Gets the Start SDK allowance, from the server's acceptance of the request to its typed outcome.</summary>
    public TimeSpan? StartSdk { get; init; }

    /// <summary>Gets the Launch SpatialAnalyzer allowance, from the start of the application process until its main window is ready or the process exits.</summary>
    public TimeSpan? LaunchSpatialAnalyzer { get; init; }

    /// <summary>Gets the Connect and Reconnect allowance, from the server's acceptance of the request to its typed outcome.</summary>
    public TimeSpan? ConnectToSpatialAnalyzer { get; init; }

    /// <summary>Gets the Recover SDK allowance, from the server's acceptance of the request to the replacement's typed outcome.</summary>
    public TimeSpan? RecoverSdk { get; init; }

    /// <summary>Gets the Stop SDK allowance, from the start of the worker stop to its typed outcome.</summary>
    public TimeSpan? StopSdk { get; init; }

    /// <summary>Gets the Close Owned SpatialAnalyzer allowance, from the application's acceptance of the close request until its process exits.</summary>
    public TimeSpan? CloseOwnedSpatialAnalyzer { get; init; }

    /// <summary>
    /// Gets the server's sizing rule for a startup that starts the SDK, launches
    /// SpatialAnalyzer, and connects. It is not a server deadline, and it
    /// excludes the client's own server startup, discovery, and round trips.
    /// </summary>
    public TimeSpan? StartupWithLaunch { get; init; }

    /// <summary>Gets the same sizing rule for a startup that starts the SDK and connects without launching.</summary>
    public TimeSpan? StartupWithoutLaunch { get; init; }

    /// <summary>
    /// Gets the most a graceful stop of the server host may wait before it
    /// cancels the remaining shutdown work. Its expiry does not prove that the
    /// worker was released or that any process exited.
    /// </summary>
    public GracefulShutdownBound HostGracefulShutdown { get; init; } = GracefulShutdownBound.Unavailable;
}

/// <summary>
/// A graceful shutdown allowance: unavailable, finite, or explicitly unbounded.
/// The client never reads an absent or unrecognized bound as unbounded.
/// </summary>
public sealed record GracefulShutdownBound
{
    internal static GracefulShutdownBound Unavailable { get; } = new();

    /// <summary>Gets which kind of bound the server published.</summary>
    public GracefulShutdownBoundKind Kind { get; init; }

    /// <summary>Gets the finite allowance when <see cref="Kind"/> is <see cref="GracefulShutdownBoundKind.Finite"/>; otherwise <see langword="null"/>.</summary>
    public TimeSpan? Limit { get; init; }
}

/// <summary>Which kind of graceful shutdown bound the server published.</summary>
public enum GracefulShutdownBoundKind
{
    /// <summary>
    /// No usable bound: the server sent none, sent zero, sent a case this client
    /// does not recognize, or sent a finite value too large to represent.
    /// </summary>
    Unavailable = 0,

    /// <summary>A positive finite bound, in <see cref="GracefulShutdownBound.Limit"/>.</summary>
    Finite = 1,

    /// <summary>The server host is configured with no graceful shutdown time limit.</summary>
    Unbounded = 2,
}

/// <summary>
/// The SpatialAnalyzer interaction mode that a server is configured to set after
/// each verified Connect. It describes server configuration, never the mode
/// SpatialAnalyzer is using now, and it is separate from the MP and MP dialog
/// interaction modes.
/// </summary>
public enum ConfiguredSpatialAnalyzerInteractionMode
{
    /// <summary>
    /// Not reported, as by Server 0.10.0, or a value this client does not recognize.
    /// </summary>
    Unknown = 0,

    /// <summary>The server never sets the mode and leaves SpatialAnalyzer's mode untouched.</summary>
    Unchanged = 1,

    /// <summary>The server sets Silent after each verified Connect.</summary>
    Silent = 2,

    /// <summary>The server sets Automatic after each verified Connect.</summary>
    Automatic = 3,

    /// <summary>The server sets Manual after each verified Connect.</summary>
    Manual = 4,
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
