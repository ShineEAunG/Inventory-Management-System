using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.Inventory;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.FileRepo;
using InventoryManagementSystem.Repository.Interface;
using InventoryManagementSystem.Services.FileHandling;
using InventoryManagementSystem.Services.Interfaces;
using InventoryManagementSystem.StaticClasses;

namespace InventoryManagementSystem.Services.Implementations;

public class ItemService : IItemService
{
    private readonly IItemRepo _itemRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFileMetaDataRepo _fileMetaDataRepo;
    private readonly IFileService _fileService;
    public ItemService(IItemRepo itemRepo, IFileMetaDataRepo fileMetaDataRepo, IHttpContextAccessor httpContextAccessor, IFileService fileStorage)
    {
        _itemRepo = itemRepo;
        _fileMetaDataRepo = fileMetaDataRepo;
        _httpContextAccessor = httpContextAccessor;
        _fileService = fileStorage;
    }
    public string? GetFileUrl(string generatedFileName)
    {
        var result = _fileService.GetUrl(generatedFileName);
        if (result.Success)
        {
            return result.Message;
        }
        return null;
    }
    public async Task<OperationResult> Create(IFormFile? file, CreateItemDto itemDto)
    {
        //if file is not null it need to upload first
        var fileName = string.Empty;
        var userEmail = _httpContextAccessor
            .HttpContext?.User?.FindFirst("email")?.Value;
        userEmail = string.IsNullOrEmpty(userEmail) ? Creator.System : userEmail;
        if (file is not null || file?.Length != 0)
        {
            var createResult = await _fileService.Upload(file!);
            if (!createResult.Success)
                return new OperationResult(false, "Failed while file uploading, Please try again", null);

            fileName = createResult.Name;
            var newFileMetaData = new FileMetadata
            {
                GeneratedName = fileName!,
                OriginalName = file!.FileName,
                ContentType = file.ContentType,
                UploadedBy = userEmail,
            };
            await _fileMetaDataRepo.Create(newFileMetaData);
        }
        // get who created it 

        var newItem = new Item
        {
            ItemId = Ulid.NewUlid(),
            ItemName = itemDto.ItemName,
            OriginalQuantity = itemDto.OriginalQuantity,
            Quantity = itemDto.Quantity,
            Location = itemDto.Location,
            FileId = fileName!,
            Description = itemDto.Description,
            CreatedBy = string.IsNullOrEmpty(userEmail) ? Creator.System : userEmail,
            CreatedAt = DateTimeOffset.UtcNow,
            CategoryId = string.IsNullOrEmpty(itemDto.CategoryId) ? null : Ulid.Parse(itemDto.CategoryId)
        };

        // save the metadata to db
        await _itemRepo.Create(newItem);
        await _itemRepo.SaveChangesAsync();
        return new OperationResult(true, "successfully created", null);
    }

    public async Task<OperationResult> Delete(Ulid itemId)
    {
        //check if item is there
        var itemToDelete = await _itemRepo.GetById(itemId);
        if(itemToDelete is null)
            return new OperationResult(false, $"There is no item wit id {itemId}",null);

        if (!string.IsNullOrEmpty(itemToDelete.FileId))
        {
            //delete filemetadata
            var metaDeleteResult = await _fileMetaDataRepo.Delete(itemToDelete.FileId);
            if (!metaDeleteResult)
            return new OperationResult(false, "file metadata failed to delete",null);
        }
        //delete item
        await _itemRepo.Delete(itemId);
        return new OperationResult(true, "File deleted successfully",null);
    }

    public async Task<PaginatedList<ItemDetails>> GetAll(ItemQueryParams queryParams)
    {
        var ItemList = await _itemRepo.GetAllDetails(queryParams);
        return ItemList;
    }

    public async Task<ItemDetails?> GetById(Ulid itemId)
    {
        var item = await _itemRepo.GetDetailsById(itemId);
        return item;
    }
    public async Task<OperationResult> Update(IFormFile? file, Ulid itemId, UpdateItemDto? itemDetails)
    {
        var itemInDb = await _itemRepo.GetById(itemId);
        if (itemInDb is null)
            return new OperationResult(false, $"There is no item with id {itemId}", null);

        var userEmail = _httpContextAccessor
            .HttpContext?.User?.FindFirst("email")?.Value;
        userEmail = string.IsNullOrEmpty(userEmail) ? Creator.System : userEmail;

        // try uploading
        try
        {
            if (file is not null && file?.Length != 0)
            {
                var createResult = await _fileService.Upload(file!);
                if (!createResult.Success)
                    return new OperationResult(false, "Failed while file uploading, Please try again", null);
                await _fileMetaDataRepo.Delete(itemInDb.FileId);
                itemInDb.FileId = createResult.Name!;
                var newFileMetaData = new FileMetadata
                {
                    GeneratedName = file!.FileName!,
                    OriginalName = file!.FileName,
                    ContentType = file.ContentType,
                    UploadedBy = userEmail,
                };
                await _fileMetaDataRepo.Create(newFileMetaData);
            }
            Ulid.TryParse(itemDetails?.CategoryId, out var catId);
            //save new metadata
            if (itemDetails is not null && itemDetails.ModifiedAt == itemInDb.ModifiedAt)
            {
                itemInDb.ItemName = itemDetails.ItemName;
                itemInDb.OriginalQuantity = itemDetails.OriginalQuantity;
                itemInDb.Quantity = itemDetails.Quantity;
                itemInDb.Location = itemDetails.Location;
                itemInDb.Description = itemDetails.Description;
                itemInDb.CategoryId = catId;
                itemInDb.ModifiedAt = DateTimeOffset.UtcNow;
                itemInDb.ModifiedBy = userEmail;
                await _itemRepo.SaveChangesAsync();
                return new OperationResult(true, "updated successfully", itemInDb.FileId);
            }
            return new OperationResult(false, "The record has been modified by another user. Please reload and try again.", null);
        }
        catch (Exception ex)
        {
            return new OperationResult(false, $"There was an error {ex.Message}", null);
        }
    }
}