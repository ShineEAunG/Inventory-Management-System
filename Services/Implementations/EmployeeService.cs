using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.Repository.Interfaces;
using InventoryManagementSystem.Services.Interfaces;
using InventoryManagementSystem.StaticClasses;

namespace InventoryManagementSystem.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmailService _emailService;
    private readonly IEmployeeRepo _empRepo;
    public EmployeeService(IEmployeeRepo empRepo, IEmailService service)
    {
        _empRepo = empRepo;
        _emailService = service;
    }
    public async Task<EmployeeDetailsDto?> GetEmployee(string email)
    {
        return await _empRepo.GetEmployeeWithRolesByEmail(email);
    }

    public async Task<PaginatedList<EmployeeDetailsDto>> GetAll(string? keyWord)
    {
        return await _empRepo.GetAllEmployeesWithRoles(keyWord);
    }

    public async Task<OperationResult> RegisterOrCreate(RegisterEmployeeDto employeeDto)
    {
        var hashedPassword = PasswordHasher.HashPassword(employeeDto.Password);
        var otp = EmailGenerator.GenerateOtp();
        var newEmployee = new Employee
        {
            EmployeeName = employeeDto.Name,
            Email = employeeDto.Email,
            PasswordHash = hashedPassword,
            IsEmailConfirmed = false,
            CreatedBy = Creator.System,
            ConfirmationCode = otp,
            CreatedAt = DateTimeOffset.UtcNow
        };
        newEmployee = await _empRepo.Create(newEmployee);
        var addToRoleResult = await _empRepo.AddEmployeeToRoleAsync(employeeDto.Email, Roles.Employee);

        var sentEmail = await SendEmail(employeeDto.Email, otp);
        if (newEmployee is null|| !addToRoleResult.Success || !sentEmail.Success)
            return new OperationResult(false, addToRoleResult.Message);
        await _empRepo.SaveChangesAsync();
        return new OperationResult(true, $"Please go to the confirmation url");
    }

    public async Task<OperationResult> Delete(string email)
    {
        return await _empRepo.DeleteEmployee(email);
    }
    private async Task<OperationResult> SendEmail(string email, string otp)
    {
        string emailContent = EmailGenerator.GenerateEmailContent(otp);
        string subject = "Confirmation Email";
        var emailSent = await _emailService.SendEmail(email, emailContent, subject);
        return emailSent;
    }
}