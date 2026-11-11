namespace CarWash.Contracts.Dtos;

/// <summary>
/// Машина на мойке в текущий момент
/// </summary>
public record ActiveCarDto(
    int CarId,
    string LicensePlate,
    string? Brand,
    int BoxNumber,
    string ServiceName,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);