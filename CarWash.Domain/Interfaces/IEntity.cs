namespace CarWash.Domain.Interfaces;

/// <summary>
/// Базовый интерфейс с Id
/// </summary>
public interface IEntity
{
    public int Id { get; set; }
}