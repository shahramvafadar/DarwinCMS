using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace DarwinCMS.WebAdmin.Infrastructure.Modules;

/// <summary>
/// Loads external CMS modules at runtime by scanning referenced assemblies.
/// Responsible for discovering MVC controllers, Razor views, static files, etc.
/// </summary>
public static class ModuleLoader
{
    /// <summary>
    /// Scans and registers all referenced CMS modules with the main MVC system.
    /// Loads controllers, views, and optionally static content per module.
    /// </summary>
    /// <param name="services">The DI service collection</param>
    /// <param name="mvcBuilder">The MVC builder for controller/view discovery</param>
    /// <param name="environmentWebRoot">The physical root path for wwwroot</param>
    public static void RegisterModules(IServiceCollection services, IMvcBuilder mvcBuilder, string environmentWebRoot)
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var moduleAssemblies = loadedAssemblies
            .Where(a => a.FullName != null && a.FullName.StartsWith("DarwinCMS.Module."))
            .ToList();

        foreach (var moduleAssembly in moduleAssemblies)
        {
            // Register the module's controllers and Razor views (if any)
            mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(moduleAssembly));

            // Optional: Serve static files for each module
            var moduleName = moduleAssembly.GetName().Name ?? "UnknownModule";

            var moduleWebRoot = Path.Combine(environmentWebRoot, moduleName.Replace('.', Path.DirectorySeparatorChar));
            if (Directory.Exists(moduleWebRoot))
            {
                services.Configure<Microsoft.AspNetCore.Builder.StaticFileOptions>(opts =>
                {
                    opts.FileProvider = new CompositeFileProvider(
                        opts.FileProvider!,
                        new PhysicalFileProvider(moduleWebRoot)
                    );
                });
            }
        }
    }

    /// <summary>
    /// Loads all available modules that follow the naming convention (DarwinCMS.Module.*)
    /// and return their assembly-qualified names. Useful for diagnostics, logging, or UI.
    /// </summary>
    /// <returns>List of loaded module names</returns>
    public static List<string> LoadAllModules()
    {
        var loadedModules = new List<string>();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName != null && a.FullName.StartsWith("DarwinCMS.Module."));

        foreach (var assembly in assemblies)
        {
            loadedModules.Add(assembly.GetName().Name ?? "UnnamedModule");
        }

        return loadedModules;
    }
}
