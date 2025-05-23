using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Content;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.Interfaces;

namespace DarwinCMS.Infrastructure.Services.Content;

/// <summary>
/// Application-level service for managing content items.
/// </summary>
public class ContentItemService : IContentItemService
{
    private readonly IContentItemRepository _repository;

    /// <summary>
    /// Initializes the service with the required repository.
    /// </summary>
    public ContentItemService(IContentItemRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Adds a new content item to the database.
    /// </summary>
    /// <param name="item">The content item entity to create.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The ID of the newly created content item.</returns>
    public async Task<Guid> CreateAsync(ContentItem item, CancellationToken cancellationToken = default)
    {
        await _repository.AddAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return item.Id;
    }

    /// <summary>
    /// Deletes the content item with the given ID.
    /// </summary>
    /// <param name="id">ID of the content item to delete.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);
        if (item is not null)
        {
            _repository.Delete(item);
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Gets all content items, optionally filtered by content type or language.
    /// </summary>
    public async Task<IEnumerable<ContentItem>> GetAllAsync(string? contentType = null, string? languageCode = null, CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(contentType, languageCode, cancellationToken);
    }

    /// <summary>
    /// Gets a content item by its unique ID.
    /// </summary>
    public async Task<ContentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    /// <summary>
    /// Gets a content item by slug and language.
    /// </summary>
    public async Task<ContentItem?> GetBySlugAsync(string slug, string languageCode, CancellationToken cancellationToken = default)
    {
        return await _repository.GetBySlugAsync(slug, languageCode, cancellationToken);
    }

    /// <summary>
    /// Updates an existing content item.
    /// </summary>
    public async Task UpdateAsync(ContentItem item, CancellationToken cancellationToken = default)
    {
        _repository.Update(item);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
