using CarWash.Application.Interfaces;
using CarWash.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>
/// Аналитические отчеты по мойке
/// </summary>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    /// <summary>
    /// Топ-5 клиентов по числу посещений
    /// </summary>
    [HttpGet("top-clients")]
    public async Task<ActionResult<List<TopClientDto>>> GetTopClients([FromQuery] int count = 5) =>
        Ok(await analyticsService.GetTopClientsAsync(count));

    /// <summary>
    /// Автомобили на мойке в текущий момент
    /// </summary>
    [HttpGet("current-cars")]
    public async Task<ActionResult<List<ActiveCarDto>>> GetCurrentCars([FromQuery] DateTimeOffset? atTime) =>
        Ok(await analyticsService.GetCurrentCarsAtWashAsync(atTime));

    /// <summary>
    /// Топ-5 самых популярных услуг
    /// </summary>
    [HttpGet("popular-services")]
    public async Task<ActionResult<List<PopularServiceDto>>> GetPopularServices([FromQuery] int count = 5) =>
        Ok(await analyticsService.GetTopServicesAsync(count));

    /// <summary>
    /// Время освобождения выбранного бокса
    /// </summary>
    [HttpGet("box-free-time")]
    public async Task<ActionResult<BoxFreeTimeDto>> GetBoxFreeTime([FromQuery] int boxNumber, [FromQuery] DateTimeOffset? fromTime) =>
        Ok(await analyticsService.GetBoxFreeTimeAsync(boxNumber, fromTime));

    /// <summary>
    /// Суммарная выручка по каждой услуге
    /// </summary>
    [HttpGet("service-revenues")]
    public async Task<ActionResult<List<ServiceRevenueDto>>> GetServiceRevenues() =>
        Ok(await analyticsService.GetServiceRevenuesAsync());
}