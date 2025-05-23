using DarwinCMS.Application.Services.AccessControl;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;

namespace DarwinCMS.WebAdmin.Services.Auth;

/// <summary>
/// Provides access to the currently authenticated user's context,
/// including ID, authentication status, and claims-based permissions.
/// This implementation depends on IHttpContextAccessor and is only valid in web requests.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">Accessor to get current HTTP context and user claims.</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the current authenticated user's ID, or null if not available.
    /// </summary>
    public Guid? UserId =>
        Guid.TryParse(
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out var id
        ) ? id : null;

    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

    /// <summary>
    /// Checks whether the current user has a given permission claim.
    /// </summary>
    /// <param name="permissionName">The name of the permission claim to check (e.g., "user.edit").</param>
    /// <returns>True if the user has the specified permission; otherwise false.</returns>
    public bool HasPermission(string permissionName)
    {
        return _httpContextAccessor.HttpContext?.User?.HasClaim("permission", permissionName) == true;
    }
}
