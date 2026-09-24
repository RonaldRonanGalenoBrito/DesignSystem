namespace DesignSystem.Components.Layout;

/// <summary>Coordinates the header and navigation inside an AppShell.</summary>
public sealed record LayoutContext(string SidebarId, bool MenuOpen, Func<Task> ToggleMenu, Func<Task> CloseMenu);
