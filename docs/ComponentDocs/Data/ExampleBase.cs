using Microsoft.AspNetCore.Components;
using DesignSystem.Enums;

namespace ComponentDocs.Data;

public abstract class ExampleBase : ComponentBase
{
    [Parameter, EditorRequired] public ComponentEntry Entry { get; set; } = default!;
    protected static string Bool(bool value) => value ? "true" : "false";
    protected static string ColorSchemeAttribute(FormColorScheme colorScheme) => colorScheme == FormColorScheme.Inherit
        ? string.Empty
        : $" ColorScheme=\"FormColorScheme.{colorScheme}\"";
    protected static string FormControlSizeAttribute(FormControlSize size) => size == FormControlSize.Medium
        ? string.Empty
        : $" Size=\"FormControlSize.{size}\"";
}
