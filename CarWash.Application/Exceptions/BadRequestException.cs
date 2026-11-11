namespace CarWash.Application.Exceptions;

/// <summary>
/// Ошибка валидации бизнес-правил
/// </summary>
public class BadRequestException(string message) : Exception(message);