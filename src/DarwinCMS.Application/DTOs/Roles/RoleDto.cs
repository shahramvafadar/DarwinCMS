namespace DarwinCMS.Application.DTOs.Roles;

/// <summary>
/// Lightweight Data Transfer Object representing a system role.
/// Used in listings, dropdowns, and summaries.
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Unique identifier of the role (GUID).
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Internal or technical name of the role (e.g., "Admin").
    /// This is used in logic and must be unique.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the role for UI purposes (e.g., "Administrator").
    /// Falls back to Name if not set.
    /// </summary>
    public string DisplayName => string.IsNullOrWhiteSpace(_displayName) ? Name : _displayName!;

    /// <summary>
    /// Backing field for display name.
    /// </summary>
    private string? _displayName;

    /// <summary>
    /// Description of the role (used in tooltips or admin panels).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the role is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Used for mapping display name from database or DTO source.
    /// </summary>
    public string? RawDisplayName
    {
        get => _displayName;
        set => _displayName = value;
    }
}
