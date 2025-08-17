using System.Globalization;

using DarwinCMS.Web.Infrastructure;
using DarwinCMS.Web.Infrastructure.Modules;

using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// === MVC & Razor Pages ===
var mvcBuilder = builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// === Web DI (EF + repos + domain services + public query services) ===
// Uses ConnectionStrings:DefaultConnection and does NOT run seed here
builder.Services.AddWebServices(builder.Configuration);

// === Modules (pluggable) ===
ModuleLoader.RegisterModules(builder.Services, mvcBuilder, builder.Environment.WebRootPath);

// === Localization ===
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("de"),
        new CultureInfo("fa")
    };

    options.DefaultRequestCulture = new RequestCulture("de");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// === Session ===
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// HttpContext access for controllers/views/components
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// === Pipeline ===
app.UseRequestLocalization();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// Custom status code pages → /error/{code} (ErrorController.Status)
app.UseStatusCodePagesWithReExecute("/error/{0}");

// === Routes ===
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
