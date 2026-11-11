using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dtos;

/// <summary>
/// Запрос создания/обновления клиента
/// </summary>
public class ClientRequest
{
    [Required(ErrorMessage = "ФИО обязательно для заполнения")]
    [StringLength(100, MinimumLength = 2)]
    public required string FullName { get; set; }

    [Phone(ErrorMessage = "Неверный формат телефона")]
    public string? Phone { get; set; }

    public List<int>? CarIds { get; set; }
}