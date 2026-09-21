namespace CarWash.Domain.Entities;

/// <summary>
/// Клиент
/// </summary>
public class Client
{
    /// <summary>
    /// ФИО клиента
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Список автомобилей клиента
    /// </summary>
    public List<Car> Cars { get; set; } = [];
}