using System.Linq.Expressions;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Generic repository interface providing basic CRUD and query operations.
/// Used as a base for all entity-specific repositories.
/// </summary>
/// <typeparam name="TEntity">The entity type managed by this repository.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Gets a single entity by its unique ID.
    /// </summary>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a queryable set for LINQ-based filtering and projection.
    /// </summary>
    IQueryable<TEntity> Query();

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    void Update(TEntity entity);

    /// <summary>
    /// Marks an entity for deletion.
    /// </summary>
    void Delete(TEntity entity);

    /// <summary>
    /// Persists all changes to the underlying data store.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
