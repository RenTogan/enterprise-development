using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>
/// Управление автомобилями
/// </summary>
[ApiController]
[Route("api/cars")]
public class CarsController(ICarService carService) : ControllerBase
{
    /// <summary>
    /// Все машины
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<CarDto>>> GetAll() => Ok(await carService.GetAllAsync());

    /// <summary>
    /// Машина по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CarDto>> GetById(int id) => Ok(await carService.GetByIdAsync(id));

    /// <summary>
    /// Создать машину
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CarDto>> Create([FromBody] CarRequest request)
    {
        var created = await carService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить данные машины
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarRequest request)
    {
        await carService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Удалить машину
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await carService.DeleteAsync(id);
        return NoContent();
    }
}