using CarWash.Application.Exceptions;
using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using CarWash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Application.Services;

/// <summary>
/// Реализация сервиса автомобилей
/// </summary>
public class CarService(ICarWashDbContext db) : ICarService
{
    public async Task<List<CarDto>> GetAllAsync()
    {
        return await db.Cars
            .AsNoTracking()
            .Select(c => new CarDto { Id = c.Id, LicensePlate = c.LicensePlate, Brand = c.Brand })
            .ToListAsync();
    }

    public async Task<CarDto> GetByIdAsync(int id)
    {
        Car car = await db.Cars
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException($"Машина с Id {id} не найдена");

        return new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Brand = car.Brand
        };
    }

    public async Task<CarDto> CreateAsync(CarRequest request)
    {
        var plate = request.LicensePlate.Trim().ToUpperInvariant();
        if (await db.Cars.AnyAsync(c => c.LicensePlate == plate))
        {
            throw new ConflictException("Машина с таким госномером уже есть");
        }

        var car = new Car
        {
            LicensePlate = plate,
            Brand = request.Brand?.Trim()
        };

        db.Cars.Add(car);
        await db.SaveChangesAsync();

        return new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Brand = car.Brand
        };
    }

    public async Task UpdateAsync(int id, CarRequest request)
    {
        Car car = await db.Cars.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException($"Машина с Id {id} не найдена");

        var plate = request.LicensePlate.Trim().ToUpperInvariant();
        if (await db.Cars.AnyAsync(c => c.Id != id && c.LicensePlate == plate))
        {
            throw new ConflictException("Другая машина уже имеет такой госномер");
        }

        car.LicensePlate = plate;
        car.Brand = request.Brand?.Trim();
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Car car = await db.Cars.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException($"Машина с Id {id} не найдена");

        if (await db.Orders.AnyAsync(o => o.Car.Id == id))
        {
            throw new ConflictException("Нельзя удалить машину, на которую есть заказы");
        }

        db.Cars.Remove(car);
        await db.SaveChangesAsync();
    }
}