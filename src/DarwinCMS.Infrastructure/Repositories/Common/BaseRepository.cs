using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories.Common;

/// <summary>
/// Generic base repository for managing common CRUD, soft deletion, and restore logic for all entities.
/// Used for all repositories managing domain entities that inherit from BaseEntity.
/// </summary>
/// <typeparam name="TEntity">The entity type managed by this repository.</typeparam>
public class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Shared DarwinDbContext instance used for querying and persistence.
    /// </summary>
    protected readonly DarwinDbContext _db;

    /// <summary>
    /// EF Core DbSet for the specified entity type.
    /// </summary>
    protected readonly DbSet<TEntity> _set;

    /// <summary>
    /// Initializes a new instance of the base repository.
    /// </summary>
    /// <param name="db">The DarwinDbContext instance injected via DI.</param>
    public BaseRepository(DarwinDbContext db)
    {
        _db = db;
        _set = db.Set<TEntity>();
    }

    /// <inheritdoc/>
    public virtual IQueryable<TEntity> Query() => _set.AsQueryable();

    /// <inheritdoc/>
    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _set.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual void Update(TEntity entity)
    {
        _set.Update(entity);
    }

    /// <inheritdoc/>
    public virtual void Delete(TEntity entity)
    {
        _set.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
            return;

        entity.MarkAsModified(userId, true); // true → mark as deleted
        Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
            return;

        entity.MarkAsModified(userId, false); // false → restore (mark as not deleted)
        Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            Delete(entity);
            await SaveChangesAsync(cancellationToken);
        }
    }

    /// <inheritdoc/>
    public virtual async Task<List<TEntity>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _set
            .Where(e => e.IsDeleted)
            .OrderByDescending(e => e.ModifiedAt ?? e.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
