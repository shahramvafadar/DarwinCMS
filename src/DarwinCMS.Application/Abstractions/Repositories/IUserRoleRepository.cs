using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Defines the contract for accessing user-role assignment data in the persistence layer.
/// This abstraction is used by the application layer to assign roles to users, fetch assigned roles,
/// and manage user-role relationships without knowing about the underlying database technology.
/// </summary>
public interface IUserRoleRepository
{
    /// <summary>
    /// Returns an <see cref="IQueryable{UserRole}"/> for querying user-role data using LINQ.
    /// This is primarily used to support dynamic filtering, sorting, or pagination at the service layer.
    /// </summary>
    /// <returns>An IQueryable for user-role entries.</returns>
    IQueryable<UserRole> Query();

    /// <summary>
    /// Retrieves a specific user-role assignment, optionally filtered by module name.
    /// </summary>
    /// <param name="userId">Unique identifier of the user.</param>
    /// <param name="roleId">Unique identifier of the role.</param>
    /// <param name="module">Optional name of the module if role is module-scoped (e.g., "Store", "CRM").</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The matching <see cref="UserRole"/> if found, otherwise null.</returns>
    Task<UserRole?> GetAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the primary role for a given user. Each user may have multiple roles, 
    /// but only one can be marked as their "primary" for display or access purposes.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The <see cref="Role"/> marked as primary, or null if none is found.</returns>
    Task<Role?> GetPrimaryRoleForUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the list of role IDs assigned to a specific user.
    /// </summary>
    /// <param name="userId">Unique identifier of the user.</param>
    /// <returns>List of GUIDs representing assigned role IDs.</returns>
    Task<List<Guid>> GetRoleIdsByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all user-role entries for a specific user, optionally scoped to a module.
    /// </summary>
    /// <param name="userId">Unique identifier of the user.</param>
    /// <param name="module">Optional module name.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of <see cref="UserRole"/> objects for the user.</returns>
    Task<List<UserRole>> GetByUserIdAsync(Guid userId, string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a specific user-role assignment exists.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="roleId">Role ID.</param>
    /// <param name="module">Optional module name to check scoped roles.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><c>true</c> if the assignment exists, otherwise <c>false</c>.</returns>
    Task<bool> ExistsAsync(Guid userId, Guid roleId, string? module = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user-role assignment to the underlying store.
    /// This is used when assigning a new role to a user.
    /// </summary>
    /// <param name="userRole">The <see cref="UserRole"/> entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing user-role assignment from the store.
    /// </summary>
    /// <param name="userRole">The <see cref="UserRole"/> to be removed.</param>
    void Delete(UserRole userRole);

    /// <summary>
    /// Persists any pending changes (such as Add or Delete operations) to the data store.
    /// Should be called explicitly to commit changes.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
