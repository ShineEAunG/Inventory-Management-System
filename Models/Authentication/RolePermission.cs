namespace InventoryManagementSystem.Models.Authentication;

public class RolePermission
{
    public Ulid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public Ulid PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }
}