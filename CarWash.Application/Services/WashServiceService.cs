using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса услуг
/// </summary>
public class WashServiceService(ICarWashDbContext db) : IWashServiceService
{
    public async Task<List<ServiceDto>> GetAllAsync()
    {
        return await db.Services
            .AsNoTracking()
            .Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Category = s.Category,
                Price = s.Price,
                DurationMinutes = s.DurationMinutes
            })
            .ToListAsync();
    }

    public async Task<ServiceDto> GetByIdAsync(int id)
    {
        var s = await db.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Услуга с Id {id} не найдена");

        return new ServiceDto
        {
            Id = s.Id,
            Name = s.Name,
            Category = s.Category,
            Price = s.Price,
            DurationMinutes = s.DurationMinutes
        };
    }

    public async Task<ServiceDto> CreateAsync(ServiceRequest request)
    {
        var service = new Service
        {
            Name = request.Name.Trim(),
            Category = request.Category!.Value,
            Price = request.Price!.Value,
            DurationMinutes = request.DurationMinutes!.Value
        };

        db.Services.Add(service);
        await db.SaveChangesAsync();

        return new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Category = service.Category,
            Price = service.Price,
            DurationMinutes = service.DurationMinutes
        };
    }

    public async Task UpdateAsync(int id, ServiceRequest request)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new NotFoundException($"Услуга с Id {id} не найдена");

        service.Name = request.Name.Trim();
        service.Category = request.Category!.Value;
        service.Price = request.Price!.Value;
        service.DurationMinutes = request.DurationMinutes!.Value;

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new NotFoundException($"Услуга с Id {id} не найдена");

        if (await db.Orders.AnyAsync(o => o.Service.Id == id))
        {
            throw new ConflictException("Нельзя удалить услугу, которая есть в заказах");
        }

        db.Services.Remove(service);
        await db.SaveChangesAsync();
    }
}