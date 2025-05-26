using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for retrieving and updating site settings.
/// </summary>
public interface ISiteSettingRepository : IRepository<SiteSetting>
{
    /// <summary>
    /// Gets a site setting by its key and optional language.
    /// </summary>
    Task<SiteSetting?> GetByKeyAsync(string key, string? languageCode = null);

    /// <summary>
    /// Loads all settings in a specific category.
    /// </summary>
    Task<List<SiteSetting>> GetByCategoryAsync(string category);
}
