using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Entity Framework Core implementation of the <see cref="IRoleRepository"/> interface.
/// Provides CRUD operations and querying for <see cref="Role"/> entities.
/// </summary>
public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleRepository"/> class.
    /// </summary>
    /// <param name="db">The application's main EF DbContext</param>
    public RoleRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _set.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _set
            .Where(r => r.IsSystem)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a list of all roles that are marked as active.
    /// </summary>
    public async Task<List<Role>> GetAllActiveAsync(CancellationToken cancellationToken = default)
        => await _set
            .Where(r => r.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public override IQueryable<Role> Query()
    {
        return _set
            .Include(r => r.RolePermissions)
            .Include(r => r.UserRoles)
            .AsQueryable();
    }
}
