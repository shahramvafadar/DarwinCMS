using DarwinCMS.Application.DTOs.Menus;

namespace DarwinCMS.Application.Services.Menus;

/// <summary>
/// Application service for managing menu items (navigation entries).
/// Supports nested structures, linking, and visibility conditions.
/// </summary>
public interface IMenuItemService
{
    /// <summary>
    /// Retrieves all items belonging to a specific menu, optionally as a tree.
    /// </summary>
    /// <param name="menuId">The unique ID of the menu.</param>
    /// <returns>A tree-structured list of items.</returns>
    Task<List<MenuItemDto>> GetItemsByMenuIdAsync(Guid menuId);

    /// <summary>
    /// Retrieves a specific item by its ID including its children.
    /// </summary>
    /// <param name="id">Unique ID of the item.</param>
    /// <returns>The item and its immediate children, or null if not found.</returns>
    Task<MenuItemDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new menu item.
    /// </summary>
    /// <param name="dto">The data required to create a new item.</param>
    Task CreateAsync(CreateMenuItemDto dto);

    /// <summary>
    /// Updates an existing menu item.
    /// </summary>
    /// <param name="dto">The data to apply to the item.</param>
    Task UpdateAsync(UpdateMenuItemDto dto);

    /// <summary>
    /// Deletes the specified menu item.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    Task DeleteAsync(Guid id);
}
