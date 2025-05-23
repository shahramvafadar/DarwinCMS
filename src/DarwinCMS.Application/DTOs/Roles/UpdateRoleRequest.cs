using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.Application.DTOs.Roles;

/// <summary>
/// DTO for updating an existing role in the system.
/// Includes all editable fields that affect access, display, and behavior.
/// </summary>
public class UpdateRoleRequest
{
    /// <summary>
    /// ID of the role to be updated.
    /// </summary>
    [Required(ErrorMessage = "Role ID is required.")]
    public Guid Id { get; set; }

    /// <summary>
    /// System identifier for the role (e.g. "Admin").
    /// Used for logic and uniqueness enforcement.
    /// </summary>
    [Required(ErrorMessage = "Role name is required.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// UI-friendly display name (e.g. "Administrator").
    /// </summary>
    [MaxLength(200)]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Optional description about the role's function or purpose.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Optional module name to which this role is scoped.
    /// </summary>
    [MaxLength(100)]
    public string? Module { get; set; }

    /// <summary>
    /// Optional display order for UI sorting.
    /// </summary>
    [Range(0, 999)]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Whether the role is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
