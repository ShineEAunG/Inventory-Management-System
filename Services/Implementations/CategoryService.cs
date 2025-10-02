using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.Interface;
using InventoryManagementSystem.Services.Interfaces;
using InventoryManagementSystem.StaticClasses;

namespace InventoryManagementSystem.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CategoryService(ICategoryRepo categoryRepo, IHttpContextAccessor httpContextAccessor)
    {
        _categoryRepo = categoryRepo;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<OperationResult> Create(CreateCategoryDto categoryDto)
    {
        var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;
        userEmail = string.IsNullOrEmpty(userEmail) ? Creator.System : userEmail;
        var newCategory = new Category
        {
            CategoryName = categoryDto.CategoryName,
            Description = categoryDto.Description,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = userEmail
        };
        var categoryInDb = await _categoryRepo.Create(newCategory);
        if(categoryInDb is null)
            return new OperationResult(false, "failed to create new, try again");
        return new OperationResult(true, "Created new Category successfully");
    }

    public async Task<OperationResult> Delete(Ulid categoryId)
    {
        var deleteResult = await _categoryRepo.Delete(categoryId);
        if (!deleteResult)
            return new OperationResult(false, "Cannot Delete for now try later");
        return new OperationResult(true, "Deleted Successfully");
    }

    public async Task<PaginatedList<CategoryDetails>> GetAll(string? keyWord)
    {
        return await _categoryRepo.GetAllCategoryDetails(keyWord);
    }

    public async Task<CategoryDetails?> GetById(Ulid categoryId)
    {
        var categoryInDb = await _categoryRepo.GetById(categoryId);
        if (categoryInDb is null)
            return null;
        var categoryDetails = new CategoryDetails
        {
            CategoryId = categoryInDb.CategoryId,
            CategoryName = categoryInDb.CategoryName,
            Description = categoryInDb.Description
        };
        return categoryDetails;
    }

    public async Task<OperationResult> Update(CategoryDetails categoryDetails)
    {
        var categoryInDb = await _categoryRepo.GetById(categoryDetails.CategoryId);
        if (categoryInDb is null)
            return new OperationResult(false, "Cannot update for now, try later");

        var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;
        userEmail = string.IsNullOrEmpty(userEmail) ? Creator.System : userEmail;
        categoryInDb.CategoryId = categoryDetails.CategoryId;
        categoryInDb.CategoryName = categoryDetails.CategoryName;
        categoryInDb.Description = string.IsNullOrEmpty(categoryDetails.Description)
            ? string.Empty
            : categoryDetails.Description;
        categoryInDb.ModifiedAt = DateTimeOffset.UtcNow;
        categoryInDb.ModifiedBy = userEmail;
        await _categoryRepo.SaveChangesAsync();
        return  new OperationResult(true, "Updated Successfully");
    }
}