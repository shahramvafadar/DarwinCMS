using System;

namespace DarwinCMS.Application.DTOs.Permissions;

/// <summary>
/// Represents the data required to update an existing permission.
/// </summary>
public class UpdatePermissionRequest
{
    /// <summary>
    /// ID of the permission being updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Updated internal name of the permission.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Updated display name shown to users.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
}
