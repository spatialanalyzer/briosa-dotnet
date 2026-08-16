using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Briosa;

internal static class OperationProtocolMapper
{
    public static TRequest BuildRequest<TRequest>(
        TRequest request,
        IReadOnlyDictionary<string, object?> values)
        where TRequest : class, IMessage<TRequest>
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(values);
        var fields = request.Descriptor.Fields.InFieldNumberOrder();
        if (values.Count != fields.Count || fields.Any(field => !values.ContainsKey(field.Name)))
        {
            throw new BriosaProtocolException("operation-request-field-drift");
        }

        foreach (var field in fields)
        {
            AssignField(request, field, values[field.Name]);
        }

        return request;
    }

    public static TResult MapResponse<TResult>(IMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);
        var fields = response.Descriptor.Fields.InFieldNumberOrder()
            .Where(field => field.Name != "execution")
            .ToArray();
        if (fields.Length == 0)
        {
            if (typeof(TResult).Name != "NoOperationResult")
            {
                throw new BriosaProtocolException("operation-result-shape-drift");
            }

            return default!;
        }

        if (fields.Length == 1)
        {
            return (TResult)ReadField(response, fields[0], typeof(TResult));
        }

        var result = Activator.CreateInstance<TResult>();
        if (result is null)
        {
            throw new BriosaProtocolException("operation-result-construction-failed");
        }

        foreach (var field in fields)
        {
            var property = typeof(TResult).GetProperty(ToPascalCase(field.Name)) ??
                throw new BriosaProtocolException("operation-result-field-drift");
            property.SetValue(result, ReadField(response, field, property.PropertyType));
        }

        return result;
    }

    private static void AssignField(IMessage message, FieldDescriptor field, object? value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(field.Name);
        }

        if (field.IsRepeated)
        {
            if (value is string or byte[] || value is not IEnumerable values)
            {
                throw new ArgumentException(
                    $"{field.Name} must be a finite non-string sequence.",
                    field.Name);
            }

            var target = field.Accessor.GetValue(message) as IList ??
                throw new BriosaProtocolException("operation-repeated-field-drift");
            foreach (var item in values)
            {
                target.Add(ToWireValue(field, item));
            }

            return;
        }

        field.Accessor.SetValue(message, ToWireValue(field, value));
    }

    private static object ToWireValue(FieldDescriptor field, object? value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(field.Name);
        }

        return field.FieldType switch
        {
            FieldType.Message => ToWireMessage(field.MessageType, value),
            FieldType.Enum => ToWireEnum(field, value),
            FieldType.Int32 or FieldType.SInt32 or FieldType.SFixed32 =>
                Convert.ToInt32(value, CultureInfo.InvariantCulture),
            FieldType.UInt32 or FieldType.Fixed32 =>
                Convert.ToUInt32(value, CultureInfo.InvariantCulture),
            FieldType.Int64 or FieldType.SInt64 or FieldType.SFixed64 =>
                Convert.ToInt64(value, CultureInfo.InvariantCulture),
            FieldType.UInt64 or FieldType.Fixed64 =>
                Convert.ToUInt64(value, CultureInfo.InvariantCulture),
            FieldType.Float => Convert.ToSingle(value, CultureInfo.InvariantCulture),
            FieldType.Double => Convert.ToDouble(value, CultureInfo.InvariantCulture),
            FieldType.Bool => Convert.ToBoolean(value, CultureInfo.InvariantCulture),
            FieldType.String => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty,
            _ => value,
        };
    }

    private static IMessage ToWireMessage(MessageDescriptor descriptor, object value)
    {
        ValidateDomainValue(value, protocolOutput: false);
        var message = Activator.CreateInstance(descriptor.ClrType) as IMessage ??
            throw new BriosaProtocolException("operation-domain-construction-failed");
        var publicType = value.GetType();
        foreach (var field in descriptor.Fields.InFieldNumberOrder())
        {
            var property = publicType.GetProperty(ToPascalCase(field.Name));
            if (property is null)
            {
                throw new BriosaProtocolException("operation-domain-field-drift");
            }

            var fieldValue = property.GetValue(value);
            if (fieldValue is null && property.IsDefined(typeof(RequiredMemberAttribute)))
            {
                throw new ArgumentNullException(property.Name);
            }

            if (fieldValue is not null)
            {
                AssignField(message, field, fieldValue);
            }
        }

        return message;
    }

    private static object ReadField(IMessage message, FieldDescriptor field, Type targetType)
    {
        if (field.HasPresence && !field.Accessor.HasValue(message))
        {
            throw new BriosaProtocolException($"required-output-missing:{field.Name}");
        }

        var value = field.Accessor.GetValue(message);
        if (field.IsRepeated)
        {
            var elementType = targetType.GetElementType() ??
                throw new BriosaProtocolException("operation-result-sequence-drift");
            var source = ((IEnumerable)value).Cast<object>().ToArray();
            var result = Array.CreateInstance(elementType, source.Length);
            for (var index = 0; index < source.Length; index++)
            {
                result.SetValue(FromWireValue(field, source[index], elementType), index);
            }

            return result;
        }

        return FromWireValue(field, value, targetType);
    }

    private static object FromWireValue(FieldDescriptor field, object value, Type targetType) =>
        field.FieldType switch
        {
            FieldType.Message => FromWireMessage((IMessage)value, targetType),
            FieldType.Enum => FromWireEnum(field, value, targetType),
            _ => Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture),
        };

    private static object FromWireMessage(IMessage message, Type targetType)
    {
        var result = Activator.CreateInstance(targetType) ??
            throw new BriosaProtocolException("operation-domain-construction-failed");
        foreach (var field in message.Descriptor.Fields.InFieldNumberOrder())
        {
            var property = targetType.GetProperty(ToPascalCase(field.Name)) ??
                throw new BriosaProtocolException("operation-domain-field-drift");
            if (field.HasPresence && !field.Accessor.HasValue(message))
            {
                if (property.IsDefined(typeof(RequiredMemberAttribute)))
                {
                    throw new BriosaProtocolException(
                        $"required-domain-field-missing:{field.Name}");
                }

                continue;
            }

            property.SetValue(result, ReadField(message, field, property.PropertyType));
        }

        ValidateDomainValue(result, protocolOutput: true);
        return result;
    }

    private static object ToWireEnum(FieldDescriptor field, object value)
    {
        var numericValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
        if (numericValue == 0 || !Enum.IsDefined(field.EnumType.ClrType, numericValue))
        {
            throw new ArgumentOutOfRangeException(
                field.Name,
                value,
                "The value is not a supported MP choice.");
        }

        return Enum.ToObject(field.EnumType.ClrType, numericValue);
    }

    private static object FromWireEnum(FieldDescriptor field, object value, Type targetType)
    {
        var numericValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
        if (numericValue == 0 || !Enum.IsDefined(field.EnumType.ClrType, numericValue))
        {
            throw new BriosaProtocolException($"unknown-enum-value:{field.Name}");
        }

        return Enum.ToObject(Nullable.GetUnderlyingType(targetType) ?? targetType, numericValue);
    }

    private static void ValidateDomainValue(object value, bool protocolOutput)
    {
        var diagnosticCode = value switch
        {
            Transform transform when transform.Values is not { Length: 16 } =>
                "transform-must-have-16-values",
            Color color when color.Red > byte.MaxValue ||
                color.Green > byte.MaxValue ||
                color.Blue > byte.MaxValue => "color-channel-out-of-range",
            ReportOutputOptions options when
                options.ExternalPath is not null && options.EmbeddedFile is not null =>
                "report-output-destination-conflict",
            _ => null,
        };

        if (diagnosticCode is null)
        {
            return;
        }

        if (protocolOutput)
        {
            throw new BriosaProtocolException(diagnosticCode);
        }

        throw new ArgumentException(diagnosticCode, nameof(value));
    }

    private static string ToPascalCase(string name) =>
        string.Concat(name.Split('_', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => char.ToUpperInvariant(part[0]) + part[1..]));
}
