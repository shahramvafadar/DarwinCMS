using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IPermissionRepository"/>.
/// Provides filtering, counting, and general data access for permissions.
/// </summary>
public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
{
    /// <summary>
    /// Initializes the repository with the provided DarwinDbContext.
    /// </summary>
    /// <param name="db">The EF Core DbContext for DarwinCMS.</param>
    public PermissionRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<List<Permission>> GetAllAsync(string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _set.AsQueryable();
        query = query.Where(p => !p.IsDeleted);

        // Filter by module if provided
        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(p => p.Module == module);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _set
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name && !p.IsDeleted, cancellationToken);
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
