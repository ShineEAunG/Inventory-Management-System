using System.Globalization;
using InventoryManagementSystem.Dtos;
using Supabase.Storage;

namespace InventoryManagementSystem.Services.FileHandling;

public class FileService : IFileService
{
    private readonly SupaBaseService _spService;
    public FileService(SupaBaseService service)
    {
        _spService = service;
    } 
    public async Task<OperationResult> Delete(List<string> fileName)
    {
        if (!_spService.CheckConfig())
        {
            return new OperationResult(false, "supabase config is missing",null);
        }
        var storage = _spService.Client.Storage.From(_spService.BucketName);
        await storage.Remove( fileName ); 
        return new OperationResult(true, "Removed successfully",null);
    }

    public OperationResult GetUrl(string fileName)
    {
        if (!_spService.CheckConfig())
        {
            return new OperationResult(false, "supabase config is missing",null);
        }
        var publicUrl = Path.Combine(_spService.Url, _spService.Directory, fileName);
        publicUrl = publicUrl.Replace("\\", "/");
        return new OperationResult(true, publicUrl,null);
    }

    public async Task<OperationResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return new OperationResult(false, "file is null",null);
        var storage = _spService.Client.Storage.From(_spService.BucketName);

        // Uploading new File
        var fileExtension = Path.GetExtension(file.FileName);
        var newFileName = $"{Guid.NewGuid()}{fileExtension}";
        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {            
            await file.CopyToAsync(ms); // copy file to memory            
            fileBytes = ms.ToArray();   // convert IFormFile to byte array
        }        
        var options = new Supabase.Storage.FileOptions  // pass the content type in uploading
        {
            ContentType = file.ContentType
        };
        var resultPath = await storage.Upload(fileBytes, newFileName, options);
        if (string.IsNullOrEmpty(resultPath))
        {
            return new OperationResult(false, "Something failed while uploading",null);
        }
        return new OperationResult(true, "Successfully Uploaded", newFileName);
    }
}