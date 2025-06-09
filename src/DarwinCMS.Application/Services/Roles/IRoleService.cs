using DarwinCMS.Application.DTOs.Roles;
using DarwinCMS.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DarwinCMS.Application.Services.Roles;

/// <summary>
/// Provides read and management operations for system roles.
/// Supports CRUD operations, lookups, filtering, and admin UI listings.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Retrieves a paged and filtered list of roles for display in admin panel.
    /// </summary>
    /// <param name="search">Optional search term (applied to name or display name).</param>
    /// <param name="sortColumn">Column name to sort by (e.g., "Name", "CreatedAt").</param>
    /// <param name="sortDirection">Sort direction: "asc" or "desc".</param>
    /// <param name="skip">Number of records to skip (for pagination).</param>
    /// <param name="take">Number of records to take (for pagination).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Paged list of roles and total count.</returns>
    Task<RoleListResultDto> GetPagedListAsync(
        string? search,
        string? sortColumn,
        string? sortDirection,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new role entity in the system.
    /// </summary>
    /// <param name="request">The request containing role data.</param>
    /// <param name="performedByUserId">ID of the user performing the action (for auditing).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The newly created role entity.</returns>
    Task<Role> CreateAsync(CreateRoleRequest request, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the information of an existing role.
    /// </summary>
    /// <param name="request">The update request containing new role data.</param>
    /// <param name="performedByUserId">ID of the user performing the update (for auditing).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdateAsync(UpdateRoleRequest request, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a role entity from the system.
    /// This will also remove any related permissions or user-role mappings.
    /// </summary>
    /// <param name="id">ID of the role to delete.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a single role entity by its ID.
    /// Typically used to pre-fill an edit form.
    /// </summary>
    /// <param name="id">The unique ID of the role.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The role entity if found; otherwise null.</returns>
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a role entity by its exact system name.
    /// </summary>
    /// <param name="name">The exact name of the role (case-sensitive).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The matching role if found; otherwise null.</returns>
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the primary role ID assigned to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The primary role ID or null if none is set.</returns>
    Task<Guid?> GetPrimaryRoleIdForUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all roles in the system.
    /// Useful for populating dropdowns or access control options.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of all roles as lightweight DTOs.</returns>
    Task<List<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a logical (soft) deletion of the specified role.
    /// </summary>
    /// <param name="id">The ID of the role to soft delete.</param>
    /// <param name="performedByUserId">ID of the user performing the deletion.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SoftDeleteAsync(Guid id, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores a previously soft-deleted role.
    /// </summary>
    /// <param name="id">The ID of the role to restore.</param>
    /// <param name="performedByUserId">ID of the user performing the restoration.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task RestoreAsync(Guid id, Guid performedByUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a list of roles that have been logically deleted (soft-deleted).
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of soft-deleted roles as lightweight DTOs.</returns>
    Task<List<RoleDto>> GetDeletedAsync(CancellationToken cancellationToken = default);

}
