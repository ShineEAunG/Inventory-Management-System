using System.Threading.Tasks;
using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.Repository.Interfaces;
using InventoryManagementSystem.Services.Interfaces;
using InventoryManagementSystem.StaticClasses;
using InventoryManagementSystem.ViewModels;

namespace InventoryManagementSystem.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IRefreshTokenRepo _refreshRepo;
    private readonly IEmployeeRepo _employeeRepo;
    private readonly ITokenGeneratorService _tokenService;
    private string? accessTokenExpiration;
    private string? refreshTokenExpiration;
    public AuthService(IConfiguration configuration, IRefreshTokenRepo refreshRepo, IEmployeeRepo employeeRepo, ITokenGeneratorService tokenservice)
    {
        this._config = configuration;
        this._refreshRepo = refreshRepo;
        this._employeeRepo = employeeRepo;
        this._tokenService = tokenservice;
        accessTokenExpiration = _config["JwtSetting:AccessTokenExpiration"];
        refreshTokenExpiration = _config["JwtSetting:RefreshTokenExpiration"];
    }
    public async Task<AuthResponse> Refresh(string email, string refreshToken)
    {
        if (accessTokenExpiration is null || refreshTokenExpiration is null)
        {
            return GenerateFailedAuthResponse();
        }
        var employee = await _employeeRepo.GetEmployeeWithRolesByEmail(email);
        if (employee is null)
            return GenerateFailedAuthResponse();
        var hashedToken = _tokenService.Sha256HashRefreshToken(refreshToken);
        var tokenInDb = await _refreshRepo.GetByHashedRefreshToken(hashedToken, employee.EmployeeId);
        if (tokenInDb is null)
        {
            return GenerateFailedAuthResponse();
        }
        // get new access token
        var newAccessToken = _tokenService.GenerateAccessToken(employee);
        var refreshExpiresAt = DateTime.UtcNow.AddDays(int.Parse(refreshTokenExpiration!));
        var accessExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(accessTokenExpiration!));
        // if Refresh token is less than 1 day
        if (tokenInDb.ExpiresAt.Subtract(DateTime.UtcNow).TotalDays < 1)
        {
            var newAuthResponse = _tokenService.GenerateAuthResponse(employee);
            var newHashedRefreshToken = _tokenService.Sha256HashRefreshToken(newAuthResponse.RefreshToken);
            var newRefreshToken = new RefreshToken
            {
                RefreshTokenHash = newHashedRefreshToken,
                ExpiresAt = refreshExpiresAt,
                IsActive = true,
                EmployeeId = employee.EmployeeId
            };
            tokenInDb.IsActive = false;
            tokenInDb.RevokeAt = DateTime.UtcNow;
            tokenInDb.ReplacedTokenId = newRefreshToken.RefreshTokenId;
            await _refreshRepo.Add(newRefreshToken);
            refreshToken = newAuthResponse.RefreshToken;
        }
        return new AuthResponse
        {
            Success = true,
            AccessToken = newAccessToken,
            RefreshToken = refreshToken,
            AccessTokenExpireAt = accessExpiresAt,
            RefreshTokenExpiresAt = refreshExpiresAt
        };
    }

    public async Task<AuthResponse> ConfirmEmail(string email, string otp)
    {
        var employeeInDb = await _employeeRepo.GetEmployeeByEmail(email);
        if (employeeInDb is null)
        {
            return GenerateFailedAuthResponse();
        }
        if (employeeInDb.ConfirmationCode != otp)
        {
            return GenerateFailedAuthResponse();
        }
        var dto = await _employeeRepo.GetEmployeeWithRolesByEmail(employeeInDb.Email);
        var newAuthResponse = _tokenService.GenerateAuthResponse(dto!);
        var newRefreshToken = new RefreshToken
        {
            RefreshTokenHash = _tokenService.Sha256HashRefreshToken(newAuthResponse.RefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(refreshTokenExpiration!)),
            IsActive = true,
            EmployeeId = employeeInDb.EmployeeId
        };
        var addToDb = await _refreshRepo.Add(newRefreshToken);
        if (addToDb.Success)
            return newAuthResponse;
        return GenerateFailedAuthResponse();
    }


    public async Task<AuthResponse> LogIn(LoginEmployeeDto logInDto)
    {
        var employeeInDb = await _employeeRepo.GetEmployeeByEmail(logInDto.Email);
        if (employeeInDb is null)
        {
            return GenerateFailedAuthResponse();
        }
        bool verifyPassword = PasswordHasher.VerifyPassword(logInDto.Password, employeeInDb.PasswordHash);
        if (!verifyPassword)
        {
            return GenerateFailedAuthResponse();
        }
        var employeeDto = await _employeeRepo.GetEmployeeWithRolesByEmail(employeeInDb.Email);
        var newAuthResponse = _tokenService.GenerateAuthResponse(employeeDto!);
        var newRefreshToken = new RefreshToken
        {
            RefreshTokenHash = _tokenService.Sha256HashRefreshToken(newAuthResponse.RefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(refreshTokenExpiration!)),
            IsActive = true,
            EmployeeId = employeeInDb.EmployeeId
        };
        var addToDb = await _refreshRepo.Add(newRefreshToken);
        if (addToDb.Success)
            return newAuthResponse;
        return GenerateFailedAuthResponse();
    }

    public async Task<OperationResult> LogOut(string employeeEmail, string rawRefreshToken)
    {
        var hashedToken = _tokenService.Sha256HashRefreshToken(rawRefreshToken);
        var employeeInDb = await _employeeRepo.GetEmployeeByEmail(employeeEmail);
        if (employeeInDb is null)
        {
            return new OperationResult(false, $"There is no user with email{employeeEmail}");
        }
        var refreshToken = await _refreshRepo.GetByHashedRefreshToken(hashedToken, employeeInDb.EmployeeId);
        if (refreshToken is null)
        {
            return new OperationResult(false, $"There is no refresh token which is equivalent to {rawRefreshToken}");
        }
        return await _refreshRepo.Revoke(refreshToken.RefreshTokenId);
    }

    private AuthResponse GenerateFailedAuthResponse()
    {
        return new AuthResponse
        {
            Success = false,
            AccessTokenExpireAt = DateTime.UtcNow,
            RefreshTokenExpiresAt = DateTime.UtcNow
        };
    }    
}