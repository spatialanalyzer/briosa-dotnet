#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Google.Protobuf;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

public sealed partial class ClientContractTests
{
    [Fact]
    public async Task LocateInstrumentsUsmnDefaultsSendNoExcludedGroups()
    {
        var transport = new FakeTransport
        {
            OperationResponse = new Transport.LocateInstrumentsUsmnResult
            {
                RmsError = 0.1,
                MaximumError = 0.2,
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        CollectionInstrumentId[] instruments =
        [
            new() { CollectionName = "Inspection", InstrumentId = 1 },
            new() { CollectionName = "Inspection", InstrumentId = 2 },
        ];
        var outputGroup = new CollectionObjectName
        {
            CollectionName = "Inspection",
            ObjectName = "USMN Results",
            ObjectType = ObjectType.PointGroup,
        };

        var fit = await client.InstrumentOperations.LocateInstrumentsUsmnAsync(
            instruments,
            outputGroup,
            ShowUsmnDialog.No);

        Assert.Equal(0.1, fit.RmsError);
        var request = Assert.IsType<Transport.LocateInstrumentsUsmnRequest>(
            transport.LastOperationRequest);
        Assert.Empty(request.ExcludedGroups);
        Assert.Null(request.NominalsGroup);

        await client.InstrumentOperations.LocateInstrumentsUsmnAsync(
            instruments,
            outputGroup,
            ShowUsmnDialog.No,
            excludedGroups: [outputGroup with { ObjectName = "Reference" }]);

        request = Assert.IsType<Transport.LocateInstrumentsUsmnRequest>(
            transport.LastOperationRequest);
        Assert.Equal("Reference", Assert.Single(request.ExcludedGroups).ObjectName);
    }

    [Fact]
    public async Task EveryOperationArgumentThatDefaultsToNullCanBeOmitted()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var invoked = new List<string>();
        var failures = new List<string>();

        foreach (var (target, method) in OperationMethods(client))
        {
            if (!method.GetParameters().Any(DefaultsToNull))
            {
                continue;
            }

            var (request, failure) = await SendAsync(transport, target, method);
            invoked.Add(method.Name);
            if (request is null)
            {
                failures.Add(
                    $"{method.DeclaringType!.Name}.{method.Name}: {failure?.GetType().Name} {failure?.Message}");
            }
        }

        Assert.Contains(nameof(BriosaInstrumentOperations.LocateInstrumentsUsmnAsync), invoked);
        if (failures.Count > 0)
        {
            Assert.Fail(string.Join(Environment.NewLine, failures));
        }
    }

    [Fact]
    public async Task EveryOptionalListIsSentOnlyWhenSupplied()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var checkedLists = new List<string>();
        var failures = new List<string>();

        foreach (var (target, method) in OperationMethods(client))
        {
            var lists = method.GetParameters()
                .Where(IsOptionalList)
                .Select(parameter => parameter.Name!)
                .ToArray();
            if (lists.Length == 0)
            {
                continue;
            }

            // Supply no list, then each list alone, so a list that is dropped or sent
            // in another list's field is caught.
            foreach (var supplied in lists.Cast<string?>().Prepend(null))
            {
                var (request, failure) = await SendAsync(transport, target, method, supplied);
                if (request is null)
                {
                    failures.Add($"{method.Name} with {supplied ?? "no list"}: {failure?.Message}");
                    continue;
                }

                foreach (var list in lists)
                {
                    var sent = ListItems(request, list);
                    var isExpected = list == supplied
                        ? sent.Length == 1 && sent[0].Contains("Sample", StringComparison.Ordinal)
                        : sent.Length == 0;
                    if (!isExpected)
                    {
                        failures.Add(
                            $"{method.Name} with {supplied ?? "no list"}: {list} sent [{string.Join(", ", sent)}]");
                    }
                }
            }

            checkedLists.AddRange(lists.Select(list => $"{method.Name}.{list}"));
        }

        Assert.Contains($"{nameof(BriosaInstrumentOperations.QuickAlignAsync)}.nominalPoints", checkedLists);
        // C3: the server requires every list it receives in a list wrapper message,
        // so none of those lists is optional any more.
        Assert.DoesNotContain($"{nameof(BriosaClient.CreatePointsToObjectsMapAsync)}.points", checkedLists);
        if (failures.Count > 0)
        {
            Assert.Fail(string.Join(Environment.NewLine, failures));
        }
    }

