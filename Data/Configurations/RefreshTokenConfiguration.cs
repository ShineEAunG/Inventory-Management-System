using InventoryManagementSystem.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("TblRefreshTokens");
        builder.Property(p => p.RefreshTokenId)
            .HasConversion(
                Ulid => Ulid.ToString(),
                str => Ulid.Parse(str)
            );
        builder.Property(p => p.ReplacedTokenId)
            .HasConversion(
                ulid => ulid.HasValue ? ulid.Value.ToString() : null,
                str => str != null ? Ulid.Parse(str) : (Ulid?)null
            );
        builder.Property(p => p.EmployeeId)
            .HasConversion(
                ulid => ulid.ToString() ,
                str => Ulid.Parse(str)
            );
        builder.HasKey(rt => rt.RefreshTokenId);
        builder.Property(rt => rt.RefreshTokenHash).IsRequired();
        builder.Property(rt => rt.ExpiresAt).IsRequired();
        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(f => f.EmployeeId);
    }
}