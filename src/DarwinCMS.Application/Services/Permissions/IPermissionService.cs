using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Permissions;

/// <summary>
/// Contract for permission-related operations.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Returns all defined permissions.
    /// </summary>
    Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// Returns a permission by ID.
    /// </summary>
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a permission by name.
    /// </summary>
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a permission by ID if exists.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
