namespace DesignSystem.Components.Accordions;

public sealed record AccordionContext(IReadOnlySet<string> ExpandedIds, Func<string, Task> Toggle);
