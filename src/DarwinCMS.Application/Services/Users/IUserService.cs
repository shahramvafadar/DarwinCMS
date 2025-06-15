using DarwinCMS.Application.DTOs.Users;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Users;

/// <summary>
/// Exposes user-related business operations, including CRUD, soft deletion, restoration, and listing deleted items.
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
    /// Soft-deletes (disables) a user by marking as logically deleted.
    /// </summary>
    Task SoftDeleteAsync(Guid userId, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted user.
    /// </summary>
    Task RestoreAsync(Guid userId, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a user from the system (hard delete).
    /// </summary>
    Task HardDeleteAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads all users marked as soft-deleted (recycle bin view).
    /// </summary>
    Task<List<User>> GetDeletedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft-deactivates (disables) a user.
    /// </summary>
    Task DisableUserAsync(Guid userId, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reactivates a previously disabled user.
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
    /// Retrieves all role IDs assigned to a user.
    /// </summary>
    Task<List<Guid>> GetUserPrimaryRoleIdsAsync(Guid userId, CancellationToken cancellationToken = default);
}
