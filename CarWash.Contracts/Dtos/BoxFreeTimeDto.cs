namespace CarWash.Contracts.Dtos;

/// <summary>
/// Время освобождения бокса
/// </summary>
public record BoxFreeTimeDto(int BoxNumber, DateTimeOffset FreeTime);