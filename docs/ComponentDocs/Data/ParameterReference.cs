using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using DesignSystem.Attributes;

namespace ComponentDocs.Data;

public sealed record ParameterRow(string Name, string Type, string Default, string Description, string? Options);

public static class ParameterReference
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<ParameterRow>> Cache = new();
    public static IReadOnlyList<ParameterRow> For(Type componentType) => Cache.GetOrAdd(componentType, Read);

    private static IReadOnlyList<ParameterRow> Read(Type componentType)
    {
        if (componentType.ContainsGenericParameters)
            throw new ArgumentException("Use o tipo fechado do Catalog; configure ComponentDoc.TypeArguments.", nameof(componentType));
        var definition = componentType.IsGenericType ? componentType.GetGenericTypeDefinition() : componentType;
        var nullability = new NullabilityInfoContext();
        // A component requiring constructor services still gets an API table; its defaults need explicit metadata.
        var instance = componentType.GetConstructor(Type.EmptyTypes) is null ? null : Activator.CreateInstance(componentType);
        try
        {
            return componentType.GetProperties()
                .Where(property => property.GetCustomAttribute<ParameterAttribute>() is not null)
                .OrderBy(property => property.Name, StringComparer.Ordinal)
                .Select(property =>
                {
                    var declared = definition.GetProperty(property.Name)!;
                    var doc = property.GetCustomAttribute<ParameterDocAttribute>(true);
                    var description = doc?.Description ?? property.GetCustomAttribute<DescriptionAttribute>(true)?.Description
                        ?? "Sem descrição. Adicione ParameterDoc ao parâmetro.";
                    if (property.GetCustomAttribute<EditorRequiredAttribute>() is not null)
                        description += " Deve ser informado (EditorRequired).";
                    var optionType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    return new ParameterRow(property.Name, TypeName(declared.PropertyType, nullability.Create(declared)),
                        doc?.Default ?? DefaultValue(property, instance), description,
                        optionType.IsEnum ? string.Join(", ", Enum.GetNames(optionType)) : null);
                }).ToList().AsReadOnly();
        }
        finally { (instance as IDisposable)?.Dispose(); }
    }

    private static string DefaultValue(PropertyInfo property, object? instance)
    {
        if (instance is null) return "Não disponível";
        var value = property.GetValue(instance);
        if (value is null) return "null";
        if (property.PropertyType == typeof(EventCallback) ||
            property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(EventCallback<>))
            return "Sem callback";
        return value switch
        {
            string text => $"“{text}”",
            bool flag => flag ? "true" : "false",
            IFormattable formattable => formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture),
            _ => value.ToString() ?? "—"
        };
    }

    private static string TypeName(Type type, NullabilityInfo? nullable = null)
    {
        if (Nullable.GetUnderlyingType(type) is { } underlying)
            return TypeName(underlying, nullable?.GenericTypeArguments.FirstOrDefault()) + "?";
        string name;
        if (type.IsGenericType)
            name = type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments()
                .Select((argument, index) => TypeName(argument, nullable?.GenericTypeArguments.ElementAtOrDefault(index)))) + ">";
        else if (type.IsArray) name = TypeName(type.GetElementType()!, nullable?.ElementType) + "[]";
        else name = type == typeof(string) ? "string" : type == typeof(bool) ? "bool" :
            type == typeof(int) ? "int" : type == typeof(double) ? "double" :
            type == typeof(decimal) ? "decimal" : type == typeof(object) ? "object" : type.Name;
        return name + (!type.IsValueType && !type.IsGenericParameter && nullable?.ReadState == NullabilityState.Nullable ? "?" : "");
    }
}
