using InventoryManagementSystem.Dtos;
using InventoryManagementSystem.Services.Interfaces;


namespace InventoryManagementSystem.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration configuration)
    {
        this._config = configuration;
    }
    private string? apiKey => _config["SendGrid:ApiKey"];
    private string? fromEmail => _config["SendGrid:FromEmail"];
    private string? fromName => _config["SendGrid:FromName"];
    private bool CheckConfig()
    {
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(fromName))
            return false;

        Console.WriteLine($"api key is {apiKey}");
        Console.WriteLine($"email is {fromEmail}");
        Console.WriteLine($"name is {fromName}");
        return true;
    }

    public async Task<OperationResult> SendEmail(string toEmail, string emailContent, string subject)
    {
        if (!CheckConfig())
        {
            return new OperationResult(false, "Failed sending confirmation email");
            // throw new InvalidOperationException("SendGrid Configuration is missing");
        }
            
        try
        {
            var client = new SendGrid.SendGridClient(apiKey);
            var from = new SendGrid.Helpers.Mail.EmailAddress(fromEmail, fromName);
            var to = new SendGrid.Helpers.Mail.EmailAddress(toEmail);
            var message = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(
                from: from,
                to: to,
                subject: subject,
                plainTextContent: emailContent,
                htmlContent: emailContent
            );
            var sendGridResponse = await client.SendEmailAsync(message);
            return new OperationResult(sendGridResponse.IsSuccessStatusCode, "Sent confirmation email Successfully");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return new OperationResult(false, "Failed sending confirmation email");
        }
    }
}