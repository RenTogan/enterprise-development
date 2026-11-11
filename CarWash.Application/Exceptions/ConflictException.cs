namespace CarWash.Application.Exceptions;

/// <summary>
/// Конфликт связей или данных
/// </summary>
public class ConflictException(string message) : Exception(message);