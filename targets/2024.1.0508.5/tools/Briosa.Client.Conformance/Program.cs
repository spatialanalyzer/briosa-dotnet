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
            "control-plane-only" => new BriosaStartOptions
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
    var exception = await RequireThrowsAsync<BriosaOperationException>(
        () => briosa.GetWorkingDirectoryAsync());
    Require(exception.Kind == OperationFailureKind.PolicyDenied,
        "Policy denial did not map to the typed public error.");
    Require(exception.ExecutionDisposition == ExecutionDisposition.NotStarted,
        "Policy denial reported an invalid execution disposition.");
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
        () => briosa.GetWorkingDirectoryAsync(cancellation.Token));
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
    Func<SpatialAnalyzerSdkLifecycleState, bool> predicate)
{
    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
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
