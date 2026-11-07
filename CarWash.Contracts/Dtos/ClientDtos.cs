using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dtos;

/// <summary>
/// DTO клиента
/// </summary>
public class ClientDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public string? Phone { get; set; }
    public List<int> CarIds { get; set; } = [];
}

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