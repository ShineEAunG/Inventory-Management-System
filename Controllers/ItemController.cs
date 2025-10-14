using InventoryManagementSystem.Dtos.Inventory;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.Interface;
using InventoryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    public ItemController( IItemService itemService)
    {

        this._itemService = itemService;
    }
    [HttpGet("fileUrl/{generatedFileName}")]
    public IActionResult GetUrl([FromQuery]string generatedFileName)
    {
        if (ModelState.IsValid)
        {
            var url = _itemService.GetFileUrl(generatedFileName);
            return Ok(url);
        }
        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]ItemQueryParams queryParams )
    {
        var items = await _itemService.GetAll(queryParams);
        return Ok(items);
    }
    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetById(string itemId)
    {
        if (!Ulid.TryParse(itemId, out var ulid))
            return BadRequest("Invalid Ulid format.");

        var item = await _itemService.GetById(ulid);
        if (item is null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    [Authorize(Policy ="CanAdd")]
    public async Task<IActionResult> Create([FromForm]CreateItemDto itemDto)
    {
        var result = await _itemService.Create(itemDto.File, itemDto);
        if (result.Success)
            return Ok(new { result.Success, Message = $"{result.Message}" });
        return BadRequest(new { result.Success, Message = $"{result.Message}" });
    }
    [HttpDelete("{itemId}")]
    [Authorize(Policy ="CanDelete")]
    public async Task<IActionResult> Delete(string itemId)
    {
        if (!Ulid.TryParse(itemId, out var ulid))
            return BadRequest("Invalid Ulid format.");
        var result = await _itemService.Delete(ulid);
        return Ok(result);
    }
    [HttpPut]
    [Authorize(Policy ="CanEdit")]
    public async Task<IActionResult> Update([FromForm]UpdateItemDto itemDto)
    {
        if (!Ulid.TryParse(itemDto.ItemId, out var ulid))
            return BadRequest("Invalid Ulid format.");
        var result = await _itemService.Update(itemDto.File, ulid, itemDto);
        if (result.Success)
            return Ok(new { result.Success, Message = $"{result.Message}" });
        return BadRequest(new { result.Success, Message = $"{result.Message}" });
    }
}