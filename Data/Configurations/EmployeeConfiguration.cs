using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{

    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(k => k.EmployeeId);
        builder.ToTable("TblEmployees");
        builder.Property(p => p.EmployeeId)
            .HasConversion(
                Ulid => Ulid.ToString(),
                str => Ulid.Parse(str)
            );
        builder.HasIndex(p => p.Email).IsUnique();
        builder.HasMany(e => e.EmployeeRoles)
            .WithOne(er => er.Employee);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(k => k.RoleId);
        builder.ToTable("TblRoles");
        builder.Property(p => p.RoleId)
            .HasConversion(
                Ulid => Ulid.ToString(),
                str => Ulid.Parse(str)
            );
        builder.HasMany(r => r.EmployeeRoles)
            .WithOne(er => er.Role);
        builder.HasMany(r => r.RolePermissions)
            .WithOne(er => er.Role);
    }
}

public class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
{
    public void Configure(EntityTypeBuilder<EmployeeRole> builder)
    {
        builder.HasKey(k => new { k.EmployeeId, k.RoleId });
        builder.Property(p => p.RoleId)
            .HasConversion(
                Ulid => Ulid.ToString(),
                str => Ulid.Parse(str)
            );
        builder.Property(p => p.EmployeeId)
            .HasConversion(
                Ulid => Ulid.ToString(),
                str => Ulid.Parse(str)
            );
        builder.ToTable("TblEmployeeRoles");
        builder.HasOne(er => er.Employee)
            .WithMany(e => e.EmployeeRoles)
            .HasForeignKey(er => er.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(er => er.Role)
            .WithMany(r => r.EmployeeRoles)
            .HasForeignKey(er => er.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}