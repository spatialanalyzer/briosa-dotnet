using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Briosa.Client;
using Briosa.Core.V1Alpha1;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Grpc.Core;
using Grpc.Net.Client;
using TargetProtocol = Briosa.Sa.V2026_1_0529_7.V1Alpha1;

return await ConformanceProgram.RunAsync(args).ConfigureAwait(false);

internal static class ConformanceProgram
{
    private const string OperationMethod =
        "/briosa.sa.v2026_1_0529_7.v1alpha1.FileOperations/GetWorkingDirectory";

    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "The external probe emits one stable value-free failure report.")]
    public static async Task<int> RunAsync(string[] arguments)
    {
        try
        {
            var errorFixture = GetArgument(arguments, "--error-fixture");
            if (errorFixture is not null)
            {
                VerifyErrorFixtures(errorFixture);
                await WriteReportAsync("typed-errors").ConfigureAwait(false);
                return 0;
            }

            var address = new Uri(
                GetRequiredArgument(arguments, "--address"),
                UriKind.Absolute);
            var fixturePath = GetRequiredArgument(arguments, "--fixture");
            var scenarioId = GetRequiredArgument(arguments, "--scenario");
            await RunLiveScenario(address, fixturePath, scenarioId)
                .ConfigureAwait(false);
            await WriteReportAsync(scenarioId).ConfigureAwait(false);
            return 0;
        }
        catch (Exception exception)
        {
            await Console.Error.WriteLineAsync(
                    JsonSerializer.Serialize(new
                    {
                        schema_version = 1,
                        success = false,
                        failure = exception.GetType().Name
                    }))
                .ConfigureAwait(false);
            return 1;
        }
    }

    private static async Task RunLiveScenario(
        Uri address,
        string fixturePath,
        string scenarioId)
    {
        var fixtureText = await File.ReadAllTextAsync(fixturePath)
            .ConfigureAwait(false);
        using var fixtureDocument = JsonDocument.Parse(fixtureText);
        var root = fixtureDocument.RootElement;
        Require(
            root.GetProperty("fixture_set_id").GetString() ==
                "briosa.client.live.v1",
            "fixture-identity");
        var scenario = root.GetProperty("scenarios")
            .EnumerateArray()
            .Single(item => item.GetProperty("id").GetString() == scenarioId);
        var expected = scenario.GetProperty("expected");

        using var client = new BriosaClient(new BriosaClientOptions
        {
            Address = address,
            DefaultTimeout = TimeSpan.FromSeconds(15)
        });
        var snapshot = await client.GetServerSnapshotAsync().ConfigureAwait(false);
        var advertised = snapshot.Capabilities.Operations.Any(
            operation => operation.FullyQualifiedMethod == OperationMethod);
        Require(
            snapshot.ReadyForMp == expected.GetProperty("ready_for_mp").GetBoolean(),
            "readiness-mismatch");
        Require(
            advertised == expected.GetProperty("operation_advertised").GetBoolean(),
            "capability-mismatch");

        var operationSucceeded = false;
        var recoverySucceeded = false;
        var typedErrorObserved = false;
        string? failureKind = null;
        var status = StatusCode.OK;

        if (scenarioId == "unsupported-version")
        {
            status = await RequireUnsupportedVersion(address).ConfigureAwait(false);
            failureKind = "OPERATION_FAILURE_KIND_UNSUPPORTED";
        }
        else
        {
            try
            {
                if (scenarioId == "deadline")
                {
                    _ = await client.GetWorkingDirectoryAsync(
                            TimeSpan.FromMilliseconds(50))
                        .ConfigureAwait(false);
                }
                else if (scenarioId == "cancellation")
                {
                    using var cancellation = new CancellationTokenSource(
                        TimeSpan.FromMilliseconds(50));
                    _ = await client.GetWorkingDirectoryAsync(
                            cancellationToken: cancellation.Token)
                        .ConfigureAwait(false);
                }
                else
                {
                    var result = await client.GetWorkingDirectoryAsync()
                        .ConfigureAwait(false);
                    ValidateSuccessfulResult(result);
                    operationSucceeded = true;
                }
            }
            catch (BriosaCallException exception)
            {
                status = exception.StatusCode;
                typedErrorObserved = exception.OperationError is not null;
                failureKind = exception.OperationError is null
                    ? null
                    : EnumName(
                        OperationError.Descriptor.FindFieldByNumber(2).EnumType,
                        (int)exception.OperationError.Kind);
            }

            if (scenarioId is "deadline" or "cancellation" or "watchdog-recovery")
            {
                var recovery = await client.GetWorkingDirectoryAsync()
                    .ConfigureAwait(false);
                ValidateSuccessfulResult(recovery);
                recoverySucceeded = true;
            }
        }

        Require(
            CanonicalStatus(status) ==
                expected.GetProperty("grpc_status").GetString(),
            "grpc-status-mismatch");
        Require(
            operationSucceeded ==
                expected.GetProperty("operation_succeeded").GetBoolean(),
            "operation-outcome-mismatch");
        Require(
            recoverySucceeded ==
                expected.GetProperty("recovery_succeeded").GetBoolean(),
            "recovery-outcome-mismatch");
        Require(
            typedErrorObserved ==
                expected.GetProperty("typed_error_required").GetBoolean(),
            "typed-error-presence-mismatch");
        var expectedFailureKinds = expected.GetProperty("failure_kinds")
            .EnumerateArray()
            .Select(item => item.GetString())
            .ToArray();
        Require(
            failureKind is null
                ? expectedFailureKinds.Length == 0
                : expectedFailureKinds.Contains(
                    failureKind,
                    StringComparer.Ordinal),
            "failure-kind-mismatch");
    }

    private static void VerifyErrorFixtures(string fixturePath)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(fixturePath));
        var root = document.RootElement;
        Require(
            root.GetProperty("fixture_set_id").GetString() ==
                "briosa.client.operation-errors.v1",
            "error-fixture-identity");

        foreach (var item in root.GetProperty("cases").EnumerateArray())
        {
            var errorJson = item.GetProperty("operation_error");
            var error = new OperationError
            {
                OperationId = RequiredString(errorJson, "operation_id"),
                Kind = (OperationFailureKind)EnumNumber(
                    OperationError.Descriptor.FindFieldByNumber(2).EnumType,
                    RequiredString(errorJson, "kind")),
                DiagnosticCode = RequiredString(errorJson, "diagnostic_code"),
                ExecutionDisposition = (ExecutionDisposition)EnumNumber(
                    OperationError.Descriptor.FindFieldByNumber(7).EnumType,
                    RequiredString(errorJson, "execution_disposition")),
                RecoveryGuidance = (RecoveryGuidance)EnumNumber(
                    OperationError.Descriptor.FindFieldByNumber(8).EnumType,
                    RequiredString(errorJson, "recovery_guidance")),
                ReplayGuidance = (ReplayGuidance)EnumNumber(
                    OperationError.Descriptor.FindFieldByNumber(9).EnumType,
                    RequiredString(errorJson, "replay_guidance")),
                ReplaySafety = (ReplaySafety)EnumNumber(
                    OperationError.Descriptor.FindFieldByNumber(10).EnumType,
                    RequiredString(errorJson, "replay_safety"))
            };
            var status = ParseStatus(RequiredString(item, "grpc_status"));
            var trailers = new Metadata
            {
                { "briosa-operation-error-bin", error.ToByteArray() }
            };
            var mapped = BriosaCallException.FromRpcException(
                new RpcException(new Status(status, "not parsed"), trailers));
            var behavior = item.GetProperty("client_behavior");

            Require(mapped.StatusCode == status, "offline-status-mismatch");
            Require(mapped.OperationError?.Equals(error) == true, "offline-error-mismatch");
            Require(
                mapped.CompletionUnknown ==
                    (error.ExecutionDisposition ==
                        ExecutionDisposition.StartedOutcomeUnknown),
                "offline-disposition-mismatch");
            Require(
                mapped.ReconciliationRequired ==
                    behavior.GetProperty("reconciliation_required").GetBoolean(),
                "offline-reconciliation-mismatch");
            Require(
                !behavior.GetProperty("automatic_replay").GetBoolean(),
                "automatic-replay-prohibited");
        }
    }

    private static async Task<StatusCode> RequireUnsupportedVersion(Uri address)
    {
        using var channel = GrpcChannel.ForAddress(address);
        var marshaller = Marshallers.Create(
            static (byte[] value) => value,
            static value => value);
        var method = new Method<byte[], byte[]>(
            MethodType.Unary,
            "briosa.sa.v1900_1_0000_0.v1alpha1.FileOperations",
            "GetWorkingDirectory",
            marshaller,
            marshaller);
        using var call = channel.CreateCallInvoker().AsyncUnaryCall(
            method,
            host: null,
            new CallOptions(deadline: DateTime.UtcNow.AddSeconds(15)),
            []);
        try
        {
            _ = await call.ResponseAsync.ConfigureAwait(false);
            throw new InvalidOperationException("Unsupported method succeeded.");
        }
        catch (RpcException exception)
        {
            return exception.StatusCode;
        }
    }

    private static void ValidateSuccessfulResult(
        TargetProtocol.GetWorkingDirectoryResult result)
    {
        Require(result.HasDirectory, "directory-presence-missing");
        var execution = result.Execution ??
            throw new InvalidOperationException("mp-execution-missing");
        Require(
            execution.State == MpExecutionState.Succeeded,
            "mp-execution-not-successful");
        Require(
            execution.OutputRetrievals.Count == 1 &&
            execution.OutputRetrievals[0].State ==
                OutputRetrievalState.Retrieved,
            "output-retrieval-not-successful");
    }

    private static string EnumName(EnumDescriptor descriptor, int number) =>
        descriptor.FindValueByNumber(number)?.Name ??
        throw new InvalidDataException("Unknown protobuf enum number.");

    private static int EnumNumber(EnumDescriptor descriptor, string name) =>
        descriptor.FindValueByName(name)?.Number ??
        throw new InvalidDataException($"Unknown protobuf enum name '{name}'.");

    private static string RequiredString(JsonElement element, string property) =>
        element.GetProperty(property).GetString() ??
        throw new InvalidDataException($"'{property}' must be a string.");

    private static string CanonicalStatus(StatusCode status) =>
        status switch
        {
            StatusCode.OK => "OK",
            StatusCode.Cancelled => "CANCELLED",
            StatusCode.DeadlineExceeded => "DEADLINE_EXCEEDED",
            StatusCode.FailedPrecondition => "FAILED_PRECONDITION",
            StatusCode.PermissionDenied => "PERMISSION_DENIED",
            StatusCode.Unavailable => "UNAVAILABLE",
            StatusCode.DataLoss => "DATA_LOSS",
            StatusCode.Unimplemented => "UNIMPLEMENTED",
            _ => throw new InvalidDataException("Unsupported conformance status.")
        };

    private static StatusCode ParseStatus(string value) =>
        value switch
        {
            "OK" => StatusCode.OK,
            "CANCELLED" => StatusCode.Cancelled,
            "DEADLINE_EXCEEDED" => StatusCode.DeadlineExceeded,
            "FAILED_PRECONDITION" => StatusCode.FailedPrecondition,
            "PERMISSION_DENIED" => StatusCode.PermissionDenied,
            "UNAVAILABLE" => StatusCode.Unavailable,
            "DATA_LOSS" => StatusCode.DataLoss,
            "UNIMPLEMENTED" => StatusCode.Unimplemented,
            _ => throw new InvalidDataException("Unsupported conformance status.")
        };

    private static string GetRequiredArgument(string[] arguments, string name) =>
        GetArgument(arguments, name) ??
        throw new ArgumentException($"Missing required argument '{name}'.");

    private static string? GetArgument(string[] arguments, string name)
    {
        var index = Array.IndexOf(arguments, name);
        return index >= 0 && index + 1 < arguments.Length
            ? arguments[index + 1]
            : null;
    }

    private static void Require(bool condition, string failure)
    {
        if (!condition)
        {
            throw new InvalidOperationException(failure);
        }
    }

    private static Task WriteReportAsync(string scenario) =>
        Console.Out.WriteLineAsync(JsonSerializer.Serialize(new
        {
            schema_version = 1,
            success = true,
            scenario
        }));
}
