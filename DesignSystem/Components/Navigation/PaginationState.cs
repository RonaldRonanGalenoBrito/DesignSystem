namespace DesignSystem.Components.Navigation;
/// <summary>/// Mantém o estado de paginação de uma coleção já carregada em memória./// A consulta, ordenação e filtragem dos dados permanecem sob responsabilidade da tela consumidora./// </summary>

public sealed class PaginationState<T> : IPaginationState
{
    private int currentPage = 1;
    private bool isLoading;
    public PaginationState(int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        PageSize = pageSize;
    }
    public int PageSize { get; }
    public event Action? Changed;
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            if (isLoading == value) return;
            isLoading = value;
            Changed?.Invoke();
        }
    }
    public IReadOnlyList<T> Items { get; private set; } = [];
    public int TotalItems => Items.Count;
    public int PageCount => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public bool IsEmpty => !IsLoading && TotalItems == 0;
    public int CurrentPage
    {
        get => currentPage;
        set
        {
            var next = Math.Clamp(value, 1, Math.Max(1, PageCount));
            if (currentPage == next) return;
            currentPage = next;
            Changed?.Invoke();
        }
    }
    public IEnumerable<T> CurrentItems => Items
        .Skip((CurrentPage - 1) * PageSize).Take(PageSize);
    public void SetItems(IEnumerable<T>? items)
    {
        Items = items?.ToArray() ?? [];
        currentPage = Math.Clamp(currentPage, 1, Math.Max(1, PageCount));
        Changed?.Invoke();
    }
}
