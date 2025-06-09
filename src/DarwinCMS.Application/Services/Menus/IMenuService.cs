using DarwinCMS.Application.DTOs.Menus;

namespace DarwinCMS.Application.Services.Menus;

/// <summary>
/// Application-level service contract for managing navigation menus and their items.
/// Supports CRUD, soft delete, restore, and permanent deletion operations.
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Returns a list of all menus for management.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A list of menus.</returns>
    Task<List<MenuListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the details of a specific menu for editing.
    /// </summary>
    /// <param name="id">The unique identifier of the menu.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The detailed DTO of the menu, or null if not found.</returns>
    Task<MenuDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new menu along with its items.
    /// </summary>
    /// <param name="dto">The input DTO containing menu data.</param>
    /// <param name="createdByUserId">The user ID who is creating the menu.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The newly created menu's unique identifier.</returns>
    Task<Guid> CreateAsync(CreateMenuDto dto, Guid createdByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing menu and its items.
    /// </summary>
    /// <param name="id">The ID of the menu to update.</param>
    /// <param name="dto">The updated menu data.</param>
    /// <param name="modifiedByUserId">The user ID who is updating the menu.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task UpdateAsync(Guid id, UpdateMenuDto dto, Guid modifiedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a logical (soft) deletion of a menu.
    /// </summary>
    /// <param name="id">The ID of the menu to soft delete.</param>
    /// <param name="userId">The user ID performing the deletion.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted menu.
    /// </summary>
    /// <param name="id">The ID of the menu to restore.</param>
    /// <param name="userId">The user ID performing the restoration.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a menu from the system.
    /// </summary>
    /// <param name="id">The ID of the menu to delete permanently.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a list of menus that have been logically deleted.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A list of deleted menus.</returns>
    Task<List<MenuListItemDto>> GetDeletedAsync(CancellationToken cancellationToken = default);
}
