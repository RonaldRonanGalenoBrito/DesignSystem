namespace ComponentDocs.Data;

/// <summary>Registers a compiled preview without adding a switch case or a library-to-docs dependency.</summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ComponentExampleAttribute(Type componentType) : Attribute
{
    public Type ComponentType { get; } = componentType;
}
