using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories.Common;

/// <summary>
/// Generic base repository implementing common data access logic.
/// Used for all repositories managing domain entities.
/// </summary>
/// <typeparam name="TEntity">The entity type managed by this repository.</typeparam>
public class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
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
        _set = _db.Set<TEntity>();
    }

    /// <inheritdoc />
    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _set.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <inheritdoc />
    public virtual IQueryable<TEntity> Query()
    {
        return _set.AsQueryable();
    }

    /// <inheritdoc />
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public virtual void Update(TEntity entity)
    {
        _set.Update(entity);
    }

    /// <inheritdoc />
    public virtual void Delete(TEntity entity)
    {
        _set.Remove(entity);
    }

    /// <inheritdoc />
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
