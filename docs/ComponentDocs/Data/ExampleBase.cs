using Microsoft.AspNetCore.Components;

namespace ComponentDocs.Data;

public abstract class ExampleBase : ComponentBase
{
    [Parameter, EditorRequired] public ComponentEntry Entry { get; set; } = default!;
    protected static string Bool(bool value) => value ? "true" : "false";
}
