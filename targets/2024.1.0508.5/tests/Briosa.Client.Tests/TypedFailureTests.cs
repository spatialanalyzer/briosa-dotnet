using Google.Protobuf;
using Grpc.Core;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

// Contract major 3 failure mappings: briosa#294 (SDK call faults), #360
// (partial success inside a typed failure), #355 (modal dialogs) and the
// lifecycle caller kinds of the freeze change (briosa#363, decision D-2).
public sealed class TypedFailureTests
{
    private const string OperationTrailer = "briosa-operation-error-bin";
    private const string SdkTrailer = "briosa-spatial-analyzer-sdk-lifecycle-error-bin";
    private const string ApplicationTrailer = "briosa-spatial-analyzer-lifecycle-error-bin";

    [Theory]
    [InlineData(
        Transport.ExecutionDisposition.NotStarted,
        Transport.ReplayGuidance.DoNotReplay,
        ExecutionDisposition.NotStarted,
        ReplayGuidance.DoNotReplay)]
    [InlineData(
        Transport.ExecutionDisposition.StartedOutcomeUnknown,
        Transport.ReplayGuidance.ReconcileBeforeReplay,
        ExecutionDisposition.StartedOutcomeUnknown,
        ReplayGuidance.ReconcileBeforeReplay)]
    [InlineData(
        Transport.ExecutionDisposition.Completed,
        Transport.ReplayGuidance.DoNotReplay,
        ExecutionDisposition.Completed,
        ReplayGuidance.DoNotReplay)]
    public void SdkCallFaultIsItsOwnKindWithThePhaseInTheDisposition(
        Transport.ExecutionDisposition wireDisposition,
        Transport.ReplayGuidance wireReplay,
        ExecutionDisposition expectedDisposition,
        ReplayGuidance expectedReplay)
    {
        var failure = OperationFailure(new Transport.OperationError
        {
            OperationId = "file_operations.get_working_directory",
            Kind = Transport.OperationFailureKind.SdkCallFaulted,
            DiagnosticCode = "sdk-execute-step-faulted",
            ExecutionDisposition = wireDisposition,
            RecoveryGuidance = Transport.RecoveryGuidance.None,
            ReplayGuidance = wireReplay,
            ReplaySafety = Transport.ReplaySafety.Safe,
        }, StatusCode.Internal);

        var mapped = Assert.IsType<BriosaOperationException>(
            ProtocolMapping.MapRpcException(failure, CancellationToken.None));

        Assert.Equal("SdkCallFaulted", mapped.Kind.ToString());
        Assert.NotEqual(OperationFailureKind.Internal, mapped.Kind);
        Assert.NotEqual(OperationFailureKind.MpResultRetrievalFailure, mapped.Kind);
        Assert.NotEqual(OperationFailureKind.OutputRetrievalFailure, mapped.Kind);
        Assert.Equal("sdk-execute-step-faulted", mapped.DiagnosticCode);
        Assert.Equal(expectedDisposition, mapped.ExecutionDisposition);
        Assert.Equal(expectedReplay, mapped.ReplayGuidance);
        Assert.Null(mapped.MpExecution);
    }

    [Fact]
    public void SdkCallFaultAfterMpSuccessKeepsRetrievedAndFailedOutputsApart()
    {
        var error = new Transport.OperationError
        {
            OperationId = "analysis_operations.get_point_coordinate",
            Kind = Transport.OperationFailureKind.SdkCallFaulted,
            DiagnosticCode = "sdk-output-getter-faulted",
            ExecutionDisposition = Transport.ExecutionDisposition.Completed,
            RecoveryGuidance = Transport.RecoveryGuidance.None,
            ReplayGuidance = Transport.ReplayGuidance.DoNotReplay,
            ReplaySafety = Transport.ReplaySafety.Safe,
            MpExecution = new Transport.MpExecutionDetails
            {
                State = Transport.MpExecutionState.Succeeded,
                MpResultCode = 2,
            },
        };
        error.MpExecution.OutputRetrievals.Add(new Transport.OutputRetrievalDetails
        {
            FieldName = "vector_representation",
            State = Transport.OutputRetrievalState.Retrieved,
        });
        error.MpExecution.OutputRetrievals.Add(new Transport.OutputRetrievalDetails
        {
            FieldName = "x_value",
            State = Transport.OutputRetrievalState.Failed,
            DiagnosticCode = "sdk-output-getter-faulted",
        });

        var mapped = Assert.IsType<BriosaOperationException>(ProtocolMapping.MapRpcException(
            OperationFailure(error, StatusCode.Internal),
            CancellationToken.None));

        Assert.Equal("SdkCallFaulted", mapped.Kind.ToString());
        var execution = Assert.IsType<MpExecutionDetails>(mapped.MpExecution);
        Assert.Equal(MpExecutionState.Succeeded, execution.State);
        Assert.Equal(2, execution.MpResultCode);
        Assert.Collection(
            execution.OutputRetrievals,
            retrieved =>
            {
                Assert.Equal("vector_representation", retrieved.FieldName);
                Assert.Equal(OutputRetrievalState.Retrieved, retrieved.State);
                Assert.Null(retrieved.DiagnosticCode);
            },
            failed =>
            {
                Assert.Equal("x_value", failed.FieldName);
                Assert.Equal(OutputRetrievalState.Failed, failed.State);
                Assert.Equal("sdk-output-getter-faulted", failed.DiagnosticCode);
            });
    }

