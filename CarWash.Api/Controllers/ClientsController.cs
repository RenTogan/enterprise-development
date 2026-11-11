using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>
/// Управление клиентами
/// </summary>
[ApiController]
[Route("api/clients")]
public class ClientsController(IClientService clientService) : ControllerBase
{
    /// <summary>
    /// Получить всех клиентов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetAll() => Ok(await clientService.GetAllAsync());

    /// <summary>
    /// Получить клиента по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientDto>> GetById(int id) => Ok(await clientService.GetByIdAsync(id));

    /// <summary>
    /// Создать клиента
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create([FromBody] ClientRequest request)
    {
        var created = await clientService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить клиента
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClientRequest request)
    {
        await clientService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Удалить клиента
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await clientService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Получить список авто клиента
    /// </summary>
    [HttpGet("{id:int}/cars")]
    public async Task<ActionResult<List<CarDto>>> GetCars(int id) => Ok(await clientService.GetClientCarsAsync(id));

    /// <summary>
    /// Получить все заказы клиента
    /// </summary>
    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult<List<OrderDto>>> GetOrders(int id) => Ok(await clientService.GetClientOrdersAsync(id));
}