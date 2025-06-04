namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;

/// <summary>
/// View model shown on the delete confirmation page for menus.
/// Only basic information is shown before deletion.
/// </summary>
public class DeleteMenuViewModel
{
    /// <summary>
    /// Unique ID of the menu.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display title of the menu.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Position label of the menu.
    /// </summary>
    public string Position { get; set; } = string.Empty;
}
