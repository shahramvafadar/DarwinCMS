using System.Linq.Expressions;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Generic repository interface providing standard CRUD, soft deletion, hard deletion, and restoration operations.
/// Designed as a base for all entity-specific repositories.
/// </summary>
/// <typeparam name="TEntity">The entity type managed by this repository.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Provides a queryable data source for LINQ-based filtering and projection.
    /// </summary>
    /// <returns>A queryable set for the entity.</returns>
    IQueryable<TEntity> Query();

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Marks an entity for permanent deletion.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Saves all changes made in this repository to the data store.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a logical (soft) deletion by marking the entity as deleted.
    /// </summary>
    /// <param name="id">The ID of the entity to soft delete.</param>
    /// <param name="userId">The ID of the user performing the deletion.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted entity.
    /// </summary>
    /// <param name="id">The ID of the entity to restore.</param>
    /// <param name="userId">The ID of the user performing the restoration.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes an entity from the data store (hard delete).
    /// </summary>
    /// <param name="id">The ID of the entity to delete permanently.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities marked as logically deleted (soft-deleted).
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A list of soft-deleted entities.</returns>
    Task<List<TEntity>> GetDeletedAsync(CancellationToken cancellationToken = default);
}
