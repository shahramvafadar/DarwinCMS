namespace DarwinCMS.Domain.Interfaces;

/// <summary>
/// Interface to support auditing for entity creation and modification.
/// Intended to be implemented by domain entities that need user traceability.
/// </summary>
public interface IAuditableEntity
{
  /// <summary>
  /// ID of the user who created the entity.
  /// This value should be set during initial creation.
  /// </summary>
  Guid CreatedByUserId { get; }

  /// <summary>
  /// ID of the user who last modified the entity.
  /// May be null if the entity has never been modified after creation.
  /// </summary>
  Guid? ModifiedByUserId { get; }
}
