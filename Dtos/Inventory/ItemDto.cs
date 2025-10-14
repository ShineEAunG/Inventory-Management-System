namespace InventoryManagementSystem.Dtos.Inventory;

public class ItemDetails
{
    public Ulid ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public int OriginalQuantity { get; set; }
    public int Quantity { get; set; }
    public string Location { get; set; } = string.Empty;
    public string FileId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
}

public class CreateItemDto
{
    public IFormFile? File { get; set; }
    public string ItemName { get; set; } = null!;
    public int OriginalQuantity { get; set; }
    public int Quantity { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
}
public class UpdateItemDto
{
    public IFormFile? File { get; set; }
    public required string ItemId { get; set; }
    public required string ItemName { get; set; }
    public int OriginalQuantity { get; set; }
    public int Quantity { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public DateTimeOffset? ModifiedAt { get; set; }
}

public class ItemQueryParams
{
    public string SearchTerm { get; set; } = string.Empty;
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public string CategoryId { get; set; } = string.Empty;
}