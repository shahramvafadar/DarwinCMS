using AutoMapper;

using DarwinCMS.Application.DTOs.Roles;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Shared.Exceptions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;
using DarwinCMS.WebAdmin.Infrastructure.Security;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Handles administrative role management including listing, creation, editing, soft deletion, restoration, and permanent deletion of roles.
/// </summary>
[Area("Admin")]
[HasPermission("manage_roles")]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesController"/> class.
    /// </summary>
    public RolesController(IRoleService roleService, ICurrentUserService currentUser, IMapper mapper)
    {
        _roleService = roleService;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    /// <summary>
    /// Displays the paginated list of active roles with optional filtering and sorting.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm, string? sortColumn, string? sortDirection, int page = 1)
    {
        const int PageSize = 10;
        var skip = (page - 1) * PageSize;

        var result = await _roleService.GetPagedListAsync(searchTerm, sortColumn, sortDirection, skip, PageSize);

        var viewModel = new RoleListViewModel
        {
            Roles = result.Roles.Select(_mapper.Map<RoleListItemViewModel>).ToList(),
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)result.TotalCount / PageSize),
            SearchTerm = searchTerm,
            SortColumn = sortColumn ?? "Name",
            SortDirection = sortDirection ?? "asc"
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the list of soft-deleted roles (recycle bin).
    /// </summary>
    [HttpGet]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Deleted()
    {
        var deletedRoles = await _roleService.GetDeletedAsync();
        var viewModel = _mapper.Map<List<RoleListItemViewModel>>(deletedRoles);

        return View(viewModel);
    }

    /// <summary>
    /// Displays the role creation form.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateRoleViewModel());
    }

    /// <summary>
    /// Submits the role creation request.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var request = _mapper.Map<CreateRoleRequest>(model);
        await _roleService.CreateAsync(request, _currentUser.UserId ?? Guid.Empty);

        this.AddSuccess("Role created successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the role edit form.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var dto = await _roleService.GetByIdAsync(id);
        if (dto == null)
            return NotFound();

        var model = _mapper.Map<EditRoleViewModel>(dto);
        return View(model);
    }

    /// <summary>
    /// Submits the updated role data.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var request = _mapper.Map<UpdateRoleRequest>(model);
            await _roleService.UpdateAsync(request, _currentUser.UserId ?? Guid.Empty);

            this.AddSuccess("Role updated successfully.");
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            this.AddError(ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// Displays the confirmation page for soft-deleting a role.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var dto = await _roleService.GetByIdAsync(id);
        if (dto == null)
            return NotFound();

        var model = _mapper.Map<DeleteRoleViewModel>(dto);
        return View(model);
    }

    /// <summary>
    /// Performs a soft delete (logical deletion) of the role.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SoftDeleteConfirmed(Guid id)
    {
        var userId = _currentUser.UserId ?? throw new InvalidOperationException("User not authenticated.");
        await _roleService.SoftDeleteAsync(id, userId);

        this.AddSuccess("Role moved to recycle bin.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Restores a previously soft-deleted role back to active status.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var userId = _currentUser.UserId ?? throw new InvalidOperationException("User not authenticated.");
        await _roleService.RestoreAsync(id, userId);

        this.AddSuccess("Role restored successfully.");
        return RedirectToAction(nameof(Deleted));
    }

    /// <summary>
    /// Permanently deletes a role from the system.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        await _roleService.HardDeleteAsync(id);

        this.AddSuccess("Role permanently deleted.");
        return RedirectToAction(nameof(Deleted));
    }
}
