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
    public DateTimeOffset StartTime { get; set; }
    public int BoxNumber { get; set; }
}