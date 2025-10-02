using InventoryManagementSystem.Dtos;

namespace InventoryManagementSystem.Services.FileHandling;

public interface IFileService
{
    OperationResult GetUrl(string fileName);
    Task<OperationResult> Upload(IFormFile file);
    Task<OperationResult> Delete(List<string> filePath);
}