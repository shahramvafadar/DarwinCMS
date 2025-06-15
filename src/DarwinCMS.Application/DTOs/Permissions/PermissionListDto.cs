using System;

namespace DarwinCMS.Application.DTOs.Permissions;

/// <summary>
/// Represents a single permission record returned in a paginated list.
/// Includes soft-delete metadata for recycle bin functionality.
/// </summary>
public class PermissionListDto
{
    /// <summary>
    /// Unique identifier of the permission.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Internal name used for system checks and authorization.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display name shown in the UI.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this permission is marked as non-deletable system permission.
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// Indicates whether the permission is logically (soft) deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// The UTC date and time when the permission was last modified (or deleted).
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// The ID of the user who last modified (or deleted) the permission.
    /// </summary>
    public Guid? ModifiedByUserId { get; set; }
}
