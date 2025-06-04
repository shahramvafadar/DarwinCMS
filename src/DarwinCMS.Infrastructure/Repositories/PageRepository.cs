using Azure;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Page entities with specific logic (slug, language, SEO, soft deletion).
/// </summary>
public class PageRepository : BaseRepository<Page>, IPageRepository
{
    /// <summary>
    /// Initializes a new instance of the Page repository.
    /// </summary>
    /// <param name="db">Darwin CMS database context.</param>
    public PageRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc/>
    public async Task<Page?> GetBySlugAsync(string slug, string languageCode)
    {
        return await _set
            .FirstOrDefaultAsync(p => p.Slug.Value == slug && p.LanguageCode == languageCode && !p.IsDeleted);
    }

    /// <inheritdoc/>
    public async Task<bool> IsSlugUniqueAsync(string slug, string languageCode, Guid? excludingId = null)
    {
        return !await _set
            .Where(p => p.SlugValue == slug &&
                        p.LanguageCode == languageCode &&
                        !p.IsDeleted &&
                        (excludingId == null || p.Id != excludingId))
            .AnyAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Page>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _set.Where(p => p.IsDeleted)
            .OrderByDescending(p => p.ModifiedAt ?? p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _set.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            _set.Remove(entity);
        }
    }

    /// <inheritdoc/>
    public async Task RestoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _set.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            entity.Restore();
            entity.MarkAsModified();
            _set.Update(entity);
        }
    }
}
