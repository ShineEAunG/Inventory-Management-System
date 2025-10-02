namespace InventoryManagementSystem.Models.Authentication;
public class Employee
{
    public Ulid EmployeeId { get; set; } = Ulid.NewUlid();
    public string EmployeeName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }
    public string? ConfirmationCode { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public DateTimeOffset? LastLogin { get; set; }
    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = [];
}