namespace InventoryManagementSystem.Models.Inventories;

public class Category
{
    public Ulid CategoryId { get; set; } = Ulid.NewUlid();
    public string CategoryName { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTimeOffset ModifiedAt { get; set; }
    public ICollection<Item> Items { get; set; } = [];

}
