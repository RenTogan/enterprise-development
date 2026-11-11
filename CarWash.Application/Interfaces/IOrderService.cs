using CarWash.Contracts.Dtos;

namespace CarWash.Application.Interfaces;

/// <summary>
/// Сервис заказов
/// </summary>
public interface IOrderService
{
    public Task<List<OrderDto>> GetAllAsync();
    public Task<OrderDto> GetByIdAsync(int id);
    public Task<OrderDto> CreateAsync(OrderRequest request);
    public Task UpdateAsync(int id, OrderRequest request);
    public Task DeleteAsync(int id);
}