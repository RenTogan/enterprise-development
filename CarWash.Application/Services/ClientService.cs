using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using CarWash.Infrastructure.InMemory;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса клиентов
/// </summary>
public class ClientService(IStorageContext storage) : IClientService
{
    public Task<List<ClientDto>> GetAllAsync()
    {
        var result = storage.Clients
            .Select(c => new ClientDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Phone = c.Phone,
                CarIds = c.Cars.Select(car => car.Id).ToList()
            })
            .ToList();

        return Task.FromResult(result);
    }

    public Task<ClientDto> GetByIdAsync(int id)
    {
        Client client = storage.Clients.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"Клиент с Id {id} не найден");

        return Task.FromResult(new ClientDto
        {
            Id = client.Id,
            FullName = client.FullName,
            Phone = client.Phone,
            CarIds = client.Cars.Select(car => car.Id).ToList()
        });
    }

    public Task<ClientDto> CreateAsync(ClientRequest request)
    {
        List<Car> clientCars = [];
        if (request.CarIds is not null && request.CarIds.Count > 0)
        {
            clientCars = storage.Cars.Where(c => request.CarIds.Contains(c.Id)).ToList();
        }

        var client = new Client
        {
            Id = storage.NextClientId(),
            FullName = request.FullName.Trim(),
            Phone = request.Phone?.Trim(),
            Cars = clientCars
        };

        storage.Clients.Add(client);

        return Task.FromResult(new ClientDto
        {
            Id = client.Id,
            FullName = client.FullName,
            Phone = client.Phone,
            CarIds = client.Cars.Select(car => car.Id).ToList()
        });
    }

    public Task UpdateAsync(int id, ClientRequest request)
    {
        Client client = storage.Clients.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"Клиент с Id {id} не найден");

        client.FullName = request.FullName.Trim();
        client.Phone = request.Phone?.Trim();

        if (request.CarIds is not null)
        {
            client.Cars = storage.Cars.Where(c => request.CarIds.Contains(c.Id)).ToList();
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Client client = storage.Clients.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"Клиент с Id {id} не найден");

        if (storage.Orders.Any(o => o.Client.Id == id))
        {
            throw new ConflictException("Нельзя удалить клиента, у которого есть активные заказы");
        }

        storage.Clients.Remove(client);
        return Task.CompletedTask;
    }

    public Task<List<CarDto>> GetClientCarsAsync(int clientId)
    {
        Client client = storage.Clients.FirstOrDefault(c => c.Id == clientId)
            ?? throw new NotFoundException($"Клиент с Id {clientId} не найден");

        var cars = client.Cars.Select(car => new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Brand = car.Brand
        }).ToList();

        return Task.FromResult(cars);
    }

    public Task<List<OrderDto>> GetClientOrdersAsync(int clientId)
    {
        _ = storage.Clients.FirstOrDefault(c => c.Id == clientId)
            ?? throw new NotFoundException($"Клиент с Id {clientId} не найден");

        var orders = storage.Orders
            .Where(o => o.Client.Id == clientId)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                ClientId = o.Client.Id,
                ClientName = o.Client.FullName,
                ServiceId = o.Service.Id,
                ServiceName = o.Service.Name,
                CarId = o.Car.Id,
                LicensePlate = o.Car.LicensePlate,
                StartTime = o.StartTime,
                BoxNumber = o.BoxNumber
            }).ToList();

        return Task.FromResult(orders);
    }
}