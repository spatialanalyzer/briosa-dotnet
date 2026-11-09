using System.Text.Json;
using Briosa;

const string ContractId = "briosa.first-party-client.v1";
const string WorkingDirectoryMethod = "/briosa.FileOperations/GetWorkingDirectory";

var arguments = ParseArguments(args);
var contract = JsonDocument.Parse(await File.ReadAllTextAsync(arguments.ContractPath));
Require(
    contract.RootElement.GetProperty("contract_id").GetString() == ContractId,
    "The fixture received an unsupported conformance contract.");
Require(
    contract.RootElement.GetProperty("scenarios").EnumerateArray().Any(
        item => item.GetProperty("id").GetString() == arguments.Scenario),
    "The requested scenario is absent from the conformance contract.");

await RunScenarioAsync(arguments.Scenario);

await Console.Out.WriteLineAsync(JsonSerializer.Serialize(new
{
    schema_version = 1,
    contract_id = ContractId,
    scenario = arguments.Scenario,
    success = true,
}));

static async Task RunScenarioAsync(string scenario)
{
    if (Environment.GetEnvironmentVariable("BRIOSA_CONFORMANCE_EXPECT_INCOMPATIBLE") == "1")
    {
        Require(scenario == "control-plane-only", "Rejection must run without SDK or application startup.");
        var selection = new BriosaServerSelection
        {
            ExecutablePath = Environment.GetEnvironmentVariable("BRIOSA_SERVER_PATH"),
            AllowPrerelease = true,
        };
        var report = BriosaInstallations.Discover(selection);
        Require(report.Selected is null && report.DiagnosticCode == "server-installation-incompatible",
            "The packaged server was not rejected for contract incompatibility.");
        await using var rejected = new BriosaClient();
        try
        {
            await rejected.StartAsync(new BriosaStartOptions
            {
                ServerSelection = selection,
                StartSpatialAnalyzerSdk = false,
                LaunchSpatialAnalyzer = false,
                ConnectToSpatialAnalyzer = false,
            });
            throw new InvalidOperationException("Startup unexpectedly accepted an incompatible server.");
        }
        catch (BriosaStartupException error) when (error.DiagnosticCode == "server-installation-incompatible")
        {
            return;
        }
    }

    var commandTimeout = scenario == "deadline"
        ? TimeSpan.FromMilliseconds(250)
        : (TimeSpan?)null;
    await using var briosa = new BriosaClient(new BriosaClientOptions
    {
        CommandTimeout = commandTimeout,
    });

    var startupSucceeded = false;
    try
    {
        var startOptions = scenario switch
        {
            // lifecycle-cancellation drives the SDK lifecycle itself.
            "control-plane-only" or "lifecycle-cancellation" => new BriosaStartOptions
            {
                StartSpatialAnalyzerSdk = false,
                LaunchSpatialAnalyzer = false,
                ConnectToSpatialAnalyzer = false,
            },
            "attach-existing" => new BriosaStartOptions
            {
                LaunchSpatialAnalyzer = false,
            },
            _ => BriosaStartOptions.Default,
        };

        startOptions = startOptions with
        {
            ServerSelection = new BriosaServerSelection
            {
                ExecutablePath = Environment.GetEnvironmentVariable("BRIOSA_SERVER_PATH"),
                AllowPrerelease = true,
            },
        };

        if (scenario == "identity-mismatch")
        {
            await RequireThrowsAsync<BriosaCompatibilityException>(
                () => briosa.StartAsync(startOptions));
            await CleanupApplicationAsync(briosa);
            return;
        }

        await briosa.StartAsync(startOptions);
        startupSucceeded = true;

        switch (scenario)
        {
            case "control-plane-only":
                await AssertControlPlaneOnlyAsync(briosa);
                break;
            case "default-ready":
                await AssertDefaultReadyAsync(briosa);
                break;
            case "attach-existing":
                await AssertAttachExistingAsync(briosa);
                break;
            case "capability-denied":
                await AssertCapabilityDeniedAsync(briosa);
                break;
            case "mp-failure":
                await AssertOperationFailureAsync(
                    briosa,
                    OperationFailureKind.MpFailure,
                    ExecutionDisposition.Completed);
                break;
            case "output-failure":
                await AssertOperationFailureAsync(
                    briosa,
                    OperationFailureKind.OutputRetrievalFailure,
                    ExecutionDisposition.Completed);
                break;
            case "sdk-call-faulted":
                await AssertSdkCallFaultedAsync(briosa);
                break;
            case "mp-partial-success":
                await AssertMpPartialSuccessAsync(briosa);
                break;
            case "lifecycle-cancellation":
                await AssertLifecycleCancellationAsync(briosa);
                break;
            case "deadline":
                await AssertDeadlineAsync(briosa);
                break;
            case "cancellation":
                await AssertCancellationAsync(briosa);
                break;
            case "watchdog-recovery":
                await AssertWatchdogRecoveryAsync(briosa);
                break;
            case "sdk-loss-recovery":
                await AssertSdkLossRecoveryAsync(briosa);
                break;
            case "owned-cleanup":
                await AssertDefaultReadyAsync(briosa);
                break;
            default:
                throw new InvalidOperationException(
                    $"Unsupported conformance scenario '{scenario}'.");
        }

        await CleanupApplicationAsync(briosa);
    }
    finally
    {
        if (startupSucceeded || scenario == "identity-mismatch")
        {
            await briosa.StopAsync();
        }
    }
}

