using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;

/// <summary>
/// View model used to create a new item in a navigation menu.
/// Includes all fields necessary to link, sort, and display the item.
/// </summary>
public class CreateMenuItemViewModel
{
    /// <summary>
    /// The ID of the parent menu that this item belongs to.
    /// </summary>
    public Guid MenuId { get; set; }

    /// <summary>
    /// Optional parent item ID for hierarchical (nested) menu structure.
    /// If set, this item will be a child of the specified parent.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// The display title shown to the user in the navigation menu.
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional CSS class for displaying an icon (e.g. FontAwesome or Bootstrap Icon).
    /// </summary>
    [MaxLength(100)]
    [Display(Name = "Icon Class")]
    public string? Icon { get; set; }

    /// <summary>
    /// The type of link this item represents: \"internal\", \"external\", or \"module\".
    /// Determines how routing is handled in the frontend.
    /// </summary>
    [Required]
    [Display(Name = "Link Type")]
    public string LinkType { get; set; } = "internal";

    /// <summary>
    /// The ID of the CMS page this item links to (if LinkType is internal).
    /// </summary>
    [Display(Name = "Linked Page")]
    public Guid? PageId { get; set; }

    /// <summary>
    /// URL or path used if the item is of type external or module.
    /// This field is ignored if LinkType is internal.
    /// </summary>
    [MaxLength(500)]
    [Display(Name = "Custom URL or Route")]
    public string? Url { get; set; }

    /// <summary>
    /// Determines when this item is shown: always, only to authenticated users (\"auth\"), or guests (\"guest\").
    /// </summary>
    [Required]
    [MaxLength(20)]
    [Display(Name = "Display Condition")]
    public string DisplayCondition { get; set; } = "always";

    /// <summary>
    /// Sort order relative to sibling items. Lower values are displayed first.
    /// </summary>
    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; } = 0;

    /// <summary>
    /// Whether this item is visible in the frontend or not.
    /// </summary>
    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;
}
