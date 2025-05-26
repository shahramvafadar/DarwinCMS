using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Repository for managing individual MenuItems, including nested children.
/// </summary>
public class MenuItemRepository : BaseRepository<MenuItem>, IMenuItemRepository
{
    /// <summary>
    /// Initializes a new instance of the MenuItem repository.
    /// </summary>
    /// <param name="db">Darwin CMS database context.</param>
    public MenuItemRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<List<MenuItem>> GetItemsByMenuIdAsync(Guid menuId)
    {
        return await _set
            .Where(mi => mi.MenuId == menuId && mi.ParentItemId == null)
            .Include(mi => mi.Children.OrderBy(c => c.DisplayOrder))
            .OrderBy(mi => mi.DisplayOrder)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<MenuItem>> GetChildrenAsync(Guid parentItemId)
    {
        return await _set
            .Where(mi => mi.ParentItemId == parentItemId)
            .OrderBy(mi => mi.DisplayOrder)
            .ToListAsync();
    }
}
