using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing individual MenuItems.
/// </summary>
public interface IMenuItemRepository : IRepository<MenuItem>
{
    /// <summary>
    /// Returns all items under a specific menu.
    /// </summary>
    Task<List<MenuItem>> GetItemsByMenuIdAsync(Guid menuId);

    /// <summary>
    /// Returns all children items of a specific menu item.
    /// </summary>
    Task<List<MenuItem>> GetChildrenAsync(Guid parentItemId);
}
