namespace InventoryManagementSystem.ViewModels;

public class AuthResponse
{
    public bool Success { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpireAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
}