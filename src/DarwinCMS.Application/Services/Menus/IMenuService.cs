using DarwinCMS.Application.DTOs.Menus;

namespace DarwinCMS.Application.Services.Menus;

/// <summary>
/// Application-level service contract for managing navigation menus and their items.
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Returns a list of all menus for management.
    /// </summary>
    Task<List<MenuListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the details of a specific menu for editing.
    /// </summary>
    Task<MenuDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new menu along with its items.
    /// </summary>
    Task<Guid> CreateAsync(CreateMenuDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing menu and its items.
    /// </summary>
    Task UpdateAsync(Guid id, UpdateMenuDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a menu and its items.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
