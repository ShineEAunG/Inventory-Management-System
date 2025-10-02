using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.ViewModels;

namespace InventoryManagementSystem.Services.Interfaces;

public interface ITokenGeneratorService
{
    AuthResponse GenerateAuthResponse(EmployeeDetailsDto employee);
    string GenerateRawRefreshToken();
    string GenerateAccessToken(EmployeeDetailsDto employee);
    string Sha256HashRefreshToken(string rawToken);
}