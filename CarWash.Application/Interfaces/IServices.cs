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

/// <summary>
/// Сервис работы с машинами
/// </summary>
public interface ICarService
{
    public Task<List<CarDto>> GetAllAsync();
    public Task<CarDto> GetByIdAsync(int id);
    public Task<CarDto> CreateAsync(CarRequest request);
    public Task UpdateAsync(int id, CarRequest request);
    public Task DeleteAsync(int id);
}

/// <summary>
/// Сервис услуг мойки
/// </summary>
public interface IWashServiceService
{
    public Task<List<ServiceDto>> GetAllAsync();
    public Task<ServiceDto> GetByIdAsync(int id);
    public Task<ServiceDto> CreateAsync(ServiceRequest request);
    public Task UpdateAsync(int id, ServiceRequest request);
    public Task DeleteAsync(int id);
}

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

/// <summary>
/// Сервис аналитических запросов
/// </summary>
public interface IAnalyticsService
{
    public Task<List<TopClientDto>> GetTopClientsAsync(int count = 5);
    public Task<List<ActiveCarDto>> GetCurrentCarsAtWashAsync(DateTime? atTime = null);
    public Task<List<PopularServiceDto>> GetTopServicesAsync(int count = 5);
    public Task<BoxFreeTimeDto> GetBoxFreeTimeAsync(int boxNumber, DateTime? fromTime = null);
    public Task<List<ServiceRevenueDto>> GetServiceRevenuesAsync();
}