using CarWash.Contracts.Dtos;

namespace CarWash.Application.Interfaces;

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