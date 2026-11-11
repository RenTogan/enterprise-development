using CarWash.Domain.Enums;

namespace CarWash.Contracts.Dtos;

/// <summary>
/// DTO услуги
/// </summary>
public class ServiceDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public CarCategory Category { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
}