using CarWash.Domain.Data;
using CarWash.Domain.Entities;

namespace CarWash.Infrastructure.InMemory;

/// <summary>
/// Потокобезоп-ая база в памяти с начальным сидом
/// </summary>
public class InMemoryStorage : IStorageContext
{
    private readonly Lock _lock = new();
    private int _carId;
    private int _clientId;
    private int _serviceId;
    private int _orderId;

    public List<Car> Cars { get; }
    public List<Client> Clients { get; }
    public List<Service> Services { get; }
    public List<Order> Orders { get; }

    public InMemoryStorage()
    {
        (List<Car>? cars, List<Client>? clients, List<Service>? services, List<Order>? orders) = CarWashSeed.CreateInitialData();
        Cars = cars;
        Clients = clients;
        Services = services;
        Orders = orders;

        _carId = Cars.Count > 0 ? Cars.Max(x => x.Id) : 0;
        _clientId = Clients.Count > 0 ? Clients.Max(x => x.Id) : 0;
        _serviceId = Services.Count > 0 ? Services.Max(x => x.Id) : 0;
        _orderId = Orders.Count > 0 ? Orders.Max(x => x.Id) : 0;
    }

    public int NextCarId() => Interlocked.Increment(ref _carId);

    public int NextClientId() => Interlocked.Increment(ref _clientId);

    public int NextServiceId() => Interlocked.Increment(ref _serviceId);

    public int NextOrderId() => Interlocked.Increment(ref _orderId);

}