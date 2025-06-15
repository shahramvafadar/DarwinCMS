using DarwinCMS.Application.DTOs.Permissions;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Permissions;

/// <summary>
/// Defines contract for all permission-related operations including retrieval, creation, update, deletion, and recycle bin functionality.
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
    /// Marks the specified permission as logically deleted (soft delete).
    /// </summary>
    /// <param name="id">The ID of the permission to soft delete.</param>
    /// <param name="userId">The ID of the user performing the soft delete.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted permission.
    /// </summary>
    /// <param name="id">The ID of the permission to restore.</param>
    /// <param name="userId">The ID of the user performing the restore.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a permission from the system (hard delete).
    /// </summary>
    /// <param name="id">The ID of the permission to hard delete.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all permissions that are logically (soft) deleted (recycle bin).
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>List of soft-deleted permissions.</returns>
    Task<List<Permission>> GetDeletedAsync(CancellationToken cancellationToken = default);
}
