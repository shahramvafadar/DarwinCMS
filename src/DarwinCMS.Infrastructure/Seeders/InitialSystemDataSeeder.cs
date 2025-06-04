using System.Globalization;

using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Shared.Constants;
using DarwinCMS.Shared.Security;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Seeders;

/// <summary>
/// Seeds initial system-level data such as roles, users, permissions, settings, pages, and menus.
/// This should run once after the first migration.
/// </summary>
public static class InitialSystemDataSeeder
{
    /// <summary>
    /// Applies initial seed data to the database.
    /// </summary>
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
        AddPermission("recycle_bin_access", "Recycle Bin Access");

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
            createdByUserId: SystemConstants.SystemUserId);

        adminUser.ConfirmEmail();
        adminUser.ConfirmMobile();
        adminUser.MarkAsSystem();

        await context.Users.AddAsync(adminUser);
        await context.UserRoles.AddAsync(new UserRole(adminUser.Id, adminRole.Id));
        await context.RolePermissions.AddAsync(new RolePermission(
            adminRole.Id,
            permissions.First(p => p.Name == "full_admin_access").Id,
            SystemConstants.SystemUserId,
            null,
            true));

        // === Site Settings (Default values for German/EU standards) ===
        var settings = new List<SiteSetting>
        {
            // Branding
            new("Site.Title", "Darwin CMS", "string", "Branding", null, "Website title", true, SystemConstants.SystemUserId),
            new("Site.LogoUrl", "/images/logo.png", "string", "Branding", null, "URL for site logo", true, SystemConstants.SystemUserId),
            new("Site.ContactEmail", "support@example.com", "string", "Branding", null, "Contact email address", true, SystemConstants.SystemUserId),
            new("Site.SupportPhoneNumber", "+49-123-4567890", "string", "Branding", null, "Support phone number", true, SystemConstants.SystemUserId),

            // Localization
            new("Site.DefaultLanguage", "en", "string", "Localization", null, "Default language for public site", true, SystemConstants.SystemUserId),
            new("Site.AdminLanguage", "en", "string", "Localization", null, "Default language for admin panel", true, SystemConstants.SystemUserId),
            new("Site.TimeZone", "Europe/Berlin", "string", "Localization", null, "Default timezone (IANA)", true, SystemConstants.SystemUserId),
            new("Site.DateFormat", "dd.MM.yyyy", "string", "Localization", null, "Date display format", true, SystemConstants.SystemUserId),
            new("Site.TimeFormat", "HH:mm", "string", "Localization", null, "Time display format", true, SystemConstants.SystemUserId),
            new("Site.CalendarType", "Gregorian", "string", "Localization", null, "Calendar system (Gregorian, Persian, Hijri, etc.)", true, SystemConstants.SystemUserId),
            new("Site.DecimalSeparator", ",", "string", "Localization", null, "Decimal separator used in numbers", true, SystemConstants.SystemUserId),
            new("Site.ThousandSeparator", ".", "string", "Localization", null, "Thousands separator used in numbers", true, SystemConstants.SystemUserId),
            new("Site.MeasurementSystem", "metric", "string", "Localization", null, "Measurement system (metric/imperial)", true, SystemConstants.SystemUserId),
            new("Site.CurrencySymbol", "€", "string", "Localization", null, "Default currency symbol", true, SystemConstants.SystemUserId),
            new("Site.DefaultCountry", "DE", "string", "Localization", null, "Default country code for Geo-specific data", true, SystemConstants.SystemUserId),

            // SEO & Analytics
            new("Site.GoogleAnalyticsId", "", "string", "SEO", null, "Google Analytics Tracking ID", true, SystemConstants.SystemUserId),
            new("Site.GoogleTagManagerId", "", "string", "SEO", null, "Google Tag Manager Container ID", true, SystemConstants.SystemUserId),
            new("Site.GoogleSearchConsoleVerification", "", "string", "SEO", null, "Google Search Console verification code", true, SystemConstants.SystemUserId),
            new("Site.MetaDescription", "Your website's default meta description", "string", "SEO", null, "Default meta description for SEO", true, SystemConstants.SystemUserId),

            // Maintenance & Performance
            new("Site.MaintenanceMode", "false", "boolean", "Maintenance", null, "If true, site is in maintenance mode", true, SystemConstants.SystemUserId)
        };


        await context.SiteSettings.AddRangeAsync(settings);


        foreach (var setting in settings)
        {
            setting.MarkAsSystem();
        }

        await context.SiteSettings.AddRangeAsync(settings);

        // === Pages ===
        var homeHtml = File.ReadAllText("SeedFiles/Home.html");

        var homePage = new Page("Welcome to Darwin CMS", new Slug("home"), "en", homeHtml, true, SystemConstants.SystemUserId);
        homePage.MarkAsSystem();

        var contactPage = new Page("Contact Us", new Slug("contact"), "en", "Contact form or details will be added here.", true, SystemConstants.SystemUserId);
        var aboutPage = new Page("About Us", new Slug("about"), "en", "Basic description about the company.", true, SystemConstants.SystemUserId);
        var privacyPage = new Page("Privacy Policy", new Slug("privacy-policy"), "en", "Legal privacy info for German websites.", true, SystemConstants.SystemUserId);
        var impressumPage = new Page("Impressum", new Slug("impressum"), "en", "Legal imprint required in Germany.", true, SystemConstants.SystemUserId);
        var termsPage = new Page("Terms of Service", new Slug("terms"), "en", "Site usage and policy information.", true, SystemConstants.SystemUserId);

        var pages = new List<Page> { homePage, contactPage, aboutPage, privacyPage, impressumPage, termsPage };
        await context.Pages.AddRangeAsync(pages);
        await context.SaveChangesAsync();

        // === Menus ===
        var mainMenu = new Menu("Main Menu", "header", "en", SystemConstants.SystemUserId);
        mainMenu.MarkAsSystem();

        var footerMenu = new Menu("Footer Menu", "footer", "en", SystemConstants.SystemUserId);
        footerMenu.MarkAsSystem();

        await context.Menus.AddRangeAsync(new[] { mainMenu, footerMenu });
        await context.SaveChangesAsync();

        var homeItem = new MenuItem(mainMenu.Id, "Home", LinkType.Internal, null, null, 0, "always", true, SystemConstants.SystemUserId);
        homeItem.SetPage(homePage.Id);

        var blogItem = new MenuItem(mainMenu.Id, "Blog", LinkType.Module, "blog", null, 1, "always", true, SystemConstants.SystemUserId);

        var contactItem = new MenuItem(mainMenu.Id, "Contact", LinkType.Internal, null, null, 2, "always", true, SystemConstants.SystemUserId);
        contactItem.SetPage(contactPage.Id);

        var aboutItem = new MenuItem(footerMenu.Id, "About", LinkType.Internal, null, null, 0, "always", true, SystemConstants.SystemUserId);
        aboutItem.SetPage(aboutPage.Id);

        var privacyItem = new MenuItem(footerMenu.Id, "Privacy Policy", LinkType.Internal, null, null, 1, "always", true, SystemConstants.SystemUserId);
        privacyItem.SetPage(privacyPage.Id);

        var impressumItem = new MenuItem(footerMenu.Id, "Impressum", LinkType.Internal, null, null, 2, "always", true, SystemConstants.SystemUserId);
        impressumItem.SetPage(impressumPage.Id);

        var termsItem = new MenuItem(footerMenu.Id, "Terms of Service", LinkType.Internal, null, null, 3, "always", true, SystemConstants.SystemUserId);
        termsItem.SetPage(termsPage.Id);

        var menuItems = new List<MenuItem>
        {
            homeItem,
            blogItem,
            contactItem,
            aboutItem,
            privacyItem,
            impressumItem,
            termsItem
        };

        await context.MenuItems.AddRangeAsync(menuItems);



        // Save all
        await context.SaveChangesAsync();
    }
}
