namespace DesignSystem.Components;

internal static class ComponentAttributes
{
    // Bootstrap 6 replaces the former light/dark contextual colors with neutral/inverse.
    public static string Theme(object variant) => variant.ToString() switch
    {
        "Light" => "secondary",
        "Dark" => "inverse",
        var name => name!.ToLowerInvariant()
    };

    public static string Classes(IReadOnlyDictionary<string, object>? attributes, params string?[] classes)
        => string.Join(" ", classes.Append(Get(attributes, "class")).Where(value => !string.IsNullOrWhiteSpace(value)));

    public static string? Get(IReadOnlyDictionary<string, object>? attributes, string key)
        => attributes?.FirstOrDefault(pair => string.Equals(pair.Key, key, StringComparison.OrdinalIgnoreCase)).Value?.ToString();
}

