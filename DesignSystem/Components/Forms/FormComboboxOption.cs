namespace DesignSystem.Components.Forms;

/// <summary>Opção exibida por FormCombobox.</summary>
public sealed record FormComboboxOption(object Value, string Label, string? Description = null, bool Disabled = false);
