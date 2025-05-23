using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Common;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories;
using DarwinCMS.Infrastructure.Services.Common;
using DarwinCMS.Infrastructure.Services.Roles;
using DarwinCMS.Infrastructure.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinCMS.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure-layer services, including DbContext, repositories, and services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the DarwinDbContext, repositories, and application services.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <param name="configuration">Application configuration for retrieving connection string.</param>
    /// <returns>The updated IServiceCollection instance.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Read connection string from appsettings.json or environment
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is missing in configuration.");

        // Register EF Core DbContext using SQL Server
        services.AddDbContext<DarwinDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
            {
                sql.MigrationsAssembly(typeof(DarwinDbContext).Assembly.FullName);
            });
        });

        // === Repositories ===
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

        // === Application Services ===
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<SmtpEmailSender>();
        services.AddScoped<MailgunEmailSender>();
        services.AddScoped<MicrosoftGraphEmailSender>();
        services.AddScoped<IEmailSender, SmartEmailSender>();


        return services;
    }
}
