using CarWash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Application.Interfaces;

/// <summary>
/// Абстракция контекста БД для слоя приложения
/// </summary>
public interface ICarWashDbContext
{
    public DbSet<Car> Cars { get; }
    public DbSet<Client> Clients { get; }
    public DbSet<Service> Services { get; }
    public DbSet<Order> Orders { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}