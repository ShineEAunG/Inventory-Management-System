using InventoryManagementSystem.Data;
using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace InventoryManagementSystem.Repository.Implementations;


public class ItemRepo : GenericRepo<Item>, IItemRepo
{
    public ItemRepo(AppDbContext context) : base(context)
    {

    }

public async Task<PaginatedList<ItemDetails>> GetAllDetails(string? keyWord, int pageIndex = 1, int pageSize = 6)
{
    var query = _context.Items.AsQueryable();

    if (!string.IsNullOrWhiteSpace(keyWord))
    {
        query = query.Where(i =>
            EF.Functions.ILike(i.ItemName, $"%{keyWord}%") ||
            EF.Functions.ILike(i.Description, $"%{keyWord}%"));
    }
    var count = await query.CountAsync();
    var itemDetails = await query
        .OrderBy(i => i.ItemName)
        .Skip(pageSize * (pageIndex - 1))
        .Take(pageSize)
        .Select(i => new ItemDetails
        {
            ItemId = i.ItemId,
            ItemName = i.ItemName,
            Quantity = i.Quantity,
            Place = i.Place,
            FileId = i.FileId,
            Description = i.Description,
            CategoryId = i.CategoryId.HasValue ? i.CategoryId.Value.ToString() : string.Empty
        })
        .ToListAsync();

    return new PaginatedList<ItemDetails>(itemDetails, count, pageIndex, pageSize);
}



    public async Task<ItemDetails?> GetDetailsById(Ulid itemId)
    {
       var itemDetail = await _context.Items
        .Where(i => i.ItemId == itemId)
        .Select(i => new ItemDetails
        {
            ItemId = i.ItemId,
            ItemName = i.ItemName,
            Quantity = i.Quantity,
            Place = i.Place,
            FileId = i.FileId,
            Description = i.Description,
            CategoryId = i.CategoryId.HasValue ? i.CategoryId.Value.ToString() : string.Empty
        })
        .FirstOrDefaultAsync();
        return itemDetail;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}