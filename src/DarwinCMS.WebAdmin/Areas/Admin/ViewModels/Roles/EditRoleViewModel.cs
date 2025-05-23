using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles;

/// <summary>
/// ViewModel for editing an existing role in the admin panel.
/// This model is used to pre-fill form fields and submit updated values.
/// </summary>
public class EditRoleViewModel
{
    /// <summary>
    /// Unique ID of the role to be edited.
    /// </summary>
    [Required(ErrorMessage = "Role ID is required.")]
    public Guid Id { get; set; }

    /// <summary>
    /// Technical name (e.g., "Admin") used for logic and uniqueness.
    /// </summary>
    [Required(ErrorMessage = "Role name is required.")]
    [MaxLength(100)]
    [Display(Name = "System Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Friendly name displayed in UI (e.g., "Administrator").
    /// </summary>
    [MaxLength(200)]
    [Display(Name = "Display Name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Optional description for admin reference or tooltips.
    /// </summary>
    [MaxLength(500)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Optional module scoping for modular roles (e.g., "CRM", "Blog").
    /// </summary>
    [MaxLength(100)]
    [Display(Name = "Module Scope")]
    public string? Module { get; set; }

    /// <summary>
    /// Determines if this role is currently active and assignable.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Optional UI display order for admin sorting.
    /// </summary>
    [Range(0, 999)]
    [Display(Name = "Display Order")]
    public int? DisplayOrder { get; set; }
}
