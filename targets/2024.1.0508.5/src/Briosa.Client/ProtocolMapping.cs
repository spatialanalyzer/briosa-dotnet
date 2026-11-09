using Google.Protobuf;
using Grpc.Core;
using System.Globalization;
using Transport = Briosa.Client.Transport;

namespace Briosa;

internal static class ProtocolMapping
{
    private const string ApplicationErrorTrailer =
        "briosa-spatial-analyzer-lifecycle-error-bin";
    private const string SdkErrorTrailer =
        "briosa-spatial-analyzer-sdk-lifecycle-error-bin";
    private const string OperationErrorTrailer = "briosa-operation-error-bin";

    public static BriosaServerSnapshot MapSnapshot(
        Transport.GetServerInfoResponse serverInfo,
        Transport.ListCapabilitiesResponse capabilities)
    {
        BriosaProtocolCompatibility.Validate(serverInfo, capabilities);
        var version = serverInfo.Version!;
        return new BriosaServerSnapshot
        {
            BriosaVersion = version.BriosaVersion,
            SourceRevision = version.SourceRevision,
            ProtocolPackage = version.ProtocolPackage,
            SpatialAnalyzerTarget = version.SpatialAnalyzerTarget,
            ReadyForMp = serverInfo.ReadyForMp,
            WorkerState = ConvertEnum<WorkerRuntimeState>(serverInfo.WorkerState, "worker-state"),
            ConnectionState = ConvertEnum<SpatialAnalyzerConnectionState>(serverInfo.SpatialAnalyzerConnectionState, "connection-state"),
            ExecutionReadinessState = ConvertEnum<SpatialAnalyzerExecutionReadinessState>(serverInfo.SpatialAnalyzerExecutionReadinessState, "execution-readiness-state"),
            TargetIsolationMode = ConvertEnum<TargetIsolationMode>(serverInfo.TargetIsolationMode, "target-isolation-mode"),
            AdmissionProfile = capabilities.AdmissionProfile,
            PolicyFingerprint = capabilities.PolicyFingerprint,
            Operations = capabilities.Operations.Select(MapCapability).ToArray(),
        };
    }

    // The include_denied listing, already validated for identity: only the
    // entries whose server-reported admission is not Admitted.
    public static BriosaDeniedOperationListing MapDeniedCapabilities(
        Transport.ListCapabilitiesResponse capabilities) => new()
        {
            AdmissionProfile = capabilities.AdmissionProfile,
            PolicyFingerprint = capabilities.PolicyFingerprint,
            Operations = capabilities.Operations
                .Select(MapCapability)
                .Where(item => item.Admission != OperationAdmission.Admitted)
                .ToArray(),
        };

    private static BriosaOperationCapability MapCapability(Transport.OperationCapability item) => new()
    {
        OperationId = item.OperationId,
        GrpcService = item.GrpcService,
        Rpc = item.Rpc,
        FullyQualifiedMethod = item.FullyQualifiedMethod,
        RiskFlags = item.RiskFlags.Select(flag => ConvertOrUnknown<OperationRiskFlag>(flag)).Distinct().ToArray(),
        DurationClass = ConvertOrUnknown<OperationDurationClass>(item.DurationClass),
        ValidationStatus = ConvertOrUnknown<OperationValidationStatus>(item.ValidationStatus),
        Admission = ConvertOrUnknown<OperationAdmission>(item.Admission),
    };

    public static SpatialAnalyzerLifecycleState MapApplicationState(
        Transport.SpatialAnalyzerLifecycleState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        int? generation = state.HasApplicationGeneration
            ? RequireGeneration(state.ApplicationGeneration, "application-generation-invalid")
            : null;
        return new SpatialAnalyzerLifecycleState
        {
            StateRevision = state.StateRevision,
            ApplicationState = ConvertEnum<SpatialAnalyzerApplicationState>(state.ApplicationState, "application-state"),
            Ownership = ConvertEnum<SpatialAnalyzerOwnership>(state.Ownership, "application-ownership"),
            ApplicationGeneration = generation,
            DiagnosticCode = state.HasDiagnosticCode ? state.DiagnosticCode : null,
        };
    }

    public static SpatialAnalyzerSdkLifecycleState MapSdkState(
        Transport.SpatialAnalyzerSdkLifecycleState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        return new SpatialAnalyzerSdkLifecycleState
        {
            StateRevision = state.StateRevision,
            SdkState = ConvertEnum<SpatialAnalyzerSdkState>(state.SdkState, "sdk-state"),
            SdkGeneration = state.HasSdkGeneration
                ? RequireGeneration(state.SdkGeneration, "sdk-generation-invalid")
                : null,
            ApplicationGeneration = state.HasApplicationGeneration
                ? RequireGeneration(state.ApplicationGeneration, "sdk-application-generation-invalid")
                : null,
            ConnectionState = ConvertEnum<SpatialAnalyzerConnectionState>(state.ConnectionState, "sdk-connection-state"),
            ExecutionReadinessState = ConvertEnum<SpatialAnalyzerExecutionReadinessState>(state.ExecutionReadinessState, "sdk-readiness-state"),
            ReadyForMp = state.ReadyForMp,
            RecoveryState = ConvertEnum<SpatialAnalyzerSdkRecoveryState>(state.RecoveryState, "sdk-recovery-state"),
            LastIncident = state.LastIncident is null ? null : MapIncident(state.LastIncident),
            DiagnosticCode = state.HasDiagnosticCode ? state.DiagnosticCode : null,
        };
    }

