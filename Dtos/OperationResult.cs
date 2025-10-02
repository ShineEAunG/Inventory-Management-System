namespace InventoryManagementSystem.Dtos;

public class PaginatedList<T> where T : class
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public List<T> Items { get; } = new();
    public PaginatedList(List<T> items, int count, int pageIndex = 1,    int pageSize = 6)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items.AddRange(items);
    }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}

public record OperationResult(bool Success, string Message, string? Name = null);