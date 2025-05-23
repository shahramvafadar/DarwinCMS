using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Entity Framework Core implementation of the <see cref="IRoleRepository"/> interface.
/// Provides CRUD operations and querying for <see cref="Role"/> entities.
/// </summary>
public class RoleRepository : IRoleRepository
{
    private readonly DarwinDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The application's main EF DbContext</param>
    public RoleRepository(DarwinDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves a role by its unique identifier (primary key).
    /// </summary>
    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Roles.FindAsync(new object[] { id }, cancellationToken);

    /// <summary>
    /// Retrieves a role by its unique technical name.
    /// </summary>
    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);

    /// <summary>
    /// Returns a list of all roles that are marked as active.
    /// </summary>
    public async Task<List<Role>> GetAllActiveAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Roles
            .Where(r => r.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Adds a new role to the database context.
    /// </summary>
    public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
        => await _dbContext.Roles.AddAsync(role, cancellationToken);

    /// <summary>
    /// Marks a role as modified, to be updated on the next SaveChanges.
    /// </summary>
    public void Update(Role role)
        => _dbContext.Roles.Update(role);

    /// <summary>
    /// Marks a role for deletion. The change is applied on SaveChanges.
    /// </summary>
    public void Delete(Role role)
        => _dbContext.Roles.Remove(role);

    /// <summary>
    /// Saves all pending changes (create, update, delete) to the database.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Returns a queryable collection of roles for advanced LINQ operations like filtering, paging, and sorting.
    /// Includes navigation to related permissions and user-role mappings.
    /// </summary>
    public IQueryable<Role> Query()
        => _dbContext.Roles
            .Include(r => r.RolePermissions)
            .Include(r => r.UserRoles)
            .AsQueryable();
}
