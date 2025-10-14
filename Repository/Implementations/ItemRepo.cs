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

public async Task<PaginatedList<ItemDetails>> GetAllDetails(ItemQueryParams queryParams)
{
    var query = _context.Items.AsNoTracking().AsQueryable();
    if (Ulid.TryParse(queryParams.CategoryId, out var categoryId))
        query = query.Where(i => i.CategoryId == categoryId);

    if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
    {
        query = query.Where(i =>
            EF.Functions.ILike(i.ItemName, $"%{queryParams.SearchTerm}%") ||
            EF.Functions.ILike(i.Description, $"%{queryParams.SearchTerm}%"));
    }
    var count = await query.CountAsync();
    var itemDetails = await query
        .OrderBy(i => i.ItemName)
        .Skip(queryParams.PageSize * (queryParams.PageIndex - 1))
        .Take(queryParams.PageSize)
        .Select(i => new ItemDetails
        {
            ItemId = i.ItemId,
            ItemName = i.ItemName,
            OriginalQuantity = i.OriginalQuantity,
            Quantity = i.Quantity,
            Location = i.Location,
            FileId = i.FileId,
            Description = i.Description,
            CategoryId = i.CategoryId.HasValue ? i.CategoryId.Value.ToString() : string.Empty
        })
        .ToListAsync();

    return new PaginatedList<ItemDetails>(itemDetails, count, queryParams.PageIndex, queryParams.PageSize);
}



    public async Task<ItemDetails?> GetDetailsById(Ulid itemId)
    {
       var itemDetail = await _context.Items
        .Where(i => i.ItemId == itemId)
        .Select(i => new ItemDetails
        {
            ItemId = i.ItemId,
            ItemName = i.ItemName,
            OriginalQuantity = i.OriginalQuantity,
            Quantity = i.Quantity,
            Location = i.Location,
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