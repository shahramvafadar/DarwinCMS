using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation for managing ContentItem persistence.
/// </summary>
public class ContentItemRepository : IContentItemRepository
{
    private readonly DarwinDbContext _dbContext;

    /// <summary>
    /// Initializes repository with the shared database context.
    /// </summary>
    public ContentItemRepository(DarwinDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<ContentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.ContentItems.FindAsync(new object[] { id }, cancellationToken);

    /// <inheritdoc />
    public async Task<ContentItem?> GetBySlugAsync(string slug, string languageCode, CancellationToken cancellationToken = default)
        => await _dbContext.ContentItems
            .FirstOrDefaultAsync(c => c.Slug.Value == slug && c.LanguageCode == languageCode, cancellationToken);

    /// <inheritdoc />
    public async Task<List<ContentItem>> GetAllAsync(string? contentType = null, string? languageCode = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.ContentItems.AsQueryable();

        if (!string.IsNullOrWhiteSpace(contentType))
            query = query.Where(c => c.ContentType == contentType);

        if (!string.IsNullOrWhiteSpace(languageCode))
            query = query.Where(c => c.LanguageCode == languageCode);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(ContentItem contentItem, CancellationToken cancellationToken = default)
        => await _dbContext.ContentItems.AddAsync(contentItem, cancellationToken);

    /// <inheritdoc />
    public void Update(ContentItem contentItem)
        => _dbContext.ContentItems.Update(contentItem);

    /// <inheritdoc />
    public void Delete(ContentItem contentItem)
        => _dbContext.ContentItems.Remove(contentItem);

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}
