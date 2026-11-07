using CarWash.Domain.Entities;

namespace CarWash.Infrastructure.InMemory;

/// <summary>
/// Интерфейс хранилища данных в памяти
/// </summary>
public interface IStorageContext
{
    public List<Car> Cars { get; }
    public List<Client> Clients { get; }
    public List<Service> Services { get; }
    public List<Order> Orders { get; }
    public int NextCarId();
    public int NextClientId();
    public int NextServiceId();
    public int NextOrderId();
}