using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing Page entities in the CMS.
/// Supports CRUD and filtering operations.
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
}
