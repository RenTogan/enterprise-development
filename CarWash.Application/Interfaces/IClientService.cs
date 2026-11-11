using CarWash.Contracts.Dtos;

namespace CarWash.Application.Interfaces;

/// <summary>
/// Сервис работы с клиентами
/// </summary>
public interface IClientService
{
    public Task<List<ClientDto>> GetAllAsync();
    public Task<ClientDto> GetByIdAsync(int id);
    public Task<ClientDto> CreateAsync(ClientRequest request);
    public Task UpdateAsync(int id, ClientRequest request);
    public Task DeleteAsync(int id);
    public Task<List<CarDto>> GetClientCarsAsync(int clientId);
    public Task<List<OrderDto>> GetClientOrdersAsync(int clientId);
}