using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Users;

/// <summary>
/// Contract for managing user-role relationships.
/// </summary>
public interface IUserRoleService
{
    /// <summary>
    /// Assigns a role to a user.
    /// </summary>
    Task AssignRoleAsync(Guid userId, Guid roleId, string? module = null, bool isSystemAssigned = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a role from a user.
    /// </summary>
    Task UnassignRoleAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all roles assigned to a user (optionally filtered by module).
    /// </summary>
    Task<IEnumerable<UserRole>> GetRolesForUserAsync(Guid userId, string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user has a specific role.
    /// </summary>
    Task<bool> UserHasRoleAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default);
}
