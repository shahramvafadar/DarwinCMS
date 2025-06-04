namespace DarwinCMS.Domain.Entities;

/// <summary>
/// The base class for all domain entities, including common metadata fields for auditing and soft-deletion.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Date and time when the entity was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; internal set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the entity was last modified (UTC). Null if never modified.
    /// </summary>
    public DateTime? ModifiedAt { get; internal set; }

    /// <summary>
    /// The ID of the user who created the entity.
    /// </summary>
    public Guid? CreatedByUserId { get; internal set; }

    /// <summary>
    /// The ID of the user who last modified or deleted the entity.
    /// </summary>
    public Guid? ModifiedByUserId { get; internal set; }

    /// <summary>
    /// Indicates whether the entity is logically (soft) deleted.
    /// </summary>
    public bool IsDeleted { get; internal set; }

    /// <summary>
    /// Marks the entity as newly created and sets metadata fields.
    /// </summary>
    /// <param name="userId">The ID of the user who created the entity.</param>
    public void MarkAsCreated(Guid? userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedByUserId = userId;
        ModifiedAt = null;
        ModifiedByUserId = null;
        IsDeleted = false;
    }

    /// <summary>
    /// Marks the entity as modified (or deleted) and sets metadata fields.
    /// </summary>
    /// <param name="userId">The ID of the user who modified or deleted the entity.</param>
    /// <param name="isDeleted">Indicates whether this modification is a soft-delete.</param>
    public void MarkAsModified(Guid? userId, bool isDeleted = false)
    {
        ModifiedAt = DateTime.UtcNow;
        ModifiedByUserId = userId;
        IsDeleted = isDeleted;
    }

    /// <summary>
    /// Override ToString to assist with debugging and testing.
    /// </summary>
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}
