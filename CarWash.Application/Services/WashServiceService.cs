using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using CarWash.Infrastructure.InMemory;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса услуг
/// </summary>
public class WashServiceService(IStorageContext storage) : IWashServiceService
{
    public Task<List<ServiceDto>> GetAllAsync()
    {
        var result = storage.Services.Select(s => new ServiceDto
        {
            Id = s.Id,
            Name = s.Name,
            Category = s.Category,
            Price = s.Price,
            DurationMinutes = s.DurationMinutes
        }).ToList();

        return Task.FromResult(result);
    }

    public Task<ServiceDto> GetByIdAsync(int id)
    {
        Service s = storage.Services.FirstOrDefault(x => x.Id == id)
            ?? throw new NotFoundException($"Услуга с Id {id} не найдена");

        return Task.FromResult(new ServiceDto
        {
            Id = s.Id,
            Name = s.Name,
            Category = s.Category,
            Price = s.Price,
            DurationMinutes = s.DurationMinutes
        });
    }

    public Task<ServiceDto> CreateAsync(ServiceRequest request)
    {
        var service = new Service
        {
            Id = storage.NextServiceId(),
            Name = request.Name.Trim(),
            Category = request.Category!.Value,
            Price = request.Price!.Value,
            DurationMinutes = request.DurationMinutes!.Value
        };

        storage.Services.Add(service);

        return Task.FromResult(new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Category = service.Category,
            Price = service.Price,
            DurationMinutes = service.DurationMinutes
        });
    }

    public Task UpdateAsync(int id, ServiceRequest request)
    {
        Service service = storage.Services.FirstOrDefault(s => s.Id == id)
            ?? throw new NotFoundException($"Услуга с Id {id} не найдена");

        service.Name = request.Name.Trim();
        service.Category = request.Category!.Value;
        service.Price = request.Price!.Value;
        service.DurationMinutes = request.DurationMinutes!.Value;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Service service = storage.Services.FirstOrDefault(s => s.Id == id)
            ?? throw new NotFoundException($"услуга с Id {id} не найдена");

        if (storage.Orders.Any(o => o.Service.Id == id))
        {
            throw new ConflictException("Нельзя удалить услугу, которая фигурирует в заказах");
        }

        storage.Services.Remove(service);
        return Task.CompletedTask;
    }
}