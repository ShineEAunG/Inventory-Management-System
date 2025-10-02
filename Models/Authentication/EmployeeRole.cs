
namespace InventoryManagementSystem.Models.Authentication;
public class EmployeeRole
{
    public Ulid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public Ulid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}