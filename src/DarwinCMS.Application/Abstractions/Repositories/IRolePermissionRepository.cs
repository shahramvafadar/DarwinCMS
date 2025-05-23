using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Defines the contract for managing role-permission assignments in persistence layer.
/// </summary>
public interface IRolePermissionRepository
{
    /// <summary>
    /// Returns all permissions assigned to the specified role.
    /// </summary>
    Task<List<RolePermission>> GetByRoleIdAsync(Guid roleId, string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all permissions for multiple roles at once.
    /// </summary>
    Task<List<RolePermission>> GetByRoleIdsAsync(List<Guid> roleIds, string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a list of unique permission names assigned to given role IDs.
    /// </summary>
    Task<List<string>> GetPermissionNamesForRolesAsync(IEnumerable<Guid> roleIds);


    /// <summary>
    /// Checks whether any of the given role IDs has the specified permission name.
    /// </summary>
    Task<bool> DoesAnyRoleHavePermissionAsync(IEnumerable<Guid> roleIds, string permissionName, string? module = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// Checks if a permission is already assigned to a role.
    /// </summary>
    Task<bool> ExistsAsync(Guid roleId, Guid permissionId, string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns a new permission to the role.
    /// </summary>
    Task AddAsync(RolePermission rolePermission, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a specific role-permission assignment.
    /// </summary>
    void Delete(RolePermission rolePermission);

    /// <summary>
    /// Persists all pending changes.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
