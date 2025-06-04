using AutoMapper;

using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Admin controller for managing menu items in the admin panel.
/// Supports nested navigation, search, filtering, and CRUD operations.
/// </summary>
[Area("Admin")]
public class MenuItemsController : Controller
{
    private readonly IMenuItemService _menuItemService;
    private readonly IMenuService _menuService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemsController"/> class.
    /// </summary>
    /// <param name="menuItemService">The service to manage menu items.</param>
    /// <param name="menuService">The service to manage menus.</param>
    /// <param name="mapper">The mapper used to transform data between models and DTOs.</param>
    public MenuItemsController(
        IMenuItemService menuItemService,
        IMenuService menuService,
        IMapper mapper)
    {
        _menuItemService = menuItemService;
        _menuService = menuService;
        _mapper = mapper;
    }

    /// <summary>
    /// Lists all menu items for a given menu (hierarchical).
    /// </summary>
    /// <param name="menuId">The ID of the parent menu.</param>
    /// <param name="sortBy">Optional sorting parameter.</param>
    /// <param name="search">Optional search term for filtering.</param>
    public async Task<IActionResult> Index(Guid menuId, string? sortBy = null, string? search = null)
    {
        var menu = await _menuService.GetByIdAsync(menuId);
        if (menu == null)
        {
            this.AddError("Menu not found.");
            return RedirectToAction("Index", "Menus");
        }

        ViewBag.MenuId = menuId;
        ViewBag.MenuTitle = menu.Title;
        ViewBag.SortBy = sortBy;
        ViewBag.Search = search;

        var items = await _menuItemService.GetItemsByMenuIdAsync(menuId);
        var mapped = _mapper.Map<List<MenuItemListItemViewModel>>(items);

        return View(mapped);
    }

    /// <summary>
    /// Renders the form to create a new menu item.
    /// </summary>
    /// <param name="menuId">The ID of the parent menu.</param>
    /// <param name="parentId">Optional ID of the parent menu item.</param>
    public IActionResult Create(Guid menuId, Guid? parentId)
    {
        var vm = new CreateMenuItemViewModel
        {
            MenuId = menuId,
            ParentId = parentId
        };
        return View(vm);
    }

    /// <summary>
    /// Handles the POST request to create a new menu item.
    /// </summary>
    /// <param name="vm">The view model containing the new item data.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMenuItemViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var dto = _mapper.Map<CreateMenuItemDto>(vm);
        await _menuItemService.CreateAsync(dto);

        this.AddSuccess("Menu item created successfully.");
        return RedirectToAction("Index", new { menuId = vm.MenuId });
    }

    /// <summary>
    /// Loads a menu item for editing.
    /// </summary>
    /// <param name="id">The ID of the menu item to edit.</param>
    public async Task<IActionResult> Edit(Guid id)
    {
        var dto = await _menuItemService.GetByIdAsync(id);
        if (dto == null)
        {
            this.AddError("Menu item not found.");
            return RedirectToAction("Index", "Menus");
        }

        var vm = _mapper.Map<EditMenuItemViewModel>(dto);
        return View(vm);
    }

    /// <summary>
    /// Updates a menu item.
    /// </summary>
    /// <param name="vm">The view model containing updated item data.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditMenuItemViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var dto = _mapper.Map<UpdateMenuItemDto>(vm);
        await _menuItemService.UpdateAsync(dto);

        this.AddSuccess("Menu item updated successfully.");
        return RedirectToAction("Index", new { menuId = vm.MenuId });
    }

    /// <summary>
    /// Displays the confirmation page for deleting a menu item.
    /// </summary>
    /// <param name="id">The ID of the menu item to delete.</param>
    public async Task<IActionResult> Delete(Guid id)
    {
        var dto = await _menuItemService.GetByIdAsync(id);
        if (dto == null)
        {
            this.AddError("Menu item not found.");
            return RedirectToAction("Index", "Menus");
        }

        var vm = _mapper.Map<EditMenuItemViewModel>(dto);
        return View(vm);
    }

    /// <summary>
    /// Confirms and deletes the menu item.
    /// </summary>
    /// <param name="id">The ID of the menu item to delete.</param>
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _menuItemService.DeleteAsync(id);

        this.AddSuccess("Menu item deleted.");
        return RedirectToAction("Index", "Menus");
    }
}
