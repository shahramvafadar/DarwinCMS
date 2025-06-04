using DarwinCMS.Domain.ValueObjects;

namespace DarwinCMS.Application.DTOs.Menus;

/// <summary>
/// DTO used when creating a new menu item.
/// This is bound from the Create form in the admin UI.
/// </summary>
public class CreateMenuItemDto
{
    /// <summary>
    /// Menu to which this item will belong.
    /// </summary>
    public Guid MenuId { get; set; }

    /// <summary>
    /// Optional parent item ID for nesting.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Display label.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional icon class.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Type of the link.
    /// </summary>
    public LinkType LinkType { get; set; }

    /// <summary>
    /// Page ID if LinkType is internal.
    /// </summary>
    public Guid? PageId { get; set; }

    /// <summary>
    /// URL for external/module links.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Display condition.
    /// </summary>
    public string DisplayCondition { get; set; } = "always";

    /// <summary>
    /// Display order among siblings.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Whether this item is currently visible.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
