namespace CarWash.Domain.Entities;

/// <summary>
/// Автомобиль
/// </summary>
public class Car
{
    /// <summary>
    /// Госномер
    /// </summary>
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Марка машины
    /// </summary>
    public string? Brand { get; set; }
}