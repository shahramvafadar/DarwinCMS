using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DarwinCMS.Domain.Interfaces;

/// <summary>
/// Defines the interface that all CMS modules must implement.
/// Used by the ModuleLoader to register dependencies.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Called at app startup to register services and dependencies for the module.
    /// </summary>
    void Register(IServiceCollection services, IConfiguration configuration);
}