static async Task AssertControlPlaneOnlyAsync(BriosaClient briosa)
{
    var snapshot = await briosa.GetServerSnapshotAsync();
    var sdk = await briosa.GetSpatialAnalyzerSdkStateAsync();
    var application = await briosa.GetSpatialAnalyzerStateAsync();
    Require(!snapshot.ReadyForMp, "An inert server reported MP readiness.");
    Require(sdk.SdkState == SpatialAnalyzerSdkState.Stopped, "The SDK started implicitly.");
    Require(
        application.ApplicationState == SpatialAnalyzerApplicationState.NotRunning,
        "SpatialAnalyzer started implicitly.");
}

static async Task AssertDefaultReadyAsync(BriosaClient briosa)
{
    var snapshot = await briosa.GetServerSnapshotAsync();
    var sdk = await briosa.GetSpatialAnalyzerSdkStateAsync();
    var application = await briosa.GetSpatialAnalyzerStateAsync();
    Require(snapshot.ReadyForMp, "Default startup did not establish MP readiness.");
    Require(snapshot.Supports(WorkingDirectoryMethod), "The expected operation is absent.");
    Require(sdk.SdkState == SpatialAnalyzerSdkState.Ready && sdk.ReadyForMp,
        "The SDK is not ready after default startup.");
    Require(application.Ownership == SpatialAnalyzerOwnership.ServerLaunched,
        "Default startup did not launch an owned application.");
    _ = await briosa.GetWorkingDirectoryAsync();
    // Positional arguments exercise old and new packaged parameter names.
    await briosa.CloudDisplayControlAsync(3, 2);
}

static async Task AssertAttachExistingAsync(BriosaClient briosa)
{
    var application = await briosa.GetSpatialAnalyzerStateAsync();
    var sdk = await briosa.GetSpatialAnalyzerSdkStateAsync();
    Require(application.Ownership == SpatialAnalyzerOwnership.External,
        "The pre-existing application was incorrectly claimed as owned.");
    Require(sdk.ReadyForMp, "Attach-existing startup did not establish MP readiness.");
}

