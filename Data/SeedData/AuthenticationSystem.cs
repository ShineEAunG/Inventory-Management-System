using System.ComponentModel.Design;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.StaticClasses;

namespace InventoryManagementSystem.Data.SeedData;

public static class AuthenticationSystem
{
    public static void SeedData(AppDbContext appDbContext)
    {
        var permissions = new List<Permission>
        {
            new()
            {
                PermissionName = "Can Read",
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = Creator.System
            },
            new()
            {
                PermissionName = "Can Edit",
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = Creator.System
            },
            new()
            {
                PermissionName = "Can Delete",
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = Creator.System
            },
            new()
            {
                PermissionName = "Can Add",
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = Creator.System
            }
        };
        appDbContext.Permissions.AddRange(permissions);
        appDbContext.SaveChanges();
        var roles = new List<Role>
        {
            new()
            {
                RoleName = Roles.Admin,
                CreatedBy =Creator.System
            },
            new()
            {
                RoleName = Roles.Manager,
                CreatedBy =Creator.System
            },
            new()
            {
                RoleName = Roles.Employee,
                CreatedBy =Creator.System
            }
        };
        appDbContext.Roles.AddRange(roles);
        appDbContext.SaveChanges();
        var rolesPermission = new List<RolePermission>
        {
            new()
            {
                RoleId = roles[0].RoleId,
                PermissionId = permissions[1].PermissionId,
                CreatedBy = Creator.System
            },
            new()
            {
                RoleId = roles[0].RoleId,
                PermissionId = permissions[2].PermissionId,
                CreatedBy = Creator.System
            },
            new()
            {
                RoleId = roles[0].RoleId,
                PermissionId = permissions[3].PermissionId,
                CreatedBy = Creator.System
            },
            new()
            {
                RoleId = roles[1].RoleId,
                PermissionId = permissions[1].PermissionId,
                CreatedBy = Creator.System
            },
            new()
            {
                RoleId = roles[1].RoleId,
                PermissionId = permissions[2].PermissionId,
                CreatedBy = Creator.System
            },
            new()
            {
                RoleId = roles[1].RoleId,
                PermissionId = permissions[3].PermissionId,
                CreatedBy = Creator.System
            }
        };
        appDbContext.RolePermissions.AddRange(rolesPermission);
        appDbContext.SaveChanges();
    }
}