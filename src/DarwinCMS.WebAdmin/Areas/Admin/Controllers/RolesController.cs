using AutoMapper;

using DarwinCMS.Application.DTOs.Roles;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Shared.Exceptions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Shared;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;
using DarwinCMS.WebAdmin.Infrastructure.Security;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// 
/// Handles administrative role management including listing, creation, editing, and deletion of roles.
/// 
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
    /// Displays the paginated list of roles with optional filtering and sorting.
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
    /// Displays the delete confirmation page for a role.
    /// </summary>
    [HttpGet]
    [Route("Admin/Roles/Delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var dto = await _roleService.GetByIdAsync(id);
        if (dto == null)
            return NotFound();

        var model = _mapper.Map<DeleteRoleViewModel>(dto);
        return View(model);
    }

    /// <summary>
    /// Confirms deletion of the role.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            await _roleService.DeleteAsync(id);
            this.AddSuccess("Role deleted successfully.");
        }
        catch (BusinessRuleException ex)
        {
            this.AddError(ex.Message);
        }

        return RedirectToAction(nameof(Index));
    }

}