    // The third call of the mp-partial-success scenario: MP code 4 whose output
    // could not be read stays PartialSuccess inside the typed failure.
    [Fact]
    public void PartialSuccessStaysDistinctInsideATypedFailure()
    {
        var error = new Transport.OperationError
        {
            OperationId = "file_operations.get_working_directory",
            Kind = Transport.OperationFailureKind.OutputRetrievalFailure,
            DiagnosticCode = "sdk-output-retrieval-failed",
            ExecutionDisposition = Transport.ExecutionDisposition.Completed,
            RecoveryGuidance = Transport.RecoveryGuidance.None,
            ReplayGuidance = Transport.ReplayGuidance.DoNotReplay,
            ReplaySafety = Transport.ReplaySafety.Safe,
            MpExecution = new Transport.MpExecutionDetails
            {
                State = Transport.MpExecutionState.PartialSuccess,
                MpResultCode = 4,
            },
        };
        error.MpExecution.OutputRetrievals.Add(new Transport.OutputRetrievalDetails
        {
            FieldName = "directory",
            State = Transport.OutputRetrievalState.Failed,
            DiagnosticCode = "sdk-output-retrieval-failed",
        });

        var mapped = Assert.IsType<BriosaOperationException>(ProtocolMapping.MapRpcException(
            OperationFailure(error, StatusCode.DataLoss),
            CancellationToken.None));

        Assert.Equal(OperationFailureKind.OutputRetrievalFailure, mapped.Kind);
        Assert.Equal(ExecutionDisposition.Completed, mapped.ExecutionDisposition);
        Assert.Equal(ReplayGuidance.DoNotReplay, mapped.ReplayGuidance);
        var execution = Assert.IsType<MpExecutionDetails>(mapped.MpExecution);
        Assert.Equal(MpExecutionState.PartialSuccess, execution.State);
        Assert.Equal(4, execution.MpResultCode);
        Assert.Equal(OutputRetrievalState.Failed, Assert.Single(execution.OutputRetrievals).State);
    }

    // Code 4 from an operation on the server's exception list arrives as an
    // ordinary MP failure with code 4.
    [Fact]
    public void ListedPartialSuccessArrivesAsAnOrdinaryMpFailureWithCodeFour()
    {
        var mapped = Assert.IsType<BriosaOperationException>(ProtocolMapping.MapRpcException(
            OperationFailure(new Transport.OperationError
            {
                OperationId = "file_operations.open_sa_file",
                Kind = Transport.OperationFailureKind.MpFailure,
                DiagnosticCode = "mp-command-failed",
                ExecutionDisposition = Transport.ExecutionDisposition.Completed,
                RecoveryGuidance = Transport.RecoveryGuidance.None,
                ReplayGuidance = Transport.ReplayGuidance.DoNotReplay,
                ReplaySafety = Transport.ReplaySafety.Unsafe,
                MpExecution = new Transport.MpExecutionDetails
                {
                    State = Transport.MpExecutionState.Failed,
                    MpResultCode = 4,
                },
            }, StatusCode.FailedPrecondition),
            CancellationToken.None));

        Assert.Equal(OperationFailureKind.MpFailure, mapped.Kind);
        var execution = Assert.IsType<MpExecutionDetails>(mapped.MpExecution);
        Assert.Equal(MpExecutionState.Failed, execution.State);
        Assert.Equal(4, execution.MpResultCode);
        Assert.Empty(execution.OutputRetrievals);
    }

    [Fact]
    public void UnknownMpExecutionStateIsAProtocolError()
    {
        var failure = OperationFailure(new Transport.OperationError
        {
            OperationId = "file_operations.get_working_directory",
            Kind = Transport.OperationFailureKind.MpFailure,
            DiagnosticCode = "mp-command-failed",
            ExecutionDisposition = Transport.ExecutionDisposition.Completed,
            RecoveryGuidance = Transport.RecoveryGuidance.None,
            ReplayGuidance = Transport.ReplayGuidance.DoNotReplay,
            ReplaySafety = Transport.ReplaySafety.Safe,
            MpExecution = new Transport.MpExecutionDetails
            {
                State = (Transport.MpExecutionState)99,
                MpResultCode = 3,
            },
        }, StatusCode.FailedPrecondition);

        // As for the other enums in a failure detail, the mapper fails closed.
        var mapped = Assert.Throws<BriosaProtocolException>(
            () => ProtocolMapping.MapRpcException(failure, CancellationToken.None));

        Assert.Equal("mp-execution-state", mapped.DiagnosticCode);
    }

