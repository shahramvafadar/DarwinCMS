using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IRolePermissionRepository.
/// Handles assignment and lookup of permissions for roles.
/// </summary>
public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly DarwinDbContext _db;
    private readonly DbSet<RolePermission> _set;

    /// <summary>
    /// Initializes repository with injected DbContext.
    /// </summary>
    public RolePermissionRepository(DarwinDbContext db)
    {
        _db = db;
        _set = _db.RolePermissions;
    }

    /// <inheritdoc />
    public async Task<List<RolePermission>> GetByRoleIdAsync(Guid roleId, string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _set.Where(rp => rp.RoleId == roleId);

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(rp => rp.Module == module);

        return await query
            .Include(rp => rp.Permission)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<RolePermission>> GetByRoleIdsAsync(List<Guid> roleIds, string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _set.Where(rp => roleIds.Contains(rp.RoleId));

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(rp => rp.Module == module);

        return await query
            .Include(rp => rp.Permission)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<string>> GetPermissionNamesForRolesAsync(IEnumerable<Guid> roleIds)
    {
        return await _set
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Select(rp => rp.Permission!.Name)
            .Distinct()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<bool> DoesAnyRoleHavePermissionAsync(IEnumerable<Guid> roleIds, string permissionName, string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _set
            .Where(rp => roleIds.Contains(rp.RoleId) && rp.Permission!.Name == permissionName);

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(rp => rp.Module == module);

        return await query.AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid roleId, Guid permissionId, string? module = null, CancellationToken cancellationToken = default)
    {
        return await _set.AnyAsync(rp =>
            rp.RoleId == roleId &&
            rp.PermissionId == permissionId &&
            rp.Module == module,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(RolePermission rolePermission, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(rolePermission, cancellationToken);
    }

    /// <inheritdoc />
    public void Delete(RolePermission rolePermission)
    {
        _set.Remove(rolePermission);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
