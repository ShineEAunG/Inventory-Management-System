using InventoryManagementSystem.Dtos.EmployeeDto;
using InventoryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;
    public EmployeeController(IEmployeeService service)
    {
        this._service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]EmployeeQueryParams queryParams)
    {
        var employees = await _service.GetAll(queryParams);
        return Ok(employees);
    }
    [HttpGet("{email}")]
    public async Task<IActionResult> Get(string email)
    {
        var employee = await _service.GetEmployee(email);
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterEmployeeDto dto)
    {
        var operationResult = await _service.RegisterOrCreate(dto);
        return Ok(operationResult);
    }
    [Authorize(Policy ="CanDelete")]
    [HttpDelete("{email}")]
    public async Task<IActionResult> Delete(string email)
    {
        var operationResult = await _service.Delete(email);
        return Ok(operationResult);
    }
}
