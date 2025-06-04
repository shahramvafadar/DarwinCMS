using System;
using System.Collections.Generic;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a navigation menu, such as header or footer.
/// Contains a collection of menu items for structured navigation.
/// </summary>
public class Menu : BaseEntity
{
    /// <summary>
    /// Title of the menu (e.g., "Main Menu", "Footer Menu").
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Location of the menu in the layout (e.g., "header", "footer", "sidebar").
    /// </summary>
    public string Position { get; private set; } = "header";

    /// <summary>
    /// Indicates whether this menu is active and visible.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Optional language code for multilingual menus.
    /// </summary>
    public string LanguageCode { get; private set; } = "en";

    /// <summary>
    /// Optional system flag to prevent deletion.
    /// </summary>
    public bool IsSystem { get; private set; } = false;

    /// <summary>
    /// Collection of items within this menu.
    /// </summary>
    public virtual ICollection<MenuItem> Items { get; private set; } = new List<MenuItem>();

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected Menu() { }

    /// <summary>
    /// Creates a new menu entity with required fields.
    /// </summary>
    /// <param name="title">Title of the menu.</param>
    /// <param name="position">Position of the menu (e.g., header, footer).</param>
    /// <param name="languageCode">Language code for the menu.</param>
    /// <param name="createdByUserId">User ID of the creator.</param>
    public Menu(string title, string position, string languageCode, Guid createdByUserId)
    {
        SetTitle(title, createdByUserId);
        SetPosition(position, createdByUserId);
        SetLanguage(languageCode, createdByUserId);
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Updates the menu title.
    /// </summary>
    public void SetTitle(string title, Guid? modifierId)
    {
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title is required.", nameof(title)) : title.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Updates the position (location) of the menu.
    /// </summary>
    public void SetPosition(string position, Guid? modifierId)
    {
        Position = string.IsNullOrWhiteSpace(position) ? "header" : position.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Updates the language code of the menu.
    /// </summary>
    public void SetLanguage(string code, Guid? modifierId)
    {
        LanguageCode = string.IsNullOrWhiteSpace(code) ? "en" : code.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Deactivates the menu.
    /// </summary>
    public void Deactivate(Guid? modifierId)
    {
        IsActive = false;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Reactivates the menu.
    /// </summary>
    public void Activate(Guid? modifierId)
    {
        IsActive = true;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Marks the menu as system-defined (cannot be deleted).
    /// </summary>
    public void MarkAsSystem(Guid? modifierId)
    {
        IsSystem = true;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Marks this menu as logically deleted.
    /// </summary>
    public void MarkAsDeleted(Guid? modifierId)
    {
        MarkAsModified(modifierId, true);
    }

    /// <summary>
    /// Restores this menu from a logically deleted state.
    /// </summary>
    public void Restore(Guid? modifierId)
    {
        MarkAsModified(modifierId, false);
    }
}
