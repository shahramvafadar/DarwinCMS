using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Default implementation of IPermissionRepository using EF Core.
/// </summary>
public class PermissionRepository : IPermissionRepository
{
    private readonly DarwinDbContext _dbContext;

    /// <summary>
    /// Initializes the repository with the provided DbContext.
    /// </summary>
    public PermissionRepository(DarwinDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Permissions.FindAsync(new object[] { id }, cancellationToken);

    /// <inheritdoc />
    public async Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);

    /// <inheritdoc />
    public async Task<List<Permission>> GetAllAsync(string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Permissions.AsQueryable();

        if (!string.IsNullOrWhiteSpace(module))
        {
            query = query.Where(p => p.Module == module);
        }

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(Permission permission, CancellationToken cancellationToken = default)
        => await _dbContext.Permissions.AddAsync(permission, cancellationToken);

    /// <inheritdoc />
    public void Update(Permission permission)
        => _dbContext.Permissions.Update(permission);

    /// <inheritdoc />
    public void Delete(Permission permission)
        => _dbContext.Permissions.Remove(permission);

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}
