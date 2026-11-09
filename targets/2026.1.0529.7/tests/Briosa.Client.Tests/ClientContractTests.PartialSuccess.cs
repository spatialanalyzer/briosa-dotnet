#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using System.Reflection;
using Google.Protobuf.Reflection;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

// Partial success at contract major 3 (spatialanalyzer/briosa#360): MP code 4 is
// a success that returns its outputs, and each call can observe how its MP
// command completed without a different return type.
public sealed partial class ClientContractTests
{
    [Fact]
    public async Task PartialSuccessReturnsTheOutputsAndReportsItsCompletion()
    {
        var transport = new FakeTransport
        {
            OperationResponse = new Transport.GetActiveCollectionNameResult
            {
                CurrentlyActiveCollectionName = "Inspection",
                Execution = Execution(Transport.MpExecutionState.PartialSuccess, 4),
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var completions = new List<MpCompletion>();

        var name = await client.GetActiveCollectionNameAsync(onMpCompletion: completions.Add);

        Assert.Equal("Inspection", name);
        var completion = Assert.Single(completions);
        Assert.Equal(MpExecutionState.PartialSuccess, completion.State);
        Assert.Equal(4, completion.MpResultCode);
        Assert.Single(transport.Calls, call => call.StartsWith("operation:", StringComparison.Ordinal));
    }

    [Fact]
    public async Task EachCallReportsOnlyItsOwnCompletion()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var first = new List<MpCompletion>();
        var second = new List<MpCompletion>();

        transport.OperationResponse = new Transport.SetPointNotesResult
        {
            Execution = Execution(Transport.MpExecutionState.PartialSuccess, 4),
        };
        await client.SetPointNotesAsync(SamplePoint(), ["first"], onMpCompletion: first.Add);
        transport.OperationResponse = new Transport.SetPointNotesResult
        {
            Execution = Execution(Transport.MpExecutionState.Succeeded, 2),
        };
        await client.SetPointNotesAsync(SamplePoint(), ["second"], onMpCompletion: second.Add);
        await client.SetPointNotesAsync(SamplePoint(), ["third"]);

        Assert.Equal(MpExecutionState.PartialSuccess, Assert.Single(first).State);
        var succeeded = Assert.Single(second);
        Assert.Equal(MpExecutionState.Succeeded, succeeded.State);
        Assert.Equal(2, succeeded.MpResultCode);
    }

    [Fact]
    public async Task GetWorkingDirectoryReportsItsMpCompletion()
    {
        var transport = new FakeTransport
        {
            WorkingDirectoryExecution = Execution(Transport.MpExecutionState.PartialSuccess, 4),
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        MpCompletion? completion = null;

        var directory = await client.GetWorkingDirectoryAsync(onMpCompletion: value => completion = value);

        Assert.Equal("C:\\Working", directory);
        Assert.Equal(MpExecutionState.PartialSuccess, completion?.State);
        Assert.Equal(4, completion?.MpResultCode);
    }

    // C1: every success is checked, whether or not the caller observes it.
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SuccessWithoutMpExecutionDetailsIsAProtocolError(bool observed)
    {
        var transport = new FakeTransport
        {
            FillMissingExecution = false,
            OperationResponse = new Transport.GetActiveCollectionNameResult
            {
                CurrentlyActiveCollectionName = "Inspection",
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var completions = new List<MpCompletion>();

        var failure = await Assert.ThrowsAsync<BriosaProtocolException>(
            () => observed
                ? client.GetActiveCollectionNameAsync(onMpCompletion: completions.Add)
                : client.GetActiveCollectionNameAsync());

        Assert.Equal("mp-execution-missing", failure.DiagnosticCode);
        Assert.Empty(completions);
    }

    // C1: the worker reports success only after it reads MP code 2 (Succeeded)
    // or 4 (PartialSuccess), and the server copies that code. A missing code,
    // or a code that disagrees with the state, is a protocol-contract failure.
    [Theory]
    [InlineData(Transport.MpExecutionState.Succeeded, null, true)]
    [InlineData(Transport.MpExecutionState.Succeeded, null, false)]
    [InlineData(Transport.MpExecutionState.PartialSuccess, null, true)]
    [InlineData(Transport.MpExecutionState.PartialSuccess, null, false)]
    [InlineData(Transport.MpExecutionState.Succeeded, 4, true)]
    [InlineData(Transport.MpExecutionState.Succeeded, 3, false)]
    [InlineData(Transport.MpExecutionState.PartialSuccess, 2, true)]
    [InlineData(Transport.MpExecutionState.PartialSuccess, 0, false)]
    public async Task SuccessWhoseMpResultCodeIsMissingOrDisagreesWithItsStateIsAProtocolError(
        Transport.MpExecutionState state,
        int? code,
        bool observed)
    {
        var execution = new Transport.MpExecutionDetails { State = state };
        if (code is { } value)
        {
            execution.MpResultCode = value;
        }

        var transport = new FakeTransport
        {
            OperationResponse = new Transport.GetActiveCollectionNameResult
            {
                CurrentlyActiveCollectionName = "Inspection",
                Execution = execution,
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var completions = new List<MpCompletion>();

        var failure = await Assert.ThrowsAsync<BriosaProtocolException>(
            () => observed
                ? client.GetActiveCollectionNameAsync(onMpCompletion: completions.Add)
                : client.GetActiveCollectionNameAsync());

        Assert.Equal("mp-execution-result-code", failure.DiagnosticCode);
        Assert.Empty(completions);
    }

    [Fact]
    public async Task GetWorkingDirectoryRequiresTheMpResultCode()
    {
        var transport = new FakeTransport
        {
            WorkingDirectoryExecution = new Transport.MpExecutionDetails
            {
                State = Transport.MpExecutionState.Succeeded,
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();

        var failure = await Assert.ThrowsAsync<BriosaProtocolException>(
            () => client.GetWorkingDirectoryAsync());

        Assert.Equal("mp-execution-result-code", failure.DiagnosticCode);
    }

    // C1: the observer runs after the execution check and before the outputs are
    // mapped, so it reports the completion even when output mapping then fails.
    [Fact]
    public async Task ObserverReceivesTheCompletionEvenWhenOutputMappingFails()
    {
        var transport = new FakeTransport
        {
            OperationResponse = new Transport.GetVectorVariableResult
            {
                Execution = Execution(Transport.MpExecutionState.PartialSuccess, 4),
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var completions = new List<MpCompletion>();

        var failure = await Assert.ThrowsAsync<BriosaProtocolException>(
            () => client.GetVectorVariableAsync("Offset", onMpCompletion: completions.Add));

        Assert.Equal("required-output-missing:value", failure.DiagnosticCode);
        var completion = Assert.Single(completions);
        Assert.Equal(MpExecutionState.PartialSuccess, completion.State);
        Assert.Equal(4, completion.MpResultCode);
    }

    // C1: a completion always carries its MP code, so the public type says so.
    [Fact]
    public void MpCompletionAlwaysCarriesItsMpResultCode()
    {
        var property = typeof(MpCompletion).GetProperty(nameof(MpCompletion.MpResultCode))!;

        Assert.Equal(typeof(int), property.PropertyType);
        Assert.True(property.IsDefined(typeof(System.Runtime.CompilerServices.RequiredMemberAttribute), inherit: false));
    }

    [Theory]
    [InlineData(Transport.MpExecutionState.Unspecified)]
    [InlineData(Transport.MpExecutionState.Failed)]
    [InlineData(Transport.MpExecutionState.ResultUnavailable)]
    [InlineData((Transport.MpExecutionState)99)]
    public async Task SuccessWithAnUnsuccessfulMpStateIsAProtocolError(Transport.MpExecutionState state)
    {
        var transport = new FakeTransport
        {
            OperationResponse = new Transport.GetActiveCollectionNameResult
            {
                CurrentlyActiveCollectionName = "Inspection",
                Execution = Execution(state, 3),
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();

        var failure = await Assert.ThrowsAsync<BriosaProtocolException>(
            () => client.GetActiveCollectionNameAsync());

        Assert.Equal("mp-execution-state", failure.DiagnosticCode);
    }

    // Every RPC of every operation service has a facade, and each one takes the
    // optional observer as a call control just before its CancellationToken.
    [Fact]
    public void EveryOperationTakesAnMpCompletionObserverBeforeItsCancellationToken()
    {
        var methods = FacadeTypes()
            .SelectMany(type => type.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .ToLookup(method => method.Name, StringComparer.Ordinal);
        var failures = new List<string>();
        var rpcs = OperationServices().SelectMany(service => service.Methods).ToArray();

        foreach (var rpc in rpcs)
        {
            var facades = methods[rpc.Name + "Async"].ToArray();
            if (facades.Length == 0)
            {
                failures.Add($"{rpc.FullName}: no facade method");
            }

            foreach (var facade in facades)
            {
                var parameters = facade.GetParameters();
                var observer = parameters.Length > 1 ? parameters[^2] : null;
                if (parameters[^1].ParameterType != typeof(CancellationToken) ||
                    observer?.Name != "onMpCompletion" ||
                    observer.ParameterType != typeof(Action<MpCompletion>) ||
                    !observer.HasDefaultValue || observer.DefaultValue is not null)
                {
                    failures.Add($"{facade.DeclaringType!.Name}.{facade.Name}: no optional onMpCompletion before the token");
                }
            }
        }

        Assert.True(rpcs.Length > 900, $"Only {rpcs.Length} operation RPCs were found.");
        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    // The protocol's MP operation services: everything except discovery and the
    // two lifecycle services.
    private static IEnumerable<ServiceDescriptor> OperationServices() =>
        typeof(BriosaClient).Assembly.GetTypes()
            .Where(type => type.Namespace == "Briosa.Client.Transport" && type.IsAbstract && type.IsSealed)
            .Select(type => type.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static))
            .Where(property => property?.PropertyType == typeof(ServiceDescriptor))
            .Select(property => (ServiceDescriptor)property!.GetValue(null)!)
            .Where(service => service.Name is not (
                "DiscoveryService" or "SpatialAnalyzerLifecycle" or "SpatialAnalyzerSdkLifecycle"))
            .OrderBy(service => service.FullName, StringComparer.Ordinal);

    private static Transport.MpExecutionDetails Execution(Transport.MpExecutionState state, int code) =>
        new() { State = state, MpResultCode = code };

    private static PointName SamplePoint() =>
        new() { CollectionName = "Inspection", GroupName = "Measured", TargetName = "P1" };
}
