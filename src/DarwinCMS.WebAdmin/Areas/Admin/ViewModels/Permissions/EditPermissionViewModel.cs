using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Permissions;

/// <summary>
/// ViewModel used for editing an existing permission in the admin panel.
/// </summary>
public class EditPermissionViewModel
{
    /// <summary>
    /// Unique identifier of the permission.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Internal technical name used for logic (unchangeable for system permissions).
    /// </summary>
    [Required]
    [StringLength(100)]
    [Display(Name = "Permission Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display name shown in the admin panel.
    /// </summary>
    [Required]
    [StringLength(200)]
    [Display(Name = "Display Name")]
    public string DisplayName { get; set; } = string.Empty;
}
