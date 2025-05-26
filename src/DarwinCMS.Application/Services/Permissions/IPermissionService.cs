using DarwinCMS.Application.DTOs.Permissions;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Permissions;

/// <summary>
/// Defines contract for all permission-related operations including retrieval, creation, update, and deletion.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Returns a paged, filtered, and sorted list of permissions.
    /// </summary>
    /// <param name="search">Optional search term for filtering.</param>
    /// <param name="sortColumn">Column to sort by (e.g. "Name").</param>
    /// <param name="sortDirection">"asc" or "desc" sorting direction.</param>
    /// <param name="skip">Number of items to skip for paging.</param>
    /// <param name="take">Number of items to take per page.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    Task<PermissionListResultDto> GetPagedListAsync(
        string? search,
        string? sortColumn,
        string? sortDirection,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all permissions regardless of paging.
    /// </summary>
    Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a permission by its unique identifier.
    /// </summary>
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a permission by its internal name.
    /// </summary>
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new permission.
    /// </summary>
    Task CreateAsync(CreatePermissionRequest request, Guid createdBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing permission.
    /// </summary>
    Task UpdateAsync(UpdatePermissionRequest request, Guid modifiedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a permission by ID if it is not marked as system-critical.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
