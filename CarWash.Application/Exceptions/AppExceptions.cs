namespace CarWash.Application.Exceptions;

/// <summary>
/// Ошибка когда запись не найдена
/// </summary>
public class NotFoundException(string message) : Exception(message);

/// <summary>
/// Ошибка валидации бизнес-правил
/// </summary>
public class BadRequestException(string message) : Exception(message);

/// <summary>
/// Конфликт связей или данных
/// </summary>
public class ConflictException(string message) : Exception(message);