using DarwinCMS.Infrastructure;
using DarwinCMS.Web.Infrastructure.Modules;

using Microsoft.AspNetCore.Localization;

using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// === MVC & Razor Pages Setup ===
// Enables traditional controllers with views and Razor Pages support
var mvcBuilder = builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// === Core CMS Services ===
// Registers core business services (e.g., content, menu, SEO, etc.)
builder.Services.AddDarwinCmsServices(builder.Configuration);

// === Module Loader ===
// Automatically loads controllers, views, and static files from modular packages
ModuleLoader.RegisterModules(builder.Services, mvcBuilder, builder.Environment.WebRootPath);

// === Localization Support ===
// Enables multilingual UI (e.g., English, German, Persian)
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("de"),
        new CultureInfo("fa")
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// === Session Configuration ===
// Used for language selection, anonymous preferences, etc.
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.SameSite = SameSiteMode.Lax; // Safe for internal usage (not cross-origin)
});

// Allows services and views to access the current request
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// === Middleware Pipeline ===

app.UseRequestLocalization();      // Activates multilingual support
app.UseStaticFiles();              // Serves files from wwwroot and modules
app.UseRouting();
app.UseSession();                  // Enables session-based features

// === Routing Configuration ===

// Area-based routing (for future modules with Areas)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default routing for the public site
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Enables modular UI pages (optional)

app.Run();
