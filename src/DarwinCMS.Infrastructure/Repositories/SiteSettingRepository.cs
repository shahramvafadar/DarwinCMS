using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Repository for managing SiteSetting values used in configuration and layout.
/// </summary>
public class SiteSettingRepository : BaseRepository<SiteSetting>, ISiteSettingRepository
{
    /// <summary>
    /// Initializes a new instance of the SiteSetting repository.
    /// </summary>
    /// <param name="db">Darwin CMS database context.</param>
    public SiteSettingRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<SiteSetting?> GetByKeyAsync(string key, string? languageCode = null)
    {
        return await _set.FirstOrDefaultAsync(s =>
            s.Key == key &&
            (languageCode == null || s.LanguageCode == languageCode));
    }

    /// <inheritdoc />
    public async Task<List<SiteSetting>> GetByCategoryAsync(string category)
    {
        return await _set
            .Where(s => s.Category == category)
            .ToListAsync();
    }
}
