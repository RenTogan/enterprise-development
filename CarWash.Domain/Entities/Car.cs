namespace CarWash.Domain.Entities;

/// <summary>
/// Автомобиль
/// </summary>
public class Car
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Госномер
    /// </summary>
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Марка машины
    /// </summary>
    public string? Brand { get; set; }
}