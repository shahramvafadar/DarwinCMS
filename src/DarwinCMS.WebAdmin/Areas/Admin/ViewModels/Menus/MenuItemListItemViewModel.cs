using System;
using System.Collections.Generic;

using DarwinCMS.Shared.ViewModels.Interfaces;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;

/// <summary>
/// View model used for listing menu items in the admin UI.
/// Supports hierarchical display (e.g., nesting).
/// Includes logical deletion tracking for Recycle Bin features.
/// </summary>
public class MenuItemListItemViewModel : ILogicalDeletableViewModel
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

    /// <summary>
    /// Indicates whether this item is logically deleted (for Recycle Bin).
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// The UTC timestamp when the item was last modified or deleted.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// ID of the user who last modified or deleted this item.
    /// </summary>
    public Guid? ModifiedByUserId { get; set; }
}
