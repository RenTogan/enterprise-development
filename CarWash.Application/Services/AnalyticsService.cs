using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using CarWash.Infrastructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Application.Services;

/// <summary>
/// Аналитические выборки
/// </summary>
public class AnalyticsService(CarWashDbContext db) : IAnalyticsService
{
    // Зап1: Топ клиентов по числу посещений
    public async Task<List<TopClientDto>> GetTopClientsAsync(int count = 5)
    {
        return await db.Orders
            .GroupBy(o => new { o.Client.Id, o.Client.FullName })
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => new TopClientDto(g.Key.Id, g.Key.FullName, g.Count()))
            .ToListAsync();
    }

    // Зап2: Автомобили на мойке прямо сейчас
    public async Task<List<ActiveCarDto>> GetCurrentCarsAtWashAsync(DateTime? atTime = null)
    {
        DateTime time = atTime ?? DateTime.Now;

        return await db.Orders
            .Where(o => o.StartTime <= time && time < o.StartTime.AddMinutes(o.Service.DurationMinutes))
            .Select(o => new ActiveCarDto(
                o.Car.Id,
                o.Car.LicensePlate,
                o.Car.Brand,
                o.BoxNumber,
                o.Service.Name,
                o.StartTime,
                o.StartTime.AddMinutes(o.Service.DurationMinutes)
            ))
            .ToListAsync();
    }

    // Зап3: Топ популярных услуг
    public async Task<List<PopularServiceDto>> GetTopServicesAsync(int count = 5)
    {
        return await db.Orders
            .GroupBy(o => new { o.Service.Id, o.Service.Name })
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => new PopularServiceDto(g.Key.Id, g.Key.Name, g.Count()))
            .ToListAsync();
    }

    // Зап4: Время освобождения бокса
    public async Task<BoxFreeTimeDto> GetBoxFreeTimeAsync(int boxNumber, DateTime? fromTime = null)
    {
        DateTime currentTime = fromTime ?? DateTime.Now;

        List<Order> busyOrders = await db.Orders
            .Include(o => o.Service)
            .Where(o => o.BoxNumber == boxNumber)
            .OrderBy(o => o.StartTime)
            .ToListAsync();

        DateTime freeTime = currentTime;

        foreach (Order? order in busyOrders.Where(o => o.StartTime.AddMinutes(o.Service.DurationMinutes) > currentTime))
        {
            DateTime orderEndTime = order.StartTime.AddMinutes(order.Service.DurationMinutes);

            if (order.StartTime <= freeTime)
            {
                if (orderEndTime > freeTime)
                {
                    freeTime = orderEndTime;
                }
            }
            else
            {
                break;
            }
        }

        return new BoxFreeTimeDto(boxNumber, freeTime);
    }

    // Запр5: Суммарная выручка по каждой услуге
    public async Task<List<ServiceRevenueDto>> GetServiceRevenuesAsync()
    {
        return await db.Services
            .GroupJoin(
                db.Orders,
                s => s.Id,
                o => o.Service.Id,
                (service, orders) => new
                {
                    service.Id,
                    service.Name,
                    TotalRevenue = orders.Sum(o => (decimal?)o.Service.Price) ?? 0m
                }
            )
            .OrderByDescending(r => r.TotalRevenue)
            .Select(r => new ServiceRevenueDto(r.Id, r.Name, r.TotalRevenue))
            .ToListAsync();
    }
}