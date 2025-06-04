namespace DarwinCMS.Application.DTOs.Menus;

/// <summary>
/// Output model for displaying full menu details (used in edit form).
/// </summary>
public class MenuDetailDto : CreateMenuDto
{
    /// <summary>
    /// Unique identifier of the menu.
    /// </summary>
    public Guid Id { get; set; }
}
