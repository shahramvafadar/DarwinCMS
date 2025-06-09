using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing site settings in the CMS.
/// Includes support for full CRUD, soft deletion, restoration, and hard deletion operations.
/// </summary>
public interface ISiteSettingRepository : IRepository<SiteSetting>
{
    /// <summary>
    /// Retrieves a site setting by its unique key and optional language code.
    /// Returns null if not found.
    /// </summary>
    /// <param name="key">The unique key of the site setting.</param>
    /// <param name="languageCode">Optional language code (e.g. "en", "fa").</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<SiteSetting?> GetByKeyAsync(string key, string? languageCode = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads all active (non-deleted) site settings for a given category.
    /// </summary>
    /// <param name="category">The category name to filter settings.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<List<SiteSetting>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a custom query and returns the total number of matching site settings.
    /// </summary>
    /// <param name="query">The LINQ query to execute.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<int> CountAsync(IQueryable<SiteSetting> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a custom query and returns the matching site settings as a list.
    /// </summary>
    /// <param name="query">The LINQ query to execute.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<List<SiteSetting>> ToListAsync(IQueryable<SiteSetting> query, CancellationToken cancellationToken = default);
}
