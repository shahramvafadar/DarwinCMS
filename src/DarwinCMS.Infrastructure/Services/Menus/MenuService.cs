using AutoMapper;
using AutoMapper.QueryableExtensions;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Menus;

/// <summary>
/// Application service for managing navigation menus and their related items.
/// </summary>
public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the MenuService.
    /// </summary>
    public MenuService(
        IMenuRepository menuRepository,
        IMenuItemRepository menuItemRepository,
        IMapper mapper)
    {
        _menuRepository = menuRepository;
        _menuItemRepository = menuItemRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<List<MenuListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var menus = await _menuRepository.Query()
            .AsNoTracking()
            .ProjectTo<MenuListItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return menus;
    }

    /// <inheritdoc />
    public async Task<MenuDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var menu = await _menuRepository.Query()
            .Include(m => m.Items.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        return menu == null ? null : _mapper.Map<MenuDetailDto>(menu);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(CreateMenuDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Menu>(dto);
        await _menuRepository.AddAsync(entity, cancellationToken);
        await _menuRepository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid id, UpdateMenuDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _menuRepository.Query()
            .Include(m => m.Items)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException("Menu not found.");

        // Remove old items and replace with new
        entity.Items.Clear();
        var updatedItems = _mapper.Map<List<MenuItem>>(dto.Items);
        foreach (var item in updatedItems)
        {
            entity.Items.Add(item);
        }

        _mapper.Map(dto, entity);
        _menuRepository.Update(entity);
        await _menuRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _menuRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            return;

        if (entity.IsSystem)
            throw new InvalidOperationException("System menus cannot be deleted.");

        _menuRepository.Delete(entity);
        await _menuRepository.SaveChangesAsync(cancellationToken);
    }
}
