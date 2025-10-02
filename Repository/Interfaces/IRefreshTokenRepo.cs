using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Models.Authentication;

namespace InventoryManagementSystem.Repository.Interfaces;

public interface IRefreshTokenRepo
{
    Task<OperationResult> Add(RefreshToken refreshToken);
    Task<OperationResult> Revoke(Ulid tokenId);
    Task<RefreshToken?> GetByHashedRefreshToken(string hashRefreshToken, Ulid employeeId);
}