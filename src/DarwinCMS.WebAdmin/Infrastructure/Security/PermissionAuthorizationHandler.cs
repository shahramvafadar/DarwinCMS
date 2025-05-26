using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace DarwinCMS.WebAdmin.Infrastructure.Security;

/// <summary>
/// Authorization handler that checks if the current user has a specific permission claim.
/// Used to enforce permission-based policies across the admin area.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// Handles the permission requirement by checking if the user has a matching permission claim.
    /// </summary>
    /// <param name="context">Authorization context containing the user.</param>
    /// <param name="requirement">The required permission key.</param>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // Allow if the user has a claim of type 'permission' with the required value
        var hasPermission = context.User.Claims
            .Any(c => c.Type == "permission" && c.Value == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
