using AutoMapper;

using DarwinCMS.Application.DTOs.Permissions;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Permissions;
using DarwinCMS.Shared.Exceptions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Permissions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Shared;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;
using DarwinCMS.WebAdmin.Infrastructure.Security;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// 
/// Handles admin-level operations related to permission management,
/// including listing, creation, editing, and deletion.
/// 
[Area("Admin")]
[HasPermission("manage_permissions")]
public class PermissionsController : Controller
{
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    private const int PageSize = 10;

    /// <summary>
    /// Initializes the controller with injected services.
    /// </summary>
    public PermissionsController(
        IPermissionService permissionService,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _permissionService = permissionService;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    /// <summary>
    /// Lists all permissions with optional search, sorting, and pagination.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(
        string? searchTerm,
        string? sortColumn,
        string? sortDirection,
        int page = 1)
    {
        int skip = (page - 1) * PageSize;

        var result = await _permissionService.GetPagedListAsync(
            search: searchTerm,
            sortColumn: sortColumn,
            sortDirection: sortDirection,
            skip: skip,
            take: PageSize);

        var viewModel = new PermissionIndexViewModel
        {
            Permissions = result.Permissions.Select(p => new PermissionListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                DisplayName = p.DisplayName,
                IsSystem = p.IsSystem
            }).ToList(),

            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)PageSize),
            CurrentPage = page,
            SearchTerm = searchTerm,
            SortColumn = sortColumn,
            SortDirection = sortDirection
        };

        return View(viewModel);
    }

    /// <summary>
    /// Shows the form for creating a new permission.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Processes the permission creation form.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePermissionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = _mapper.Map<CreatePermissionRequest>(model);
        await _permissionService.CreateAsync(dto, _currentUser.UserId ?? Guid.Empty);

        this.AddSuccess("Permission created successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Shows the form to edit a specific permission.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var entity = await _permissionService.GetByIdAsync(id);
        if (entity == null)
            return NotFound();

        var model = _mapper.Map<EditPermissionViewModel>(entity);
        return View(model);
    }

    /// <summary>
    /// Processes the permission update form.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditPermissionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var dto = _mapper.Map<UpdatePermissionRequest>(model);
            await _permissionService.UpdateAsync(dto, _currentUser.UserId ?? Guid.Empty);
            this.AddSuccess("Permission updated successfully.");
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            this.AddError(ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// Loads the confirmation page before deleting a permission.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _permissionService.GetByIdAsync(id);
        if (entity == null)
            return NotFound();

        var model = new PermissionListViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            DisplayName = entity.DisplayName ?? string.Empty,
            IsSystem = entity.IsSystem
        };

        return View(model);
    }

    /// <summary>
    /// Permanently deletes a permission after confirmation.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _permissionService.DeleteAsync(id);
            this.AddSuccess("Permission deleted successfully.");
        }
        catch (BusinessRuleException ex)
        {
            this.AddError(ex.Message);
        }

        return RedirectToAction(nameof(Index));
    }

}

