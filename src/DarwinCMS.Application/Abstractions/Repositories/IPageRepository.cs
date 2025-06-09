using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing Page entities in the CMS.
/// Provides page-specific query methods in addition to generic CRUD and soft-deletion logic inherited from IRepository.
/// </summary>
public interface IPageRepository : IRepository<Page>
{
    /// <summary>
    /// Checks if a given slug is already used by another page, with an optional exclusion of a specific page ID.
    /// </summary>
    /// <param name="slug">The slug to check for uniqueness.</param>
    /// <param name="languageCode">The language code to limit the search (e.g., "en", "fa").</param>
    /// <param name="excludingId">Optional ID of the page to exclude from the check (e.g., when updating a page).</param>
    /// <returns>True if the slug is unique within the language; otherwise, false.</returns>
    Task<bool> IsSlugUniqueAsync(string slug, string languageCode, Guid? excludingId = null);

    /// <summary>
    /// Loads a page by its slug and language code for public or frontend usage.
    /// </summary>
    /// <param name="slug">The slug of the page to retrieve.</param>
    /// <param name="languageCode">The language code (e.g., "en").</param>
    /// <returns>The matching page or null if not found.</returns>
    Task<Page?> GetBySlugAsync(string slug, string languageCode);
}
