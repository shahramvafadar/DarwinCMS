using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing site settings in the database.
/// Includes support for soft delete, restore, and permanent deletion.
/// </summary>
public interface ISiteSettingRepository : IRepository<SiteSetting>
{
    /// <summary>
    /// Gets a site setting by its key and optional language.
    /// </summary>
    Task<SiteSetting?> GetByKeyAsync(string key, string? languageCode = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads all active (non-deleted) settings in a specific category.
    /// </summary>
    Task<List<SiteSetting>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a site setting as logically deleted.
    /// </summary>
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted setting.
    /// </summary>
    Task RestoreAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a site setting from the database.
    /// Only for soft-deleted items.
    /// </summary>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all logically deleted site settings.
    /// </summary>
    Task<List<SiteSetting>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the query and returns number of matching items.
    /// </summary>
    Task<int> CountAsync(IQueryable<SiteSetting> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the query and returns the results as a list.
    /// </summary>
    Task<List<SiteSetting>> ToListAsync(IQueryable<SiteSetting> query, CancellationToken cancellationToken = default);

}
