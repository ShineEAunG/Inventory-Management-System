namespace InventoryManagementSystem.Models.Authentication;

public class Permission
{
    public Ulid PermissionId { get; set; } = Ulid.NewUlid();
    public string PermissionName { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    //public ICollection<Role> Roles { get; set; } = []; //bidirectional and many to many but role only will lost the metadata about join table
    public ICollection<RolePermission> RolePermissions { get; set; } = []; //here we can keep the metadata about join table like createdat, createdby etc
}