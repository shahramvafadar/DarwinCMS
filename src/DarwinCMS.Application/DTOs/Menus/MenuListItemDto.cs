namespace DarwinCMS.Application.DTOs.Menus;

/// <summary>
/// Represents a simplified view of a Menu used for admin list display.
/// </summary>
public class MenuListItemDto
{
    /// <summary>
    /// Unique identifier of the menu.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Menu display title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Location where this menu is shown (e.g., header, footer).
    /// </summary>
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Language code of the menu (e.g., en, fa).
    /// </summary>
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Whether the menu is currently active.
    /// </summary>
    public bool IsActive { get; set; }
}
