using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Contract for querying and managing users in the system.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Gets a user by username (case-insensitive).
    /// </summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by email (case-insensitive).
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by either username or email (case-insensitive).
    /// </summary>
    Task<User?> GetByUsernameOrEmailAsync(string username, string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all active users.
    /// </summary>
    Task<List<User>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a user and immediately saves changes.
    /// </summary>
    Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns only users that are marked as system-critical (e.g., admin).
    /// </summary>
    Task<List<User>> GetSystemUsersAsync(CancellationToken cancellationToken = default);
}
