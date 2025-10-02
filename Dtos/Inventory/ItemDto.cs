namespace InventoryManagementSystem.Dtos.Inventory;

public class ItemDetails
{
    public Ulid ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public int Quantity { get; set; }
    public string Place { get; set; } = string.Empty;
    public string FileId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
}

public class CreateItemDto
{
    public IFormFile? File { get; set; }
    public string ItemName { get; set; } = null!;
    public int Quantity { get; set; }
    public string Place { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
}
public class UpdateItemDto
{
    public IFormFile? File { get; set; }
    public required string ItemId{ get; set; }
    public required string ItemName { get; set; } 
    public int Quantity { get; set; }
    public string Place { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
}