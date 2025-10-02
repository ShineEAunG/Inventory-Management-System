using InventoryManagementSystem.Dtos;

namespace InventoryManagementSystem.Services.Interfaces;

public interface IEmailService
{
    Task<OperationResult> SendEmail(string toEmail, string emailContent, string subject);
}