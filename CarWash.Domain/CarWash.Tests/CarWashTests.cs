using CarWash.Domain.Entities;

namespace CarWash.Tests;

public class CarWashTests : IClassFixture<CarWashFixture>
{
    private readonly CarWashFixture _fixture;

    public CarWashTests(CarWashFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Топ 5 клиентов по количеству посещений
    /// </summary>
    [Fact]
    public void GetTop5Clients_ReturnsCorrectOrder()
    {
        var topClients = _fixture.Orders
            .GroupBy(o => o.Client)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key)
            .ToList();

        Assert.Equal(5, topClients.Count);
        Assert.Equal("Яцентюк Данила", topClients[0].FullName);
    }

    /// <summary>
    /// Машины, которые моются прямо сейчас
    /// </summary>
    [Fact]
    public void GetCurrentCarsAtWash_ReturnsActiveCars()
    {
        // на момент времени 10:15
        var currentTime = new DateTime(2026, 5, 10, 10, 15, 0);

        var currentCars = _fixture.Orders
            .Where(o => o.StartTime <= currentTime && currentTime < o.StartTime.AddMinutes(o.Service.DurationMinutes))
            .Select(o => o.Car)
            .ToList();

        // 4 машины
        //car1 (10:00-10:20), car2 (10:00-10:45), car3 (10:00-11:00), car4 (10:10-10:40)
        Assert.Equal(4, currentCars.Count);
        Assert.Contains(currentCars, c => c.LicensePlate == "А111АА163");
        Assert.Contains(currentCars, c => c.LicensePlate == "В222ВВ163");
        Assert.Contains(currentCars, c => c.LicensePlate == "С333СС163");
        Assert.Contains(currentCars, c => c.LicensePlate == "Е444ЕЕ163");
    }

    /// <summary>
    /// Топ 5 самых популярных услуг
    /// </summary>
    [Fact]
    public void GetTop5PopularServices_ReturnsCorrectServices()
    {
        var topServices = _fixture.Orders
            .GroupBy(o => o.Service)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key)
            .ToList();

        Assert.True(topServices.Count <= 5);
        Assert.Equal("Экспресс мойка", topServices[0].Name); // 3 заказа
        Assert.Equal("Комплексная мойка", topServices[1].Name); //3 заказа
    }

    /// <summary>
    /// Время освобождения выбранного бокса
    /// </summary>
    [Fact]
    public void GetBoxFreeTime_ReturnsNextAvailableTime()
    {
        var targetBox = 1;
        // Время проверки 10:10
        // в боксе 1 идет экспресс мойка с 10:00 до 10:20.
        var currentTime = new DateTime(2026, 5, 10, 10, 10, 0);

        // все заказы в боксе, которые закончатся после текущего времени
        var busyOrders = _fixture.Orders
            .Where(o => o.BoxNumber == targetBox && o.StartTime.AddMinutes(o.Service.DurationMinutes) > currentTime)
            .OrderBy(o => o.StartTime)
            .ToList();

        DateTime freeTime = currentTime;

        foreach (Order? order in busyOrders)
        {
            DateTime orderEndTime = order.StartTime.AddMinutes(order.Service.DurationMinutes);

            // если заказ идет прямо сейчас/ накладывается на текущее время освобождения
            if (order.StartTime <= freeTime)
            {
                if (orderEndTime > freeTime)
                {
                    freeTime = orderEndTime;
                }
            }
            else
            {
                //если между заказами есть перерыв, бокс освободится к началу этого окна
                break;
            }
        }

        // В 10:20 бокс освободится (первый заказ с 10:00 до 10:20, следующий в этом боксе только в 12:00)
        Assert.Equal(new DateTime(2026, 5, 10, 10, 20, 0), freeTime);
    }

    /// <summary>
    /// Суммарная выручка по каждой услуге
    /// </summary>
    [Fact]
    public void GetRevenueByService_CalculatesTotalRevenue()
    {
        var serviceRevenue = _fixture.Orders
            .GroupBy(o => o.Service)
            .Select(g => new
            {
                ServiceName = g.Key.Name,
                TotalRevenue = g.Sum(o => o.Service.Price)
            })
            .ToList();

        // Экспресс мойка 3 заказа * 500 = 1500
        var expressRevenue = serviceRevenue.First(s => s.ServiceName == "Экспресс мойка");
        Assert.Equal(1500, expressRevenue.TotalRevenue);

        // Комплексная мойка 3 заказа * 1200 = 3600
        var complexRevenue = serviceRevenue.First(s => s.ServiceName == "Комплексная мойка");
        Assert.Equal(3600, complexRevenue.TotalRevenue);
    }
}