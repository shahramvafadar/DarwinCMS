using DarwinCMS.Application.DTOs.SiteSettings;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Settings;

/// <summary>
/// Service interface for retrieving and modifying site configuration settings.
/// Includes support for localization, caching, and soft deletion.
/// </summary>
public interface ISiteSettingService
{
    /// <summary>
    /// Returns the raw string value of the specified setting key.
    /// Throws NotFoundException if not found.
    /// </summary>
    Task<string> GetValueAsync(string key, string? languageCode = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the setting value converted to a specific type.
    /// </summary>
    Task<T> GetValueAsAsync<T>(string key, string? languageCode = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all active (non-deleted) site settings.
    /// </summary>
    Task<List<SiteSetting>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all logically deleted site settings for admin restoration.
    /// </summary>
    Task<List<SiteSetting>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a site setting's key, language code, value, and other metadata.
    /// Uses OldKey and OldLanguageCode for lookup to support editable composite keys.
    /// </summary>
    Task UpdateValueAsync(
        string oldKey,
        string? oldLanguageCode,
        string newKey,
        string? newLanguageCode,
        string newValue,
        Guid modifiedByUserId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new setting entry.
    /// </summary>
    Task CreateAsync(SiteSetting newSetting, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes the specified setting. System-protected entries cannot be deleted.
    /// </summary>
    /// <param name="id">The ID of the setting to soft delete.</param>
    /// <param name="deletedByUserId">The ID of the user performing the deletion.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task SoftDeleteAsync(Guid id, Guid deletedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted setting.
    /// </summary>
    /// <param name="id">The ID of the setting to restore.</param>
    /// <param name="userId">The ID of the user performing the restoration.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a site setting from the database.
    /// Only for non-system soft-deleted settings.
    /// </summary>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paged, filtered, and sorted list of site settings.
    /// </summary>
    Task<SiteSettingListResultDto> GetPagedListAsync(
        string? searchTerm,
        string? sortColumn,
        string? sortDirection,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
}
