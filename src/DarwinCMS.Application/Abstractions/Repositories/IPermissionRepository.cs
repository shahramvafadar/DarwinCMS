using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for accessing and modifying Permission entities.
/// </summary>
public interface IPermissionRepository : IRepository<Permission>
{
    /// <summary>
    /// Returns all permissions optionally filtered by module name.
    /// </summary>
    Task<List<Permission>> GetAllAsync(string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a permission by internal name.
    /// </summary>
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the query and returns number of matching items.
    /// </summary>
    Task<int> CountAsync(IQueryable<Permission> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the query and returns the results as a list.
    /// </summary>
    Task<List<Permission>> ToListAsync(IQueryable<Permission> query, CancellationToken cancellationToken = default);
}
