namespace CarWash.Contracts.Dtos;

/// <summary>
/// Топ клиент по визитам
/// </summary>
public record TopClientDto(int ClientId, string FullName, int VisitsCount);