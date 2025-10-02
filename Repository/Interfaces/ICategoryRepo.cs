using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.Interfaces;

namespace InventoryManagementSystem.Repository.Interface;

public interface ICategoryRepo : IGenericRepo<Category>
{
    Task<PaginatedList<CategoryDetails>> GetAllCategoryDetails(string? keyWord,int pageIndex = 1, int pageSize = 6);
    Task SaveChangesAsync();
}