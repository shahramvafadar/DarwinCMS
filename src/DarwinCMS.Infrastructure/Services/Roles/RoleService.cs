using AutoMapper;
using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Roles;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Roles;

/// <summary>
/// Provides business logic for creating, updating, listing, and deleting roles.
/// Handles permission scoping and user-role bindings.
/// </summary>
public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructs the RoleService with required repositories and AutoMapper.
    /// </summary>
    public RoleService(
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IMapper mapper)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a list of all active roles in the system.
    /// Used for UI dropdowns and filtering.
    /// </summary>
    public async Task<List<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _roleRepository.Query()
            .Where(r => r.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<RoleDto>>(roles);
    }

    /// <summary>
    /// Gets the primary role (first) assigned to a specific user.
    /// </summary>
    public async Task<Guid?> GetPrimaryRoleIdForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userRoles = await _userRoleRepository.GetByUserIdAsync(userId, null, cancellationToken);
        return userRoles.FirstOrDefault()?.RoleId;
    }

    /// <summary>
    /// Retrieves a role by its unique identifier.
    /// </summary>
    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _roleRepository.GetByIdAsync(id, cancellationToken);

    /// <summary>
    /// Retrieves a role by its unique technical name.
    /// </summary>
    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _roleRepository.GetByNameAsync(name, cancellationToken);

    /// <summary>
    /// Creates a new role with the provided data.
    /// </summary>
    public async Task<Role> CreateAsync(CreateRoleRequest request, Guid performedByUserId, CancellationToken cancellationToken = default)
    {
        var role = new Role(
            request.Name,
            createdByUserId: performedByUserId,
            displayName: request.DisplayName,
            description: request.Description,
            module: request.Module,
            displayOrder: request.DisplayOrder
        );

        await _roleRepository.AddAsync(role, cancellationToken);
        await _roleRepository.SaveChangesAsync(cancellationToken);

        return role;
    }

    /// <summary>
    /// Updates an existing role's metadata.
    /// </summary>
    public async Task UpdateAsync(UpdateRoleRequest request, Guid performedByUserId, CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Role not found.");

        role.UpdateInfo(request.DisplayName, request.Description, performedByUserId);
        role.SetModule(request.Module, performedByUserId);
        role.SetDisplayOrder(request.DisplayOrder, performedByUserId);

        if (request.IsActive)
            role.Activate(performedByUserId);
        else
            role.Deactivate(performedByUserId);

        _roleRepository.Update(role);
        await _roleRepository.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a role from the system permanently.
    /// </summary>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException("Role not found.");

        _roleRepository.Delete(role);
        await _roleRepository.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a paged list of roles with optional filtering and sorting.
    /// Used by admin UI listing page.
    /// </summary>
    public async Task<RoleListResultDto> GetPagedListAsync(
        string? search,
        string? sortColumn,
        string? sortDirection,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _roleRepository.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r =>
                r.Name.Contains(search) ||
                r.DisplayName!.Contains(search));
        }

        query = sortColumn?.ToLowerInvariant() switch
        {
            "name" => sortDirection == "desc"
                ? query.OrderByDescending(r => r.Name)
                : query.OrderBy(r => r.Name),

            "displayname" => sortDirection == "desc"
                ? query.OrderByDescending(r => r.DisplayName)
                : query.OrderBy(r => r.DisplayName),

            "createdat" => sortDirection == "desc"
                ? query.OrderByDescending(r => r.CreatedAt)
                : query.OrderBy(r => r.CreatedAt),

            _ => query.OrderBy(r => r.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var pagedRoles = await query.Skip(skip).Take(take).ToListAsync(cancellationToken);

        return new RoleListResultDto
        {
            TotalCount = totalCount,
            Roles = _mapper.Map<List<RoleDto>>(pagedRoles)
        };
    }
}
