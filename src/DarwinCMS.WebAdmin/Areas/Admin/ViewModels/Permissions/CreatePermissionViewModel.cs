using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Permissions;

/// <summary>
/// ViewModel used for creating a new permission from the admin panel.
/// </summary>
public class CreatePermissionViewModel
{
    /// <summary>
    /// Internal technical name used for permission logic (e.g. "manage_roles").
    /// Must be unique.
    /// </summary>
    [Required]
    [StringLength(100)]
    [Display(Name = "Permission Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display name shown in the admin panel (e.g. "Manage Roles").
    /// </summary>
    [Required]
    [StringLength(200)]
    [Display(Name = "Display Name")]
    public string DisplayName { get; set; } = string.Empty;
}
