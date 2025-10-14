using InventoryManagementSystem.Dtos.AuthDto;
using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService service)
    {
        this._authService = service;
    }
    [HttpPost]
    public async Task<IActionResult> LogIn([FromBody]LoginEmployeeDto employeeDto)
    {
        if (ModelState.IsValid)
        {
            var authResponse = await _authService.LogIn(employeeDto);
            return Ok(authResponse);
        }
        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> LogOut([FromBody]LogOutDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.LogOut(dto.Email, dto.RefreshToken);
            return Ok(result);
        }
        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmationDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.ConfirmEmail(dto.Email, dto.Otp);
            return Ok(result);
        }
        return BadRequest();
    }
    [HttpPost]
    public async Task<IActionResult> Refresh([FromBody]RefreshDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.Refresh(dto.Email, dto.RefreshToken);
            if (result.Success)
                return Ok(result);
        }
        return BadRequest();
    }
    
}
