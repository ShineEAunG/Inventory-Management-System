using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.ViewModels;

namespace InventoryManagementSystem.Repository.Interfaces;

public interface IEmployeeRepo : IGenericRepo<Employee>
{
    Task<EmployeeDetailsDto?> GetEmployeeWithRolesByEmail(string email);
    Task<Employee?> GetEmployeeByEmail(string email);
    Task<PaginatedList<EmployeeDetailsDto>> GetAllEmployeesWithRoles(string? keyWord,int pageIndex = 1, int pageSize = 6);
    Task<AddToRoleResponse> AddEmployeeToRoleAsync(string email, string roleName);
    Task<OperationResult> DeleteEmployee(string email);
    Task SaveChangesAsync();
}