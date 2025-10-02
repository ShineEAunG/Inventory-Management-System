namespace InventoryManagementSystem.Models.Authentication;
public class RefreshToken
{
    public Ulid RefreshTokenId { get; set; } = Ulid.NewUlid();
    public string RefreshTokenHash { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    // public bool IsActive => RevokeAt == null; // this is only c# code the column will not appear in db table
    public bool IsActive { get; set; }
    public DateTimeOffset? RevokeAt { get; set; }
    public Ulid? ReplacedTokenId { get; set; }
    public Ulid EmployeeId { get; set; }   
    public Employee? Employee { get; set; }
}