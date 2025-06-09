using DarwinCMS.Application.DTOs.Menus;

namespace DarwinCMS.Application.Services.Menus;

/// <summary>
/// Application service for managing menu items (navigation entries).
/// Supports nested structures, linking, visibility conditions, and full lifecycle management.
/// </summary>
public interface IMenuItemService
{
    /// <summary>
    /// Retrieves all items belonging to a specific menu, optionally as a tree.
    /// </summary>
    /// <param name="menuId">The unique ID of the menu.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A tree-structured list of items.</returns>
    Task<List<MenuItemDto>> GetItemsByMenuIdAsync(Guid menuId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific item by its ID including its children.
    /// </summary>
    /// <param name="id">Unique ID of the item.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The item and its immediate children, or null if not found.</returns>
    Task<MenuItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new menu item.
    /// </summary>
    /// <param name="dto">The data required to create a new item.</param>
    /// <param name="createdByUserId">ID of the user creating the item.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<Guid> CreateAsync(CreateMenuItemDto dto, Guid createdByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing menu item.
    /// </summary>
    /// <param name="dto">The data to apply to the item.</param>
    /// <param name="modifiedByUserId">ID of the user modifying the item.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task UpdateAsync(UpdateMenuItemDto dto, Guid modifiedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a logical (soft) deletion of a menu item.
    /// </summary>
    /// <param name="id">The ID of the item to soft delete.</param>
    /// <param name="userId">ID of the user performing the deletion.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted menu item.
    /// </summary>
    /// <param name="id">The ID of the item to restore.</param>
    /// <param name="userId">ID of the user performing the restoration.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a menu item from the system.
    /// </summary>
    /// <param name="id">The ID of the item to delete permanently.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of menu items that have been logically deleted.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A list of deleted menu items.</returns>
    Task<List<MenuItemDto>> GetDeletedAsync(CancellationToken cancellationToken = default);
}
