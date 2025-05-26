using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Defines repository contract for managing Role entities,
/// including extra querying methods.
/// </summary>
public interface IRoleRepository : IRepository<Role>
{
    /// <summary>
    /// Returns a single role by its unique technical name.
    /// </summary>
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all system roles (undeletable, reserved by the system).
    /// </summary>
    Task<List<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default);
}
