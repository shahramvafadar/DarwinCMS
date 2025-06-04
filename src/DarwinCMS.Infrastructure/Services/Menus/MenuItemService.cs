using AutoMapper;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Infrastructure.Services.Menus;

/// <summary>
/// Service responsible for managing menu items.
/// Handles creation, updating, deletion and tree-building logic.
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
    public async Task<List<MenuItemDto>> GetItemsByMenuIdAsync(Guid menuId)
    {
        var items = await _menuItemRepository.GetByMenuIdAsync(menuId);
        var roots = items.Where(i => i.ParentId == null).ToList();
        return roots.Select(item => BuildTree(item, items)).ToList();
    }

    /// <inheritdoc />
    public async Task<MenuItemDto?> GetByIdAsync(Guid id)
    {
        var entity = await _menuItemRepository.GetWithChildrenAsync(id);
        return entity == null ? null : _mapper.Map<MenuItemDto>(entity);
    }

    /// <inheritdoc />
    public async Task CreateAsync(CreateMenuItemDto dto)
    {
        var entity = _mapper.Map<MenuItem>(dto);
        await _menuItemRepository.AddAsync(entity);
        await _menuItemRepository.SaveChangesAsync();
    }

    /// <inheritdoc />
    /// <inheritdoc />
    public async Task UpdateAsync(UpdateMenuItemDto dto)
    {
        var entity = await _menuItemRepository.GetByIdAsync(dto.Id);
        if (entity == null)
            throw new InvalidOperationException("Menu item not found.");

        entity.SetTitle(dto.Title);
        entity.SetIcon(dto.Icon);
        entity.SetLinkType(dto.LinkType);
        entity.SetPage(dto.PageId);
        entity.SetUrl(dto.Url);
        entity.SetDisplayCondition(dto.DisplayCondition);
        entity.SetDisplayOrder(dto.DisplayOrder);
        entity.SetIsActive(dto.IsActive);
        entity.SetParentId(dto.ParentId);

        _menuItemRepository.Update(entity);
        await _menuItemRepository.SaveChangesAsync();
    }


    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _menuItemRepository.GetByIdAsync(id);
        if (entity == null)
            throw new InvalidOperationException("Menu item not found.");

        _menuItemRepository.Delete(entity);
        await _menuItemRepository.SaveChangesAsync();
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
