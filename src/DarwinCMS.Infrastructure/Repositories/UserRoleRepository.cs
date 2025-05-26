using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Provides an EF Core-based implementation of the IUserRoleRepository interface.
/// Handles user-role mapping queries and persistence.
/// </summary>
public class UserRoleRepository : IUserRoleRepository
{
    private readonly DarwinDbContext _db;
    private readonly DbSet<UserRole> _set;

    /// <summary>
    /// Initializes a new instance of the UserRoleRepository.
    /// </summary>
    /// <param name="db">EF Core DbContext used for data access.</param>
    public UserRoleRepository(DarwinDbContext db)
    {
        _db = db;
        _set = _db.UserRoles;
    }

    /// <inheritdoc />
    public IQueryable<UserRole> Query()
    {
        // Returns a LINQ-queryable collection of user-role relationships including associated roles.
        // Useful for building dynamic filters or joins.
        return _set.Include(ur => ur.Role).AsQueryable();
    }

    /// <inheritdoc />
    public async Task<UserRole?> GetAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default)
    {
        // Retrieves a single UserRole mapping based on user ID, role ID, and optional module scope.
        return await _set.FirstOrDefaultAsync(ur =>
            ur.UserId == userId &&
            ur.RoleId == roleId &&
            ur.Module == module,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<UserRole>> GetByUserIdAsync(Guid userId, string? module = null, CancellationToken cancellationToken = default)
    {
        // Start query including Role navigation for later use (e.g., role.DisplayName)
        var query = _set
            .Include(ur => ur.Role) // Allows access to ur.Role.Name / DisplayName
            .Where(ur => ur.UserId == userId)
            .AsQueryable();

        // Apply optional module filter if provided
        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(ur => ur.Module == module);

        // Return result as read-only list
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Guid>> GetRoleIdsByUserIdAsync(Guid userId)
    {
        // Gets the list of role IDs associated with a specific user.
        return await _set
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .Distinct()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default)
    {
        // Checks whether a specific user-role mapping exists.
        return await _set.AnyAsync(ur =>
            ur.UserId == userId &&
            ur.RoleId == roleId &&
            ur.Module == module,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        // Adds a new user-role mapping to the database context.
        await _set.AddAsync(userRole, cancellationToken);
    }

    /// <inheritdoc />
    public void Delete(UserRole userRole)
    {
        // Removes a user-role mapping from the database context.
        _set.Remove(userRole);
    }

    /// <inheritdoc />
    public async Task<Role?> GetPrimaryRoleForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Returns the first role entity assigned to a user.
        // Useful for displaying a user's primary role.
        return await _set
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .AsNoTracking()
            .Select(ur => ur.Role)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Persists all pending changes in the database context.
        await _db.SaveChangesAsync(cancellationToken);
    }
}
