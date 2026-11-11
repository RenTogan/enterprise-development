namespace CarWash.Application.Exceptions;

/// <summary>
/// Ошибка когда запись не найдена
/// </summary>
public class NotFoundException(string message) : Exception(message);