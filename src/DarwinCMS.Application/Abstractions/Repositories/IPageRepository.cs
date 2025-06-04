using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing Page entities in the CMS.
/// Supports CRUD, filtering, and soft deletion operations.
/// </summary>
public interface IPageRepository : IRepository<Page>
{
    /// <summary>
    /// Checks if a given slug is already used by another page (optional language filter).
    /// </summary>
    Task<bool> IsSlugUniqueAsync(string slug, string languageCode, Guid? excludingId = null);

    /// <summary>
    /// Loads a page by its slug and language (for frontend usage).
    /// </summary>
    Task<Page?> GetBySlugAsync(string slug, string languageCode);

    /// <summary>
    /// Loads all pages that have been marked as logically deleted.
    /// </summary>
    Task<List<Page>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a permanent deletion of a page.
    /// </summary>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted page.
    /// </summary>
    Task RestoreAsync(Guid id, CancellationToken cancellationToken = default);
}
