using AutoMapper;

using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Admin controller for managing menus and their items.
/// Uses ControllerMessageExtensions for displaying user-friendly messages.
/// </summary>
[Area("Admin")]
public class MenusController : Controller
{
    private readonly IMenuService _menuService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenusController"/> class.
    /// </summary>
    public MenusController(IMenuService menuService, IMapper mapper)
    {
        _menuService = menuService;
        _mapper = mapper;
    }

    /// <summary>
    /// Lists all menus in the system.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var dtos = await _menuService.GetAllAsync();
        var model = _mapper.Map<List<MenuListItemViewModel>>(dtos);
        return View(model);
    }

    /// <summary>
    /// Displays the form to create a new menu.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateMenuViewModel());
    }

    /// <summary>
    /// Handles creation of a new menu.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMenuViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var dto = _mapper.Map<CreateMenuDto>(viewModel);
        await _menuService.CreateAsync(dto);

        this.AddSuccess("Menu created successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the form to edit an existing menu.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var dto = await _menuService.GetByIdAsync(id);
        if (dto == null)
        {
            this.AddError("Menu not found.");
            return RedirectToAction(nameof(Index));
        }

        var model = _mapper.Map<EditMenuViewModel>(dto);
        return View(model);
    }

    /// <summary>
    /// Handles update of a menu.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditMenuViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var dto = _mapper.Map<UpdateMenuDto>(viewModel);
        await _menuService.UpdateAsync(viewModel.Id, dto);

        this.AddSuccess("Menu updated successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays confirmation page for menu deletion.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var dto = await _menuService.GetByIdAsync(id);
        if (dto == null)
        {
            this.AddError("Menu not found.");
            return RedirectToAction(nameof(Index));
        }

        var model = _mapper.Map<DeleteMenuViewModel>(dto);
        return View(model);
    }

    /// <summary>
    /// Confirms and performs menu deletion.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _menuService.DeleteAsync(id);

        this.AddSuccess("Menu deleted successfully.");
        return RedirectToAction(nameof(Index));
    }
}
