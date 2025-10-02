using InventoryManagementSystem.Models.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("TblItem");
        builder.Property(p => p.ItemId)
            .HasConversion(
                Ulid => Ulid.ToString(),
                str => Ulid.Parse(str)
            );
        builder.HasOne(e => e.Category)
            .WithMany(e=>e.Items)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}