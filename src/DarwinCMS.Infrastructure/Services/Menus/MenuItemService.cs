using AutoMapper;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Infrastructure.Services.Menus;

/// <summary>
/// Service responsible for managing menu items.
/// Handles creation, updating, soft deletion, restoration, hard deletion and tree-building logic.
/// </summary>
public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemService"/> class.
    /// </summary>
    public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper)
    {
        _menuItemRepository = menuItemRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<List<MenuItemDto>> GetItemsByMenuIdAsync(Guid menuId, CancellationToken cancellationToken = default)
    {
        var items = await _menuItemRepository.GetByMenuIdAsync(menuId, cancellationToken);
        var activeItems = items.Where(i => !i.IsDeleted).ToList();
        var roots = activeItems.Where(i => i.ParentId == null).ToList();
        return roots.Select(item => BuildTree(item, activeItems)).ToList();
    }

    /// <inheritdoc />
    public async Task<MenuItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _menuItemRepository.GetWithChildrenAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<MenuItemDto>(entity);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(CreateMenuItemDto dto, Guid createdByUserId, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<MenuItem>(dto);
        entity.MarkAsCreated(createdByUserId);
        await _menuItemRepository.AddAsync(entity, cancellationToken);
        await _menuItemRepository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UpdateMenuItemDto dto, Guid modifiedByUserId, CancellationToken cancellationToken = default)
    {
        var entity = await _menuItemRepository.GetByIdAsync(dto.Id, cancellationToken);
        if (entity == null)
            throw new InvalidOperationException("Menu item not found.");

        entity.SetTitle(dto.Title, modifiedByUserId);
        entity.SetIcon(dto.Icon, modifiedByUserId);
        entity.SetLinkType(dto.LinkType, modifiedByUserId);
        entity.SetPage(dto.PageId, modifiedByUserId);
        entity.SetUrl(dto.Url, modifiedByUserId);
        entity.SetDisplayCondition(dto.DisplayCondition, modifiedByUserId);
        entity.SetDisplayOrder(dto.DisplayOrder, modifiedByUserId);
        entity.SetIsActive(dto.IsActive, modifiedByUserId);
        entity.SetParentId(dto.ParentId, modifiedByUserId);

        _menuItemRepository.Update(entity);
        await _menuItemRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        await _menuItemRepository.SoftDeleteAsync(id, userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        await _menuItemRepository.RestoreAsync(id, userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _menuItemRepository.HardDeleteAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<MenuItemDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var deletedItems = await _menuItemRepository.GetDeletedAsync(cancellationToken);
        return _mapper.Map<List<MenuItemDto>>(deletedItems);
    }

    /// <summary>
    /// Recursively builds a tree of menu items starting from the given root.
    /// </summary>
    private MenuItemDto BuildTree(MenuItem root, List<MenuItem> allItems)
    {
        var dto = _mapper.Map<MenuItemDto>(root);
        var children = allItems.Where(i => i.ParentId == root.Id).ToList();
        dto.Children = children.Select(child => BuildTree(child, allItems)).ToList();
        return dto;
    }
}
