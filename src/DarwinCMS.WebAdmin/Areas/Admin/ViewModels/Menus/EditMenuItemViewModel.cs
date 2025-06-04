using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;

/// <summary>
/// View model used to edit an existing menu item.
/// All editable fields are included and match those used in creation.
/// </summary>
public class EditMenuItemViewModel
{
    /// <summary>
    /// Unique identifier of the item being edited.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The parent menu to which this item belongs.
    /// </summary>
    public Guid MenuId { get; set; }

    /// <summary>
    /// Optional parent ID for nesting under another item.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Display label of the menu item.
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional icon class name (e.g. \"fas fa-home\").
    /// </summary>
    [MaxLength(100)]
    [Display(Name = "Icon Class")]
    public string? Icon { get; set; }

    /// <summary>
    /// Type of link: internal, external, or module.
    /// </summary>
    [Required]
    [Display(Name = "Link Type")]
    public string LinkType { get; set; } = "internal";

    /// <summary>
    /// If internal link, references a CMS Page.
    /// </summary>
    [Display(Name = "Linked Page")]
    public Guid? PageId { get; set; }

    /// <summary>
    /// Custom URL or route path if LinkType is external/module.
    /// </summary>
    [MaxLength(500)]
    [Display(Name = "URL or Module Route")]
    public string? Url { get; set; }

    /// <summary>
    /// Visibility condition: always / auth / guest.
    /// </summary>
    [Required]
    [MaxLength(20)]
    [Display(Name = "Display Condition")]
    public string DisplayCondition { get; set; } = "always";

    /// <summary>
    /// Sort order (lower numbers appear first).
    /// </summary>
    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this menu item is currently visible in frontend.
    /// </summary>
    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;
}
