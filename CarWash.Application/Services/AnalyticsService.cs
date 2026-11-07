using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using CarWash.Infrastructure.InMemory;

namespace CarWash.Application.Services;

/// <summary>
/// Аналитические выборки
/// </summary>
public class AnalyticsService(IStorageContext storage) : IAnalyticsService
{
    // Зап1: Топ клиентов по числу посещений
    public Task<List<TopClientDto>> GetTopClientsAsync(int count = 5)
    {
        var result = storage.Orders
            .GroupBy(o => o.Client)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => new TopClientDto(g.Key.Id, g.Key.FullName, g.Count()))
            .ToList();

        return Task.FromResult(result);
    }

    // Зап2: Автомобили на мойке прямо сейчас
    public Task<List<ActiveCarDto>> GetCurrentCarsAtWashAsync(DateTime? atTime = null)
    {
        DateTime time = atTime ?? DateTime.Now;

        var result = storage.Orders
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
            .ToList();

        return Task.FromResult(result);
    }

    // Зап3: Топ популярных услуг
    public Task<List<PopularServiceDto>> GetTopServicesAsync(int count = 5)
    {
        var result = storage.Orders
            .GroupBy(o => o.Service)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => new PopularServiceDto(g.Key.Id, g.Key.Name, g.Count()))
            .ToList();

        return Task.FromResult(result);
    }

    // Зап4: Время освобождения бокса
    public Task<BoxFreeTimeDto> GetBoxFreeTimeAsync(int boxNumber, DateTime? fromTime = null)
    {
        DateTime currentTime = fromTime ?? DateTime.Now;

        var busyOrders = storage.Orders
            .Where(o => o.BoxNumber == boxNumber && o.StartTime.AddMinutes(o.Service.DurationMinutes) > currentTime)
            .OrderBy(o => o.StartTime)
            .ToList();

        DateTime freeTime = currentTime;

        foreach (Order order in busyOrders)
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

        return Task.FromResult(new BoxFreeTimeDto(boxNumber, freeTime));
    }

    // Запр5: Суммарная выручка по каждой услуге
    public Task<List<ServiceRevenueDto>> GetServiceRevenuesAsync()
    {
        var result = storage.Services
            .Select(s =>
            {
                var revenue = storage.Orders
                    .Where(o => o.Service.Id == s.Id)
                    .Sum(o => o.Service.Price);

                return new ServiceRevenueDto(s.Id, s.Name, revenue);
            })
            .OrderByDescending(r => r.TotalRevenue)
            .ToList();

        return Task.FromResult(result);
    }
}