using CarWash.Domain.Entities;
using CarWash.Domain.Enums;

namespace CarWash.Tests;

/// <summary> 
/// Тестовые данные для автомойки 
/// </summary>
public class CarWashFixture
{
    /// <summary>
    /// Список клиентов
    /// </summary>
    public List<Client> Clients { get; set; } = [];

    /// <summary>
    /// Список услуг
    /// </summary>
    public List<Service> Services { get; set; } = [];

    /// <summary>
    /// Список заказов
    /// </summary>
    public List<Order> Orders { get; set; } = [];

    public CarWashFixture()
    {
        // Машины
        var car1 = new Car { Id = 1, LicensePlate = "А111АА163", Brand = "Lada Azimut" };
        var car2 = new Car { Id = 2, LicensePlate = "В222ВВ163", Brand = "Toyota Camry" };
        var car3 = new Car { Id = 3, LicensePlate = "С333СС163", Brand = "BELAZ" };
        var car4 = new Car { Id = 4, LicensePlate = "Е444ЕЕ163", Brand = "BMW X6" };
        var car5 = new Car { Id = 5, LicensePlate = "D555DD163", Brand = "BMW X5" };
        var car6 = new Car { Id = 6, LicensePlate = "К666КК163", Brand = "Kia Rio" };
        var car7 = new Car { Id = 7, LicensePlate = "М777ММ163", Brand = "Hyundai Solaris" };
        var car8 = new Car { Id = 8, LicensePlate = "Н888НН163", Brand = "GAZelle" };
        var car9 = new Car { Id = 9, LicensePlate = "О999ОО163", Brand = "Audi A6" };
        var car10 = new Car { Id = 10, LicensePlate = "Р000РР163", Brand = "Renault Logan" };

        // Клиенты
        var client1 = new Client { Id = 1, FullName = "Яцентюк Данила", Phone = "+79991112233", Cars = [car1] };
        var client2 = new Client { Id = 2, FullName = "Мансуров Роман", Phone = null, Cars = [car2, car5] };
        var client3 = new Client { Id = 3, FullName = "Сарычев Никита", Phone = "+79993334455", Cars = [car3] };
        var client4 = new Client { Id = 4, FullName = "Смирнова Анна", Phone = "+79995556677", Cars = [car4] };
        var client5 = new Client { Id = 5, FullName = "Кузнецов Александр", Phone = null };
        var client6 = new Client { Id = 6, FullName = "Кушнерев Георгий", Phone = "+79997778899" };
        var client7 = new Client { Id = 7, FullName = "Васильев Сергей", Phone = "+79998881122", Cars = [car6, car7] };
        var client8 = new Client { Id = 8, FullName = "Соколов Михаил", Phone = "+79993332211", Cars = [car8] };
        var client9 = new Client { Id = 9, FullName = "Новикова Ольга", Phone = "+79994445566", Cars = [car9] };
        var client10 = new Client { Id = 10, FullName = "Морозов Павел", Phone = "+79990001122", Cars = [car10] };

        Clients.AddRange([client1, client2, client3, client4, client5, client6]);

        // Услуги
        var s1 = new Service { Id = 1, Name = "Экспресс мойка", Category = CarCategory.Passenger, Price = 500, DurationMinutes = 20 };
        var s2 = new Service { Id = 2, Name = "Комплексная мойка", Category = CarCategory.Passenger, Price = 1200, DurationMinutes = 45 };
        var s3 = new Service { Id = 3, Name = "Мойка грузовика", Category = CarCategory.Truck, Price = 2500, DurationMinutes = 60 };
        var s4 = new Service { Id = 4, Name = "Химчистка салона", Category = CarCategory.Passenger, Price = 5000, DurationMinutes = 120 };
        var s5 = new Service { Id = 5, Name = "Мойка двигателя", Category = CarCategory.Suv, Price = 1500, DurationMinutes = 30 };
        var s6 = new Service { Id = 6, Name = "Полировка кузова", Category = CarCategory.Passenger, Price = 3000, DurationMinutes = 90 };
        var s7 = new Service { Id = 7, Name = "Чернение шин", Category = CarCategory.Passenger, Price = 300, DurationMinutes = 10 };
        var s8 = new Service { Id = 8, Name = "Мойка днища", Category = CarCategory.Suv, Price = 800, DurationMinutes = 25 };
        var s9 = new Service { Id = 9, Name = "Восковое покрытие", Category = CarCategory.Passenger, Price = 700, DurationMinutes = 15 };
        var s10 = new Service { Id = 10, Name = "Удаление битумных пятен", Category = CarCategory.Minivan, Price = 1000, DurationMinutes = 40 };

        Services.AddRange([s1, s2, s3, s4, s5, s6, s7, s8, s9, s10]);

        // Время для тестов
        var baseTime = new DateTime(2026, 5, 10, 10, 0, 0);

        // Заказы
        Orders =
        [
            // Данила
            new Order { Id = 1, Client = client1, Car = car1, Service = s1, StartTime = baseTime, BoxNumber = 1 },
            new Order { Id = 2, Client = client1, Car = car1, Service = s1, StartTime = baseTime.AddHours(2), BoxNumber = 1 },
            new Order { Id = 3, Client = client1, Car = car1, Service = s2, StartTime = baseTime.AddDays(1), BoxNumber = 2 },
            new Order { Id = 4, Client = client1, Car = car1, Service = s7, StartTime = baseTime.AddDays(2), BoxNumber = 1 },
            new Order { Id = 5, Client = client1, Car = car1, Service = s9, StartTime = baseTime.AddDays(3), BoxNumber = 1 },

            // Романа
            new Order { Id = 6, Client = client2, Car = car2, Service = s2, StartTime = baseTime, BoxNumber = 2 },
            new Order { Id = 7, Client = client2, Car = car2, Service = s2, StartTime = baseTime.AddHours(3), BoxNumber = 1 },
            new Order { Id = 8, Client = client2, Car = car5, Service = s5, StartTime = baseTime.AddDays(1), BoxNumber = 3 },
            new Order { Id = 9, Client = client2, Car = car5, Service = s1, StartTime = baseTime.AddDays(2), BoxNumber = 2 },

            // Никиты
            new Order { Id = 10, Client = client3, Car = car3, Service = s3, StartTime = baseTime, BoxNumber = 3 },
            new Order { Id = 11, Client = client3, Car = car3, Service = s3, StartTime = baseTime.AddDays(2), BoxNumber = 3 },
            new Order { Id = 12, Client = client3, Car = car3, Service = s3, StartTime = baseTime.AddDays(4), BoxNumber = 3 },

            // Анны
            new Order { Id = 13, Client = client4, Car = car4, Service = s5, StartTime = baseTime.AddMinutes(10), BoxNumber = 4 },
            new Order { Id = 14, Client = client4, Car = car4, Service = s1, StartTime = baseTime.AddDays(1), BoxNumber = 1 },

            // Александра
            new Order { Id = 15, Client = client5, Car = car1, Service = s4, StartTime = baseTime.AddHours(5), BoxNumber = 2 },
            new Order { Id = 16, Client = client5, Car = car1, Service = s6, StartTime = baseTime.AddDays(3), BoxNumber = 2 },

            // Остальные клиенты
            new Order { Id = 17, Client = client6, Car = car2, Service = s8, StartTime = baseTime.AddHours(4), BoxNumber = 4 },
            new Order { Id = 18, Client = client7, Car = car6, Service = s10, StartTime = baseTime.AddHours(1), BoxNumber = 5 },
            new Order { Id = 19, Client = client8, Car = car8, Service = s2, StartTime = baseTime.AddHours(2), BoxNumber = 5 },
            new Order { Id = 20, Client = client9, Car = car9, Service = s1, StartTime = baseTime.AddHours(3), BoxNumber = 5 }
        ];
    }
}