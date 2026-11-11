using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса клиентов
/// </summary>
public class ClientService(ICarWashDbContext db) : IClientService
{
    public async Task<List<ClientDto>> GetAllAsync()
    {
        var clients = await db.Clients
            .AsNoTracking()
            .Include(c => c.Cars)
            .ToListAsync();

        return [.. clients.Select(client => new ClientDto
        {
            Id = client.Id,
            FullName = client.FullName,
            Phone = client.Phone,
            CarIds = [.. client.Cars.Select(car => car.Id)]
        })];
    }

    public async Task<ClientDto> GetByIdAsync(int id)
    {
        Client client = await db.Clients
            .AsNoTracking()
            .Include(c => c.Cars)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException($"Клиент с Id {id} не найден");

        return new ClientDto
        {
            Id = client.Id,
            FullName = client.FullName,
            Phone = client.Phone,
            CarIds = [.. client.Cars.Select(car => car.Id)]
        };
    }

    public async Task<ClientDto> CreateAsync(ClientRequest request)
    {
        List<Car> clientCars = [];
        if (request.CarIds != null && request.CarIds.Count > 0)
        {
            clientCars = await db.Cars.Where(c => request.CarIds.Contains(c.Id)).ToListAsync();
        }

        var client = new Client
        {
            FullName = request.FullName.Trim(),
            Phone = request.Phone?.Trim(),
            Cars = clientCars
        };

        db.Clients.Add(client);
        await db.SaveChangesAsync();

        return new ClientDto
        {
            Id = client.Id,
            FullName = client.FullName,
            Phone = client.Phone,
            CarIds = [.. client.Cars.Select(c => c.Id)]
        };
    }

    public async Task UpdateAsync(int id, ClientRequest request)
    {
        Client client = await db.Clients
            .Include(c => c.Cars)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException($"Клиент с Id {id} не найден");

        client.FullName = request.FullName.Trim();
        client.Phone = request.Phone?.Trim();

        if (request.CarIds != null)
        {
            client.Cars = await db.Cars.Where(c => request.CarIds.Contains(c.Id)).ToListAsync();
        }

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Client client = await db.Clients.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException($"Клиент с Id {id} не найден");

        if (await db.Orders.AnyAsync(o => o.Client.Id == id))
        {
            throw new ConflictException("Нельзя удалить клиента с заказами");
        }

        db.Clients.Remove(client);
        await db.SaveChangesAsync();
    }

    public async Task<List<CarDto>> GetClientCarsAsync(int clientId)
    {
        Client client = await db.Clients
            .AsNoTracking()
            .Include(c => c.Cars)
            .FirstOrDefaultAsync(c => c.Id == clientId)
            ?? throw new NotFoundException($"Клиент с Id {clientId} не найден");

        return [.. client.Cars.Select(c => new CarDto
        {
            Id = c.Id,
            LicensePlate = c.LicensePlate,
            Brand = c.Brand
        })];
    }

    public async Task<List<OrderDto>> GetClientOrdersAsync(int clientId)
    {
        if (!await db.Clients.AnyAsync(c => c.Id == clientId))
        {
            throw new NotFoundException($"Клиент с Id {clientId} не найден");
        }

        return await db.Orders
            .AsNoTracking()
            .Where(o => o.Client.Id == clientId)
            .OrderBy(o => o.StartTime)
            .ThenBy(o => o.Id)
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
            })
            .ToListAsync();
    }
}