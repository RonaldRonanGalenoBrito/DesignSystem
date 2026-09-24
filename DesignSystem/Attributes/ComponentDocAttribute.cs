namespace DesignSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ComponentDocAttribute : Attribute
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Guidance { get; set; } = string.Empty;
    /// <summary>Representative arguments used to document an open generic component.</summary>
    public Type[] TypeArguments { get; set; } = [];
    /// <summary>Position within the group; ties are sorted by name.</summary>
    public int Order { get; set; }

    public ComponentDocAttribute() { }

    public ComponentDocAttribute(
        string slug,
        string name,
        string group,
        string description,
        string code,
        string guidance)
    {
        Slug = slug;
        Name = name;
        Group = group;
        Description = description;
        Code = code;
        Guidance = guidance;
    }
}
