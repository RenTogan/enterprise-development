using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using CarWash.Infrastructure.InMemory;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса заказов
/// </summary>
public class OrderService(IStorageContext storage) : IOrderService
{
    public Task<List<OrderDto>> GetAllAsync()
    {
        var list = storage.Orders.Select(Map).ToList();
        return Task.FromResult(list);
    }

    public Task<OrderDto> GetByIdAsync(int id)
    {
        Order order = storage.Orders.FirstOrDefault(o => o.Id == id)
            ?? throw new NotFoundException($"Заказ с Id {id} не найден");

        return Task.FromResult(Map(order));
    }

    public Task<OrderDto> CreateAsync(OrderRequest request)
    {
        Client client = storage.Clients.FirstOrDefault(c => c.Id == request.ClientId!.Value)
            ?? throw new BadRequestException($"Клиент с Id {request.ClientId} не существует");

        Service service = storage.Services.FirstOrDefault(s => s.Id == request.ServiceId!.Value)
            ?? throw new BadRequestException($"Услуга с Id {request.ServiceId} не существует");

        Car car = storage.Cars.FirstOrDefault(c => c.Id == request.CarId!.Value)
            ?? throw new BadRequestException($"Автомобиль с Id {request.CarId} не существует");

        var order = new Order
        {
            Id = storage.NextOrderId(),
            Client = client,
            Service = service,
            Car = car,
            StartTime = request.StartTime!.Value,
            BoxNumber = request.BoxNumber!.Value
        };

        storage.Orders.Add(order);

        return Task.FromResult(Map(order));
    }

    public Task UpdateAsync(int id, OrderRequest request)
    {
        Order order = storage.Orders.FirstOrDefault(o => o.Id == id)
            ?? throw new NotFoundException($"Заказ с Id {id} не найден");

        Client client = storage.Clients.FirstOrDefault(c => c.Id == request.ClientId!.Value)
            ?? throw new BadRequestException($"Клиент с Id {request.ClientId} не существует");

        Service service = storage.Services.FirstOrDefault(s => s.Id == request.ServiceId!.Value)
            ?? throw new BadRequestException($"Услуга с Id {request.ServiceId} не существует");

        Car car = storage.Cars.FirstOrDefault(c => c.Id == request.CarId!.Value)
            ?? throw new BadRequestException($"Автомобиль с Id {request.CarId} не существует");

        order.Client = client;
        order.Service = service;
        order.Car = car;
        order.StartTime = request.StartTime!.Value;
        order.BoxNumber = request.BoxNumber!.Value;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Order order = storage.Orders.FirstOrDefault(o => o.Id == id)
            ?? throw new NotFoundException($"заказ с Id {id} не найден");

        storage.Orders.Remove(order);
        return Task.CompletedTask;
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