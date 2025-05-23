using DarwinCMS.Application.Services.Auth;
using DarwinCMS.Application.Services.Common;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.Infrastructure.Services.Auth;
using DarwinCMS.Infrastructure.Services.Common;
using DarwinCMS.Infrastructure.Services.Roles;
using DarwinCMS.Infrastructure.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinCMS.Infrastructure;

/// <summary>
/// Centralized service registration entry point for all infrastructure services.
/// This method should be called in Program.cs to wire up dependencies.
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// Registers all application-level and infrastructure services.
    /// This method is extendable for future modules (e.g., Content, CRM, Store).
    /// </summary>
    public static IServiceCollection AddDarwinCmsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // === Core domain services ===
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IEmailSender, DebugEmailSender>();


        // === Future modular services can be registered here ===
        // Example:
        // services.AddScoped<IContentItemService, ContentItemService>();
        // services.AddScoped<IPermissionService, PermissionService>();

        return services;
    }
}
