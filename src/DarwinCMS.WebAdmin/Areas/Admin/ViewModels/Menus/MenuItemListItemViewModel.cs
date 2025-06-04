namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;

/// <summary>
/// View model used for listing menu items in the admin UI.
/// Supports hierarchical display (e.g., nesting).
/// </summary>
public class MenuItemListItemViewModel
{
    /// <summary>
    /// Unique identifier of the item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Label shown to the user.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Position of this item relative to siblings.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Type of link (internal, external, module).
    /// </summary>
    public string LinkType { get; set; } = "internal";

    /// <summary>
    /// Optional URL or route.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Display status of the item (active or hidden).
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Nested child items (if any).
    /// </summary>
    public List<MenuItemListItemViewModel> Children { get; set; } = new();
}