    // Calls an operation with only its required arguments and, if named, one optional
    // argument, so every other default is used. A failure after dispatch comes from
    // mapping the fake's empty response, which these tests do not cover. Only a call
    // that never reached the transport failed while building its request, so only
    // that call returns a failure in place of the request.
    private static async Task<(IMessage? Request, Exception? Failure)> SendAsync(
        FakeTransport transport,
        object target,
        MethodInfo method,
        string? suppliedArgument = null)
    {
        var arguments = method.GetParameters()
            .Select(parameter => parameter.HasDefaultValue && parameter.Name != suppliedArgument
                ? Type.Missing
                : SampleArgument(parameter.ParameterType))
            .ToArray();
        var callsBefore = transport.Calls.Count;
        var exception = await Record.ExceptionAsync(
            () => (Task)method.Invoke(target, arguments)!).ConfigureAwait(false);
        return transport.Calls.Count > callsBefore
            ? (transport.LastOperationRequest, null)
            : (null, (exception as TargetInvocationException)?.InnerException ?? exception);
    }

    // What a request carries for a list argument, whether its field is repeated or a
    // list wrapper message, which stays unset when the argument is omitted.
    private static string[] ListItems(IMessage request, string argument)
    {
        var field = request.Descriptor.Fields.InDeclarationOrder()
            .Single(candidate => candidate.JsonName == argument);
        var value = field.Accessor.GetValue(request);
        if (value is IMessage wrapper)
        {
            value = wrapper.Descriptor.FindFieldByName("values").Accessor.GetValue(wrapper);
        }

        return value is IEnumerable items
            ? items.Cast<object>().Select(item => item.ToString()!).ToArray()
            : [];
    }

    private static (object Target, MethodInfo Method)[] OperationMethods(BriosaClient client)
    {
        var operationNames = new[]
        {
            Transport.CloudAndMeshOperations.Descriptor,
            Transport.ConstructionOperations.Descriptor,
            Transport.GdtOperations.Descriptor,
            Transport.InstrumentOperations.Descriptor,
            Transport.RelationshipOperations.Descriptor,
            Transport.RobotCalibrationApplianceNodeOperations.Descriptor,
            Transport.RobotOperations.Descriptor,
        }
            .SelectMany(service => service.Methods)
            .Select(method => method.Name)
            .Concat(BriosaClient.WaveAOperationNames)
            .Select(operation => operation + "Async")
            .ToHashSet(StringComparer.Ordinal);
        object[] targets =
        [
            client,
            client.ConstructionOperations,
            client.GdtOperations,
            client.InstrumentOperations,
            client.RobotCalibrationApplianceNodeOperations,
            client.RobotOperations,
        ];
        return targets
            .SelectMany(target => target.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(method => operationNames.Contains(method.Name))
                .Select(method => (target, method)))
            .ToArray();
    }

    // An MP argument that defaults to null. The onMpCompletion observer is a call
    // control, not an MP argument.
    private static bool DefaultsToNull(ParameterInfo parameter) =>
        parameter.ParameterType != typeof(Action<MpCompletion>) &&
        parameter.HasDefaultValue &&
        parameter.DefaultValue is null &&
        (!parameter.ParameterType.IsValueType ||
            Nullable.GetUnderlyingType(parameter.ParameterType) is not null);

    private static bool IsOptionalList(ParameterInfo parameter) =>
        DefaultsToNull(parameter) &&
        parameter.ParameterType.IsGenericType &&
        parameter.ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>);

    // A valid value for a required argument: non-zero enum choices, one-element
    // sequences, and domain values with every required member set.
    private static object SampleArgument(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type == typeof(string))
        {
            return "Sample";
        }

        if (type == typeof(Transform))
        {
            return new Transform { Values = [1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1] };
        }

        // Given report output options name exactly one destination.
        if (type == typeof(ReportOutputOptions))
        {
            return new ReportOutputOptions { OutputType = ReportOutputType.SaReport, ExternalPath = "Sample" };
        }

        if (type.IsEnum)
        {
            return Enum.GetValues(type)
                .Cast<object>()
                .First(value => Convert.ToInt64(value, CultureInfo.InvariantCulture) != 0);
        }

        if (type.IsPrimitive)
        {
            return Convert.ChangeType(1, type, CultureInfo.InvariantCulture);
        }

        if (type.IsGenericType &&
            (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                type.GetGenericTypeDefinition() == typeof(IReadOnlyList<>)))
        {
            var elementType = type.GetGenericArguments()[0];
            var values = Array.CreateInstance(elementType, 1);
            values.SetValue(SampleArgument(elementType), 0);
            return values;
        }

        var value = Activator.CreateInstance(type) ??
            throw new InvalidOperationException($"No sample value for {type.Name}.");
        foreach (var property in type.GetProperties()
            .Where(property => property.IsDefined(typeof(RequiredMemberAttribute))))
        {
            property.SetValue(value, SampleArgument(property.PropertyType));
        }

        return value;
    }
}
