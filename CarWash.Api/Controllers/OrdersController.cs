using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>
/// Заказы мойки
/// </summary>
[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    /// <summary>
    /// Все заказы
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll() => Ok(await orderService.GetAllAsync());

    /// <summary>
    /// Заказ по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById(int id) => Ok(await orderService.GetByIdAsync(id));

    /// <summary>
    /// Оформить новый заказ
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] OrderRequest request)
    {
        OrderDto created = await orderService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Изменить заказ
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderRequest request)
    {
        await orderService.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>
    /// Удалить заказ
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await orderService.DeleteAsync(id);
        return NoContent();
    }
}