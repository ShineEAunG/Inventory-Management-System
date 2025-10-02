using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.Inventories;

public class Item
{
    public Ulid ItemId { get; set; } = Ulid.NewUlid();
    public string ItemName { get; set; } = null!;
    public int Quantity { get; set; }
    public string Place { get; set; } = string.Empty;
    public string FileId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTimeOffset? ModifiedAt { get; set; }    
    [ForeignKey("Category")]
    public Ulid? CategoryId { get; set; }
    public Category? Category { get; set; }
}


public class FileMetadata
{
    [Key]
    public string GeneratedName { get; set; } = string.Empty;
    public string OriginalName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
    public string UploadedBy { get; set; } = string.Empty;
    public bool isDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }
}