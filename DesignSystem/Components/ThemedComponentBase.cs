using DesignSystem.Attributes;
using DesignSystem.Enums;
using Microsoft.AspNetCore.Components;

namespace DesignSystem.Components;

/// <summary>Shared access to the native Bootstrap 6 theme tokens.</summary>
public abstract class ThemedComponentBase : ComponentBase
{
    [Parameter, ParameterDoc("Tema nativo do Bootstrap 6. Quando informado, tem prioridade sobre a cor de Variant. Null preserva a cor padrão ou herdada. O efeito visual depende do componente; não substitui os estados de validação.")]
    public ThemeColor? Theme { get; set; }

    protected string? ThemeClass => Theme is { } theme ? $"theme-{ComponentAttributes.Theme(theme)}" : null;
    protected string? ThemeForegroundClass => Theme is { } theme ? $"fg-{ComponentAttributes.Theme(theme)}" : null;
    protected string ResolveTheme(object fallback) => ComponentAttributes.Theme(Theme is { } theme ? theme : fallback);

    protected string ThemedClasses(IReadOnlyDictionary<string, object>? attributes, params string?[] classes)
        => ComponentAttributes.Classes(attributes, classes.Append(ThemeClass).ToArray());
}
