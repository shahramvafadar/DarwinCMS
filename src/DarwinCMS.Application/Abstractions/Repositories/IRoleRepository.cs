using DarwinCMS.Domain.Entities;
using System.Linq.Expressions;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Defines repository contract for managing Role entities,
/// including CRUD operations and flexible querying.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Returns a single role by its unique identifier.
    /// </summary>
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single role by its unique technical name.
    /// </summary>
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all roles that are marked as active.
    /// </summary>
    Task<List<Role>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new Role to the repository for persistence.
    /// </summary>
    Task AddAsync(Role role, CancellationToken cancellationToken = default);

    /// <summary>
    /// Flags an existing Role as modified (for update scenarios).
    /// </summary>
    void Update(Role role);

    /// <summary>
    /// Permanently deletes the given Role from persistence.
    /// </summary>
    void Delete(Role role);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Exposes an IQueryable of Roles for advanced querying
    /// (e.g., filtering, paging, sorting).
    /// </summary>
    IQueryable<Role> Query();
}
