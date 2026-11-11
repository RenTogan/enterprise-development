namespace CarWash.Contracts.Dtos;

/// <summary>
/// Популярная услуга
/// </summary>
public record PopularServiceDto(int ServiceId, string ServiceName, int OrdersCount);