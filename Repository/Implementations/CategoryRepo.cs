using InventoryManagementSystem.Data;
using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.Interface;
using InventoryManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InventoryManagementSystem.Repository.Implementations;


public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
{
    public CategoryRepo(AppDbContext context):base(context)
    {
        
    }

    public async Task<PaginatedList<CategoryDetails>> GetAllCategoryDetails(string? keyWord, int pageIndex = 1, int pageSize = 6)
    {
        var query = _context.Categories.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(keyWord))
        {
            query = query.Where(c =>
                EF.Functions.ILike(c.CategoryName, $"%{keyWord}%") ||
                EF.Functions.ILike(c.Description!, $"%{keyWord}%"));
        }

        var count = await query.CountAsync();

        var categoryList = await query.OrderBy(c => c.CategoryName)
            .Skip(pageSize * (pageIndex - 1))
            .Take(pageSize)
            .Select(c => new CategoryDetails
            {
                CategoryId = c.CategoryId,
                ItemCount = _context.Items.Count(i => i.CategoryId == c.CategoryId),
                CategoryName = c.CategoryName,
                Description = c.Description
            }).ToListAsync();
        return new PaginatedList<CategoryDetails>(categoryList, count, pageIndex, pageSize);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}