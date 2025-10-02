using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventoryManagementSystem.Dtos.EmployeeDto;

public class RegisterEmployeeDto
{
    // this data annotation will available in runtime
    [Required(ErrorMessage = "User name is required")]
    public string Name { get; set; } = string.Empty;
    
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    [StrongPassword]    // here check if the password got special or upper at least one
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    public string? ConfirmPassword { get; set; }
}


public class LoginEmployeeDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}

public class EmployeeDetailsDto
{
    public Ulid EmployeeId { get; set; }
    public string Email { get; set; } = null!;
    public string EmployeeName { get; set; } = null!;
    //public List<string> Roles = []; this is field it will not showup through api but can be manual access
    // if you still want to access in backend and didnt want to expose then use [jsonignore]
    [JsonIgnore] // or if you want to show to public just remove this attribute
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}