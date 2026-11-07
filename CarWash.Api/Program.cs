using CarWash.Api.Infrastructure;
using CarWash.Application.Interfaces;
using CarWash.Application.Services;
using CarWash.Infrastructure.InMemory;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
        document.Info.Description = "Лабораторная работа №2. Документация REST API автомойки.";
        return Task.CompletedTask;
    });
});

builder.Services.AddSingleton<IStorageContext, InMemoryStorage>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IWashServiceService, WashServiceService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.MapControllers();

app.Run();

public partial class Program { }