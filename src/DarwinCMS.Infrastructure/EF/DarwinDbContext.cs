using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DarwinCMS.Infrastructure.EF;

/// <summary>
/// Entity Framework Core database context for DarwinCMS.
/// This context defines all entity sets and applies configuration and auditing behavior.
/// </summary>
public class DarwinDbContext : DbContext
{
    /// <summary>
    /// Gets the Users entity set.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets the Roles entity set.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// Gets the Permissions entity set.
    /// </summary>
    public DbSet<Permission> Permissions => Set<Permission>();

    /// <summary>
    /// Gets the user-role assignments.
    /// </summary>
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    /// <summary>
    /// Gets the role-permission assignments.
    /// </summary>
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    /// <summary>
    /// Gets the password reset tokens.
    /// </summary>
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    /// <summary>
    /// Creates a new instance of the <see cref="DarwinDbContext"/> class using the specified options.
    /// </summary>
    /// <param name="options">The EF Core options passed by dependency injection.</param>
    public DarwinDbContext(DbContextOptions<DarwinDbContext> options) : base(options) { }

    /// <summary>
    /// Applies all entity type configurations using Fluent API from this project and all loaded module assemblies.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance used to define mappings.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply Fluent API configurations defined in this assembly (Infrastructure)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DarwinDbContext).Assembly);

        // Load module assemblies dynamically based on name pattern
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName()?.Name?.StartsWith("DarwinCMS.Module.") == true)
            .ToList();

        // Search for module DLLs in bin folder that aren't loaded yet
        var binFolder = AppDomain.CurrentDomain.BaseDirectory;
        var moduleDlls = Directory.GetFiles(binFolder, "DarwinCMS.Module.*.dll", SearchOption.AllDirectories);

        foreach (var dllPath in moduleDlls)
        {
            var assemblyName = Path.GetFileNameWithoutExtension(dllPath);
            if (!loadedAssemblies.Any(a => a.GetName()?.Name == assemblyName))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dllPath);
                    loadedAssemblies.Add(assembly);
                }
                catch
                {
                    // Ignore invalid or corrupted module assemblies
                }
            }
        }

        // Apply configurations from discovered module assemblies
        foreach (var assembly in loadedAssemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }

        // You may add global query filters, naming conventions, or auditing conventions here
    }

    /// <summary>
    /// Intercepts save operations to apply automatic auditing behavior (e.g., CreatedAt and ModifiedAt).
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token for the async save operation.</param>
    /// <returns>The number of affected rows after saving changes.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // CreatedAt is set by default constructor in BaseEntity; no action needed unless overridden
                    break;

                case EntityState.Modified:
                    // Mark entity as modified using domain method (internal setter)
                    entry.Entity.MarkAsModified();
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
