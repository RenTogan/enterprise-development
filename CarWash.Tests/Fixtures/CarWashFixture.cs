using CarWash.Domain.Data;
using CarWash.Domain.Entities;

namespace CarWash.Tests.Fixtures;

/// <summary>
/// Фикстура с начальными данными для тестов
/// </summary>
public class CarWashFixture
{
    public List<Car> Cars { get; }
    public List<Client> Clients { get; }
    public List<Service> Services { get; }
    public List<Order> Orders { get; }

    public CarWashFixture()
    {
        (List<Car>? cars, List<Client>? clients, List<Service>? services, List<Order>? orders) = CarWashSeed.CreateInitialData();
        Cars = cars;
        Clients = clients;
        Services = services;
        Orders = orders;
    }
}