using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;

/// <summary>
/// View model used for creating a new menu in the admin panel.
/// This model is bound to the Create.cshtml form.
/// </summary>
public class CreateMenuViewModel
{
    /// <summary>
    /// Display name of the menu shown to the admin (e.g., “Main Menu”, “Footer Links”).
    /// </summary>
    [Required]
    [StringLength(100)]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Menu location indicator (e.g., header, footer, sidebar).
    /// This value may be used for filtering menus by layout region.
    /// </summary>
    [Required]
    [StringLength(50)]
    [Display(Name = "Position")]
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Determines whether this menu is currently active (rendered in frontend).
    /// </summary>
    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;
}
