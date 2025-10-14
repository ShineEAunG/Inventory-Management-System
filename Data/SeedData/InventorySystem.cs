using System.ComponentModel.Design;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.StaticClasses;

namespace InventoryManagementSystem.Data.SeedData;

public static class InventorySystem
{
    public static void SeedData(AppDbContext appDbContext)
    {
        var categories = new List<Category>()
        {
            new Category
            {
                CategoryId = Ulid.NewUlid(),
                CategoryName = "Electronics",
                Description = "Electronic devices",
                CreatedBy = Creator.System,
            },
            new Category
            {
                CategoryId = Ulid.NewUlid(),
                CategoryName = "Machenical",
                Description = "Hand tools for mechanic",
                CreatedBy = Creator.System
            },
            new Category
            {
                CategoryId = Ulid.NewUlid(),
                CategoryName = "Stationery",
                Description = "Pens, notebooks, etc.",
                CreatedBy = Creator.System
            }
        };
        var items = new List<Item>()
        {
            new Item
            {
                ItemName = "Laptop",
                OriginalQuantity = 4,
                Quantity = 2,
                Location = "Warehouse A",
                FileId = "",
                Description = "Dell laptop",
                CreatedBy = Creator.System,
                CategoryId = categories[0].CategoryId
            },
            new Item
            {
                ItemName = "Hand Drill",
                OriginalQuantity = 30,
                Quantity = 25,
                Location = "Warehouse B",
                Description = "DC drilling mechanical tool",
                CreatedBy = Creator.System,
                CategoryId = categories[1].CategoryId
            },
            new Item
            {
                ItemName = "A4",
                OriginalQuantity = 34,
                Quantity = 20,
                Location = "Warehouse C",
                Description = "A4 size papers",
                CreatedBy = Creator.System,
                CategoryId = categories[2].CategoryId
            },
            new Item
            {
                ItemName = "3D Printer",
                OriginalQuantity = 1,
                Quantity = 1,
                Location = "Warehouse C",
                Description = "Creality Brand 3D printer",
                CreatedBy = Creator.System,
                CategoryId = categories[2].CategoryId
            },
            new Item
            {
                ItemName = "Smartphone",
                OriginalQuantity = 6,
                Quantity = 5,
                Location = "Warehouse A",
                Description = "Company issued phones",
                CreatedBy = Creator.System,
                CategoryId = categories[0].CategoryId
            }
        };

        appDbContext.Categories.AddRange(categories);
        appDbContext.SaveChanges();
        appDbContext.Items.AddRange(items);
        appDbContext.SaveChanges();
    }
    
}
