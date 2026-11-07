using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dtos;

/// <summary>
/// DTO автомобиля
/// </summary>
public class CarDto
{
    public int Id { get; set; }
    public required string LicensePlate { get; set; }
    public string? Brand { get; set; }
}

/// <summary>
/// Запрос создания/обновления автомобиля
/// </summary>
public class CarRequest
{
    [Required(ErrorMessage = "Госномер обязателен")]
    [StringLength(20, MinimumLength = 3)]
    public required string LicensePlate { get; set; }

    [StringLength(50)]
    public string? Brand { get; set; }
}