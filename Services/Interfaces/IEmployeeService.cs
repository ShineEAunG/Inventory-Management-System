using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.ViewModels;

namespace InventoryManagementSystem.Services.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDetailsDto?> GetEmployee(string email);
    Task<PaginatedList<EmployeeDetailsDto>> GetAll(string? keyWord);
    Task<OperationResult> RegisterOrCreate(RegisterEmployeeDto employeeDto);
    Task<OperationResult> Delete(string email);
}