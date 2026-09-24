using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using DesignSystem.Attributes;

namespace ComponentDocs.Data;

public sealed record ComponentEntry(
    string Slug, string Name, string Group, string Description,
    Type ComponentType, string Code, string Guidance, int Order = 0);

public static class Catalog
{
    private static readonly Lazy<IReadOnlyList<ComponentEntry>> Entries =
        new(() => Discover(typeof(ComponentDocAttribute).Assembly.GetExportedTypes()));
    public static IReadOnlyList<ComponentEntry> All => Entries.Value;

    public static IReadOnlyList<ComponentEntry> Discover(IEnumerable<Type> types)
    {
        var entries = new List<ComponentEntry>();
        var slugs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var type in types)
        {
            var doc = type.GetCustomAttribute<ComponentDocAttribute>();
            if (doc is null) continue;
            void Invalid(string reason) => throw new InvalidOperationException($"ComponentDoc em {type.FullName}: {reason}");
            if (!type.IsClass || type.IsAbstract || !typeof(IComponent).IsAssignableFrom(type))
                Invalid("o atributo deve decorar um componente Blazor concreto.");
            if (string.IsNullOrWhiteSpace(doc.Slug) || !Regex.IsMatch(doc.Slug, "^[a-z0-9]+(?:-[a-z0-9]+)*$"))
                Invalid("Slug deve usar letras minúsculas, números e hífens, sem espaços.");
            if (!slugs.Add(doc.Slug)) Invalid($"Slug duplicado: '{doc.Slug}'.");
            if (string.IsNullOrWhiteSpace(doc.Group) || string.IsNullOrWhiteSpace(doc.Description))
                Invalid("Group e Description são obrigatórios.");

            var arguments = doc.TypeArguments ?? [];
            var resolvedType = type;
            if (type.IsGenericTypeDefinition)
            {
                if (arguments.Length != type.GetGenericArguments().Length || arguments.Any(t => t is null || t.ContainsGenericParameters))
                    Invalid("informe TypeArguments com um tipo fechado para cada parâmetro genérico.");
                try { resolvedType = type.MakeGenericType(arguments); }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException($"ComponentDoc em {type.FullName}: TypeArguments não satisfaz as restrições genéricas.", ex);
                }
            }
            else if (arguments.Length > 0) Invalid("TypeArguments só é aceito em componentes genéricos abertos.");
            if (resolvedType.ContainsGenericParameters) Invalid("o tipo resultante ainda contém parâmetros genéricos abertos.");

            entries.Add(new(doc.Slug, string.IsNullOrWhiteSpace(doc.Name) ? type.Name.Split('`')[0] : doc.Name.Trim(),
                doc.Group.Trim(), doc.Description.Trim(), resolvedType, doc.Code ?? "", doc.Guidance ?? "", doc.Order));
        }
        return entries.OrderBy(e => e.Group, StringComparer.Ordinal)
            .ThenBy(e => e.Order).ThenBy(e => e.Name, StringComparer.Ordinal).ToList().AsReadOnly();
    }
}
