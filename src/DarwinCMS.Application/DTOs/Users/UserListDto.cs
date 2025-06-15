using System;
using System.Collections.Generic;

namespace DarwinCMS.Application.DTOs.Users;

/// <summary>
/// Data Transfer Object used for listing users in a simple and clean format,
/// including soft delete metadata for recycle bin.
/// </summary>
public class UserListDto
{
    /// <summary>
    /// Unique identifier of the user (GUID).
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Username or login name of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Primary email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// List of roles assigned to this user (e.g., ["Admin", "Editor"]).
    /// Displayed as comma-separated string in UI.
    /// </summary>
    public List<string> RoleNames { get; set; } = new();

    /// <summary>
    /// Date and time the user was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the user was last modified (used as deletion timestamp in Recycle Bin).
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// The ID of the user who last modified or deleted the user.
    /// </summary>
    public Guid? ModifiedByUserId { get; set; }

    /// <summary>
    /// Indicates whether the user is logically (soft) deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
