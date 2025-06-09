using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Entity Framework Core implementation of the <see cref="ISiteSettingRepository"/>.
/// Provides data access, soft delete, restore, and permanent delete capabilities for <see cref="SiteSetting"/> entities.
/// </summary>
public class SiteSettingRepository : BaseRepository<SiteSetting>, ISiteSettingRepository
{
    /// <summary>
    /// Initializes the repository with the EF Core database context.
    /// </summary>
    /// <param name="db">The application's main EF DbContext.</param>
    public SiteSettingRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc/>
    public async Task<SiteSetting?> GetByKeyAsync(string key, string? languageCode, CancellationToken cancellationToken = default)
    {
        return await _set
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Key == key && s.LanguageCode == languageCode && !s.IsDeleted, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<SiteSetting>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _set
            .Where(s => s.Category == category && !s.IsDeleted)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(IQueryable<SiteSetting> query, CancellationToken cancellationToken = default)
    {
        return await query.CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<SiteSetting>> ToListAsync(IQueryable<SiteSetting> query, CancellationToken cancellationToken = default)
    {
        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

}
