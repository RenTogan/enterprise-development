using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarWash.Infrastructure.PostgreSQL;

/// <summary>
/// фабрика контекста для миграций через dotnet
/// </summary>
public class CarWashDbContextFactory : IDesignTimeDbContextFactory<CarWashDbContext>
{
    public CarWashDbContext CreateDbContext(string[] args)
    {
        DbContextOptions<CarWashDbContext> options = new DbContextOptionsBuilder<CarWashDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=carwashdb;Username=postgres;Password=postgres")
            .Options;

        return new CarWashDbContext(options);
    }
}