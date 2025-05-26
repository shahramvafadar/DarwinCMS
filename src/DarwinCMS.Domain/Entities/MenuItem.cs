namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents an individual item in a navigation menu.
/// Can point to a page, external URL, or system route.
/// </summary>
public class MenuItem : BaseEntity
{
    /// <summary>
    /// Display text for the menu item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Font Awesome or custom icon for the item.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// External or internal link URL.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Optional reference to a Page for dynamic linking.
    /// </summary>
    public Guid? PageId { get; set; }

    /// <summary>
    /// Foreign key to the parent menu this item belongs to.
    /// </summary>
    public Guid MenuId { get; set; }

    /// <summary>
    /// Optional parent item for submenu structures.
    /// </summary>
    public Guid? ParentItemId { get; set; }

    /// <summary>
    /// Whether the item is visible only to authenticated users.
    /// </summary>
    public bool VisibleForAuthenticatedOnly { get; set; }

    /// <summary>
    /// Display order of the item.
    /// </summary>
    public int DisplayOrder { get; set; } = 0;

    /// <summary>
    /// Whether this item is currently active and rendered.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property to the parent Menu.
    /// </summary>
    public virtual Menu? Menu { get; set; }

    /// <summary>
    /// Optional navigation to the linked Page.
    /// </summary>
    public virtual Page? Page { get; set; }

    /// <summary>
    /// Optional parent item navigation.
    /// </summary>
    public virtual MenuItem? ParentItem { get; set; }

    /// <summary>
    /// Sub-items for nested navigation (if any).
    /// </summary>
    public virtual ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
}
