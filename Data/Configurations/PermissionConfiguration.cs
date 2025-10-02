using InventoryManagementSystem.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Data.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(k => k.PermissionId);
        builder.ToTable("TblPermissions");
        builder.Property(p => p.PermissionId)
            .HasConversion(
                id => id.ToString(),
                str => Ulid.Parse(str)
            );
        builder.HasMany(r => r.RolePermissions)
            .WithOne(er => er.Permission);
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(k => new { k.RoleId, k.PermissionId });
        builder.Property(p => p.PermissionId)
            .HasConversion(
                id => id.ToString(),
                str => Ulid.Parse(str)
            );
        builder.Property(p => p.RoleId)
            .HasConversion(
                id => id.ToString(),
                str => Ulid.Parse(str)
            );
        builder.ToTable("TblRolePermissions");
        builder.HasOne(er => er.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(er => er.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(er => er.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(er => er.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}