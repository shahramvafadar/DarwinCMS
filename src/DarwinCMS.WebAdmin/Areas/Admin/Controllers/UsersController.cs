using AutoMapper;

using DarwinCMS.Application.DTOs.Users;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Users;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Controller responsible for managing user accounts in the admin area,
/// including listing, creating, editing, and deleting users.
/// </summary>
[Area("Admin")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">User management service.</param>
    /// <param name="roleService">Role lookup service.</param>
    /// <param name="currentUser">Current authenticated user context.</param>
    /// <param name="mapper">AutoMapper instance used for mapping DTOs.</param>
    public UsersController(
        IUserService userService,
        IRoleService roleService,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _userService = userService;
        _roleService = roleService;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    /// <summary>
    /// Displays the paginated list of users with optional filtering and sorting.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(
        string? searchTerm,
        Guid? roleFilterId,
        string? sortColumn,
        string? sortDirection,
        int page = 1,
        int pageSize = 10)
    {
        int skip = (page - 1) * pageSize;

        var result = await _userService.GetUserListAsync(
            searchTerm,
            roleFilterId,
            sortColumn,
            sortDirection,
            skip,
            pageSize);

        var allRoles = await _roleService.GetAllRolesAsync();
        var viewModel = new UserListViewModel
        {
            Users = result.Users,
            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize),
            CurrentPage = page,
            SearchTerm = searchTerm,
            RoleFilterId = roleFilterId,
            Roles = allRoles.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name }).ToList(),
            SortColumn = sortColumn,
            SortDirection = sortDirection
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the create user form.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateUserViewModel
        {
            Roles = await GetRoleSelectListAsync()
        };

        return View(model);
    }

    /// <summary>
    /// Processes the create user form submission.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Roles = await GetRoleSelectListAsync();
            return View(model);
        }

        var request = _mapper.Map<CreateUserRequest>(model);
        await _userService.CreateAsync(request, _currentUser.UserId ?? Guid.Empty);

        TempData["SuccessMessage"] = "User created successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the edit form for a specific user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null)
            return NotFound();

        var model = _mapper.Map<EditUserViewModel>(user);
        model.RoleIds = await _userService.GetUserPrimaryRoleIdsAsync(id);
        model.Roles = await GetRoleSelectListAsync();

        return View(model);
    }

    /// <summary>
    /// Processes the update of a user.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Roles = await GetRoleSelectListAsync();
            return View(model);
        }

        var request = _mapper.Map<UpdateUserRequest>(model);
        await _userService.UpdateAsync(request, _currentUser.UserId ?? Guid.Empty);

        TempData["SuccessMessage"] = "User updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Loads the delete confirmation view.
    /// </summary>
    [HttpGet]
    [Route("Admin/Users/Delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null)
            return NotFound();

        var model = _mapper.Map<EditUserViewModel>(user);
        model.RoleIds = await _userService.GetUserPrimaryRoleIdsAsync(id);
        model.Roles = await GetRoleSelectListAsync();

        return View(model);
    }

    /// <summary>
    /// Confirms and deletes a user by ID.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        TempData["SuccessMessage"] = "User deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Generates a SelectList of roles for form dropdowns.
    /// </summary>
    /// <returns>List of SelectListItem representing roles.</returns>
    private async Task<List<SelectListItem>> GetRoleSelectListAsync()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return roles.Select(r => new SelectListItem
        {
            Value = r.Id.ToString(),
            Text = r.Name
        }).ToList();
    }
}
