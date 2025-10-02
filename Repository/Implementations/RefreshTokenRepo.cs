using System.Data.Common;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Repository.Implementations;

public class RefreshTokenRepo : IRefreshTokenRepo
{
    private readonly AppDbContext _dbcontext;
    public RefreshTokenRepo(AppDbContext dbcontext)
    {
        this._dbcontext = dbcontext;
    }
    public async Task<OperationResult> Add(RefreshToken refreshToken)
    {
        await _dbcontext.RefreshTokens.AddAsync(refreshToken);
        await _dbcontext.SaveChangesAsync();
        return new OperationResult(Success: true, Message: $"refresh token added succecssfully",null);

    }
    public async Task<RefreshToken?> GetByHashedRefreshToken(string hashRefreshToken , Ulid employeeId)
    {
        return await _dbcontext.RefreshTokens.FirstOrDefaultAsync(rt => rt.RefreshTokenHash == hashRefreshToken
            && rt.EmployeeId == employeeId
            && rt.IsActive
            && rt.RevokeAt == null);
    }


    // force revocation and log out 
    public async Task<OperationResult> Revoke(Ulid tokenId)
    {
        var result = await TryRevokeAndDeactivate(tokenId);
        await SaveChangeAsync();
        return result;
    }

    private async Task<OperationResult> TryRevokeAndDeactivate(Ulid tokenId)
    {
        var refreshTokenInDb = await GetById(tokenId);
        if (refreshTokenInDb is null)
        {
            return new OperationResult(Success: false, Message: $"no id {tokenId}", null);
        }
        refreshTokenInDb.IsActive = false;
        refreshTokenInDb.RevokeAt = DateTimeOffset.UtcNow;
        return new OperationResult(Success: true, Message: $"refresh token revoked succecssfully");
    }
    private async Task<RefreshToken?> GetById(Ulid refreshTokenId)
    {
        var refreshTokenInDb = await _dbcontext.RefreshTokens
            .FirstOrDefaultAsync(r => r.RefreshTokenId == refreshTokenId);
        return refreshTokenInDb;
    }

    private async Task SaveChangeAsync()
    {
        await _dbcontext.SaveChangesAsync();
    }

    
}