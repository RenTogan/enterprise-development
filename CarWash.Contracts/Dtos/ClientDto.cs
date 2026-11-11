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