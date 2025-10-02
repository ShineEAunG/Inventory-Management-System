using InventoryManagementSystem.Data;
using InventoryManagementSystem.Repository.Interface;
using InventoryManagementSystem.Services.FileHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IEmployeeRoleRepo _erRepo;
    private readonly IFileService _fileService;
    private readonly AppDbContext _context;
    public AdminController(IEmployeeRoleRepo erRepo, IFileService fileService, AppDbContext context)
    {
        _erRepo = erRepo;
        _fileService = fileService;
        _context = context;
    }
    [HttpPost("employee/{employeeId}/roles/{roleId}/add")]
    public async Task<IActionResult> AddEmployeeToRole(string employeeId, string roleId)
    {
        if (!Ulid.TryParse(employeeId, out var eId) || !Ulid.TryParse(roleId, out var rId))
        {
            return BadRequest();
        }
        var result = await _erRepo.AddEmployeeToRole(eId, rId);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    [HttpPost("employee/{employeeId}/roles/{roleId}/remove")]
    public async Task<IActionResult> RemoveEmployeeFromRole(string employeeId, string roleId)
    {
        if (!Ulid.TryParse(employeeId, out var eId) || !Ulid.TryParse(roleId, out var rId))
        {
            return BadRequest();
        }
        var result = await _erRepo.RemoveEmployeeFromRole(eId, rId);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    [HttpDelete("files")]
    public async Task<IActionResult> RemoveAllTheDeletedFilesFromStorage()
    {
        var fileList = await _context.FileMetadatas
            .Where(f => f.isDeleted == true)
            .Select(f => f.GeneratedName)
            .ToListAsync();
        if (!fileList.Any())
            return Ok(new { message = "No files to remove" });
        var result = await _fileService.Delete(fileList);
        if (!result.Success)
            return BadRequest();
        return Ok(new {
            removedCount = fileList.Count,
            message = "Deleted files removed from storage"
        });
    }
}