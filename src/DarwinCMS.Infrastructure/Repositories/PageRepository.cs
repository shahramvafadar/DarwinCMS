using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Page entities with specific logic (slug, language, SEO).
/// </summary>
public class PageRepository : BaseRepository<Page>, IPageRepository
{
    /// <summary>
    /// Initializes a new instance of the Page repository.
    /// </summary>
    /// <param name="db">Darwin CMS database context.</param>
    public PageRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<Page?> GetBySlugAsync(string slug, string languageCode)
    {
        return await _set.FirstOrDefaultAsync(p => p.Slug == slug && p.LanguageCode == languageCode);
    }

    /// <inheritdoc />
    public async Task<bool> IsSlugUniqueAsync(string slug, string languageCode, Guid? excludingId = null)
    {
        return !await _set.AnyAsync(p =>
            p.Slug == slug &&
            p.LanguageCode == languageCode &&
            (excludingId == null || p.Id != excludingId));
    }
}
