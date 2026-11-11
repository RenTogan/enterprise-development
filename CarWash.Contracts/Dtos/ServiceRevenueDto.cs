namespace CarWash.Contracts.Dtos;

/// <summary>
/// Выручка по услуге
/// </summary>
public record ServiceRevenueDto(int ServiceId, string ServiceName, decimal TotalRevenue);