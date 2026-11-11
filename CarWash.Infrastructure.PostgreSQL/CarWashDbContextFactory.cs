using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CarWash.Infrastructure.PostgreSQL;

/// <summary>
/// Фабрика контекста для миграций через dotnet ef
/// </summary>
public class CarWashDbContextFactory : IDesignTimeDbContextFactory<CarWashDbContext>
{
    public CarWashDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("carwashdb")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Строка подключения 'carwashdb' не найдена в конфигурации.");

        DbContextOptions<CarWashDbContext> options = new DbContextOptionsBuilder<CarWashDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new CarWashDbContext(options);
    }
}