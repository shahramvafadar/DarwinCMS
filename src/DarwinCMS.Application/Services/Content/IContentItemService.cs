using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Content;

/// <summary>
/// Contract for managing content items in the CMS.
/// </summary>
public interface IContentItemService
{
    /// <summary>
    /// Gets a content item by its unique identifier.
    /// </summary>
    Task<ContentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a content item by its slug and language code.
    /// </summary>
    Task<ContentItem?> GetBySlugAsync(string slug, string languageCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all content items for a specific content type or language.
    /// </summary>
    Task<IEnumerable<ContentItem>> GetAllAsync(string? contentType = null, string? languageCode = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new content item.
    /// </summary>
    Task<Guid> CreateAsync(ContentItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing content item.
    /// </summary>
    Task UpdateAsync(ContentItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a content item by its identifier.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
