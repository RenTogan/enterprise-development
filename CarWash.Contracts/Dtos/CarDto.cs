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