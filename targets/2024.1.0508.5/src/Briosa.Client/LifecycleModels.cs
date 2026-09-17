#pragma warning disable CS1591 // Public contract is documented in briosa-docs.

namespace Briosa;

/// <summary>One detached SpatialAnalyzer application lifecycle snapshot.</summary>
public sealed record SpatialAnalyzerLifecycleState
{
    public ulong StateRevision { get; init; }
    public SpatialAnalyzerApplicationState ApplicationState { get; init; }
    public SpatialAnalyzerOwnership Ownership { get; init; }
    public int? ApplicationGeneration { get; init; }
    public string? DiagnosticCode { get; init; }
}

public enum SpatialAnalyzerApplicationState
{
    Unspecified,
    NotRunning,
    Starting,
    Running,
    Closing,
    Exited,
    Ambiguous,
    Faulted,
}

public enum SpatialAnalyzerOwnership
{
    Unspecified,
    None,
    External,
    ServerLaunched,
}

/// <summary>One detached SpatialAnalyzer SDK lifecycle snapshot.</summary>
public sealed record SpatialAnalyzerSdkLifecycleState
{
    public ulong StateRevision { get; init; }
    public SpatialAnalyzerSdkState SdkState { get; init; }
    public int? SdkGeneration { get; init; }
    public int? ApplicationGeneration { get; init; }
    public SpatialAnalyzerConnectionState ConnectionState { get; init; }
    public SpatialAnalyzerExecutionReadinessState ExecutionReadinessState { get; init; }
    public bool ReadyForMp { get; init; }
    public SpatialAnalyzerSdkRecoveryState RecoveryState { get; init; }
    public SpatialAnalyzerSdkIncident? LastIncident { get; init; }
    public string? DiagnosticCode { get; init; }
}

public sealed record SpatialAnalyzerSdkIncident
{
    public int SdkGeneration { get; init; }
    public SpatialAnalyzerSdkTerminationKind TerminationKind { get; init; }
    public ExecutionDisposition? ExecutionDisposition { get; init; }

    /// <summary>Gets the stable Briosa operation identifier, when an MP call was involved.</summary>
    public string? OperationId { get; init; }

    public string? DiagnosticCode { get; init; }
}

public enum SpatialAnalyzerSdkState
{
    Unspecified,
    Stopped,
    Starting,
    Running,
    Connecting,
    Verifying,
    Ready,
    Stopping,
    Recovering,
    Faulted,
}

public enum SpatialAnalyzerSdkRecoveryState
{
    Unspecified,
    NotRequired,
    RecoveryAvailable,
    OperatorActionRequired,
    Blocked,
}

public enum SpatialAnalyzerSdkTerminationKind
{
    Unspecified,
    StartFailed,
    SdkProcessExited,
    SdkConnectionLost,
    WorkerProcessExited,
    ControlChannelLost,
    WatchdogTerminated,
}

#pragma warning disable CA1008 // The only public recovery mode is the explicit safe mode.
public enum SpatialAnalyzerSdkRecoveryMode
{
    ReplaceWithoutReplay = 1,
}
#pragma warning restore CA1008

public enum SpatialAnalyzerConnectionState
{
    Unspecified,
    Disconnected,
    Connecting,
    Connected,
    Faulted,
    Stopping,
}

public enum SpatialAnalyzerExecutionReadinessState
{
    Unspecified,
    Unverified,
    Verifying,
    ExecutionReady,
    CompetingClientSuspected,
    OperatorRecoveryRequired,
}

public enum ExecutionDisposition
{
    Unspecified,
    NotStarted,
    StartedOutcomeUnknown,
    Completed,
}

public enum SpatialAnalyzerLifecycleFailureKind
{
    Unspecified,
    Validation,
    StateConflict,
    ApplicationNotFound,
    ApplicationAmbiguous,
    LaunchFailed,
    NotOwned,
    SdkNotStopped,
    Timeout,
    Internal,
}

public enum SpatialAnalyzerSdkLifecycleFailureKind
{
    Unspecified,
    Validation,
    StateConflict,
    ApplicationNotFound,
    ApplicationAmbiguous,
    SdkAlreadyActive,
    SdkNotRunning,
    SdkStartFailed,
    SdkStopFailed,
    RecoveryNotRequired,
    SdkRecoveryFailed,
    IdentityMismatch,
    OperatorActionRequired,
    Timeout,
    Internal,
    SdkAlreadyConnected,
    SdkConnectionFailed,
    ReconnectNotRequired,
    SdkRecoveryRequired,
}

public enum LifecycleRecoveryGuidance
{
    Unspecified,
    None,
    RefreshState,
    RetryAfterStateChange,
    CorrectEnvironment,
    StopSdkFirst,
    RecoverSdkWithoutReplay,
    OperatorActionRequired,
}
