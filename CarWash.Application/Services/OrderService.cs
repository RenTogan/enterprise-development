using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса заказов
/// </summary>
public class OrderService(ICarWashDbContext db) : IOrderService
{
    private static readonly Expression<Func<Order, OrderDto>> _orderDtoProjection = o => new OrderDto
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

    public async Task<List<OrderDto>> GetAllAsync()
    {
        return await db.Orders
            .AsNoTracking()
            .OrderBy(o => o.StartTime)
            .ThenBy(o => o.Id)
            .Select(_orderDtoProjection)
            .ToListAsync();
    }

    public async Task<OrderDto> GetByIdAsync(int id)
    {
        return await db.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(_orderDtoProjection)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException($"Заказ с Id {id} не найден");
    }

    public async Task<OrderDto> CreateAsync(OrderRequest request)
    {
        var client = await db.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId!.Value)
            ?? throw new BadRequestException($"Клиент с Id {request.ClientId} не существует");

        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId!.Value)
            ?? throw new BadRequestException($"Услуга с Id {request.ServiceId} не существует");

        var car = await db.Cars.FirstOrDefaultAsync(c => c.Id == request.CarId!.Value)
            ?? throw new BadRequestException($"Автомобиль с Id {request.CarId} не существует");

        var order = new Order
        {
            Client = client,
            Service = service,
            Car = car,
            StartTime = request.StartTime!.Value.UtcDateTime,
            BoxNumber = request.BoxNumber!.Value
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        return new OrderDto
        {
            Id = order.Id,
            ClientId = client.Id,
            ClientName = client.FullName,
            ServiceId = service.Id,
            ServiceName = service.Name,
            CarId = car.Id,
            LicensePlate = car.LicensePlate,
            StartTime = order.StartTime,
            BoxNumber = order.BoxNumber
        };
    }

    public async Task UpdateAsync(int id, OrderRequest request)
    {
        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundException($"Заказ с Id {id} не найден");

        var client = await db.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId!.Value)
            ?? throw new BadRequestException($"Клиент с Id {request.ClientId} не существует");

        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId!.Value)
            ?? throw new BadRequestException($"Услуга с Id {request.ServiceId} не существует");

        var car = await db.Cars.FirstOrDefaultAsync(c => c.Id == request.CarId!.Value)
            ?? throw new BadRequestException($"Автомобиль с Id {request.CarId} не существует");

        order.Client = client;
        order.Service = service;
        order.Car = car;
        order.StartTime = request.StartTime!.Value.UtcDateTime;
        order.BoxNumber = request.BoxNumber!.Value;

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundException($"Заказ с Id {id} не найден");

        db.Orders.Remove(order);
        await db.SaveChangesAsync();
    }
}