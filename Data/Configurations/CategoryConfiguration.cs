using InventoryManagementSystem.Models.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("TblCategory");
        builder.Property(p => p.CategoryId)
            .HasConversion(
                Ulid => Ulid.ToString(),
                str => Ulid.Parse(str)
            );
        builder.HasMany(e => e.Items)
            .WithOne(e => e.Category);
    }
}