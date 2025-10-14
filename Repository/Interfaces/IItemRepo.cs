using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.Interfaces;

namespace InventoryManagementSystem.Repository.Interface;

public interface IItemRepo: IGenericRepo<Item>
{
    Task<PaginatedList<ItemDetails>> GetAllDetails(ItemQueryParams queryParams);
    Task<ItemDetails?> GetDetailsById(Ulid itemId);
    Task SaveChangesAsync();
}