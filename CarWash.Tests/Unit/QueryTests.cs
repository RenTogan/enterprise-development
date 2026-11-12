using CarWash.Tests.Fixtures;

namespace CarWash.Tests.Unit;

/// <summary>
/// Тесты пяти аналитических запросов из 1ой лабы
/// </summary>
public class QueryTests(CarWashFixture fixture) : IClassFixture<CarWashFixture>
{
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

        var actualNames = fixture.Orders
            .GroupBy(o => o.Client)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key.FullName)
            .ToList();

        Assert.Equal(expectedNames, actualNames);
    }

    [Fact]
    public void GetCurrentCarsAtWash_ReturnsActiveCars()
    {
        var currentTime = new DateTime(2026, 5, 10, 10, 15, 0);
        List<string> expectedPlates =
        [
            "А111АА163",
            "В222ВВ163",
            "С333СС163",
            "Е444ЕЕ163"
        ];

        var actualPlates = fixture.Orders
            .Where(o => o.StartTime <= currentTime && currentTime < o.StartTime.AddMinutes(o.Service.DurationMinutes))
            .Select(o => o.Car.LicensePlate)
            .ToList();

        Assert.Equal(expectedPlates, actualPlates);
    }

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

        var actualServices = fixture.Orders
            .GroupBy(o => o.Service)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => g.Key.Name)
            .ToList();

        Assert.Equal(expectedServices, actualServices);
    }

    [Fact]
    public void GetBoxFreeTime_ReturnsNextAvailableTime()
    {
        var targetBox = 1;
        var currentTime = new DateTime(2026, 5, 10, 10, 10, 0);
        var expectedFreeTime = new DateTime(2026, 5, 10, 10, 20, 0);

        var busyOrders = fixture.Orders
            .Where(o => o.BoxNumber == targetBox && o.StartTime.AddMinutes(o.Service.DurationMinutes) > currentTime)
            .OrderBy(o => o.StartTime)
            .ToList();

        var freeTime = currentTime;

        foreach (var order in busyOrders)
        {
            var orderEndTime = order.StartTime.AddMinutes(order.Service.DurationMinutes);
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

        Assert.Equal(expectedFreeTime, freeTime);
    }

    [Fact]
    public void GetRevenueByService_CalculatesTotalRevenue()
    {
        var expectedExpressRevenue = 2500m;
        var expectedComplexRevenue = 4800m;

        var expressRevenue = fixture.Orders
            .Where(o => o.Service.Name == "Экспресс мойка")
            .Sum(o => o.Service.Price);

        var complexRevenue = fixture.Orders
            .Where(o => o.Service.Name == "Комплексная мойка")
            .Sum(o => o.Service.Price);

        Assert.Equal(expectedExpressRevenue, expressRevenue);
        Assert.Equal(expectedComplexRevenue, complexRevenue);
    }
}