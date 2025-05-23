using DarwinCMS.Domain.Entities;
using System.Linq.Expressions;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Contract for querying and managing users in the system.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Returns an IQueryable for advanced LINQ operations (filtering, sorting, paging).
    /// This should be used with AsNoTracking when possible in queries.
    /// </summary>
    IQueryable<User> Query();

    /// <summary>
    /// Gets a user by their unique ID.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by username (case-insensitive).
    /// </summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by email (case-insensitive).
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by either username or email.
    /// </summary>
    Task<User?> GetByUsernameOrEmailAsync(string username, string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all active users.
    /// </summary>
    Task<List<User>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the user entity as modified for saving.
    /// </summary>
    void Update(User user);

    /// <summary>
    /// Updates a user and immediately saves changes.
    /// </summary>
    Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);


    /// <summary>
    /// Marks the user entity as deleted for removal.
    /// </summary>
    void Delete(User user);

    /// <summary>
    /// Persists changes to the underlying data store.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
