using DarwinCMS.Domain.ValueObjects;

namespace DarwinCMS.Application.DTOs.Menus;

/// <summary>
/// DTO used to update an existing menu item.
/// </summary>
public class UpdateMenuItemDto
{
    /// <summary>
    /// ID of the item being updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Optional parent item ID for nesting.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Updated display label.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Updated icon (optional).
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Updated link type.
    /// </summary>
    public LinkType LinkType { get; set; }

    /// <summary>
    /// Page ID for internal links.
    /// </summary>
    public Guid? PageId { get; set; }

    /// <summary>
    /// Updated external URL or route.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Updated display condition.
    /// </summary>
    public string DisplayCondition { get; set; } = "always";

    /// <summary>
    /// Display order among siblings.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Whether this item is visible.
    /// </summary>
    public bool IsActive { get; set; }
}
