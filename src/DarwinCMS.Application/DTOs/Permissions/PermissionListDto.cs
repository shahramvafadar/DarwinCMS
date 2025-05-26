using System;

namespace DarwinCMS.Application.DTOs.Permissions;

/// <summary>
/// Represents a single permission record returned in a paginated list.
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
}
