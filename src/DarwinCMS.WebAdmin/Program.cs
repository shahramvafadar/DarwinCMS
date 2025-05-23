using DarwinCMS.Infrastructure;
using DarwinCMS.WebAdmin.Infrastructure;
using DarwinCMS.WebAdmin.Infrastructure.Middleware;
using DarwinCMS.WebAdmin.Infrastructure.Modules;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;

using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// === Dependency Injection Configuration ===

// Add MVC (Controllers + Views) and Razor Pages support
var mvcBuilder = builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register shared infrastructure services (EF Core, repositories, etc.)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDarwinCmsServices(builder.Configuration);

// Register WebAdmin-specific services (AutoMapper, login, permissions, etc.)
builder.Services.AddWebAdminServices();

// === CORS Support (Open for now, override per environment) ===
// Useful for future OAuth callback scenarios across domains
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        policy
            .AllowAnyOrigin()      // You can override this via appsettings.{Environment}.json
            .AllowAnyHeader()
            .AllowAnyMethod();
        // .AllowCredentials(); // Only enable if using WithOrigins(...)
    });
});

// === Authentication & External OAuth ===

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // These paths are used by login/logout flows in the Admin area
    options.LoginPath = "/Admin/Account/Login";
    options.LogoutPath = "/Admin/Account/Logout";
    options.AccessDeniedPath = "/Admin/Account/AccessDenied";

    // Set cookie expiration (extendable via sliding expiration logic)
    options.ExpireTimeSpan = TimeSpan.FromHours(2);

    // Required for cross-site OAuth flow (Google, Microsoft)
    // Allows secure login even if WebAdmin is running on a subdomain (e.g., admin.example.com)
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Cookie only sent via HTTPS
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "DarwinAdminAuth";
    // Do NOT set Cookie.Domain → Let the environment determine it dynamically
})
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "your-google-client-id";
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "your-google-client-secret";
    options.CallbackPath = "/signin-google"; // OAuth callback endpoint
})
.AddMicrosoftAccount("Microsoft", options =>
{
    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? "your-ms-client-id";
    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? "your-ms-client-secret";
    options.CallbackPath = "/signin-microsoft"; // OAuth callback endpoint
});

// === Pluggable Module Support (Controllers, Views, Static Content) ===
ModuleLoader.RegisterModules(builder.Services, mvcBuilder, builder.Environment.WebRootPath);

// === Localization Configuration ===
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
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.SameSite = SameSiteMode.Lax; // Suitable for session without cross-domain involvement
});

// === Build Application ===
var app = builder.Build();

// === Middleware Pipeline Configuration ===

// Apply localization preferences (e.g., culture switch per user)
app.UseRequestLocalization();

// Enable global CORS policy (configurable per environment)
app.UseCors("DefaultCors");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Dev-friendly exception page
}
else
{
    // Custom global error handler (logs + returns friendly UI)
    app.UseMiddleware<GlobalExceptionMiddleware>();
}

// Serve static files from wwwroot and module paths
app.UseStaticFiles();

// Enable routing, sessions, and authentication/authorization
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// === Routing Configuration ===

// Area-aware routing for Admin UI
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default fallback routing (non-area)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Enable Razor Pages (used for partial UIs or module UIs)
app.MapRazorPages();

// === Start the Application ===
app.Run();
