namespace DesignSystem.Components.Navigation;

/// <summary>Provides the values required by a pagination control.</summary>
public interface IPaginationState
{
    /// <summary>Raised after the state changes.</summary>
    event Action? Changed;

    int PageSize { get; }
    int TotalItems { get; }
    int PageCount { get; }
    int CurrentPage { get; set; }
    bool IsLoading { get; }
}
