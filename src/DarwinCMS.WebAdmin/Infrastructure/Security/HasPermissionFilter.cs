using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace DarwinCMS.WebAdmin.Infrastructure.Security
{
    /// <summary>
    /// Custom filter that injects an authorization policy dynamically based on the permission name.
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
        /// </summary>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new ForbidResult();
                return;
            }

            var result = await _authorizationService.AuthorizeAsync(user, null, new PermissionRequirement(_permission));

            if (!result.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
