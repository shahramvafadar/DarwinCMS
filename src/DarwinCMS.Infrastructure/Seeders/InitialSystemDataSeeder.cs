using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Shared.Constants;
using DarwinCMS.Shared.Security;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Seeders
{
    /// <summary>
    /// Seeds initial system-level data such as roles, users, permissions, settings, pages, and menus.
    /// This implementation is idempotent: running it multiple times does not create duplicates.
    /// </summary>
    public static class InitialSystemDataSeeder
    {
        /// <summary>
        /// Applies initial seed data to the database in an idempotent manner.
        /// </summary>
        public static async Task SeedAsync(DarwinDbContext context)
        {
            // ===== Permissions =====

            var existingPermissionNames = await context.Permissions
                .AsNoTracking()
                .Select(p => p.Name)
                .ToListAsync();

            Permission CreatePermission(string name, string display)
            {
                var p = new Permission(name, SystemConstants.SystemUserId, display);
                p.MarkAsSystem(null);
                return p;
            }

            async Task EnsurePermissionAsync(string name, string display)
            {
                if (existingPermissionNames.Contains(name)) return;
                await context.Permissions.AddAsync(CreatePermission(name, display));
                await context.SaveChangesAsync();
                existingPermissionNames.Add(name);
            }

            await EnsurePermissionAsync("access_admin_panel", "Access Admin Panel");
            await EnsurePermissionAsync("manage_users", "Manage Users");
            await EnsurePermissionAsync("manage_roles", "Manage Roles");
            await EnsurePermissionAsync("manage_permissions", "Manage Permissions");
            await EnsurePermissionAsync("access_member_area", "Access Member Area");
            await EnsurePermissionAsync("full_admin_access", "Full Admin Access");
            await EnsurePermissionAsync("recycle_bin_access", "Recycle Bin Access");

            // ===== Roles =====

            async Task<Role> EnsureRoleAsync(string name, string description)
            {
                var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == name);
                if (role is not null) return role;

                role = new Role(name, SystemConstants.SystemUserId, description);
                role.MarkAsSystem(null);
                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();
                return role;
            }

            var adminRole = await EnsureRoleAsync("Administrators", "System Administrators");
            var memberRole = await EnsureRoleAsync("Members", "Site Members");

            // ===== Admin user =====

            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
            if (adminUser is null)
            {
                adminUser = new User(
                    firstName: "System",
                    lastName: "Admin",
                    gender: "Other",
                    birthDate: new DateTime(1985, 10, 1),
                    username: "admin",
                    email: new("admin@example.com"),
                    passwordHash: PasswordHasher.Hash("Admin123!"),
                    createdByUserId: SystemConstants.SystemUserId);

                adminUser.ConfirmEmail();
                adminUser.ConfirmMobile();
                adminUser.MarkAsSystem();

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();

                await context.UserRoles.AddAsync(new UserRole(adminUser.Id, adminRole.Id));
                await context.SaveChangesAsync();
            }

            // ===== Role-Permission bindings =====

            async Task EnsureRolePermissionAsync(Role role, string permissionName, bool isAllowed)
            {
                var perm = await context.Permissions.FirstAsync(p => p.Name == permissionName);
                var exists = await context.RolePermissions
                    .AnyAsync(rp => rp.RoleId == role.Id && rp.PermissionId == perm.Id);

                if (exists) return;

                await context.RolePermissions.AddAsync(new RolePermission(
                    role.Id,
                    perm.Id,
                    SystemConstants.SystemUserId,
                    null,
                    isAllowed));
                await context.SaveChangesAsync();
            }

            await EnsureRolePermissionAsync(adminRole, "full_admin_access", true);
            await EnsureRolePermissionAsync(memberRole, "access_member_area", true);

            // ===== Site Settings (German defaults where applicable) =====

            var existingSettingKeys = await context.SiteSettings
                .AsNoTracking()
                .Select(s => s.Key)
                .ToListAsync();

            async Task EnsureSettingAsync(
                string key,
                string value,
                string valueType,
                string category,
                string? languageCode,
                string description,
                bool isSystem)
            {
                if (existingSettingKeys.Contains(key)) return;

                var s = new SiteSetting(key, value, valueType, category, languageCode, description, isSystem, SystemConstants.SystemUserId);
                s.MarkAsSystem(null);
                await context.SiteSettings.AddAsync(s);
                await context.SaveChangesAsync();
                existingSettingKeys.Add(key);
            }

            // Branding
            await EnsureSettingAsync("Site.Title", "Darwin CMS", "string", "Branding", null, "Website title", true);
            await EnsureSettingAsync("Site.LogoUrl", "/images/logo.png", "string", "Branding", null, "URL for site logo", true);
            await EnsureSettingAsync("Site.ContactEmail", "support@example.com", "string", "Branding", null, "Contact email address", true);
            await EnsureSettingAsync("Site.SupportPhoneNumber", "+49-123-4567890", "string", "Branding", null, "Support phone number", true);
            await EnsureSettingAsync("Site.HomeSlug", "home", "string", "Localization", null, "Default homepage slug", true);


            // Localization (default to German for public + admin)
            await EnsureSettingAsync("Site.DefaultLanguage", "de", "string", "Localization", null, "Default language for public site", true);
            await EnsureSettingAsync("Site.AdminLanguage", "de", "string", "Localization", null, "Default language for admin panel", true);
            await EnsureSettingAsync("Site.TimeZone", "Europe/Berlin", "string", "Localization", null, "Default timezone (IANA)", true);
            await EnsureSettingAsync("Site.DateFormat", "dd.MM.yyyy", "string", "Localization", null, "Date display format", true);
            await EnsureSettingAsync("Site.TimeFormat", "HH:mm", "string", "Localization", null, "Time display format", true);
            await EnsureSettingAsync("Site.CalendarType", "Gregorian", "string", "Localization", null, "Calendar system", true);
            await EnsureSettingAsync("Site.DecimalSeparator", ",", "string", "Localization", null, "Decimal separator", true);
            await EnsureSettingAsync("Site.ThousandSeparator", ".", "string", "Localization", null, "Thousands separator", true);
            await EnsureSettingAsync("Site.MeasurementSystem", "metric", "string", "Localization", null, "Measurement system", true);
            await EnsureSettingAsync("Site.CurrencySymbol", "€", "string", "Localization", null, "Default currency symbol", true);
            await EnsureSettingAsync("Site.DefaultCountry", "DE", "string", "Localization", null, "Default country code", true);

            // SEO & Analytics
            await EnsureSettingAsync("Site.GoogleAnalyticsId", "", "string", "SEO", null, "Google Analytics Tracking ID", true);
            await EnsureSettingAsync("Site.GoogleTagManagerId", "", "string", "SEO", null, "Google Tag Manager Container ID", true);
            await EnsureSettingAsync("Site.GoogleSearchConsoleVerification", "", "string", "SEO", null, "Search Console verification code", true);
            // Keep existing MetaDescription if present; otherwise add a German default
            await EnsureSettingAsync("Site.MetaDescription", "Standardbeschreibung Ihrer Website auf Deutsch.", "string", "SEO", "de", "Default meta description (DE)", true);

            // Maintenance & Performance
            await EnsureSettingAsync("Site.MaintenanceMode", "false", "boolean", "Maintenance", null, "If true, site is in maintenance mode", true);

            // Navigation / Menu caching (Phase-1 TTL)
            await EnsureSettingAsync("Navigation.MainMenuCode", "Main", "string", "Navigation", null, "Header menu code used by the public site", true);
            await EnsureSettingAsync("Navigation.MenuCacheTtlSeconds", "60", "int", "Navigation", null, "Menu cache TTL (seconds)", true);

            // ===== Pages (German default content) =====

            async Task<Page> EnsurePageAsync(string title, string slug, string languageCode, string html, bool published, bool system)
            {
                var existing = await context.Pages
                    .FirstOrDefaultAsync(p => p.LanguageCode == languageCode && p.SlugValue == slug);

                if (existing is not null) return existing;

                var page = new Page(title, new Slug(slug), languageCode, html, published, SystemConstants.SystemUserId);
                if (system) page.MarkAsSystem(null);

                await context.Pages.AddAsync(page);
                await context.SaveChangesAsync();
                return page;
            }

            var homeHtml = File.Exists("SeedFiles/Home.html")
                ? File.ReadAllText("SeedFiles/Home.html")
                : "<h1>Willkommen</h1><p>Startseite von Darwin CMS.</p>";

            var homePage = await EnsurePageAsync("Startseite", "startseite", "de", homeHtml, true, true);
            var contactPage = await EnsurePageAsync("Kontakt", "kontakt", "de", "Kontaktinformationen folgen.", true, false);
            var aboutPage = await EnsurePageAsync("Über uns", "ueber-uns", "de", "Kurzbeschreibung des Unternehmens.", true, false);
            var privacyPage = await EnsurePageAsync("Datenschutz", "datenschutz", "de", "Datenschutzerklärung gemäß DSGVO.", true, false);
            var impressumPage = await EnsurePageAsync("Impressum", "impressum", "de", "Gesetzliche Anbieterkennzeichnung.", true, false);
            var termsPage = await EnsurePageAsync("AGB", "agb", "de", "Allgemeine Geschäftsbedingungen.", true, false);

            // ===== Menus (German; no reliance on non-existent fields) =====

            async Task<Menu> EnsureMenuAsync(string title, string placement, string languageCode, bool system)
            {
                // We de-duplicate by (Title + LanguageCode) which are guaranteed fields in your codebase.
                var existing = await context.Menus.FirstOrDefaultAsync(x => x.Title == title && x.LanguageCode == languageCode);
                if (existing is not null) return existing;

                // Constructor used previously in your code: (title, placement, languageCode, createdByUserId)
                var menu = new Menu(title, placement, languageCode, SystemConstants.SystemUserId);
                if (system) menu.MarkAsSystem(null);

                await context.Menus.AddAsync(menu);
                await context.SaveChangesAsync();
                return menu;
            }

            var mainMenu = await EnsureMenuAsync("Hauptmenü", "header", "de", true);
            var footerMenu = await EnsureMenuAsync("Fußzeilenmenü", "footer", "de", true);

            async Task EnsureMenuItemInternalAsync(Menu menu, string title, Page page, int order)
            {
                // Safe uniqueness: (MenuId, Title, LinkType.Internal, PageId)
                var exists = await context.MenuItems.AnyAsync(mi =>
                    mi.MenuId == menu.Id &&
                    mi.Title == title &&
                    mi.LinkType == LinkType.Internal &&
                    mi.PageId == page.Id);

                if (exists) return;

                // (menuId, title, linkType, moduleCode, url, order, visibilityCondition, isVisible, createdByUserId)
                var item = new MenuItem(menu.Id, title, LinkType.Internal, null, null, order, "always", true, SystemConstants.SystemUserId);
                item.SetPage(page.Id, null);

                await context.MenuItems.AddAsync(item);
                await context.SaveChangesAsync();
            }

            async Task EnsureMenuItemModuleAsync(Menu menu, string title, string moduleHint, int order)
            {
                // Without relying on a specific property for module code, de-duplicate by (MenuId, Title, LinkType.Module).
                var exists = await context.MenuItems.AnyAsync(mi =>
                    mi.MenuId == menu.Id &&
                    mi.Title == title &&
                    mi.LinkType == LinkType.Module);

                if (exists) return;

                var item = new MenuItem(menu.Id, title, LinkType.Module, moduleHint, null, order, "always", true, SystemConstants.SystemUserId);
                await context.MenuItems.AddAsync(item);
                await context.SaveChangesAsync();
            }

            await EnsureMenuItemInternalAsync(mainMenu, "Startseite", homePage, 0);
            await EnsureMenuItemModuleAsync(mainMenu, "Blog", "blog", 1);
            await EnsureMenuItemInternalAsync(mainMenu, "Kontakt", contactPage, 2);

            await EnsureMenuItemInternalAsync(footerMenu, "Über uns", aboutPage, 0);
            await EnsureMenuItemInternalAsync(footerMenu, "Datenschutz", privacyPage, 1);
            await EnsureMenuItemInternalAsync(footerMenu, "Impressum", impressumPage, 2);
            await EnsureMenuItemInternalAsync(footerMenu, "AGB", termsPage, 3);
        }
    }
}
