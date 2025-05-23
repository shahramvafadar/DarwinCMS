using AutoMapper;
using DarwinCMS.Application.DTOs.Roles;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles;
using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Handles administrative role management including listing, creation, editing, and deletion of roles.
/// </summary>
[Area("Admin")]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesController"/> class.
    /// </summary>
    /// <param name="roleService">Role service for CRUD operations.</param>
    /// <param name="currentUser">Current authenticated user context.</param>
    /// <param name="mapper">AutoMapper instance.</param>
    public RolesController(IRoleService roleService, ICurrentUserService currentUser, IMapper mapper)
    {
        _roleService = roleService;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    /// <summary>
    /// Displays the paginated list of roles with optional filtering and sorting.
    /// </summary>
    /// <param name="searchTerm">Search term for filtering roles by name.</param>
    /// <param name="sortColumn">Column to sort by (e.g., Name).</param>
    /// <param name="sortDirection">Sort direction (asc or desc).</param>
    /// <param name="page">Current page number for pagination.</param>
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
    /// <param name="model">Form model containing role data.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var request = _mapper.Map<CreateRoleRequest>(model);
        await _roleService.CreateAsync(request, _currentUser.UserId ?? Guid.Empty);

        TempData["SuccessMessage"] = "Role created successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the role edit form.
    /// </summary>
    /// <param name="id">ID of the role to edit.</param>
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
    /// <param name="model">Form model containing updated role information.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var request = _mapper.Map<UpdateRoleRequest>(model);
        await _roleService.UpdateAsync(request, _currentUser.UserId ?? Guid.Empty);

        TempData["SuccessMessage"] = "Role updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the delete confirmation page for a role.
    /// </summary>
    /// <param name="id">ID of the role to delete.</param>
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
    /// <param name="id">ID of the role to permanently delete.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _roleService.DeleteAsync(id);
        TempData["SuccessMessage"] = "Role deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