static async Task AssertCapabilityDeniedAsync(BriosaClient briosa)
{
    var snapshot = await briosa.GetServerSnapshotAsync();
    Require(!snapshot.Supports(WorkingDirectoryMethod),
        "A policy-denied operation remained advertised.");

    // Contract major 3: the packaged profile, each admitted capability's reviewed
    // classification, and the diagnostic listing of denied operations.
    Require(snapshot.AdmissionProfile == "standard",
        "Discovery did not report the packaged standard admission profile.");
    Require(!string.IsNullOrEmpty(snapshot.PolicyFingerprint),
        "Discovery did not report a policy fingerprint.");
    Require(snapshot.Operations.Count > 0 && snapshot.Operations.All(operation =>
            operation.Admission == OperationAdmission.Admitted &&
            operation.DurationClass is not (OperationDurationClass.Unspecified or OperationDurationClass.Unknown) &&
            operation.ValidationStatus is not (OperationValidationStatus.Unspecified or OperationValidationStatus.Unknown)),
        "An admitted capability lacks its reviewed classification.");
    var denied = await briosa.GetDeniedOperationsAsync();
    var workingDirectory = denied.SingleOrDefault(
        operation => operation.FullyQualifiedMethod == WorkingDirectoryMethod);
    Require(workingDirectory?.Admission == OperationAdmission.DeniedOverride,
        "The denied listing did not report the override that denies the operation.");
    Require(denied.All(operation => operation.Admission != OperationAdmission.Admitted),
        "The denied listing reported an admitted operation.");
    var refreshed = await briosa.GetServerSnapshotAsync();
    Require(!refreshed.Supports(WorkingDirectoryMethod) &&
            refreshed.PolicyFingerprint == snapshot.PolicyFingerprint,
        "The denied listing changed the admitted capabilities.");

    var exception = await RequireThrowsAsync<BriosaOperationException>(
        () => briosa.GetWorkingDirectoryAsync());
    Require(exception.Kind == OperationFailureKind.PolicyDenied,
        "Policy denial did not map to the typed public error.");
    Require(exception.ExecutionDisposition == ExecutionDisposition.NotStarted,
        "Policy denial reported an invalid execution disposition.");
}

// The fake worker faults in ExecuteStep: the fault is its own kind, its phase is
// the disposition, and the next call succeeds on the same generation.
static async Task AssertSdkCallFaultedAsync(BriosaClient briosa)
{
    var exception = await RequireThrowsAsync<BriosaOperationException>(
        () => briosa.GetWorkingDirectoryAsync());
    Require(exception.Kind == OperationFailureKind.SdkCallFaulted &&
            exception.DiagnosticCode == "sdk-execute-step-faulted",
        "The SDK call fault was not reported as its own kind.");
    Require(exception.OperationId == "file_operations.get_working_directory",
        "The operation identity was not preserved.");
    Require(exception.ExecutionDisposition == ExecutionDisposition.StartedOutcomeUnknown &&
            exception.RecoveryGuidance == RecoveryGuidance.None &&
            exception.ReplayGuidance == ReplayGuidance.ReconcileBeforeReplay,
        "The SDK call fault phase or guidance was not preserved.");
    Require(exception.MpExecution is { State: MpExecutionState.ResultUnavailable, MpResultCode: null },
        "The SDK call fault's MP execution details were not preserved.");
    _ = await briosa.GetWorkingDirectoryAsync();
}

