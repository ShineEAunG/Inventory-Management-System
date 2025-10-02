using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;

namespace InventoryManagementSystem.Services.Interfaces;

public interface ICategoryService
{
    Task<OperationResult> Create(CreateCategoryDto categoryDto);
    Task<PaginatedList<CategoryDetails>> GetAll(string? keyWord);
    Task<CategoryDetails?> GetById(Ulid categoryId);
    Task<OperationResult> Delete(Ulid categoryId);
    Task<OperationResult> Update(CategoryDetails categoryDetails);
}