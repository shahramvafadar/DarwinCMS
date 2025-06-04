using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Defines the contract for accessing and modifying MenuItem entities in persistence.
/// Used by application services to manage hierarchical navigation menus.
/// </summary>
public interface IMenuItemRepository : IRepository<MenuItem>
{
    /// <summary>
    /// Returns all items belonging to a specific menu (including nested items).
    /// </summary>
    Task<List<MenuItem>> GetByMenuIdAsync(Guid menuId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all children of a specific menu item (direct descendants only).
    /// </summary>
    Task<List<MenuItem>> GetChildrenAsync(Guid parentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single menu item by ID including navigation to its children.
    /// </summary>
    Task<MenuItem?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken = default);
}
