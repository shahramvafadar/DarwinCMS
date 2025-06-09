using AutoMapper;

using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;
using DarwinCMS.WebAdmin.Infrastructure.Security;

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
    private readonly ICurrentUserService _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemsController"/> class.
    /// </summary>
    public MenuItemsController(
        IMenuItemService menuItemService,
        IMenuService menuService,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _menuItemService = menuItemService;
        _menuService = menuService;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Lists all active menu items for a given menu.
    /// </summary>
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
        var activeItems = items.Where(i => !i.IsDeleted).ToList();
        var mapped = _mapper.Map<List<MenuItemListItemViewModel>>(activeItems);

        return View(mapped);
    }

    /// <summary>
    /// Displays the recycle bin (deleted items).
    /// </summary>
    [HttpGet]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Deleted()
    {
        var deletedItems = await _menuItemService.GetDeletedAsync();
        var viewModel = _mapper.Map<List<MenuItemListItemViewModel>>(deletedItems);
        return View(viewModel);
    }

    /// <summary>
    /// Renders the form to create a new menu item.
    /// </summary>
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMenuItemViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var dto = _mapper.Map<CreateMenuItemDto>(vm);
        await _menuItemService.CreateAsync(dto, _currentUser.UserId!.Value);

        this.AddSuccess("Menu item created successfully.");
        return RedirectToAction("Index", new { menuId = vm.MenuId });
    }

    /// <summary>
    /// Loads a menu item for editing.
    /// </summary>
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditMenuItemViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var dto = _mapper.Map<UpdateMenuItemDto>(vm);
        await _menuItemService.UpdateAsync(dto, _currentUser.UserId!.Value);

        this.AddSuccess("Menu item updated successfully.");
        return RedirectToAction("Index", new { menuId = vm.MenuId });
    }

    /// <summary>
    /// Displays the confirmation page for deleting a menu item.
    /// </summary>
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
    /// Performs a logical (soft) deletion of a menu item.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SoftDeleteConfirmed(Guid id)
    {
        await _menuItemService.SoftDeleteAsync(id, _currentUser.UserId!.Value);
        this.AddSuccess("Menu item moved to recycle bin.");
        return RedirectToAction("Index", "Menus");
    }

    /// <summary>
    /// Permanently deletes a menu item from the system.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        await _menuItemService.HardDeleteAsync(id);
        this.AddSuccess("Menu item permanently deleted.");
        return RedirectToAction("Deleted");
    }

    /// <summary>
    /// Restores a previously soft-deleted menu item.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Restore(Guid id)
    {
        await _menuItemService.RestoreAsync(id, _currentUser.UserId!.Value);
        this.AddSuccess("Menu item restored successfully.");
        return RedirectToAction("Index", "Menus");
    }
}
