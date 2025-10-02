using InventoryManagementSystem.Models.Inventories;

namespace InventoryManagementSystem.Repository.FileRepo;

public interface IFileMetaDataRepo
{
    Task<FileMetadata> Create(FileMetadata fileMetadata);

    Task<FileMetadata?> GetById(string generatedFileName);
    Task<bool> Delete(string generatedFileName);
}