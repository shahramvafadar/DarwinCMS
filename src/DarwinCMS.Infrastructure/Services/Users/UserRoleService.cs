using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Infrastructure.Services.Users;

/// <summary>
/// Implements logic for managing user-role assignments.
/// </summary>
public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository;

    /// <summary>
    /// Initializes the service with a user-role repository.
    /// </summary>
    public UserRoleService(IUserRoleRepository userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;
    }

    /// <inheritdoc />
    public async Task AssignRoleAsync(Guid userId, Guid roleId, string? module = null, bool isSystemAssigned = false, CancellationToken cancellationToken = default)
    {
        var existing = await _userRoleRepository.GetAsync(userId, roleId, module, cancellationToken);
        if (existing is not null)
            return;

        var userRole = new UserRole(userId, roleId, module, isSystemAssigned);
        await _userRoleRepository.AddAsync(userRole, cancellationToken);
        await _userRoleRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UnassignRoleAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default)
    {
        var existing = await _userRoleRepository.GetAsync(userId, roleId, module, cancellationToken);
        if (existing is null)
            return;

        _userRoleRepository.Delete(existing);
        await _userRoleRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserRole>> GetRolesForUserAsync(Guid userId, string? module = null, CancellationToken cancellationToken = default)
        => await _userRoleRepository.GetByUserIdAsync(userId, module, cancellationToken);

    /// <inheritdoc />
    public async Task<bool> UserHasRoleAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default)
        => await _userRoleRepository.ExistsAsync(userId, roleId, module, cancellationToken);
}
