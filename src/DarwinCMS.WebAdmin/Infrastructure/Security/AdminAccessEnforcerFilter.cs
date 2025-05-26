using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using DarwinCMS.WebAdmin.Infrastructure.Security;

namespace DarwinCMS.WebAdmin.Infrastructure.Security;

/// <summary>
/// Automatically enforces the 'access_admin_panel' permission on all Admin area controllers,
/// unless an explicit [HasPermission] or [AllowAnonymousPermissions] attribute is present.
/// </summary>
public class AdminAccessEnforcerFilter : IAsyncAuthorizationFilter
{
    /// <summary>
    /// Executes permission enforcement logic for Admin area actions.
    /// </summary>
    /// <param name="context">The authorization filter context.</param>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var area = context.RouteData.Values["area"]?.ToString();
        if (!string.Equals(area, "Admin", StringComparison.OrdinalIgnoreCase))
            return;

        var allowAnonymous = context.ActionDescriptor.EndpointMetadata
            .OfType<AllowAnonymousPermissionsAttribute>()
            .Any();

        if (allowAnonymous)
            return; // skip permission check for login/logout/reset-password endpoints

        var hasCustomPermission = context.ActionDescriptor.EndpointMetadata
            .OfType<HasPermissionAttribute>()
            .Any();

        if (hasCustomPermission)
            return; // custom [HasPermission(...)] takes over

        var filter = new HasPermissionFilter(
            context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>(),
            "access_admin_panel");

        await filter.OnAuthorizationAsync(context);
    }
}
