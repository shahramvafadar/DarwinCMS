using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;

/// <summary>
/// View model used for editing an existing menu in the admin UI.
/// Mapped from the service layer and posted to update.
/// </summary>
public class EditMenuViewModel
{
    /// <summary>
    /// Unique identifier of the menu being edited.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Editable title of the menu.
    /// </summary>
    [Required]
    [StringLength(100)]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Editable menu position.
    /// </summary>
    [Required]
    [StringLength(50)]
    [Display(Name = "Position")]
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether this menu is currently active.
    /// </summary>
    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }
}
