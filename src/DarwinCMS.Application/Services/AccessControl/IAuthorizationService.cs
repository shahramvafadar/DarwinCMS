namespace DarwinCMS.Application.Services.AccessControl;

/// <summary>
/// Provides permission checks for the current user or for a specific user.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Checks whether the current user has the specified permission.
    /// </summary>
    /// <param name="permissionName">The technical name of the permission (e.g. "user.manage")</param>
    /// <returns>True if the user has permission, otherwise false</returns>
    Task<bool> HasPermission(string permissionName);

    /// <summary>
    /// Checks whether a given user (by ID) has the specified permission.
    /// Used in backend services or impersonation.
    /// </summary>
    Task<bool> HasPermissionAsync(Guid userId, string permissionName);
}
