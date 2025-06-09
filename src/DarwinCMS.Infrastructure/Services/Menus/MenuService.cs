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
/// Includes creation, update, soft delete, hard delete, and restoration logic.
/// </summary>
public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuService"/> class.
    /// </summary>
    /// <param name="menuRepository">The repository for managing Menu entities.</param>
    /// <param name="menuItemRepository">The repository for managing MenuItem entities.</param>
    /// <param name="mapper">The AutoMapper instance for DTO mapping.</param>
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
            .Where(m => !m.IsDeleted)
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
    public async Task<Guid> CreateAsync(CreateMenuDto dto, Guid createdByUserId, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Menu>(dto);
        entity.MarkAsCreated(createdByUserId);
        await _menuRepository.AddAsync(entity, cancellationToken);
        await _menuRepository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid id, UpdateMenuDto dto, Guid modifiedByUserId, CancellationToken cancellationToken = default)
    {
        var entity = await _menuRepository.Query()
            .Include(m => m.Items)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException("Menu not found.");

        entity.SetTitle(dto.Title, modifiedByUserId);
        entity.SetPosition(dto.Position, modifiedByUserId);
        entity.SetLanguage(dto.LanguageCode, modifiedByUserId);

        // Remove old items and replace with new ones
        entity.Items.Clear();
        var updatedItems = _mapper.Map<List<MenuItem>>(dto.Items);
        foreach (var item in updatedItems)
        {
            entity.Items.Add(item);
        }

        _menuRepository.Update(entity);
        await _menuRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        await _menuRepository.SoftDeleteAsync(id, userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        await _menuRepository.RestoreAsync(id, userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _menuRepository.HardDeleteAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<MenuListItemDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var deletedMenus = await _menuRepository.GetDeletedAsync(cancellationToken);
        return _mapper.Map<List<MenuListItemDto>>(deletedMenus);
    }
}
