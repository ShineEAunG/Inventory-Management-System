using InventoryManagementSystem.Data;
using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace InventoryManagementSystem.Repository.Implementations;

public class EmployeeRoleRepo : IEmployeeRoleRepo
{
    private readonly AppDbContext _context;
    public EmployeeRoleRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<OperationResult> AddEmployeeToRole(Ulid employeeId, Ulid roleId)
    {
        var check = await CheckEmployeeAndRole(employeeId, roleId);
        if (!check.Success)
            return check;
        var newEmployeeRole = new EmployeeRole
        {
            EmployeeId = employeeId,
            RoleId = roleId
        };
        var exists = await _context.EmployeeRoles
            .AnyAsync(er => er.EmployeeId == employeeId && er.RoleId == roleId);

        if (exists)
            return new OperationResult(false, "Employee already assigned to this role");

        await _context.EmployeeRoles.AddAsync(newEmployeeRole);
        await _context.SaveChangesAsync();
        return new OperationResult(true, "Added successfully");
    }

    public async Task<OperationResult> RemoveEmployeeFromRole(Ulid employeeId, Ulid roleId)
    {
        var check = await CheckEmployeeAndRole(employeeId, roleId);
        if (!check.Success)
            return check;
            
        var employeeRoleToDelete = await _context.EmployeeRoles.FirstOrDefaultAsync(er => er.EmployeeId == employeeId && er.RoleId == roleId);
        _context.EmployeeRoles.Remove(employeeRoleToDelete!);
        await _context.SaveChangesAsync();
        return new OperationResult(true, "Removed successfully");
    }
    private async Task<OperationResult> CheckEmployeeAndRole(Ulid employeeId, Ulid roleId)
    {
        var employeeExist = await _context.Employees.FindAsync(employeeId);
        var roleExist = await _context.Roles.FindAsync(roleId);
        if (employeeExist is null)
        {
            return new OperationResult(false,$"Employee doesnt exist in table");
        }
        else if (roleExist is null)
        {
            return new OperationResult(false,$"Role doesnt exist in table");
        }
        return new OperationResult(true ,$"both exist");;
    }
}