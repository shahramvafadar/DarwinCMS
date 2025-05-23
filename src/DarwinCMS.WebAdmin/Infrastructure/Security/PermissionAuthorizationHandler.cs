using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace DarwinCMS.WebAdmin.Infrastructure.Security;

/// <summary>
/// Authorization handler that checks if the current user has a specific permission claim.
/// This is typically used with the [Authorize(Policy = "...")] attribute in controllers or actions.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// Handles the permission requirement by checking if the user has a matching claim.
    /// </summary>
    /// <param name="context">The authorization context containing user claims and resource information.</param>
    /// <param name="requirement">The required permission that needs to be validated.</param>
    /// <returns>A completed task once evaluation is done.</returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // Check if the user has a claim of type "permission" matching the required permission value
        if (context.User.HasClaim("permission", requirement.Permission))
        {
            // Mark the requirement as succeeded
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
