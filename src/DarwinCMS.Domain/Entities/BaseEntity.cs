namespace DarwinCMS.Domain.Entities;

/// <summary>
/// The base class for all domain entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; internal set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the entity was last modified.
    /// </summary>
    public DateTime? ModifiedAt { get; internal set; }

    /// <summary>
    /// Mark entity as updated by setting ModifiedAt to now.
    /// </summary>
    public void MarkAsModified()
    {
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Override ToString to assist with debugging and testing.
    /// </summary>
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}
