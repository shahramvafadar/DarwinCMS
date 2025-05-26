using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using DarwinCMS.Shared.Security;
using DarwinCMS.Shared.Constants;

namespace DarwinCMS.Infrastructure.Seeders;

/// <summary>
/// Seeds initial system-level data such as system roles, users, and permissions.
/// This should run once after the first migration.
/// </summary>
public static class InitialSystemDataSeeder
{
    /// <summary>
    /// Applies initial system-critical seed data to the database.
    /// </summary>
    /// <param name="context">The EF Core database context.</param>
    public static async Task SeedAsync(DarwinDbContext context)
    {
        if (context.Permissions.Any() || context.Users.Any() || context.Roles.Any())
            return; // already seeded

        // === Permissions ===
        var permissions = new List<Permission>();

        void AddPermission(string name, string display)
        {
            var p = new Permission(name, SystemConstants.SystemUserId, display);
            p.MarkAsSystem();
            permissions.Add(p);
        }

        AddPermission("access_admin_panel", "Access Admin Panel");
        AddPermission("manage_users", "Manage Users");
        AddPermission("manage_roles", "Manage Roles");
        AddPermission("manage_permissions", "Manage Permissions");
        AddPermission("access_member_area", "Access Member Area");
        AddPermission("full_admin_access", "Full Admin Access");

        await context.Permissions.AddRangeAsync(permissions);

        // === Roles ===
        var adminRole = new Role("Administrators", SystemConstants.SystemUserId, "System Administrators");
        adminRole.MarkAsSystem();

        var memberRole = new Role("Members", SystemConstants.SystemUserId, "Site Members");
        memberRole.MarkAsSystem();

        await context.Roles.AddRangeAsync(adminRole, memberRole);

        // === Admin user ===
        var adminUser = new User(
            firstName: "System",
            lastName: "Admin",
            gender: "Other",
            birthDate: new DateTime(1985, 10, 1),
            username: "admin",
            email: new("admin@example.com"),
            passwordHash: PasswordHasher.Hash("Admin123!"),
            createdByUserId: SystemConstants.SystemUserId
        );
        adminUser.ConfirmEmail();
        adminUser.ConfirmMobile();
        adminUser.MarkAsSystem();

        await context.Users.AddAsync(adminUser);

        // === UserRole connection ===
        var userRole = new UserRole(adminUser.Id, adminRole.Id);
        await context.UserRoles.AddAsync(userRole);

        // === RolePermission connection ===
        var fullAccess = permissions.First(p => p.Name == "full_admin_access");
        var rolePermission = new RolePermission(
            roleId: adminRole.Id,
            permissionId: fullAccess.Id,
            createdByUserId: SystemConstants.SystemUserId,
            module: null,
            isSystemPermission: true
        );
        await context.RolePermissions.AddAsync(rolePermission);

        await context.SaveChangesAsync();
    }
}
