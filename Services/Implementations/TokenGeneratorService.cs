using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.Services.Interfaces;
using InventoryManagementSystem.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagementSystem.Services.Implementations;

//static class here cannot be injected so we need to make it service class
public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly IConfiguration _config;
    public TokenGeneratorService(IConfiguration configuration)
    {
        this._config = configuration;
    }
    private string? issure => _config["JwtSetting:Issuer"];
    private string? audience => _config["JwtSetting:Audience"];
    private string? secretKey => _config["JwtSetting:Secret"];
    private string? accessTokenExpiration => _config["JwtSetting:AccessTokenExpiration"];
    private string? refreshTokenExpiration => _config["JwtSetting:RefreshTokenExpiration"];

    private AuthResponse CheckConfiguration()
    {
        if (string.IsNullOrEmpty(issure) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(accessTokenExpiration) || string.IsNullOrEmpty(refreshTokenExpiration))
        {
            return GenerateFakeOfAuthResponse(false);
        }
        return GenerateFakeOfAuthResponse(true);
    }
    private AuthResponse GenerateFakeOfAuthResponse(bool check)
    {
        return new AuthResponse
        {
            Success = check,
            AccessToken = string.Empty,
            RefreshToken = string.Empty,
            AccessTokenExpireAt = DateTime.UtcNow,
            RefreshTokenExpiresAt = DateTime.UtcNow
        };
    }
    public AuthResponse GenerateAuthResponse(EmployeeDetailsDto employee)
    {
        var config = CheckConfiguration();
        if (!config.Success)
        {
            return config;
        }
        var newAuthResponse = new AuthResponse
        {
            Success = true,
            AccessToken = GenerateAccessToken(employee),
            RefreshToken = GenerateRawRefreshToken(),
            AccessTokenExpireAt = DateTime.UtcNow.AddMinutes(int.Parse(accessTokenExpiration!)),
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(int.Parse(refreshTokenExpiration!))
        };
        return newAuthResponse;
    }
    public string GenerateRawRefreshToken()
    {
        var tokenSize = 64; //   512 bits
        byte[] newBytes = new byte[tokenSize];
        using (var rnd = RandomNumberGenerator.Create())
        {
            rnd.GetBytes(newBytes);
        }
        var rawRefreshToken = Convert.ToBase64String(newBytes);
        return rawRefreshToken;
    }
    public string GenerateAccessToken(EmployeeDetailsDto employee)
    {
        //jwt header
        var symmetricKeyBytes = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(secretKey!));
        var shaAlgorithm = SecurityAlgorithms.HmacSha256;
        var signingCredentials = new SigningCredentials(symmetricKeyBytes, shaAlgorithm);

        //jwt payload
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sub, employee.EmployeeId.ToString()),
            new (ClaimTypes.Email, employee.Email)
        };
        foreach (var role in employee.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        foreach (var permission in employee.Permissions)
        {
            claims.Add(new Claim("permission", permission));
        }


        // Token Descriptor => Token Handler => Create Token => Write Token
        var identity = new ClaimsIdentity(claims, "forJWT");
        var tokenDiscriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = issure,
            Audience = audience,
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(accessTokenExpiration!)),
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        //tokenHandler.OutboundClaimTypeMap.Clear();  // this here is important coz we add customized Permission up there
        var securityToken = tokenHandler.CreateToken(tokenDiscriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return token;
    }
    public string Sha256HashRefreshToken(string rawToken)
    {
        using var sha256 = SHA256.Create(); // Standard algorithm for hashing
        {
            var byteOfRefreshToken = Encoding.UTF8.GetBytes(rawToken);
            var bytesHashed = sha256.ComputeHash(byteOfRefreshToken);
            string bytesTo64String = Convert.ToBase64String(bytesHashed);
            return bytesTo64String;
        }
    }
}