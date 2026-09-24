namespace DesignSystem.Attributes;

/// <summary>Documentation colocated with a Blazor parameter, including inherited parameters.</summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class ParameterDocAttribute(string description) : Attribute
{
    public string Description { get; } = description;
    /// <summary>Optional explanatory default for values generated at runtime.</summary>
    public string? Default { get; set; }
}
