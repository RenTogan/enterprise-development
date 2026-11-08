using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using CarWash.Infrastructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса заказов
/// </summary>
public class OrderService(CarWashDbContext db) : IOrderService
{
    public async Task<List<OrderDto>> GetAllAsync()
    {
        return await db.Orders
            .Include(o => o.Client)
            .Include(o => o.Car)
            .Include(o => o.Service)
            .Select(o => Map(o))
            .ToListAsync();
    }

    public async Task<OrderDto> GetByIdAsync(int id)
    {
        Order order = await db.Orders
            .Include(o => o.Client)
            .Include(o => o.Car)
            .Include(o => o.Service)
            .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundException($"Заказ с Id {id} не найден");

        return Map(order);
    }

    public async Task<OrderDto> CreateAsync(OrderRequest request)
    {
        Client client = await db.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId!.Value)
            ?? throw new BadRequestException($"Клиент с Id {request.ClientId} не существует");

        Service service = await db.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId!.Value)
            ?? throw new BadRequestException($"Услуга с Id {request.ServiceId} не существует");

        Car car = await db.Cars.FirstOrDefaultAsync(c => c.Id == request.CarId!.Value)
            ?? throw new BadRequestException($"Автомобиль с Id {request.CarId} не существует");

        var order = new Order
        {
            Client = client,
            Service = service,
            Car = car,
            StartTime = request.StartTime!.Value,
            BoxNumber = request.BoxNumber!.Value
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        return Map(order);
    }

    public async Task UpdateAsync(int id, OrderRequest request)
    {
        Order order = await db.Orders.FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundException($"Заказ с Id {id} не найден");

        Client client = await db.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId!.Value)
            ?? throw new BadRequestException($"Клиент с Id {request.ClientId} не существует");

        Service service = await db.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId!.Value)
            ?? throw new BadRequestException($"Услуга с Id {request.ServiceId} не существует");

        Car car = await db.Cars.FirstOrDefaultAsync(c => c.Id == request.CarId!.Value)
            ?? throw new BadRequestException($"Автомобиль с Id {request.CarId} не существует");

        order.Client = client;
        order.Service = service;
        order.Car = car;
        order.StartTime = request.StartTime!.Value;
        order.BoxNumber = request.BoxNumber!.Value;

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Order order = await db.Orders.FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundException($"Заказ с Id {id} не найден");

        db.Orders.Remove(order);
        await db.SaveChangesAsync();
    }

    private static OrderDto Map(Order o) => new()
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
    };
}