using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for accessing and modifying Permission entities in the system.
/// Provides module-based filtering and custom query execution.
/// </summary>
public interface IPermissionRepository : IRepository<Permission>
{
    /// <summary>
    /// Returns all permissions optionally filtered by module name.
    /// </summary>
    /// <param name="module">Optional module name to filter by.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>List of permissions matching the criteria.</returns>
    Task<List<Permission>> GetAllAsync(string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a permission entity by its unique internal name.
    /// </summary>
    /// <param name="name">The internal name of the permission.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>The matching permission entity or null if not found.</returns>
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the provided query and returns the number of matching entities.
    /// </summary>
    /// <param name="query">The query to execute.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>The number of entities matching the query.</returns>
    Task<int> CountAsync(IQueryable<Permission> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the provided query and returns the results as a list.
    /// </summary>
    /// <param name="query">The query to execute.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>List of permissions matching the query.</returns>
    Task<List<Permission>> ToListAsync(IQueryable<Permission> query, CancellationToken cancellationToken = default);
}
