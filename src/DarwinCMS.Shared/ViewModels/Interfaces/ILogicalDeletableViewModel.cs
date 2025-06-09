namespace DarwinCMS.Shared.ViewModels.Interfaces;

/// <summary>
/// Interface for ViewModels representing logically deleted items in the system.
/// </summary>
public interface ILogicalDeletableViewModel
{
    /// <summary>
    /// The unique identifier of the item.
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Indicates whether the item is logically (soft) deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// The UTC date and time when the item was last modified (i.e., deleted or updated).
    /// </summary>
    DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// The ID of the user who last modified (or deleted) the item.
    /// </summary>
    Guid? ModifiedByUserId { get; set; }
}
