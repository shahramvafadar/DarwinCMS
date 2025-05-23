using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IUserRepository for managing users.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DarwinDbContext _db;

    /// <summary>
    /// Creates a new instance of the user repository using EF Core context.
    /// </summary>
    public UserRepository(DarwinDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Returns a queryable collection of users.
    /// Use AsNoTracking for read-only queries to improve performance.
    /// </summary>
    public IQueryable<User> Query()
    {
        return _db.Users.AsQueryable();
    }

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Users.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <summary>
    /// Gets a user by username (case-insensitive).
    /// </summary>
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalized = username.Trim().ToLowerInvariant();
        return await _db.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == normalized, cancellationToken);
    }

    /// <summary>
    /// Gets a user by email (case-insensitive).
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return await _db.Users
            .FirstOrDefaultAsync(u => u.Email.Value.ToLower() == normalized, cancellationToken);
    }

    /// <summary>
    /// Gets a user by either username or email (case-insensitive).
    /// </summary>
    public async Task<User?> GetByUsernameOrEmailAsync(string username, string email, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await _db.Users
            .FirstOrDefaultAsync(u =>
                u.Username.ToLower() == normalizedUsername ||
                u.Email.Value.ToLower() == normalizedEmail,
                cancellationToken);
    }

    /// <summary>
    /// Returns all active users.
    /// </summary>
    public async Task<List<User>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Users
            .Where(u => u.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    

    /// <summary>
    /// Adds a new user to the database.
    /// </summary>
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _db.Users.AddAsync(user, cancellationToken);
    }

    /// <summary>
    /// Marks a user as modified for EF tracking.
    /// </summary>
    public void Update(User user)
    {
        _db.Users.Update(user);
    }

    /// <summary>
    /// Updates the specified user in the database.
    /// </summary>
    /// <param name="user">The user entity to update. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the operation to complete.  The default value is
    /// <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync(cancellationToken);
    }


    /// <summary>
    /// Removes a user from the database.
    /// </summary>
    public void Delete(User user)
    {
        _db.Users.Remove(user);
    }

    /// <summary>
    /// Commits all pending changes to the database.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