// MP code 4 is a success with its output, and each call observes its own MP
// completion. The third call's output is unavailable: the typed failure keeps
// PartialSuccess and the code.
static async Task AssertMpPartialSuccessAsync(BriosaClient briosa)
{
    var completions = new List<MpCompletion>();
    var directory = await briosa.GetWorkingDirectoryAsync(onMpCompletion: completions.Add);
    Require(!string.IsNullOrEmpty(directory) &&
            completions is [{ State: MpExecutionState.PartialSuccess, MpResultCode: 4 }],
        "The partial success was not returned with its MP completion.");

    completions.Clear();
    directory = await briosa.GetWorkingDirectoryAsync(onMpCompletion: completions.Add);
    Require(!string.IsNullOrEmpty(directory) &&
            completions is [{ State: MpExecutionState.Succeeded, MpResultCode: 2 }],
        "The full success did not report its own MP completion.");

    completions.Clear();
    var exception = await RequireThrowsAsync<BriosaOperationException>(
        () => briosa.GetWorkingDirectoryAsync(onMpCompletion: completions.Add));
    Require(exception.Kind == OperationFailureKind.OutputRetrievalFailure &&
            exception.ExecutionDisposition == ExecutionDisposition.Completed &&
            exception.ReplayGuidance == ReplayGuidance.DoNotReplay,
        "The output failure after partial success was not preserved.");
    var keptPartialSuccess = exception.MpExecution is
    {
        State: MpExecutionState.PartialSuccess,
        MpResultCode: 4,
        OutputRetrievals: [{ State: OutputRetrievalState.Failed }],
    };
    Require(keptPartialSuccess, "The typed failure did not keep the partial success.");
    Require(completions.Count == 0, "A failed call reported an MP completion.");
}

