using System.Reflection;
using Microsoft.AspNetCore.Components;
using DesignSystem.Attributes;

namespace ComponentDocs.Data;

public static class ExampleCatalog
{
    private static readonly Lazy<IReadOnlyDictionary<Type, Type>> Examples =
        new(() => Discover(typeof(ExampleCatalog).Assembly.GetExportedTypes()));

    public static Type? Find(Type componentType) => Examples.Value.GetValueOrDefault(Key(componentType));

    public static IReadOnlyDictionary<Type, Type> Discover(IEnumerable<Type> types)
    {
        var result = new Dictionary<Type, Type>();
        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<ComponentExampleAttribute>();
            if (attribute is null) continue;
            if (type.IsAbstract || type.ContainsGenericParameters || !typeof(IComponent).IsAssignableFrom(type))
                throw new InvalidOperationException($"A prévia {type.FullName} deve ser um componente concreto sem parâmetros genéricos abertos.");
            var entry = type.GetProperty(nameof(ExampleBase.Entry));
            if (entry?.PropertyType != typeof(ComponentEntry) || entry.GetCustomAttribute<ParameterAttribute>() is null)
                throw new InvalidOperationException($"A prévia {type.FullName} deve herdar ExampleBase ou declarar o parâmetro Entry.");
            var key = Key(attribute.ComponentType);
            if (key.GetCustomAttribute<ComponentDocAttribute>() is null)
                throw new InvalidOperationException($"O alvo de {type.FullName} não possui ComponentDoc.");
            if (!result.TryAdd(key, type))
                throw new InvalidOperationException($"Prévia duplicada para {key.FullName}.");
        }
        return new System.Collections.ObjectModel.ReadOnlyDictionary<Type, Type>(result);
    }

    private static Type Key(Type type) => type.IsGenericType ? type.GetGenericTypeDefinition() : type;
}
