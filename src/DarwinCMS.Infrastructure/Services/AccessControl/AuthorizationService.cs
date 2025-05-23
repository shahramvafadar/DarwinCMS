using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.AccessControl;

namespace DarwinCMS.Infrastructure.Services.AccessControl;

/// <summary>
/// Implementation of IAuthorizationService that checks user permissions
/// either via injected current user context or from the database.
/// </summary>
public class AuthorizationService : IAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    /// <summary>
    /// Initializes the authorization service with required dependencies.
    /// </summary>
    /// <param name="currentUserService">The current user context provider.</param>
    /// <param name="rolePermissionRepository">Repository for role-permission mappings.</param>
    /// <param name="userRoleRepository">Repository for user-role assignments.</param>
    public AuthorizationService(
        ICurrentUserService currentUserService,
        IRolePermissionRepository rolePermissionRepository,
        IUserRoleRepository userRoleRepository)
    {
        _currentUserService = currentUserService;
        _rolePermissionRepository = rolePermissionRepository;
        _userRoleRepository = userRoleRepository;
    }

    /// <summary>
    /// Checks whether the current user has the specified permission.
    /// </summary>
    /// <param name="permissionName">The technical name of the permission (e.g. "user.manage").</param>
    /// <returns>True if the current user has the permission; otherwise false.</returns>
    public Task<bool> HasPermission(string permissionName)
    {
        if (!_currentUserService.IsAuthenticated)
            return Task.FromResult(false);

        return Task.FromResult(_currentUserService.HasPermission(permissionName));
    }

    /// <summary>
    /// Checks whether a given user (by ID) has the specified permission.
    /// This is used in backend logic or impersonation scenarios.
    /// </summary>
    /// <param name="userId">The ID of the user to check.</param>
    /// <param name="permissionName">The name of the permission to validate.</param>
    /// <returns>True if the user has the specified permission; otherwise false.</returns>
    public async Task<bool> HasPermissionAsync(Guid userId, string permissionName)
    {
        var roleIds = await _userRoleRepository.GetRoleIdsByUserIdAsync(userId);
        if (roleIds == null || !roleIds.Any())
            return false;

        return await _rolePermissionRepository.DoesAnyRoleHavePermissionAsync(roleIds, permissionName);
    }
}
