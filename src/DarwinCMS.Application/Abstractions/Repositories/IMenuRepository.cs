using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing Menu entities and their related items.
/// </summary>
public interface IMenuRepository : IRepository<Menu>
{
    /// <summary>
    /// Loads a menu and its items based on position and language code.
    /// </summary>
    Task<Menu?> GetMenuWithItemsAsync(string position, string languageCode);

    /// <summary>
    /// Returns all menus with their item counts for admin overview.
    /// </summary>
    Task<List<Menu>> GetAllWithItemsAsync();
}
