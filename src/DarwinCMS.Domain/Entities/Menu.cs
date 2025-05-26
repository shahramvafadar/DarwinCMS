namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a navigation menu, such as header or footer.
/// Contains a collection of menu items for structured navigation.
/// </summary>
public class Menu : BaseEntity
{
    /// <summary>
    /// Title of the menu (e.g. Main Menu, Footer Menu).
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Location of the menu in the layout (e.g. header, footer, sidebar).
    /// </summary>
    public string Position { get; set; } = "header";

    /// <summary>
    /// Indicates whether this menu is active and visible.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Optional language code for multilingual menus.
    /// </summary>
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Optional system flag to prevent deletion.
    /// </summary>
    public bool IsSystem { get; set; } = false;

    /// <summary>
    /// Collection of items within this menu.
    /// </summary>
    public virtual ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
}
