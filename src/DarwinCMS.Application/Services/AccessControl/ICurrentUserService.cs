namespace DarwinCMS.Application.Services.AccessControl;

/// <summary>
/// Abstraction to provide information about the current authenticated user.
/// This is implemented in the Web layer and used throughout Application/Infrastructure.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user's ID, if authenticated.
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Indicates whether the current request is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Checks if the current user has a specific permission claim.
    /// </summary>
    bool HasPermission(string permissionName);
}
