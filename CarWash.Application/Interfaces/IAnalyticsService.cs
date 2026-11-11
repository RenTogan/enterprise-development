using CarWash.Contracts.Dtos;

namespace CarWash.Application.Interfaces;

/// <summary>
/// Сервис аналитических запросов
/// </summary>
public interface IAnalyticsService
{
    public Task<List<TopClientDto>> GetTopClientsAsync(int count = 5);
    public Task<List<ActiveCarDto>> GetCurrentCarsAtWashAsync(DateTimeOffset? atTime = null);
    public Task<List<PopularServiceDto>> GetTopServicesAsync(int count = 5);
    public Task<BoxFreeTimeDto> GetBoxFreeTimeAsync(int boxNumber, DateTimeOffset? fromTime = null);
    public Task<List<ServiceRevenueDto>> GetServiceRevenuesAsync();
}