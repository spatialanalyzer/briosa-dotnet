#pragma warning disable CS1591 // Public contract is documented in briosa-docs.

using System.Diagnostics.CodeAnalysis;

namespace Briosa;

/// <summary>Base class for failures reported by the Briosa client.</summary>
public abstract class BriosaException : Exception
{
    protected BriosaException()
    {
    }

    protected BriosaException(string message)
        : base(message)
    {
    }

    protected BriosaException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}

[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "A stable diagnostic code is required.")]
public sealed class BriosaStartupException : BriosaException
{
    internal BriosaStartupException(string diagnosticCode, Exception? innerException = null)
        : base($"Briosa startup failed ({diagnosticCode}).", innerException) =>
        DiagnosticCode = diagnosticCode;

    public string DiagnosticCode { get; }
}

[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "A stable diagnostic code is required.")]
public sealed class BriosaLifecycleException : BriosaException
{
    internal BriosaLifecycleException(string diagnosticCode)
        : base($"The Briosa client lifecycle does not allow this operation ({diagnosticCode}).") =>
        DiagnosticCode = diagnosticCode;

    public string DiagnosticCode { get; }
}

[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "A stable diagnostic code is required.")]
public sealed class BriosaProtocolException : BriosaException
{
    internal BriosaProtocolException(string diagnosticCode, Exception? innerException = null)
        : base($"Briosa returned an invalid protocol value ({diagnosticCode}).", innerException) =>
        DiagnosticCode = diagnosticCode;

    public string DiagnosticCode { get; }
}

[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "A stable diagnostic code is required.")]
public sealed class BriosaCompatibilityException : BriosaException
{
    internal BriosaCompatibilityException(string diagnosticCode, Exception? innerException = null)
        : base($"The Briosa runtime is incompatible with this client ({diagnosticCode}).", innerException) =>
        DiagnosticCode = diagnosticCode;

    public string DiagnosticCode { get; }
}

[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Typed lifecycle detail is required.")]
public sealed class BriosaSpatialAnalyzerException : BriosaException
{
    internal BriosaSpatialAnalyzerException(
        SpatialAnalyzerLifecycleFailureKind kind,
        string diagnosticCode,
        LifecycleRecoveryGuidance recoveryGuidance,
        SpatialAnalyzerLifecycleState state,
        Exception? innerException = null)
        : base($"SpatialAnalyzer lifecycle operation failed ({diagnosticCode}).", innerException)
    {
        Kind = kind;
        DiagnosticCode = diagnosticCode;
        RecoveryGuidance = recoveryGuidance;
        State = state;
    }

    public SpatialAnalyzerLifecycleFailureKind Kind { get; }
    public string DiagnosticCode { get; }
    public LifecycleRecoveryGuidance RecoveryGuidance { get; }
    public SpatialAnalyzerLifecycleState State { get; }
}

[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Typed lifecycle detail is required.")]
public sealed class BriosaSpatialAnalyzerSdkException : BriosaException
{
    internal BriosaSpatialAnalyzerSdkException(
        SpatialAnalyzerSdkLifecycleFailureKind kind,
        string diagnosticCode,
        LifecycleRecoveryGuidance recoveryGuidance,
        SpatialAnalyzerSdkLifecycleState state,
        Exception? innerException = null)
        : base($"SpatialAnalyzer SDK lifecycle operation failed ({diagnosticCode}).", innerException)
    {
        Kind = kind;
        DiagnosticCode = diagnosticCode;
        RecoveryGuidance = recoveryGuidance;
        State = state;
    }

    public SpatialAnalyzerSdkLifecycleFailureKind Kind { get; }
    public string DiagnosticCode { get; }
    public LifecycleRecoveryGuidance RecoveryGuidance { get; }
    public SpatialAnalyzerSdkLifecycleState State { get; }
}

public enum OperationFailureKind
{
    Unspecified,
    Validation,
    Unsupported,
    SpatialAnalyzerUnavailable,
    WorkerUnavailable,
    CallerCancelled,
    CallerDeadlineExceeded,
    WorkerWatchdogTimeout,
    WorkerFailure,
    ExecuteStepRejected,
    MpFailure,
    OutputRetrievalFailure,
    Internal,
    PolicyDenied,
    MpResultRetrievalFailure,
    SdkArgumentRejected,
}

public enum RecoveryGuidance
{
    Unspecified,
    None,
    WaitForReadiness,
    WorkerReplacement,
    OperatorInterventionRequired,
}

public enum ReplayGuidance
{
    Unspecified,
    DoNotReplay,
    MayReplay,
    ReconcileBeforeReplay,
}

public enum ReplaySafety
{
    Unspecified,
    Safe,
    Unsafe,
    Unknown,
}

[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Typed operation detail is required.")]
public sealed class BriosaOperationException : BriosaException
{
    internal BriosaOperationException(
        string operationId,
        OperationFailureKind kind,
        string diagnosticCode,
        ExecutionDisposition executionDisposition,
        RecoveryGuidance recoveryGuidance,
        ReplayGuidance replayGuidance,
        ReplaySafety replaySafety,
        Exception innerException)
        : base($"Briosa operation '{operationId}' failed ({diagnosticCode}).", innerException)
    {
        OperationId = operationId;
        Kind = kind;
        DiagnosticCode = diagnosticCode;
        ExecutionDisposition = executionDisposition;
        RecoveryGuidance = recoveryGuidance;
        ReplayGuidance = replayGuidance;
        ReplaySafety = replaySafety;
    }

    public string OperationId { get; }
    public OperationFailureKind Kind { get; }
    public string DiagnosticCode { get; }
    public ExecutionDisposition ExecutionDisposition { get; }
    public RecoveryGuidance RecoveryGuidance { get; }
    public ReplayGuidance ReplayGuidance { get; }
    public ReplaySafety ReplaySafety { get; }
}

[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "A stable diagnostic code is required.")]
public sealed class BriosaTransportException : BriosaException
{
    internal BriosaTransportException(string diagnosticCode, Exception innerException)
        : base($"Briosa transport operation failed ({diagnosticCode}).", innerException) =>
        DiagnosticCode = diagnosticCode;

    public string DiagnosticCode { get; }
}
