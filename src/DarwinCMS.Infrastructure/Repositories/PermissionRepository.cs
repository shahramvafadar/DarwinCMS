using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Default implementation of IPermissionRepository using EF Core.
/// </summary>
public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
{
    /// <summary>
    /// Initializes the repository with the provided DarwinDbContext.
    /// </summary>
    public PermissionRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<List<Permission>> GetAllAsync(string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _set.AsQueryable();

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(p => p.Module == module);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _set.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(IQueryable<Permission> query, CancellationToken cancellationToken = default)
    {
        return await query.CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Permission>> ToListAsync(IQueryable<Permission> query, CancellationToken cancellationToken = default)
    {
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
}
