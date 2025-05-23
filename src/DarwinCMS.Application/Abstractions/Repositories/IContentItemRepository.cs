using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository contract for managing content items in persistence.
/// </summary>
public interface IContentItemRepository
{
    /// <summary>
    /// Finds a content item by ID.
    /// </summary>
    Task<ContentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a content item by slug and language.
    /// </summary>
    Task<ContentItem?> GetBySlugAsync(string slug, string languageCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all content items, optionally filtered by type and language.
    /// </summary>
    Task<List<ContentItem>> GetAllAsync(string? contentType = null, string? languageCode = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new content item.
    /// </summary>
    Task AddAsync(ContentItem contentItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing content item.
    /// </summary>
    void Update(ContentItem contentItem);

    /// <summary>
    /// Removes a content item.
    /// </summary>
    void Delete(ContentItem contentItem);

    /// <summary>
    /// Commits pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
