#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
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
            outputGroup);

        Assert.Equal(0.1, fit.RmsError);
        var request = Assert.IsType<Transport.LocateInstrumentsUsmnRequest>(
            transport.LastOperationRequest);
        Assert.Empty(request.ExcludedGroups);
        Assert.Null(request.NominalsGroup);

        await client.InstrumentOperations.LocateInstrumentsUsmnAsync(
            instruments,
            outputGroup,
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
            var parameters = method.GetParameters();
            if (!parameters.Any(DefaultsToNull))
            {
                continue;
            }

            // Supply only the required arguments, so every null default is used.
            var arguments = parameters
                .Select(parameter => parameter.HasDefaultValue
                    ? Type.Missing
                    : SampleArgument(parameter.ParameterType))
                .ToArray();
            var callsBefore = transport.Calls.Count;
            var exception = await Record.ExceptionAsync(
                () => (Task)method.Invoke(target, arguments)!);
            invoked.Add(method.Name);

            // A failure after dispatch comes from mapping the fake's empty response,
            // which this test does not cover. Only a call that never reached the
            // transport failed while building its request.
            if (transport.Calls.Count == callsBefore)
            {
                var cause = (exception as TargetInvocationException)?.InnerException ?? exception;
                failures.Add(
                    $"{method.DeclaringType!.Name}.{method.Name}: {cause?.GetType().Name} {cause?.Message}");
            }
        }

        Assert.Contains(nameof(BriosaInstrumentOperations.LocateInstrumentsUsmnAsync), invoked);
        if (failures.Count > 0)
        {
            Assert.Fail(string.Join(Environment.NewLine, failures));
        }
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

    private static bool DefaultsToNull(ParameterInfo parameter) =>
        parameter.HasDefaultValue &&
        parameter.DefaultValue is null &&
        (!parameter.ParameterType.IsValueType ||
            Nullable.GetUnderlyingType(parameter.ParameterType) is not null);

    // A valid value for a required argument: non-zero enum choices, one-element
    // sequences, and domain values with every required member set.
    private static object SampleArgument(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type == typeof(string))
        {
            return "Sample";
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

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
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
