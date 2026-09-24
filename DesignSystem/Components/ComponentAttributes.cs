namespace DesignSystem.Components;

internal static class ComponentAttributes
{
    public static string Classes(IReadOnlyDictionary<string, object>? attributes, params string?[] classes)
        => string.Join(" ", classes.Append(Get(attributes, "class")).Where(value => !string.IsNullOrWhiteSpace(value)));

    public static string? Get(IReadOnlyDictionary<string, object>? attributes, string key)
        => attributes?.FirstOrDefault(pair => string.Equals(pair.Key, key, StringComparison.OrdinalIgnoreCase)).Value?.ToString();
}

