namespace CarWash.Domain.Entities;

/// <summary>
/// Заказ на мойку
/// </summary>
public class Order
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Клиент
    /// </summary>
    public required Client Client { get; set; }

    /// <summary>
    /// Выбранная услуга
    /// </summary>
    public required Service Service { get; set; }

    /// <summary>
    /// Автомобиль
    /// </summary>
    public required Car Car { get; set; }

    /// <summary>
    /// Время начала мойки
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Номер бокса
    /// </summary>
    public int BoxNumber { get; set; }
}