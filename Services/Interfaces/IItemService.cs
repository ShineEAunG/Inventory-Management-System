using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;

namespace InventoryManagementSystem.Services.Interfaces;

public interface IItemService
{
    Task<OperationResult> Create(IFormFile? file, CreateItemDto itemDto);
    Task<OperationResult> Delete(Ulid itemId);
    Task<OperationResult> Update(IFormFile? file, Ulid itemId, UpdateItemDto? itemDetails);
    Task<ItemDetails?> GetById(Ulid itemId);

    Task<PaginatedList<ItemDetails>> GetAll(ItemQueryParams queryParams);
    string? GetFileUrl(string generatedFileName);
}