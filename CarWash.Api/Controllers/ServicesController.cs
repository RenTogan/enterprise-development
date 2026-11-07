using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>
/// Услуги мойки
/// </summary>
[ApiController]
[Route("api/services")]
public class ServicesController(IWashServiceService washService) : ControllerBase
{
    /// <summary>
    /// Все услуги
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ServiceDto>>> GetAll() => Ok(await washService.GetAllAsync());

    /// <summary>
    /// Услуга по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceDto>> GetById(int id) => Ok(await washService.GetByIdAsync(id));

    /// <summary>
    /// Добавить услугу
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ServiceDto>> Create([FromBody] ServiceRequest request)
    {
        ServiceDto created = await washService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить услугу
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceRequest request)
    {
        await washService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Удалить услугу
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await washService.DeleteAsync(id);
        return NoContent();
    }
}