using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dtos;

/// <summary>
/// DTO заказа
/// </summary>
public class OrderDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int CarId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public int BoxNumber { get; set; }
}

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
    public DateTime? StartTime { get; set; }

    [Required(ErrorMessage = "Укажите номер бокса")]
    [Range(1, 100, ErrorMessage = "Номер бокса должен быть от 1 до 100.")]
    public int? BoxNumber { get; set; }
}