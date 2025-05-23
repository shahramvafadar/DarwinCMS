using DarwinCMS.Application.DTOs.Users;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Users;

/// <summary>
/// Exposes user-related business operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    Task<User> CreateAsync(CreateUserRequest request, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft-deactivates a user.
    /// </summary>
    Task DisableUserAsync(Guid userId, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reactivates a disabled user.
    /// </summary>
    Task EnableUserAsync(Guid userId, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a user's password.
    /// </summary>
    Task ChangePasswordAsync(Guid userId, string newPassword, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paged, filtered list of users for admin UI.
    /// </summary>
    Task<UserListResultDto> GetUserListAsync(
        string? search,
        Guid? roleId,
        string? sortColumn,
        string? sortDirection,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the profile and role of a user.
    /// </summary>
    Task UpdateAsync(UpdateUserRequest request, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the user and any associated mappings.
    /// </summary>
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all role IDs assigned to a user.
    /// </summary>
    Task<List<Guid>> GetUserPrimaryRoleIdsAsync(Guid userId, CancellationToken cancellationToken = default);
}
