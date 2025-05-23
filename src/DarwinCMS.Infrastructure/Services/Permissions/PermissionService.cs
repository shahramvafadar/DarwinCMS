using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Permissions;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Repositories;
using System.Threading;

namespace DarwinCMS.Infrastructure.Services.Permissions;

/// <summary>
/// Implements business logic related to permissions.
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    /// <summary>
    /// Initializes the permission service with repository dependency.
    /// </summary>
    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _permissionRepository.GetAllAsync(module: null, cancellationToken);


    /// <inheritdoc />
    public async Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _permissionRepository.GetByIdAsync(id, cancellationToken);

    /// <inheritdoc />
    public async Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _permissionRepository.GetByNameAsync(name, cancellationToken);

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetByIdAsync(id, cancellationToken);
        if (permission is not null)
        {
            _permissionRepository.Delete(permission);
            await _permissionRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
