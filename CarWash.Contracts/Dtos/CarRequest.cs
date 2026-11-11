using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dtos;

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