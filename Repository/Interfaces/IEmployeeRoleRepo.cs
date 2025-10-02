using InventoryManagementSystem.Dtos;

namespace InventoryManagementSystem.Repository.Interface;

public interface IEmployeeRoleRepo
{
    Task<OperationResult> AddEmployeeToRole(Ulid employeeId, Ulid roleId);
    Task<OperationResult> RemoveEmployeeFromRole(Ulid employeeId, Ulid roleId);
}