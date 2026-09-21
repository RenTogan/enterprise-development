using CarWash.Domain.Entities;
using CarWash.Domain.Enums;

namespace CarWash.Tests;

/// <summary> 
/// Тестовые данные для автомойки 
/// </summary>
public class CarWashFixture
{
    public List<Client> Clients { get; set; } = [];
    public List<Service> Services { get; set; } = [];
    public List<Order> Orders { get; set; } = [];

    public CarWashFixture()
    {
        // Машины
        var car1 = new Car { LicensePlate = "А111АА163", Brand = "Lada Azimut" };
        var car2 = new Car { LicensePlate = "В222ВВ163", Brand = "Toyota Camry" };
        var car3 = new Car { LicensePlate = "С333СС163", Brand = "BELAZ" };
        var car4 = new Car { LicensePlate = "Е444ЕЕ163", Brand = "BMW X6" };
        var car5 = new Car { LicensePlate = "D555DD163", Brand = "BMW X5" };

        // Клиенты
        var client1 = new Client { FullName = "Яцентюк Данила", Phone = "+79991112233", Cars = { car1 } };
        var client2 = new Client { FullName = "Мансуров Роман", Phone = null, Cars = { car2, car5 } };
        var client3 = new Client { FullName = "Сарычев Никита", Phone = "+79993334455", Cars = { car3 } };
        var client4 = new Client { FullName = "Смирнова Анна", Phone = "+79995556677", Cars = { car4 } };
        var client5 = new Client { FullName = "Кузнецов Александр", Phone = null };
        var client6 = new Client { FullName = "Кушнерев Георгий", Phone = "+79997778899" };

        Clients.AddRange(new[] { client1, client2, client3, client4, client5, client6 });
        
        // Услуги
        var s1 = new Service { Name = "Экспресс мойка", Category = CarCategory.Passenger, Price = 500, DurationMinutes = 20 };
        var s2 = new Service { Name = "Комплексная мойка", Category = CarCategory.Passenger, Price = 1200, DurationMinutes = 45 };
        var s3 = new Service { Name = "Мойка грузовика", Category = CarCategory.Truck, Price = 2500, DurationMinutes = 60 };
        var s4 = new Service { Name = "Химчистка салона", Category = CarCategory.Passenger, Price = 5000, DurationMinutes = 120 };
        var s5 = new Service { Name = "Мойка двигателя", Category = CarCategory.Suv, Price = 1500, DurationMinutes = 30 };
        var s6 = new Service { Name = "Полировка кузова", Category = CarCategory.Passenger, Price = 3000, DurationMinutes = 90 };

        Services.AddRange(new[] { s1, s2, s3, s4, s5, s6 });

        // Время для тестов
        var baseTime = new DateTime(2026, 5, 10, 10, 0, 0);

        // Заказы
        Orders =
        [
            // Данила
            new() { Client = client1, Car = car1, Service = s1, StartTime = baseTime, BoxNumber = 1 },
            new() { Client = client1, Car = car1, Service = s1, StartTime = baseTime.AddHours(2), BoxNumber = 1 },
            new() { Client = client1, Car = car1, Service = s2, StartTime = baseTime.AddDays(1), BoxNumber = 2 },

            // Романа
            new() { Client = client2, Car = car2, Service = s2, StartTime = baseTime, BoxNumber = 2 },
            new() { Client = client2, Car = car2, Service = s2, StartTime = baseTime.AddHours(3), BoxNumber = 1 },

            // Никиты
            new() { Client = client3, Car = car3, Service = s3, StartTime = baseTime, BoxNumber = 3 },
            new() { Client = client3, Car = car3, Service = s3, StartTime = baseTime.AddDays(2), BoxNumber = 3 },

            // Анны
            new() { Client = client4, Car = car4, Service = s5, StartTime = baseTime.AddMinutes(10), BoxNumber = 4 },
            new() { Client = client4, Car = car4, Service = s1, StartTime = baseTime.AddDays(1), BoxNumber = 1 },

            // Александра
            new() { Client = client5, Car = car1, Service = s4, StartTime = baseTime.AddHours(5), BoxNumber = 2 },

            // заказ прямо сейчас
            // baseTime = 10:00
            // В 10:15 машина car1 на s1 (10:00-10:20), car2 на s2 (10:00-10:45), car3 на s3, car4 на s5 (10:10-10:40)
        ];
    }
}