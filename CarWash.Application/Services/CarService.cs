using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using CarWash.Infrastructure.InMemory;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса автомобилей
/// </summary>
public class CarService(IStorageContext storage) : ICarService
{
    public Task<List<CarDto>> GetAllAsync()
    {
        var result = storage.Cars
            .Select(c => new CarDto { Id = c.Id, LicensePlate = c.LicensePlate, Brand = c.Brand })
            .ToList();
        return Task.FromResult(result);
    }

    public Task<CarDto> GetByIdAsync(int id)
    {
        Car car = storage.Cars.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"машина с Id {id} не найдена");

        return Task.FromResult(new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Brand = car.Brand
        });
    }

    public Task<CarDto> CreateAsync(CarRequest request)
    {
        var plate = request.LicensePlate.Trim().ToUpperInvariant();
        if (storage.Cars.Any(c => c.LicensePlate.Equals(plate, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ConflictException("Машина с таким госномером уже зарегистрирована");
        }

        var car = new Car
        {
            Id = storage.NextCarId(),
            LicensePlate = plate,
            Brand = request.Brand?.Trim()
        };

        storage.Cars.Add(car);

        return Task.FromResult(new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Brand = car.Brand
        });
    }

    public Task UpdateAsync(int id, CarRequest request)
    {
        Car car = storage.Cars.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"Машина с Id {id} не найдена");

        var plate = request.LicensePlate.Trim().ToUpperInvariant();
        if (storage.Cars.Any(c => c.Id != id && c.LicensePlate.Equals(plate, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ConflictException("другая машина уже имеет такой госномер");
        }

        car.LicensePlate = plate;
        car.Brand = request.Brand?.Trim();

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Car car = storage.Cars.FirstOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"Машина с Id {id} не найдена");

        if (storage.Orders.Any(o => o.Car.Id == id))
        {
            throw new ConflictException("Нельзя удалить машину, на которую оформлены заказы");
        }

        foreach (Client client in storage.Clients)
        {
            client.Cars.RemoveAll(c => c.Id == id);
        }

        storage.Cars.Remove(car);
        return Task.CompletedTask;
    }
}