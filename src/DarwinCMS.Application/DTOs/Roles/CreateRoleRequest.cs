using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.Application.DTOs.Roles;

/// <summary>
/// Represents the data required to create a new role in the system.
/// </summary>
public class CreateRoleRequest
{
    /// <summary>
    /// Unique technical identifier of the role (e.g. "Admin").
    /// Must be lowercase, trimmed, and unique across the system.
    /// </summary>
    [Required(ErrorMessage = "Role name is required.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional UI-friendly name (e.g. "Administrator").
    /// Used for display purposes in the admin panel.
    /// </summary>
    [MaxLength(200)]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Optional description or tooltip about what this role represents.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Optional module name to scope the role to (e.g. "Blog", "CRM").
    /// If null, this role is considered global.
    /// </summary>
    [MaxLength(100)]
    public string? Module { get; set; }

    /// <summary>
    /// Optional numeric value to control display order or evaluation priority.
    /// </summary>
    [Range(0, 999)]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Flag to determine if the role is enabled upon creation.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
