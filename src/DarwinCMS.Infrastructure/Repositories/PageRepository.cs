using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Page entities with additional logic (like slug uniqueness).
/// </summary>
public class PageRepository : BaseRepository<Page>, IPageRepository
{
    /// <summary>
    /// Initializes a new PageRepository.
    /// </summary>
    /// <param name="db">Darwin CMS database context.</param>
    public PageRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc/>
    public async Task<Page?> GetBySlugAsync(string slug, string languageCode)
    {
        return await _set
            .FirstOrDefaultAsync(p => p.SlugValue == slug && p.LanguageCode == languageCode && !p.IsDeleted);
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
}
