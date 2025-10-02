using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.Interfaces;

namespace InventoryManagementSystem.Repository.Interface;

public interface IItemRepo: IGenericRepo<Item>
{
    Task<PaginatedList<ItemDetails>> GetAllDetails(string? keyWord,int pageIndex = 1, int pageSize = 6);
    Task<ItemDetails?> GetDetailsById(Ulid itemId);
    Task SaveChangesAsync();
}