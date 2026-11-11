using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dtos;

/// <summary>
/// Запрос создания/обновления заказа
/// </summary>
public class OrderRequest
{
    [Required(ErrorMessage = "Укажите клиента")]
    [Range(1, int.MaxValue)]
    public int? ClientId { get; set; }

    [Required(ErrorMessage = "Укажите услугу")]
    [Range(1, int.MaxValue)]
    public int? ServiceId { get; set; }

    [Required(ErrorMessage = "Укажите автомобиль")]
    [Range(1, int.MaxValue)]
    public int? CarId { get; set; }

    [Required(ErrorMessage = "Укажите время начала")]
    public DateTimeOffset? StartTime { get; set; }

    [Required(ErrorMessage = "Укажите номер бокса")]
    [Range(1, 100, ErrorMessage = "Номер бокса должен быть от 1 до 100.")]
    public int? BoxNumber { get; set; }
}