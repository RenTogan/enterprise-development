using CarWash.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dtos;

/// <summary>
/// Запрос создания/обновления услуги
/// </summary>
public class ServiceRequest
{
    [Required(ErrorMessage = "Название услуги обязательно")]
    [StringLength(100, MinimumLength = 2)]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Категория авто обязательна")]
    public CarCategory? Category { get; set; }

    [Required(ErrorMessage = "Цена обязательна")]
    [Range(0.01, 1000000, ErrorMessage = "Цена должна быть положительной")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Длительность обязательна")]
    [Range(1, 1440, ErrorMessage = "Длительность должна быть от 1 до 1440 минут")]
    public int? DurationMinutes { get; set; }
}