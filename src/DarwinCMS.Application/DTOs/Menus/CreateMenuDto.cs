namespace DarwinCMS.Application.DTOs.Menus;

/// <summary>
/// Input model for creating a new navigation menu and its items.
/// </summary>
public class CreateMenuDto
{
    /// <summary>
    /// Title of the menu.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Location where the menu is shown (e.g., header, footer).
    /// </summary>
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Optional language code for multilingual menus.
    /// </summary>
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Whether the menu is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// List of menu items (flat list with parent-child references).
    /// </summary>
    public List<MenuItemDto> Items { get; set; } = new();
}
