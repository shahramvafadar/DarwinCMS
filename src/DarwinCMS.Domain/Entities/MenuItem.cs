using System;
using System.Collections.Generic;

using DarwinCMS.Domain.ValueObjects;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a single menu item inside a navigation menu.
/// Supports nesting and dynamic linking to pages, modules, or external URLs.
/// </summary>
public class MenuItem : BaseEntity
{
    /// <summary>
    /// Foreign key to the menu that this item belongs to.
    /// </summary>
    public Guid MenuId { get; private set; }

    /// <summary>
    /// Navigation property to the parent menu.
    /// </summary>
    public Menu? Menu { get; private set; }

    /// <summary>
    /// Optional foreign key to a parent menu item (for nested menus).
    /// </summary>
    public Guid? ParentId { get; private set; }

    /// <summary>
    /// Navigation to parent item (for nesting).
    /// </summary>
    public MenuItem? Parent { get; private set; }

    /// <summary>
    /// Collection of child items (if any).
    /// </summary>
    public List<MenuItem> Children { get; private set; } = new();

    /// <summary>
    /// The visible label (text) shown to users.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Optional icon class (e.g. FontAwesome or Bootstrap icon class).
    /// </summary>
    public string? Icon { get; private set; }

    /// <summary>
    /// Indicates the type of link this menu item represents: internal, external, or module.
    /// Defaults to internal link.
    /// </summary>
    public LinkType LinkType { get; private set; } = LinkType.Internal;

    /// <summary>
    /// Internal reference to linked Page (if LinkType = internal).
    /// </summary>
    public Guid? PageId { get; private set; }

    /// <summary>
    /// External URL or route for this item.
    /// </summary>
    public string? Url { get; private set; }

    /// <summary>
    /// Determines whether this item is currently visible in the frontend.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Optional display condition (e.g., "auth", "guest", "always").
    /// </summary>
    public string DisplayCondition { get; private set; } = "always";

    /// <summary>
    /// Sort order for this item (within siblings).
    /// </summary>
    public int DisplayOrder { get; private set; } = 0;

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected MenuItem() { }

    /// <summary>
    /// Creates a full-featured menu item for initialization or seeding.
    /// </summary>
    /// <param name="menuId">The parent menu ID.</param>
    /// <param name="title">The item title.</param>
    /// <param name="linkType">Type of the link.</param>
    /// <param name="url">URL or route (if external or module).</param>
    /// <param name="icon">Optional icon class.</param>
    /// <param name="displayOrder">Order in the menu.</param>
    /// <param name="displayCondition">Display condition (auth, guest, always).</param>
    /// <param name="isActive">Is this item active?</param>
    /// <param name="createdByUserId">ID of the creator.</param>
    public MenuItem(
        Guid menuId,
        string title,
        LinkType linkType,
        string? url,
        string? icon,
        int displayOrder,
        string displayCondition,
        bool isActive,
        Guid createdByUserId)
    {
        MenuId = menuId;
        SetTitle(title, createdByUserId);
        SetLinkType(linkType, createdByUserId);
        SetUrl(url, createdByUserId);
        SetIcon(icon, createdByUserId);
        SetDisplayOrder(displayOrder, createdByUserId);
        SetDisplayCondition(displayCondition, createdByUserId);
        SetIsActive(isActive, createdByUserId);
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Sets the title of the menu item.
    /// </summary>
    public void SetTitle(string title, Guid? modifierId)
    {
        Title = string.IsNullOrWhiteSpace(title)
            ? throw new ArgumentException("Title is required.", nameof(title))
            : title.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the parent ID for nesting menu items.
    /// </summary>
    public void SetParentId(Guid? parentId, Guid? modifierId)
    {
        ParentId = parentId;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the icon class.
    /// </summary>
    public void SetIcon(string? icon, Guid? modifierId)
    {
        Icon = icon?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the page ID if linking to a CMS page.
    /// </summary>
    public void SetPage(Guid? pageId, Guid? modifierId)
    {
        PageId = pageId;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the external or module URL.
    /// </summary>
    public void SetUrl(string? url, Guid? modifierId)
    {
        Url = url?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the type of link this item represents.
    /// </summary>
    public void SetLinkType(LinkType type, Guid? modifierId)
    {
        LinkType = type;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Enables or disables the menu item.
    /// </summary>
    public void SetIsActive(bool isActive, Guid? modifierId)
    {
        IsActive = isActive;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the display condition (auth, guest, always).
    /// </summary>
    public void SetDisplayCondition(string condition, Guid? modifierId)
    {
        DisplayCondition = string.IsNullOrWhiteSpace(condition) ? "always" : condition.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the order in which this item appears.
    /// </summary>
    public void SetDisplayOrder(int order, Guid? modifierId)
    {
        DisplayOrder = order;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Marks this menu item as logically deleted.
    /// </summary>
    public void MarkAsDeleted(Guid? modifierId)
    {
        MarkAsModified(modifierId, true);
    }

    /// <summary>
    /// Restores this menu item from a logically deleted state.
    /// </summary>
    public void Restore(Guid? modifierId)
    {
        MarkAsModified(modifierId, false);
    }
}
