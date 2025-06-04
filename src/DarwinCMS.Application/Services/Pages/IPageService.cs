using DarwinCMS.Application.DTOs.Pages;

namespace DarwinCMS.Application.Services.Pages;

/// <summary>
/// Application-level service contract for managing Page entities.
/// Includes support for listing, CRUD, and soft deletion features.
/// </summary>
public interface IPageService
{
    /// <summary>
    /// Retrieves a paged and filtered list of pages for admin listing.
    /// </summary>
    Task<List<PageListItemDto>> GetListAsync(PageFilterDto filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single page for editing.
    /// </summary>
    Task<PageDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new CMS page based on user input.
    /// </summary>
    Task<Guid> CreateAsync(CreatePageDto input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing page.
    /// </summary>
    Task UpdateAsync(Guid id, UpdatePageDto input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a logical (soft) deletion of a page.
    /// </summary>
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a page from the system.
    /// </summary>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads a list of soft-deleted pages (recycle bin).
    /// </summary>
    Task<List<PageListItemDto>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted page.
    /// </summary>
    Task RestoreAsync(Guid id, CancellationToken cancellationToken = default);
}
