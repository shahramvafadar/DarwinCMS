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
    /// <param name="oldKey">The old key of the setting for composite lookup.</param>
    /// <param name="oldLanguageCode">The old language code of the setting for composite lookup.</param>
    /// <param name="newKey">The new key to update.</param>
    /// <param name="newLanguageCode">The new language code to update.</param>
    /// <param name="newValue">The new value to update.</param>
    /// <param name="modifiedBy">The user who modifies the setting.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    Task UpdateValueAsync(
        string oldKey,
        string? oldLanguageCode,
        string newKey,
        string? newLanguageCode,
        string newValue,
        Guid modifiedBy,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Creates a new setting entry.
    /// </summary>
    Task CreateAsync(SiteSetting newSetting, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes the specified setting. System-protected entries cannot be deleted.
    /// </summary>
    Task SoftDeleteAsync(Guid id, Guid? deletedBy = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted setting.
    /// </summary>
    Task RestoreAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a site setting from the database.
    /// Only for non-system soft-deleted settings.
    /// </summary>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paged, filtered, and sorted list of site settings.
    /// </summary>
    /// <param name="searchTerm">Optional search term for filtering.</param>
    /// <param name="sortColumn">Column to sort by (e.g., "Key", "Category").</param>
    /// <param name="sortDirection">"asc" or "desc" sorting direction.</param>
    /// <param name="skip">Number of items to skip for paging.</param>
    /// <param name="take">Number of items to take per page.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    Task<SiteSettingListResultDto> GetPagedListAsync(
        string? searchTerm,
        string? sortColumn,
        string? sortDirection,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

}
