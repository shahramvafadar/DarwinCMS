using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IUserRepository for managing users.
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the User repository.
    /// </summary>
    /// <param name="db">Darwin CMS database context.</param>
    public UserRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalized = username.Trim().ToLowerInvariant();
        return await _set.FirstOrDefaultAsync(u => u.Username.ToLower() == normalized, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return await _set.FirstOrDefaultAsync(u => u.Email.Value.ToLower() == normalized, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameOrEmailAsync(string username, string email, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await _set.FirstOrDefaultAsync(u =>
            u.Username.ToLower() == normalizedUsername ||
            u.Email.Value.ToLower() == normalizedEmail,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<User>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _set
            .Where(u => u.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _set.Update(user);
        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<User>> GetSystemUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _set
            .Where(u => u.IsSystem)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
