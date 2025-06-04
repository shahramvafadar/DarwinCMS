using DarwinCMS.Application.Services.Auth;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Permissions;
using DarwinCMS.Application.Services.Helpers;
using DarwinCMS.Infrastructure.Services.Helpers;
using DarwinCMS.Infrastructure.Services.Permissions;

using DarwinCMS.WebAdmin.Infrastructure.Security;
using DarwinCMS.WebAdmin.Mapping;
using DarwinCMS.WebAdmin.Services.Auth;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinCMS.WebAdmin.Infrastructure;

/// <summary>
/// Registers WebAdmin-specific services such as login handlers, permission policies,
/// current user context services, formatting helpers, and AutoMapper profiles.
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// Registers all WebAdmin services with the dependency injection container.
    /// This method should be called from Program.cs during application startup.
    /// </summary>
    /// <param name="services">The IServiceCollection to register dependencies into.</param>
    /// <returns>The same IServiceCollection for chaining.</returns>
    public static IServiceCollection AddWebAdminServices(this IServiceCollection services)
    {
        // Register login service used for cookie-based authentication and user sign-in
        services.AddScoped<ILoginService, LoginService>();

        // Register IHttpContextAccessor to allow access to HttpContext in services
        services.AddHttpContextAccessor();

        // Register current user context service that exposes authenticated user's ID and claims
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Register custom permission-based authorization handler used for [HasPermission] attributes
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // Register helper service to format dates, times, and numbers based on site-wide settings
        services.AddScoped<IFormatHelperService, FormatHelperService>();

        // Register AutoMapper with profiles defined in the WebAdmin layer
        services.AddAutoMapper(typeof(AdminMapperProfile).Assembly);

        return services;
    }
}
