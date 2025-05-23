using DarwinCMS.Domain.Entities;
using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IRolePermissionRepository.
/// Handles assignment and lookup of permissions for roles.
/// </summary>
public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly DarwinDbContext _dbContext;

    /// <summary>
    /// Initializes repository with injected DbContext.
    /// </summary>
    public RolePermissionRepository(DarwinDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Returns all permissions assigned to a role (optionally scoped to a module).
    /// </summary>
    public async Task<List<RolePermission>> GetByRoleIdAsync(Guid roleId, string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.RolePermissions.Where(rp => rp.RoleId == roleId);

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(rp => rp.Module == module);

        return await query.Include(rp => rp.Permission).AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns all permissions assigned to multiple roles (optionally scoped to a module).
    /// </summary>
    public async Task<List<RolePermission>> GetByRoleIdsAsync(List<Guid> roleIds, string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.RolePermissions.Where(rp => roleIds.Contains(rp.RoleId));

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(rp => rp.Module == module);

        return await query.Include(rp => rp.Permission).AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a list of distinct permission names for the given role IDs.
    /// </summary>
    public async Task<List<string>> GetPermissionNamesForRolesAsync(IEnumerable<Guid> roleIds)
    {
        return await _dbContext.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Select(rp => rp.Permission!.Name)
            .Distinct()
            .ToListAsync();
    }

    /// <summary>
    /// Checks whether any of the specified roles has the given permission name.
    /// </summary>
    public async Task<bool> DoesAnyRoleHavePermissionAsync(IEnumerable<Guid> roleIds, string permissionName, string? module = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId) && rp.Permission!.Name == permissionName);

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(rp => rp.Module == module);

        return await query.AnyAsync(cancellationToken);
    }


    /// <summary>
    /// Checks if a specific permission is already assigned to the given role.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid roleId, Guid permissionId, string? module = null, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RolePermissions.AnyAsync(rp =>
            rp.RoleId == roleId &&
            rp.PermissionId == permissionId &&
            rp.Module == module, cancellationToken);
    }

    /// <summary>
    /// Adds a new role-permission link to the database.
    /// </summary>
    public async Task AddAsync(RolePermission rolePermission, CancellationToken cancellationToken = default)
    {
        await _dbContext.RolePermissions.AddAsync(rolePermission, cancellationToken);
    }

    /// <summary>
    /// Deletes a role-permission link from the database.
    /// </summary>
    public void Delete(RolePermission rolePermission)
    {
        _dbContext.RolePermissions.Remove(rolePermission);
    }

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
