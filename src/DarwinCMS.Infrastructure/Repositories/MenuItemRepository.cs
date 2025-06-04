using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IMenuItemRepository"/>.
/// Supports full CRUD and tree traversal for menu items.
/// </summary>
public class MenuItemRepository : BaseRepository<MenuItem>, IMenuItemRepository
{
    /// <summary>
    /// Initializes the repository with EF Core context.
    /// </summary>
    public MenuItemRepository(DarwinDbContext db) : base(db) { }


    /// <inheritdoc />
    public async Task<List<MenuItem>> GetByMenuIdAsync(Guid menuId, CancellationToken cancellationToken = default)
    {
        return await _db.MenuItems
            .Where(mi => mi.MenuId == menuId)
            .Include(mi => mi.Children)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<MenuItem>> GetChildrenAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        return await _db.MenuItems
            .Where(mi => mi.ParentId == parentId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<MenuItem?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.MenuItems
            .Include(mi => mi.Children)
            .FirstOrDefaultAsync(mi => mi.Id == id, cancellationToken);
    }
}
