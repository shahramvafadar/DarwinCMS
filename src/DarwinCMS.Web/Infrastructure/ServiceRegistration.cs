using DarwinCMS.Infrastructure; // AddInfrastructure, AddDarwinCmsServices

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Public-site query services (read-only)
using DarwinCMS.Application.Services.Pages;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.Application.Services.Settings;
using DarwinCMS.Application.Services.Files;
using DarwinCMS.Application.Services.Seo;

using DarwinCMS.Infrastructure.Services.Pages;
using DarwinCMS.Infrastructure.Services.Menus;
using DarwinCMS.Infrastructure.Services.Settings;
using DarwinCMS.Infrastructure.Services.Files;
using DarwinCMS.Infrastructure.Services.Seo;

using DarwinCMS.Web.Infrastructure.Settings; // <-- for ISiteSettingsAccessor / SiteSettingsAccessor

namespace DarwinCMS.Web.Infrastructure
{
    /// <summary>
    /// Registers all public-site (Web) dependencies in one place to keep Program.cs clean.
    /// Mirrors WebAdmin's pattern and wires read-only query services used by the public UI.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Adds EF Core, repositories, domain services (via Infrastructure extensions),
        /// AutoMapper, SiteSettings accessor, and read-only query services for the public website.
        /// </summary>
        /// <param name="services">ASP.NET Core DI container.</param>
        /// <param name="configuration">App configuration (for connection strings, etc.).</param>
        /// <returns>The same IServiceCollection for chaining.</returns>
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            // === Core Infrastructure (DbContext, repositories, domain services) ===
            // Uses ConnectionStrings:DefaultConnection; consistent with WebAdmin
            services.AddInfrastructure(configuration);
            services.AddDarwinCmsServices(configuration);

            // === AutoMapper ===
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // === Cross-cutting for Web ===
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            // === Site settings accessor (used by _Layout, VC, etc.) ===
            services.AddScoped<ISiteSettingsAccessor, SiteSettingsAccessor>();

            // === Read-only query services (public site) ===
            services.AddScoped<IPageQueryService, PageQueryService>();
            services.AddScoped<IMenuQueryService, MenuQueryService>();
            services.AddScoped<ISiteSettingQueryService, SiteSettingQueryService>();
            services.AddScoped<IFileQueryService, FileQueryService>();
            services.AddScoped<IHreflangQueryService, HreflangQueryService>();

            return services;
        }
    }
}
