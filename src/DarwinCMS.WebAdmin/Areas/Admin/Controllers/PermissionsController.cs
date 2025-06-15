using AutoMapper;

using DarwinCMS.Application.DTOs.Permissions;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Permissions;
using DarwinCMS.Shared.Exceptions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Permissions;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;
using DarwinCMS.WebAdmin.Infrastructure.Security;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Handles admin-level operations related to permission management,
/// including listing, creation, editing, soft deletion, restoration, and hard deletion.
/// </summary>
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
            Permissions = _mapper.Map<List<PermissionListViewModel>>(result.Permissions),
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
    /// Loads the confirmation page before soft deleting a permission.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _permissionService.GetByIdAsync(id);
        if (entity == null)
            return NotFound();

        var model = _mapper.Map<PermissionListViewModel>(entity);
        return View(model);
    }

    /// <summary>
    /// Performs a soft delete (moves the permission to recycle bin).
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SoftDeleteConfirmed(Guid id)
    {
        await _permissionService.SoftDeleteAsync(id, _currentUser.UserId ?? Guid.Empty);
        this.AddSuccess("Permission moved to recycle bin.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Lists all permissions that are soft-deleted (recycle bin).
    /// </summary>
    [HttpGet]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Deleted()
    {
        var deletedPermissions = await _permissionService.GetDeletedAsync();
        var viewModel = _mapper.Map<List<PermissionListViewModel>>(deletedPermissions);
        return View(viewModel);
    }

    /// <summary>
    /// Restores a previously soft-deleted permission.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Restore(Guid id)
    {
        await _permissionService.RestoreAsync(id, _currentUser.UserId ?? Guid.Empty);
        this.AddSuccess("Permission restored successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Permanently deletes a permission from the system (hard delete).
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        await _permissionService.HardDeleteAsync(id);
        this.AddSuccess("Permission permanently deleted.");
        return RedirectToAction(nameof(Deleted));
    }
}
