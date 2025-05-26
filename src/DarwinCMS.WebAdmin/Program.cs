using System.Globalization;

using DarwinCMS.Infrastructure;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Seeders;
using DarwinCMS.WebAdmin.Infrastructure;
using DarwinCMS.WebAdmin.Infrastructure.Middleware;
using DarwinCMS.WebAdmin.Infrastructure.Modules;
using DarwinCMS.WebAdmin.Infrastructure.Security;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// === Dependency Injection Configuration ===

// Add MVC (Controllers + Views) and Razor Pages support
var mvcBuilder = builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AdminAccessEnforcerFilter>();
});
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
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
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
    options.LoginPath = "/Admin/Login";
    options.LogoutPath = "/Admin/Logout";
    options.AccessDeniedPath = "/Admin/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "DarwinAdminAuth";
})
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "your-google-client-id";
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "your-google-client-secret";
    options.CallbackPath = "/signin-google";
})
.AddMicrosoftAccount("Microsoft", options =>
{
    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? "your-ms-client-id";
    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? "your-ms-client-secret";
    options.CallbackPath = "/signin-microsoft";
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
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// === Build Application ===
var app = builder.Build();

// === Apply initial seed data for permissions, roles, and admin user ===
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DarwinDbContext>();
    await InitialSystemDataSeeder.SeedAsync(dbContext);
}


// === Middleware Pipeline Configuration ===

app.UseRequestLocalization();
app.UseCors("DefaultCors");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<GlobalExceptionMiddleware>();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// === Routing Configuration ===
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
