using DarwinCMS.Application.DTOs.Pages;

namespace DarwinCMS.Application.Services.Pages;

/// <summary>
/// Application-level service contract for managing Page entities.
/// Includes support for listing, CRUD, soft deletion, restoration, and hard deletion features.
/// </summary>
public interface IPageService
{
    /// <summary>
    /// Retrieves a paged and filtered list of pages for admin listing.
    /// </summary>
    /// <param name="filter">Filtering and sorting criteria.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<List<PageListItemDto>> GetListAsync(PageFilterDto filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single page for editing.
    /// </summary>
    /// <param name="id">The ID of the page.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<PageDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new CMS page based on user input.
    /// </summary>
    /// <param name="input">Data for creating a new page.</param>
    /// <param name="createdByUserId">The ID of the user who creates the page.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<Guid> CreateAsync(CreatePageDto input, Guid createdByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing page.
    /// </summary>
    /// <param name="id">The ID of the page to update.</param>
    /// <param name="input">Updated data for the page.</param>
    /// <param name="modifiedByUserId">The ID of the user who updates the page.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task UpdateAsync(Guid id, UpdatePageDto input, Guid modifiedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a logical (soft) deletion of a page.
    /// </summary>
    /// <param name="id">The ID of the page to delete.</param>
    /// <param name="userId">The ID of the user performing the deletion.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted page.
    /// </summary>
    /// <param name="id">The ID of the page to restore.</param>
    /// <param name="userId">The ID of the user performing the restore.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a page from the system (hard delete).
    /// </summary>
    /// <param name="id">The ID of the page to delete permanently.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads a list of soft-deleted pages (recycle bin).
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task<List<PageListItemDto>> GetDeletedAsync(CancellationToken cancellationToken = default);
}
