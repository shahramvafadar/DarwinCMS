using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DarwinCMS.Shared.Constants;

namespace DarwinCMS.WebAdmin.Infrastructure.Security;

/// <summary>
/// Custom filter that injects an authorization policy dynamically based on the permission name.
/// Also checks for full admin access which bypasses all other checks.
/// </summary>
public class HasPermissionFilter : IAsyncAuthorizationFilter
{
    private readonly IAuthorizationService _authorizationService;
    private readonly string _permission;

    /// <summary>
    /// Initializes the filter with injected services and target permission.
    /// </summary>
    public HasPermissionFilter(
        IAuthorizationService authorizationService,
        string permission)
    {
        _authorizationService = authorizationService;
        _permission = permission;
    }

    /// <summary>
    /// Runs authorization logic against the current user using the configured permission.
    /// If user has full_admin_access, bypasses normal checks.
    /// </summary>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            // This causes redirect to LoginPath (e.g., /Admin/Login)
            context.Result = new ChallengeResult(); // Redirects to login page if not authenticated
            return;
        }


        // If user has full admin access, skip individual permission check
        var superAccess = await _authorizationService.AuthorizeAsync(user, null, new PermissionRequirement(SystemConstants.FullAdminAccessPermission));
        if (superAccess.Succeeded)
        {
            return;
        }

        // Check specific permission
        var result = await _authorizationService.AuthorizeAsync(user, null, new PermissionRequirement(_permission));

        if (!result.Succeeded)
        {
            context.Result = new ForbidResult(); // Returns 403 Forbidden if permission check fails
        }
    }
}
