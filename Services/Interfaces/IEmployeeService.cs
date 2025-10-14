using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.ViewModels;

namespace InventoryManagementSystem.Services.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDetailsDto?> GetEmployee(string email);
    Task<PaginatedList<EmployeeDetailsDto>> GetAll(EmployeeQueryParams queryParams);
    Task<OperationResult> RegisterOrCreate(RegisterEmployeeDto employeeDto);
    Task<OperationResult> Delete(string email);
}