using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles;

/// <summary>
/// ViewModel for creating a new role in the admin panel.
/// Used for binding create role form fields.
/// </summary>
public class CreateRoleViewModel
{
    /// <summary>
    /// System identifier of the role (e.g., "Admin").
    /// Must be unique and is used programmatically.
    /// </summary>
    [Required(ErrorMessage = "Role name is required.")]
    [MaxLength(100)]
    [Display(Name = "System Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Friendly label displayed in UI (optional).
    /// </summary>
    [MaxLength(200)]
    [Display(Name = "Display Name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Description for internal use or tooltips.
    /// </summary>
    [MaxLength(500)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Optional module scope (e.g., "Blog", "CRM").
    /// If null, role is global.
    /// </summary>
    [MaxLength(100)]
    [Display(Name = "Module")]
    public string? Module { get; set; }

    /// <summary>
    /// Determines whether the role is currently active.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Optional UI display order (used for sorting).
    /// </summary>
    [Range(0, 999)]
    [Display(Name = "Display Order")]
    public int? DisplayOrder { get; set; }
}