    public static Exception MapRpcException(
        RpcException exception,
        CancellationToken cancellationToken,
        SpatialAnalyzerLifecycleState? applicationState = null)
    {
        if (cancellationToken.IsCancellationRequested ||
            exception.StatusCode == StatusCode.Cancelled)
        {
            return new OperationCanceledException(
                "The Briosa call was cancelled.",
                exception,
                cancellationToken);
        }

        if (TryParseTrailer(exception, ApplicationErrorTrailer, Transport.SpatialAnalyzerLifecycleError.Parser, out var applicationError))
        {
            return new BriosaSpatialAnalyzerException(
                ConvertEnum<SpatialAnalyzerLifecycleFailureKind>(applicationError.Kind, "application-failure-kind"),
                applicationError.DiagnosticCode,
                ConvertEnum<LifecycleRecoveryGuidance>(applicationError.RecoveryGuidance, "application-recovery-guidance"),
                MapApplicationState(applicationError.State),
                exception);
        }

        if (TryParseTrailer(exception, SdkErrorTrailer, Transport.SpatialAnalyzerSdkLifecycleError.Parser, out var sdkError))
        {
            var kind = ConvertEnum<SpatialAnalyzerSdkLifecycleFailureKind>(sdkError.Kind, "sdk-failure-kind");
            if (kind == SpatialAnalyzerSdkLifecycleFailureKind.IdentityMismatch)
            {
                return new BriosaCompatibilityException(sdkError.DiagnosticCode, exception);
            }

            if (kind is SpatialAnalyzerSdkLifecycleFailureKind.ApplicationNotFound or
                SpatialAnalyzerSdkLifecycleFailureKind.ApplicationAmbiguous)
            {
                return new BriosaSpatialAnalyzerException(
                    kind == SpatialAnalyzerSdkLifecycleFailureKind.ApplicationNotFound
                        ? SpatialAnalyzerLifecycleFailureKind.ApplicationNotFound
                        : SpatialAnalyzerLifecycleFailureKind.ApplicationAmbiguous,
                    sdkError.DiagnosticCode,
                    ConvertEnum<LifecycleRecoveryGuidance>(sdkError.RecoveryGuidance, "sdk-recovery-guidance"),
                    applicationState ?? new SpatialAnalyzerLifecycleState
                    {
                        ApplicationState = kind == SpatialAnalyzerSdkLifecycleFailureKind.ApplicationNotFound
                            ? SpatialAnalyzerApplicationState.NotRunning
                            : SpatialAnalyzerApplicationState.Ambiguous,
                        Ownership = SpatialAnalyzerOwnership.None,
                        DiagnosticCode = sdkError.DiagnosticCode,
                    },
                    exception);
            }

            return new BriosaSpatialAnalyzerSdkException(
                kind,
                sdkError.DiagnosticCode,
                ConvertEnum<LifecycleRecoveryGuidance>(sdkError.RecoveryGuidance, "sdk-recovery-guidance"),
                MapSdkState(sdkError.State),
                exception);
        }

        if (TryParseTrailer(exception, OperationErrorTrailer, Transport.OperationError.Parser, out var operationError))
        {
            return new BriosaOperationException(
                operationError.OperationId,
                ConvertEnum<OperationFailureKind>(operationError.Kind, "operation-failure-kind"),
                operationError.DiagnosticCode,
                ConvertEnum<ExecutionDisposition>(operationError.ExecutionDisposition, "execution-disposition"),
                ConvertEnum<RecoveryGuidance>(operationError.RecoveryGuidance, "recovery-guidance"),
                ConvertEnum<ReplayGuidance>(operationError.ReplayGuidance, "replay-guidance"),
                ConvertEnum<ReplaySafety>(operationError.ReplaySafety, "replay-safety"),
                exception,
                operationError.MpExecution is null
                    ? null
                    : MapMpExecution(operationError.MpExecution));
        }

        return new BriosaTransportException(
            TransportDiagnosticCode(exception.StatusCode),
            exception);
    }

