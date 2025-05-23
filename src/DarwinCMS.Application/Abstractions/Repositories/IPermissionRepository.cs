using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Contract for managing permission-related operations.
/// </summary>
public interface IPermissionRepository
{
    /// <summary>
    /// Gets a permission by its unique identifier.
    /// </summary>
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a permission by its unique name.
    /// </summary>
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all permissions, optionally filtered by module.
    /// </summary>
    Task<List<Permission>> GetAllAsync(string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new permission to the repository.
    /// </summary>
    Task AddAsync(Permission permission, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the permission entity as modified.
    /// </summary>
    void Update(Permission permission);

    /// <summary>
    /// Deletes a permission from the store.
    /// </summary>
    void Delete(Permission permission);

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
