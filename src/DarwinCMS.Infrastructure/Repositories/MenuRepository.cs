using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Menu entities and loading their items.
/// </summary>
public class MenuRepository : BaseRepository<Menu>, IMenuRepository
{
    /// <summary>
    /// Initializes a new instance of the Menu repository.
    /// </summary>
    /// <param name="db">Darwin CMS database context.</param>
    public MenuRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<Menu?> GetMenuWithItemsAsync(string position, string languageCode)
    {
        return await _set
            .Include(m => m.Items.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(m => m.Position == position && m.LanguageCode == languageCode && m.IsActive);
    }

    /// <inheritdoc />
    public async Task<List<Menu>> GetAllWithItemsAsync()
    {
        return await _set
            .Include(m => m.Items)
            .OrderBy(m => m.Title)
            .ToListAsync();
    }
}