    // A failure without a typed trailer reports `transport-` plus the gRPC status
    // name in lower kebab case, as briosa-js and briosa-py do. Cancelled never gets
    // here; it is OperationCanceledException above.
    private static string TransportDiagnosticCode(StatusCode status) => status switch
    {
        StatusCode.OK => "transport-ok",
        StatusCode.Unknown => "transport-unknown",
        StatusCode.InvalidArgument => "transport-invalid-argument",
        StatusCode.DeadlineExceeded => "transport-deadline-exceeded",
        StatusCode.NotFound => "transport-not-found",
        StatusCode.AlreadyExists => "transport-already-exists",
        StatusCode.PermissionDenied => "transport-permission-denied",
        StatusCode.ResourceExhausted => "transport-resource-exhausted",
        StatusCode.FailedPrecondition => "transport-failed-precondition",
        StatusCode.Aborted => "transport-aborted",
        StatusCode.OutOfRange => "transport-out-of-range",
        StatusCode.Unimplemented => "transport-unimplemented",
        StatusCode.Internal => "transport-internal",
        StatusCode.Unavailable => "transport-unavailable",
        StatusCode.DataLoss => "transport-data-loss",
        StatusCode.Unauthenticated => "transport-unauthenticated",
        _ => "transport-failure",
    };

    // Every successful MP result carries its execution details, checked on every
    // call whether or not the caller observes them. A success is Succeeded with
    // MP code 2 or PartialSuccess with MP code 4: the worker reports success only
    // after it reads one of those codes, and the server copies it. Anything else
    // is protocol drift.
    public static MpCompletion MapMpCompletion(IMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);
        if (response.Descriptor.FindFieldByName("execution")?.Accessor.GetValue(response)
            is not Transport.MpExecutionDetails execution)
        {
            throw new BriosaProtocolException("mp-execution-missing");
        }

        var state = ConvertEnum<MpExecutionState>(execution.State, "mp-execution-state");
        if (state is not (MpExecutionState.Succeeded or MpExecutionState.PartialSuccess))
        {
            throw new BriosaProtocolException("mp-execution-state");
        }

        var expectedCode = state == MpExecutionState.Succeeded ? 2 : 4;
        if (!execution.HasMpResultCode || execution.MpResultCode != expectedCode)
        {
            throw new BriosaProtocolException("mp-execution-result-code");
        }

        return new MpCompletion
        {
            State = state,
            MpResultCode = execution.MpResultCode,
        };
    }

    private static MpExecutionDetails MapMpExecution(Transport.MpExecutionDetails execution) => new()
    {
        State = ConvertEnum<MpExecutionState>(execution.State, "mp-execution-state"),
        MpResultCode = execution.HasMpResultCode ? execution.MpResultCode : null,
        OutputRetrievals = execution.OutputRetrievals.Select(item => new OutputRetrieval
        {
            FieldName = item.FieldName,
            State = ConvertEnum<OutputRetrievalState>(item.State, "output-retrieval-state"),
            DiagnosticCode = item.HasDiagnosticCode ? item.DiagnosticCode : null,
        }).ToArray(),
    };

    private static SpatialAnalyzerSdkIncident MapIncident(
        Transport.SpatialAnalyzerSdkIncident incident) => new()
        {
            SdkGeneration = RequireGeneration(incident.SdkGeneration, "incident-generation-invalid"),
            TerminationKind = ConvertEnum<SpatialAnalyzerSdkTerminationKind>(incident.TerminationKind, "sdk-termination-kind"),
            ExecutionDisposition = incident.HasExecutionDisposition
            ? ConvertEnum<ExecutionDisposition>(incident.ExecutionDisposition, "incident-execution-disposition")
            : null,
            OperationId = incident.HasOperationId ? incident.OperationId : null,
            DiagnosticCode = incident.HasDiagnosticCode ? incident.DiagnosticCode : null,
        };

    private static int RequireGeneration(int value, string diagnosticCode)
    {
        if (value <= 0)
        {
            throw new BriosaProtocolException(diagnosticCode);
        }

        return value;
    }

    private static TPublic ConvertEnum<TPublic>(Enum value, string diagnosticCode)
        where TPublic : struct, Enum
    {
        var converted = (TPublic)Enum.ToObject(
            typeof(TPublic),
            Convert.ToInt32(value, CultureInfo.InvariantCulture));
        if (!Enum.IsDefined(converted))
        {
            throw new BriosaProtocolException(diagnosticCode);
        }

        return converted;
    }

    // Discovery classification enums gain values in later revisions. An unknown
    // value maps to the enum's Unknown member (-1) instead of failing discovery.
    private static TPublic ConvertOrUnknown<TPublic>(Enum value)
        where TPublic : struct, Enum
    {
        const int Unknown = -1;
        var number = Convert.ToInt32(value, CultureInfo.InvariantCulture);
        var converted = (TPublic)Enum.ToObject(typeof(TPublic), number);
        return number != Unknown && Enum.IsDefined(converted)
            ? converted
            : (TPublic)Enum.ToObject(typeof(TPublic), Unknown);
    }

    private static bool TryParseTrailer<T>(
        RpcException exception,
        string name,
        MessageParser<T> parser,
        out T value)
        where T : class, IMessage<T>
    {
        var entry = exception.Trailers.FirstOrDefault(item => item.Key == name);
        if (entry is null)
        {
            value = null!;
            return false;
        }

        try
        {
            value = parser.ParseFrom(entry.ValueBytes);
            return true;
        }
        catch (InvalidProtocolBufferException protocolException)
        {
            throw new BriosaProtocolException("typed-error-malformed", protocolException);
        }
    }
}
