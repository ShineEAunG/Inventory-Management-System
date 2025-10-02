namespace InventoryManagementSystem.Dtos.AuthDto;

public class RefreshDto
{
    public required string Email { get; set; }
    public required string RefreshToken { get; set; }
}

public class ConfirmationDto
{
    public required string Email { get; set; }
    public required string Otp { get; set; }
}
public class LogOutDto: RefreshDto
{

}