using CarWash.Application.Interfaces;
using CarWash.Domain.Data;
using CarWash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Infrastructure.PostgreSQL;

/// <summary>
/// Контекст базы данных мойки
/// </summary>
public class CarWashDbContext(DbContextOptions<CarWashDbContext> options) : DbContext(options), ICarWashDbContext
{
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Car>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.LicensePlate).HasMaxLength(20).IsRequired();
            b.Property(c => c.Brand).HasMaxLength(50);
            b.HasIndex(c => c.LicensePlate).IsUnique();
        });

        modelBuilder.Entity<Client>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.FullName).HasMaxLength(100).IsRequired();
            b.Property(c => c.Phone).HasMaxLength(30);

            b.HasMany(c => c.Cars)
             .WithMany()
             .UsingEntity(
                 "ClientCars",
                 l => l.HasOne(typeof(Car)).WithMany().HasForeignKey("CarId"),
                 r => r.HasOne(typeof(Client)).WithMany().HasForeignKey("ClientId"),
                 j => j.HasKey("ClientId", "CarId")
             );
        });

        modelBuilder.Entity<Service>(b =>
        {
            b.HasKey(s => s.Id);
            b.Property(s => s.Name).HasMaxLength(100).IsRequired();
            b.Property(s => s.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.HasKey(o => o.Id);
            b.HasOne(o => o.Client).WithMany().OnDelete(DeleteBehavior.Restrict);
            b.HasOne(o => o.Car).WithMany().OnDelete(DeleteBehavior.Restrict);
            b.HasOne(o => o.Service).WithMany().OnDelete(DeleteBehavior.Restrict);
            b.Property(o => o.StartTime).HasColumnType("timestamp with time zone");
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        (var cars, var clients, var services, var orders) = CarWashSeed.CreateInitialData();

        modelBuilder.Entity<Car>().HasData(cars.Select(c => new
        {
            c.Id,
            c.LicensePlate,
            c.Brand
        }));

        modelBuilder.Entity<Client>().HasData(clients.Select(c => new
        {
            c.Id,
            c.FullName,
            c.Phone
        }));

        modelBuilder.Entity<Service>().HasData(services.Select(s => new
        {
            s.Id,
            s.Name,
            s.Category,
            s.Price,
            s.DurationMinutes
        }));

        modelBuilder.Entity<Order>().HasData(orders.Select(o => new
        {
            o.Id,
            ClientId = o.Client.Id,
            CarId = o.Car.Id,
            ServiceId = o.Service.Id,
            o.StartTime,
            o.BoxNumber
        }));

        var clientCars = clients
            .SelectMany(c => c.Cars.Select(car => new { ClientId = c.Id, CarId = car.Id }))
            .ToList();

        modelBuilder.Entity("ClientCars").HasData(clientCars);
    }
}