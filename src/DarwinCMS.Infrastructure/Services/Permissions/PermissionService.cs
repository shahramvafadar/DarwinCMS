using System.Linq.Expressions;

using Ardalis.GuardClauses;

using AutoMapper;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Permissions;
using DarwinCMS.Application.Services.Permissions;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Shared.Exceptions;

namespace DarwinCMS.Infrastructure.Services.Permissions;

/// <summary>
/// Implements the permission-related application logic,
/// including creation, update, retrieval, filtering, and deletion.
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the service with required repository and mapper.
    /// </summary>
    public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.GetAllAsync(null, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.GetByIdAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.GetByNameAsync(name, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CreateAsync(CreatePermissionRequest request, Guid createdBy, CancellationToken cancellationToken = default)
    {
        var entity = new Permission(
            name: request.Name,
            createdByUserId: createdBy,
            displayName: request.DisplayName,
            module: null,
            description: null
        );

        await _permissionRepository.AddAsync(entity, cancellationToken);
        await _permissionRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UpdatePermissionRequest request, Guid modifiedBy, CancellationToken cancellationToken = default)
    {
        var entity = await _permissionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new BusinessRuleException("Permission not found.");

        if (entity.IsSystem && entity.Name != request.Name)
            throw new BusinessRuleException("System permissions cannot be renamed.");

        entity.SetName(request.Name);
        entity.UpdateInfo(request.DisplayName, null, null, modifiedBy);
        
        _permissionRepository.Update(entity);

        await _permissionRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Permission not found.", id.ToString());

        if (permission.IsSystem)
            throw new BusinessRuleException("System permissions cannot be deleted.");

        await _permissionRepository.SoftDeleteAsync(id, userId, cancellationToken);
        await _permissionRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        await _permissionRepository.RestoreAsync(id, userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetByIdAsync(id, cancellationToken);
        if (permission == null)
            return;

        if (permission.IsSystem)
            throw new BusinessRuleException("System permissions cannot be deleted.");

        _permissionRepository.Delete(permission);
        await _permissionRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Permission>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.GetDeletedAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PermissionListResultDto> GetPagedListAsync(
        string? search,
        string? sortColumn,
        string? sortDirection,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _permissionRepository.Query();
        query = query.Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.Name.Contains(search) ||
                (p.DisplayName != null && p.DisplayName.Contains(search)));
        }

        // Default sorting
        Expression<Func<Permission, object>> sortExpr = sortColumn?.ToLower() switch
        {
            "displayname" => p => p.DisplayName!,
            "issystem" => p => p.IsSystem,
            _ => p => p.Name
        };

        query = sortDirection?.ToLower() == "desc"
            ? query.OrderByDescending(sortExpr)
            : query.OrderBy(sortExpr);

        var total = await _permissionRepository.CountAsync(query, cancellationToken);
        var items = await _permissionRepository.ToListAsync(query.Skip(skip).Take(take), cancellationToken);

        var result = new PermissionListResultDto
        {
            TotalCount = total,
            Permissions = _mapper.Map<List<PermissionListDto>>(items)
        };

        return result;
    }
}
