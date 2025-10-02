using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.ViewModels;

namespace InventoryManagementSystem.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LogIn(LoginEmployeeDto dto);
    Task<OperationResult> LogOut(string employeeEmail ,string rawRefreshToken);
    Task<AuthResponse> Refresh(string email, string refreshToken);
    Task<AuthResponse> ConfirmEmail(string email, string otp);
}