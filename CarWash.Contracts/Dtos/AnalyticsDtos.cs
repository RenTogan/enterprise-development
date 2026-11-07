namespace CarWash.Contracts.Dtos;

/// <summary>
/// Топ клиент по визитам
/// </summary>
public record TopClientDto(int ClientId, string FullName, int VisitsCount);

/// <summary>
/// Машина на мойке в текущий момент
/// </summary>
public record ActiveCarDto(int CarId, string LicensePlate, string? Brand,
    int BoxNumber, string ServiceName, DateTime StartTime, DateTime EndTime);

/// <summary>
/// Популярная услуга
/// </summary>
public record PopularServiceDto(int ServiceId, string ServiceName, int OrdersCount);

/// <summary>
/// Время освобождения бокса
/// </summary>
public record BoxFreeTimeDto(int BoxNumber, DateTime FreeTime);

/// <summary>
/// Выручка по услуге
/// </summary>
public record ServiceRevenueDto(int ServiceId, string ServiceName, decimal TotalRevenue);