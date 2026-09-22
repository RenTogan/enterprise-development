using CarWash.Domain.Entities;

namespace CarWash.Tests;

public class CarWashTests(CarWashFixture fixture) : IClassFixture<CarWashFixture>
{
    private readonly CarWashFixture _fixture = fixture;

    /// <summary>
    /// Топ 5 клиентов по количеству посещений
    /// </summary>
    [Fact]
    public void GetTop5Clients_ReturnsCorrectOrder()
    {
        List<string> expectedNames =
        [
            "Яцентюк Данила",
            "Мансуров Роман",
            "Сарычев Никита",
            "Смирнова Анна",
            "Кузнецов Александр"
        ];

        var actualNames = _fixture.Orders
            .GroupBy(o => o.Client)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key.FullName)
            .ToList();

        Assert.Equal(expectedNames, actualNames);
    }

    /// <summary>
    /// Машины, которые моются прямо сейчас
    /// </summary>
    [Fact]
    public void GetCurrentCarsAtWash_ReturnsActiveCars()
    {
        // На момент времени 10:15
        var currentTime = new DateTime(2026, 5, 10, 10, 15, 0);
        // car1 (10:00-10:20), car2 (10:00-10:45), car3 (10:00-11:00), car4 (10:10-10:40)
        List<string> expectedPlates =
        [
            "А111АА163",
            "В222ВВ163",
            "С333СС163",
            "Е444ЕЕ163"
        ];

        var actualPlates = _fixture.Orders
            .Where(o => o.StartTime <= currentTime && currentTime < o.StartTime.AddMinutes(o.Service.DurationMinutes))
            .Select(o => o.Car.LicensePlate)
            .ToList();

        Assert.Equal(expectedPlates, actualPlates);
    }

    /// <summary>
    /// Топ 5 самых популярных услуг
    /// </summary>
    [Fact]
    public void GetTop5PopularServices_ReturnsCorrectServices()
    {
        List<string> expectedServices =
        [
            "Экспресс мойка",
            "Комплексная мойка",
            "Мойка грузовика",
            "Мойка двигателя",
            "Чернение шин"
        ];

        var actualServices = _fixture.Orders
            .GroupBy(o => o.Service)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key.Name)
            .ToList();

        Assert.Equal(expectedServices, actualServices);
    }

    /// <summary>
    /// Время освобождения выбранного бокса
    /// </summary>
    [Fact]
    public void GetBoxFreeTime_ReturnsNextAvailableTime()
    {
        var targetBox = 1;
        // Время проверки 10:10
        // В боксе 1 идет экспресс мойка с 10:00 до 10:20.
        var currentTime = new DateTime(2026, 5, 10, 10, 10, 0);
        // В 10:20 бокс освободится (первый заказ с 10:00 до 10:20, следующий в этом боксе только в 12:00)
        var expectedFreeTime = new DateTime(2026, 5, 10, 10, 20, 0);

        // Все заказы в боксе, которые закончатся после текущего времени
        var busyOrders = _fixture.Orders
            .Where(o => o.BoxNumber == targetBox && o.StartTime.AddMinutes(o.Service.DurationMinutes) > currentTime)
            .OrderBy(o => o.StartTime)
            .ToList();

        DateTime freeTime = currentTime;

        foreach (Order? order in busyOrders)
        {
            DateTime orderEndTime = order.StartTime.AddMinutes(order.Service.DurationMinutes);

            // Если заказ идет прямо сейчас / накладывается на текущее время освобождения
            if (order.StartTime <= freeTime)
            {
                if (orderEndTime > freeTime)
                {
                    freeTime = orderEndTime;
                }
            }
            else
            {
                // Если между заказами есть перерыв, бокс освободится к началу этого окна
                break;
            }
        }

        Assert.Equal(expectedFreeTime, freeTime);
    }

    /// <summary>
    /// Суммарная выручка по каждой услуге
    /// </summary>
    [Fact]
    public void GetRevenueByService_CalculatesTotalRevenue()
    {
        // Экспресс мойка 5 заказа * 500 = 2500
        var expectedExpressRevenue = 2500m;
        // Комплексная мойка 4 заказа * 1200 = 4800
        var expectedComplexRevenue = 4800m;

        var expressRevenue = _fixture.Orders
            .Where(o => o.Service.Name == "Экспресс мойка")
            .Sum(o => o.Service.Price);

        var complexRevenue = _fixture.Orders
            .Where(o => o.Service.Name == "Комплексная мойка")
            .Sum(o => o.Service.Price);

        Assert.Equal(expectedExpressRevenue, expressRevenue);
        Assert.Equal(expectedComplexRevenue, complexRevenue);
    }
}