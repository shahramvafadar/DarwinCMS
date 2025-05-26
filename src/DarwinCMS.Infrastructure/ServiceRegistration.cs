using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Auth;
using DarwinCMS.Application.Services.Common;
using DarwinCMS.Application.Services.Permissions;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.Infrastructure.Repositories;
using DarwinCMS.Infrastructure.Services.Auth;
using DarwinCMS.Infrastructure.Services.Common;
using DarwinCMS.Infrastructure.Services.Permissions;
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
        services.AddScoped<IPermissionService, PermissionService>();

        // === Core repositories ===
        services.AddScoped<IPageRepository, PageRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
        services.AddScoped<IMediaFileRepository, MediaFileRepository>();

        // === Future modular services can be registered here ===
        // Example:
        // services.AddScoped<IContentItemService, ContentItemService>();

        return services;
    }
}
