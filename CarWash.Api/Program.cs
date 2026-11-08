using CarWash.Api.Infrastructure;
using CarWash.Application.Interfaces;
using CarWash.Application.Services;
using CarWash.Infrastructure.PostgreSQL;
using CarWash.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Автомойка API";
        document.Info.Description = "Лабораторная работа №3. PostgreSQL и .NET Aspire.";
        return Task.CompletedTask;
    });
});

var connectionString = builder.Configuration.GetConnectionString("carwashdb")
    ?? "Host=localhost;Port=5432;Database=carwashdb;Username=postgres;Password=postgres";

builder.Services.AddDbContext<CarWashDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IWashServiceService, WashServiceService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    CarWashDbContext db = scope.ServiceProvider.GetRequiredService<CarWashDbContext>();
    await db.Database.MigrateAsync();
}

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.MapControllers();

app.Run();

public partial class Program { }