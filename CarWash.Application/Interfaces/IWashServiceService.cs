using CarWash.Contracts.Dtos;

namespace CarWash.Application.Interfaces;

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