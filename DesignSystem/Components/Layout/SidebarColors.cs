namespace DesignSystem.Components.Layout;

internal static class SidebarColors
{
    internal static string Style(Dictionary<string, object>? attributes, params (string Variable, string? Color)[] colors)
    {
        var styles = new List<string>();
        if (attributes is not null && attributes.TryGetValue("style", out var existing))
            styles.Add(existing?.ToString() ?? "");
        foreach (var (variable, color) in colors)
        {
            if (string.IsNullOrWhiteSpace(color)) continue;
            var hex = color.Trim();
            if (hex[0] != '#' || hex.Length is not (4 or 5 or 7 or 9) || !hex.Skip(1).All(Uri.IsHexDigit))
                throw new ArgumentException($"A cor '{color}' deve ser hexadecimal: #RGB, #RGBA, #RRGGBB ou #RRGGBBAA.");
            styles.Add($"{variable}: {hex}");
        }
        return string.Join(";", styles);
    }
}
