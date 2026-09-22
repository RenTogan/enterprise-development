using CarWash.Domain.Enums;

namespace CarWash.Domain.Entities;

/// <summary>
/// Услуга автомойки
/// </summary>
public class Service
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название услуги
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Категория авто
    /// </summary>
    public CarCategory Category { get; set; }

    /// <summary>
    /// Стоимость
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Длительность (в минутах)
    /// </summary>
    public int DurationMinutes { get; set; }
}