using InventoryManagementSystem.Data;
using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.Repository.Interfaces;
using InventoryManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace InventoryManagementSystem.Repository.Implementations;

public class EmployeeRepo : GenericRepo<Employee>, IEmployeeRepo
{
    public EmployeeRepo(AppDbContext context): base(context)
    {
        
    }

    public async Task<AddToRoleResponse> AddEmployeeToRoleAsync(string email, string roleName)
    {
        var employeeInDb = await _context.Employees            
            .FirstOrDefaultAsync(e=>e.Email == email);
        if (employeeInDb is null)
        {
            return new AddToRoleResponse
            {
                Success = false,
                Message = $"Employee with {email} doesnt exist"
            };
        }

        var roleInDb = await _context.Roles.FirstOrDefaultAsync(r=>r.RoleName == roleName);
        if (roleInDb is null)
        {
            return new AddToRoleResponse
            {
                Success = false,
                Message = $"Role with {roleName} doesnt exist"
            };
        }
        bool isInRoleAlready = await _context.EmployeeRoles.AnyAsync(er => er.EmployeeId == employeeInDb.EmployeeId && er.RoleId == roleInDb.RoleId);
        if (isInRoleAlready)
        {
            return new AddToRoleResponse
            {
                Success = true,
                Message = $"{email} is in {roleName} role already"
            };
        }
        var newEmployeeRole = new EmployeeRole
        {
            EmployeeId = employeeInDb.EmployeeId,
            RoleId = roleInDb.RoleId
        };
        await _context.EmployeeRoles.AddAsync(newEmployeeRole);
        await SaveChangesAsync();
        return new AddToRoleResponse
            {
                Success = true,
                Message = $"Added {email} to role {roleName} successfully"
            };
    }

    public async Task<OperationResult> DeleteEmployee(string email)
    {
        var employeeToDelete = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        if (employeeToDelete is null)
        {
            return new OperationResult(false, $"employee with {email} doesnt exist");
        }
        _context.Employees.Remove(employeeToDelete);
        await SaveChangesAsync();
        return new OperationResult(true, $"Deleteed {email} successfully");
    }

    public async Task<PaginatedList<EmployeeDetailsDto>> GetAllEmployeesWithRoles(string? keyWord,int pageIndex = 1, int pageSize = 6)
    {
        var query = _context.Employees.AsQueryable();
        if (!string.IsNullOrEmpty(keyWord))
        {
            query = query.Where(e =>
                EF.Functions.ILike(e.Email, $"%{keyWord}%") ||
                EF.Functions.ILike(e.EmployeeName, $"%{keyWord}%")
            );
        }
        var count = await query.CountAsync();
        var employeeList = await query
            .OrderBy(e => e.EmployeeName)
            .Skip(pageSize * (pageIndex - 1))
            .Take(pageSize)
            .Include(e => e.EmployeeRoles)
            .ThenInclude(er => er.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp=>rp.Permission)
            .Select(e => new EmployeeDetailsDto
            {
                EmployeeId = e.EmployeeId,
                Email = e.Email,
                EmployeeName = e.EmployeeName,
                Roles = e.EmployeeRoles.Select(
                    er => er.Role.RoleName)
                    .ToList(),
                Permissions = e.EmployeeRoles
                .SelectMany(er => er.Role.RolePermissions)
                .Select(rp => rp.Permission.PermissionName)
                .Distinct()
                .ToList()
            }).ToListAsync();
        Console.WriteLine(employeeList);
        return new PaginatedList<EmployeeDetailsDto>(employeeList, count, pageIndex, pageSize);
    }

    public Task<Employee?> GetEmployeeByEmail(string email)
    {
        var employeeInDb = _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        return employeeInDb;
    }

    public async Task<EmployeeDetailsDto?> GetEmployeeWithRolesByEmail(string email)
    {
        var employeeDetailsDto = await _context.Employees
            .Where(e => e.Email == email)
            .Include(e => e.EmployeeRoles)
            .ThenInclude(er => er.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp=>rp.Permission)
            .Select(e => new EmployeeDetailsDto
            {
                EmployeeId = e.EmployeeId,
                Email = e.Email,
                EmployeeName = e.EmployeeName,
                Roles = e.EmployeeRoles.Select(
                    er => er.Role.RoleName)
                    .ToList(),
                Permissions = e.EmployeeRoles
                    .SelectMany(er => er.Role.RolePermissions)
                    .Select(rp => rp.Permission.PermissionName)
                    .Distinct()
                    .ToList()
            }).FirstOrDefaultAsync();
        return employeeDetailsDto;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}