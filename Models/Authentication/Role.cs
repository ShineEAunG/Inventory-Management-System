namespace InventoryManagementSystem.Models.Authentication;

public class Role
{
    public Ulid RoleId { get; set; } = Ulid.NewUlid();
    public string RoleName { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTimeOffset ModifiedAt { get; set; } 
    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = [];  //bidrectional navigation property
    
    //public ICollection<Role> Roles { get; set; } = []; //bidirectional and many to many but Permission only will lost the metadata about join table
    public ICollection<RolePermission> RolePermissions { get; set; } = [];  //bidirectional and many to many but here we can keep the metadata about join table like createdat, createdby etc
}