    // A blocking SpatialAnalyzer dialog (briosa#355) is reported as it arrives:
    // the watchdog kind with operator intervention, never as worker replacement.
    [Fact]
    public void ModalDialogWatchdogTimeoutKeepsOperatorIntervention()
    {
        var mapped = Assert.IsType<BriosaOperationException>(ProtocolMapping.MapRpcException(
            OperationFailure(new Transport.OperationError
            {
                OperationId = "reporting_operations.create_chart_from_vector_group",
                Kind = Transport.OperationFailureKind.WorkerWatchdogTimeout,
                DiagnosticCode = "sa-modal-dialog-open",
                ExecutionDisposition = Transport.ExecutionDisposition.StartedOutcomeUnknown,
                RecoveryGuidance = Transport.RecoveryGuidance.OperatorInterventionRequired,
                ReplayGuidance = Transport.ReplayGuidance.ReconcileBeforeReplay,
                ReplaySafety = Transport.ReplaySafety.Unsafe,
            }, StatusCode.Unavailable),
            CancellationToken.None));

        Assert.Equal(OperationFailureKind.WorkerWatchdogTimeout, mapped.Kind);
        Assert.Equal("sa-modal-dialog-open", mapped.DiagnosticCode);
        Assert.Equal(RecoveryGuidance.OperatorInterventionRequired, mapped.RecoveryGuidance);
        Assert.Equal(ExecutionDisposition.StartedOutcomeUnknown, mapped.ExecutionDisposition);
        Assert.Equal(ReplayGuidance.ReconcileBeforeReplay, mapped.ReplayGuidance);
    }

    [Fact]
    public void ModalDialogReadinessFailureKeepsItsDiagnosticAndGuidance()
    {
        var mapped = Assert.IsType<BriosaSpatialAnalyzerSdkException>(ProtocolMapping.MapRpcException(
            SdkFailure(
                Transport.SpatialAnalyzerSdkLifecycleFailureKind.Timeout,
                "sa-modal-dialog-open",
                Transport.LifecycleRecoveryGuidance.OperatorActionRequired,
                StatusCode.DeadlineExceeded),
            CancellationToken.None));

        Assert.Equal(SpatialAnalyzerSdkLifecycleFailureKind.Timeout, mapped.Kind);
        Assert.Equal("sa-modal-dialog-open", mapped.DiagnosticCode);
        Assert.Equal(LifecycleRecoveryGuidance.OperatorActionRequired, mapped.RecoveryGuidance);
    }

    // A cancelled call is the .NET cancellation outcome, whatever detail arrives:
    // the client never reports it as a state conflict and never retries.
    [Theory]
    [InlineData("sdk-lifecycle-request-withdrawn")]
    [InlineData("sdk-lifecycle-caller-stopped-waiting")]
    public void CancelledLifecycleCallIsOperationCanceled(string diagnosticCode)
    {
        var failure = SdkFailure(
            Transport.SpatialAnalyzerSdkLifecycleFailureKind.CallerCancelled,
            diagnosticCode,
            Transport.LifecycleRecoveryGuidance.RefreshState,
            StatusCode.Cancelled);

        var mapped = ProtocolMapping.MapRpcException(failure, CancellationToken.None);

        Assert.IsType<OperationCanceledException>(mapped);
        Assert.Same(failure, mapped.InnerException);
    }

    [Theory]
    [InlineData(Transport.SpatialAnalyzerSdkLifecycleFailureKind.CallerCancelled, "CallerCancelled")]
    [InlineData(Transport.SpatialAnalyzerSdkLifecycleFailureKind.CallerDeadlineExceeded, "CallerDeadlineExceeded")]
    public void SdkLifecycleCallerKindsAreDistinct(
        Transport.SpatialAnalyzerSdkLifecycleFailureKind wireKind,
        string expectedKind)
    {
        var mapped = Assert.IsType<BriosaSpatialAnalyzerSdkException>(ProtocolMapping.MapRpcException(
            SdkFailure(
                wireKind,
                "sdk-lifecycle-caller-stopped-waiting",
                Transport.LifecycleRecoveryGuidance.RefreshState,
                StatusCode.DeadlineExceeded),
            CancellationToken.None));

        Assert.Equal(expectedKind, mapped.Kind.ToString());
        Assert.NotEqual(SpatialAnalyzerSdkLifecycleFailureKind.StateConflict, mapped.Kind);
        Assert.Equal("sdk-lifecycle-caller-stopped-waiting", mapped.DiagnosticCode);
        Assert.Equal(LifecycleRecoveryGuidance.RefreshState, mapped.RecoveryGuidance);
        Assert.Equal(7, mapped.State.SdkGeneration);
    }

