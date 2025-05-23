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
    private readonly DarwinDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the UserRoleRepository.
    /// </summary>
    /// <param name="dbContext">EF Core DbContext used for data access.</param>
    public UserRoleRepository(DarwinDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Returns a LINQ-queryable collection of user-role relationships including associated roles.
    /// Useful for building dynamic filters or joins.
    /// </summary>
    public IQueryable<UserRole> Query()
        => _dbContext.UserRoles.Include(ur => ur.Role).AsQueryable();

    /// <summary>
    /// Retrieves a single UserRole mapping based on user ID, role ID, and optional module scope.
    /// </summary>
    public async Task<UserRole?> GetAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default)
        => await _dbContext.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId && ur.Module == module, cancellationToken);

    /// <summary>
    /// Retrieves all user-role mappings for a specific user, optionally scoped to a module.
    /// Includes related Role entity for projection.
    /// </summary>
    public async Task<List<UserRole>> GetByUserIdAsync(Guid userId, string? module = null, CancellationToken cancellationToken = default)
    {
        // Start query including Role navigation for later use (e.g., role.DisplayName)
        var query = _dbContext.UserRoles
            .Include(ur => ur.Role)  // 🔥 Critical: allows access to ur.Role.Name / DisplayName
            .AsQueryable()
            .Where(ur => ur.UserId == userId);

        // Apply optional module filter if provided
        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(ur => ur.Module == module);

        // Return result as read-only list
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the list of role IDs associated with a specific user.
    /// </summary>
    public async Task<List<Guid>> GetRoleIdsByUserIdAsync(Guid userId)
    {
        return await _dbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .Distinct()
            .ToListAsync();
    }


    /// <summary>
    /// Checks whether a specific user-role mapping exists.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default)
        => await _dbContext.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId && ur.Module == module, cancellationToken);

    /// <summary>
    /// Adds a new user-role mapping to the database context.
    /// </summary>
    public async Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default)
        => await _dbContext.UserRoles.AddAsync(userRole, cancellationToken);

    /// <summary>
    /// Removes a user-role mapping from the database context.
    /// </summary>
    public void Delete(UserRole userRole)
        => _dbContext.UserRoles.Remove(userRole);

    /// <summary>
    /// Returns the first role entity assigned to a user.
    /// Useful for displaying a user's primary role.
    /// </summary>
    /// <param name="userId">The ID of the user whose role is to be retrieved.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>The first <see cref="Role"/> assigned to the user, or null if none exist.</returns>
    public async Task<Role?> GetPrimaryRoleForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .AsNoTracking()
            .Select(ur => ur.Role)
            .FirstOrDefaultAsync(cancellationToken);
    }


    /// <summary>
    /// Persists all pending changes in the database context.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}
