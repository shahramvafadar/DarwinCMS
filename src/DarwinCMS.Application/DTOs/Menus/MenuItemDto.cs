using DarwinCMS.Domain.ValueObjects;

namespace DarwinCMS.Application.DTOs.Menus;

/// <summary>
/// DTO used to return information about a single menu item, including nesting and link data.
/// Used in admin panel for listing and editing.
/// </summary>
public class MenuItemDto
{
    /// <summary>
    /// Unique identifier of the menu item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The ID of the menu this item belongs to.
    /// </summary>
    public Guid MenuId { get; set; }

    /// <summary>
    /// Optional ID of the parent item (for nested menus).
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Display label of the menu item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional icon class (e.g., FontAwesome).
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Type of link (internal, external, or module).
    /// </summary>
    public LinkType LinkType { get; set; }

    /// <summary>
    /// Page ID if this is an internal link.
    /// </summary>
    public Guid? PageId { get; set; }

    /// <summary>
    /// URL if this is an external link or module route.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Display condition (e.g., always, auth, guest).
    /// </summary>
    public string DisplayCondition { get; set; } = "always";

    /// <summary>
    /// Sort order of the item among siblings.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this item is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Indicates whether this item is logically (soft) deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Children items (for tree display in UI).
    /// </summary>
    public List<MenuItemDto> Children { get; set; } = new();
}