    [Theory]
    [InlineData(Transport.SpatialAnalyzerLifecycleFailureKind.CallerCancelled, "CallerCancelled")]
    [InlineData(Transport.SpatialAnalyzerLifecycleFailureKind.CallerDeadlineExceeded, "CallerDeadlineExceeded")]
    public void ApplicationLifecycleCallerKindsAreDistinct(
        Transport.SpatialAnalyzerLifecycleFailureKind wireKind,
        string expectedKind)
    {
        var detail = new Transport.SpatialAnalyzerLifecycleError
        {
            Rpc = "LaunchSpatialAnalyzer",
            Kind = wireKind,
            DiagnosticCode = "application-lifecycle-caller-stopped-waiting",
            RecoveryGuidance = Transport.LifecycleRecoveryGuidance.RefreshState,
            State = new Transport.SpatialAnalyzerLifecycleState
            {
                StateRevision = 4,
                ApplicationState = Transport.SpatialAnalyzerApplicationState.Starting,
                Ownership = Transport.SpatialAnalyzerOwnership.ServerLaunched,
                ApplicationGeneration = 3,
            },
        };
        var failure = new RpcException(
            new Status(StatusCode.DeadlineExceeded, "not parsed"),
            new Metadata { { ApplicationTrailer, detail.ToByteArray() } });

        var mapped = Assert.IsType<BriosaSpatialAnalyzerException>(
            ProtocolMapping.MapRpcException(failure, CancellationToken.None));

        Assert.Equal(expectedKind, mapped.Kind.ToString());
        Assert.NotEqual(SpatialAnalyzerLifecycleFailureKind.StateConflict, mapped.Kind);
        Assert.Equal("application-lifecycle-caller-stopped-waiting", mapped.DiagnosticCode);
        Assert.Equal(SpatialAnalyzerApplicationState.Starting, mapped.State.ApplicationState);
    }

    // The lifecycle-cancellation scenario's second Connect: a transition still
    // in progress is a state conflict to refresh and retry later.
    [Fact]
    public void TransitionInProgressIsAStateConflictToRefresh()
    {
        var mapped = Assert.IsType<BriosaSpatialAnalyzerSdkException>(ProtocolMapping.MapRpcException(
            SdkFailure(
                Transport.SpatialAnalyzerSdkLifecycleFailureKind.StateConflict,
                "sdk-lifecycle-transition-in-progress",
                Transport.LifecycleRecoveryGuidance.RefreshState,
                StatusCode.Aborted),
            CancellationToken.None));

        Assert.Equal(SpatialAnalyzerSdkLifecycleFailureKind.StateConflict, mapped.Kind);
        Assert.Equal("sdk-lifecycle-transition-in-progress", mapped.DiagnosticCode);
        Assert.Equal(LifecycleRecoveryGuidance.RefreshState, mapped.RecoveryGuidance);
        Assert.Equal(SpatialAnalyzerSdkState.Connecting, mapped.State.SdkState);
    }

    private static RpcException OperationFailure(Transport.OperationError detail, StatusCode status) =>
        new(
            new Status(status, "not parsed"),
            new Metadata { { OperationTrailer, detail.ToByteArray() } });

    private static RpcException SdkFailure(
        Transport.SpatialAnalyzerSdkLifecycleFailureKind kind,
        string diagnosticCode,
        Transport.LifecycleRecoveryGuidance guidance,
        StatusCode status)
    {
        var detail = new Transport.SpatialAnalyzerSdkLifecycleError
        {
            Rpc = "ConnectToSpatialAnalyzer",
            Kind = kind,
            DiagnosticCode = diagnosticCode,
            RecoveryGuidance = guidance,
            State = new Transport.SpatialAnalyzerSdkLifecycleState
            {
                StateRevision = 9,
                SdkState = Transport.SpatialAnalyzerSdkState.Connecting,
                SdkGeneration = 7,
                ConnectionState = Transport.SpatialAnalyzerConnectionState.Connecting,
                ExecutionReadinessState = Transport.SpatialAnalyzerExecutionReadinessState.Unverified,
                RecoveryState = Transport.SpatialAnalyzerSdkRecoveryState.NotRequired,
            },
        };
        return new RpcException(
            new Status(status, "not parsed"),
            new Metadata { { SdkTrailer, detail.ToByteArray() } });
    }
}
