using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models.Inventories;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Repository.FileRepo;

public class FileMetaDataRepo : IFileMetaDataRepo
{
    public readonly AppDbContext _context;

    public FileMetaDataRepo(AppDbContext context)
    {
        this._context = context;

    }
    private DbSet<FileMetadata> dbSet => _context.Set<FileMetadata>();
   
    public async Task<FileMetadata> Create(FileMetadata fileMetadata)
    {
        await dbSet.AddAsync(fileMetadata);
        await _context.SaveChangesAsync();
        return fileMetadata;
    }

    public async Task<bool> Delete(string generatedFileName)
    {
        var fileToDelete = await dbSet.FindAsync(generatedFileName);
        if (fileToDelete is null)
            return false;
        fileToDelete.isDeleted = true;
        fileToDelete.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }



    public async Task<FileMetadata?> GetById(string generatedFileName)
    {
        var fileMetaData = await dbSet.FindAsync(generatedFileName);
        if (fileMetaData is null)
            return null;
        return fileMetaData;
    }
}