// An accepted Connect continues after its caller stops waiting (briosa#305).
static async Task AssertLifecycleCancellationAsync(BriosaClient briosa)
{
    var application = await briosa.GetSpatialAnalyzerStateAsync();
    var externalRunning = application is
    {
        ApplicationState: SpatialAnalyzerApplicationState.Running,
        Ownership: SpatialAnalyzerOwnership.External,
        ApplicationGeneration: not null,
    };
    Require(externalRunning, "The external application is not running.");
    var started = await briosa.StartSpatialAnalyzerSdkAsync();
    var startedDisconnected = started is
    {
        SdkState: SpatialAnalyzerSdkState.Running,
        ConnectionState: SpatialAnalyzerConnectionState.Disconnected,
        SdkGeneration: not null,
        ReadyForMp: false,
    };
    Require(startedDisconnected, "The SDK did not start disconnected.");
    var generation = started.SdkGeneration;

    // 1. The caller stops waiting after about a second, which surfaces as .NET
    //    cancellation. The client invents no disposition and does not retry.
    using (var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
    {
        await RequireThrowsAsync<OperationCanceledException>(
            () => briosa.ConnectToSpatialAnalyzerAsync(deadline.Token));
    }

    // 2. The accepted Connect still runs, so a second one is a transition in progress.
    var inProgress = await RequireThrowsAsync<BriosaSpatialAnalyzerSdkException>(
        () => briosa.ConnectToSpatialAnalyzerAsync());
    Require(inProgress.Kind == SpatialAnalyzerSdkLifecycleFailureKind.StateConflict &&
            inProgress.DiagnosticCode == "sdk-lifecycle-transition-in-progress" &&
            inProgress.RecoveryGuidance == LifecycleRecoveryGuidance.RefreshState,
        "The concurrent Connect was not a transition in progress.");
    Require(inProgress.State.SdkGeneration == generation &&
            inProgress.State.SdkState is SpatialAnalyzerSdkState.Connecting or SpatialAnalyzerSdkState.Verifying,
        "The transition in progress reported another generation or phase.");

    // 3. Refreshing SDK state shows the same generation ready, with no incident.
    var ready = await WaitForSdkAsync(
        briosa,
        state =>
        {
            Require(state.SdkGeneration == generation, "The detached Connect changed the SDK generation.");
            Require(state.SdkState is SpatialAnalyzerSdkState.Connecting or
                    SpatialAnalyzerSdkState.Verifying or SpatialAnalyzerSdkState.Ready,
                "The detached Connect ended unready.");
            return state.SdkState == SpatialAnalyzerSdkState.Ready;
        },
        TimeSpan.FromSeconds(20));
    var readyOnExternal = ready is
    {
        ReadyForMp: true,
        ConnectionState: SpatialAnalyzerConnectionState.Connected,
        ExecutionReadinessState: SpatialAnalyzerExecutionReadinessState.ExecutionReady,
        RecoveryState: SpatialAnalyzerSdkRecoveryState.NotRequired,
        LastIncident: null,
    } && ready.ApplicationGeneration == application.ApplicationGeneration;
    Require(readyOnExternal, "The detached Connect did not finish ready on the external application.");

    // 4. MP work runs on that generation once discovery is refreshed too.
    Require((await briosa.GetServerSnapshotAsync()).ReadyForMp,
        "Discovery did not report MP readiness after the detached Connect.");
    _ = await briosa.GetWorkingDirectoryAsync();

    // 5. A clean stop leaves the external application running.
    var stopped = await briosa.StopSpatialAnalyzerSdkAsync();
    var after = await briosa.GetSpatialAnalyzerStateAsync();
    Require(stopped.SdkState == SpatialAnalyzerSdkState.Stopped && !stopped.ReadyForMp &&
            after.ApplicationState == SpatialAnalyzerApplicationState.Running &&
            after.Ownership == SpatialAnalyzerOwnership.External &&
            after.ApplicationGeneration == application.ApplicationGeneration,
        "The SDK stop did not leave the external application running.");
}

static async Task AssertOperationFailureAsync(
    BriosaClient briosa,
    OperationFailureKind expectedKind,
    ExecutionDisposition expectedDisposition)
{
    var exception = await RequireThrowsAsync<BriosaOperationException>(
        () => briosa.GetWorkingDirectoryAsync());
    Require(exception.Kind == expectedKind, "The operation failure kind was not preserved.");
    Require(exception.ExecutionDisposition == expectedDisposition,
        "The operation execution disposition was not preserved.");
    Require(exception.OperationId == "file_operations.get_working_directory",
        "The operation identity was not preserved.");
}

static async Task AssertDeadlineAsync(BriosaClient briosa)
{
    var exception = await RequireThrowsAsync<BriosaTransportException>(
        () => briosa.GetWorkingDirectoryAsync());
    Require(exception.DiagnosticCode == "grpc-DeadlineExceeded",
        "The caller deadline did not remain a transport outcome.");
    await Task.Delay(400);
    try
    {
        _ = await briosa.GetWorkingDirectoryAsync();
    }
    catch (BriosaTransportException recoveryException)
        when (recoveryException.DiagnosticCode == "grpc-DeadlineExceeded")
    {
        // If the initial deadline expired before worker dispatch, this call consumes
        // the one scripted delay. A final caller-initiated read verifies recovery.
        await Task.Delay(400);
        _ = await briosa.GetWorkingDirectoryAsync();
    }
}

static async Task AssertCancellationAsync(BriosaClient briosa)
{
    using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
    await RequireThrowsAsync<OperationCanceledException>(
        () => briosa.GetWorkingDirectoryAsync(cancellationToken: cancellation.Token));
    _ = await briosa.GetWorkingDirectoryAsync();
}

static async Task AssertWatchdogRecoveryAsync(BriosaClient briosa)
{
    var exception = await RequireThrowsAsync<BriosaOperationException>(
        () => briosa.GetWorkingDirectoryAsync());
    Require(exception.Kind == OperationFailureKind.WorkerWatchdogTimeout,
        "The watchdog failure kind was not preserved.");
    Require(exception.ExecutionDisposition == ExecutionDisposition.StartedOutcomeUnknown,
        "The watchdog outcome was not preserved as ambiguous.");
    Require(exception.RecoveryGuidance == RecoveryGuidance.WorkerReplacement,
        "The watchdog recovery guidance was not preserved.");
    Require(exception.ReplayGuidance == ReplayGuidance.MayReplay &&
        exception.ReplaySafety == ReplaySafety.Safe,
        "The watchdog replay guidance was not preserved.");

    var faulted = await WaitForSdkAsync(
        briosa,
        state => state.SdkState == SpatialAnalyzerSdkState.Faulted);
    Require(faulted.LastIncident?.TerminationKind ==
        SpatialAnalyzerSdkTerminationKind.WatchdogTerminated,
        "The watchdog incident was not retained.");
    await RecoverAndReconnectAsync(briosa);
}

static async Task AssertSdkLossRecoveryAsync(BriosaClient briosa)
{
    var signalPath = Environment.GetEnvironmentVariable(
        "BRIOSA_CONFORMANCE_WORKER_EXIT_SIGNAL_PATH");
    Require(!string.IsNullOrWhiteSpace(signalPath),
        "The shared host did not provide the worker-loss signal path.");
    await File.WriteAllTextAsync(signalPath!, "exit");
    var faulted = await WaitForSdkAsync(
        briosa,
        state => state.SdkState == SpatialAnalyzerSdkState.Faulted);
    Require(faulted.LastIncident?.TerminationKind ==
        SpatialAnalyzerSdkTerminationKind.WorkerProcessExited,
        "Unexpected worker loss was not diagnosed.");
    File.Delete(signalPath!);
    await RecoverAndReconnectAsync(briosa);
}

static async Task RecoverAndReconnectAsync(BriosaClient briosa)
{
    var recovered = await briosa.RecoverSpatialAnalyzerSdkAsync();
    Require(recovered.SdkState == SpatialAnalyzerSdkState.Running &&
        recovered.ConnectionState == SpatialAnalyzerConnectionState.Disconnected,
        "SDK replacement did not create a disconnected generation.");
    var connected = await briosa.ConnectToSpatialAnalyzerAsync();
    Require(connected.ReadyForMp, "The replacement SDK did not restore readiness.");
    _ = await briosa.GetWorkingDirectoryAsync();
}

static async Task<SpatialAnalyzerSdkLifecycleState> WaitForSdkAsync(
    BriosaClient briosa,
    Func<SpatialAnalyzerSdkLifecycleState, bool> predicate,
    TimeSpan? limit = null)
{
    using var timeout = new CancellationTokenSource(limit ?? TimeSpan.FromSeconds(10));
    while (true)
    {
        timeout.Token.ThrowIfCancellationRequested();
        var state = await briosa.GetSpatialAnalyzerSdkStateAsync(timeout.Token);
        if (predicate(state))
        {
            return state;
        }

        await Task.Delay(50, timeout.Token);
    }
}

static async Task CleanupApplicationAsync(BriosaClient briosa)
{
    var sdk = await briosa.GetSpatialAnalyzerSdkStateAsync();
    if (sdk.SdkGeneration is not null && sdk.SdkState != SpatialAnalyzerSdkState.Stopped)
    {
        await briosa.StopSpatialAnalyzerSdkAsync();
    }

    var application = await briosa.GetSpatialAnalyzerStateAsync();
    if (application.Ownership == SpatialAnalyzerOwnership.ServerLaunched &&
        application.ApplicationGeneration is not null &&
        application.ApplicationState is not SpatialAnalyzerApplicationState.Exited and
            not SpatialAnalyzerApplicationState.NotRunning)
    {
        await briosa.CloseOwnedSpatialAnalyzerAsync();
    }
}

static async Task<TException> RequireThrowsAsync<TException>(Func<Task> action)
    where TException : Exception
{
    try
    {
        await action();
    }
    catch (TException exception)
    {
        return exception;
    }

    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}

static void Require(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

static FixtureArguments ParseArguments(string[] values)
{
    string? scenario = null;
    string? contractPath = null;
    for (var index = 0; index < values.Length; index++)
    {
        switch (values[index])
        {
            case "--scenario" when index + 1 < values.Length:
                scenario = values[++index];
                break;
            case "--contract" when index + 1 < values.Length:
                contractPath = values[++index];
                break;
            default:
                throw new ArgumentException($"Unknown fixture argument '{values[index]}'.");
        }
    }

    return new FixtureArguments(
        scenario ?? throw new ArgumentException("--scenario is required."),
        contractPath ?? throw new ArgumentException("--contract is required."));
}

internal sealed record FixtureArguments(string Scenario, string ContractPath);
