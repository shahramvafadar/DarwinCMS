using AutoMapper;

using DarwinCMS.Application.DTOs.Pages;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Common;
using DarwinCMS.Application.Services.Pages;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Pages;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;
using DarwinCMS.WebAdmin.Infrastructure.Security;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Admin controller for managing CMS pages, including listing, creating, editing, soft deletion, restoration, and permanent deletion.
/// </summary>
[Area("Admin")]
public class PagesController : Controller
{
    private readonly IPageService _pageService;
    private readonly IMapper _mapper;
    private readonly ILanguageProvider _languageProvider;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PagesController"/> class.
    /// </summary>
    public PagesController(
        IPageService pageService,
        IMapper mapper,
        ILanguageProvider languageProvider,
        ICurrentUserService currentUserService)
    {
        _pageService = pageService;
        _mapper = mapper;
        _languageProvider = languageProvider;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Displays a paginated, searchable, and sortable list of CMS pages.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(string? search, string? language, bool? published, string? sortBy, bool desc = false, int page = 1)
    {
        var filter = new PageFilterDto
        {
            Search = search,
            LanguageCode = language,
            IsPublished = published,
            Page = page,
            PageSize = 20,
            SortColumn = sortBy,
            SortDescending = desc
        };

        var dtoList = await _pageService.GetListAsync(filter);
        var itemViewModels = _mapper.Map<List<PageListItemViewModel>>(dtoList);

        var viewModel = new PageListViewModel
        {
            Pages = itemViewModels,
            Search = search,
            LanguageCode = language,
            IsPublished = published,
            CurrentPage = page,
            PageSize = filter.PageSize,
            SortColumn = sortBy,
            SortDirection = desc ? "desc" : "asc",
            LanguageList = _languageProvider.GetDisplayNames()
        };

        return View(viewModel);
    }

    /// <summary>
    /// Renders the form for creating a new CMS page.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        SetLanguageList();

        return View(new CreatePageViewModel
        {
            LanguageCode = "en",
            PublishDateUtc = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Handles form submission for creating a new CMS page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePageViewModel viewModel)
    {
        SetLanguageList();

        if (!ModelState.IsValid)
            return View(viewModel);

        var dto = _mapper.Map<CreatePageDto>(viewModel);
        var userId = _currentUserService.UserId ?? throw new InvalidOperationException("User not authenticated.");
        await _pageService.CreateAsync(dto, userId);

        this.AddSuccess("Page created successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the edit form for an existing CMS page.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        SetLanguageList();

        var page = await _pageService.GetByIdAsync(id);
        if (page == null)
            return NotFound();

        var viewModel = _mapper.Map<EditPageViewModel>(page);
        return View(viewModel);
    }

    /// <summary>
    /// Handles form submission for updating an existing CMS page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditPageViewModel viewModel)
    {
        SetLanguageList();

        if (!ModelState.IsValid)
            return View(viewModel);

        var dto = _mapper.Map<UpdatePageDto>(viewModel);
        var userId = _currentUserService.UserId ?? throw new InvalidOperationException("User not authenticated.");
        await _pageService.UpdateAsync(id, dto, userId);

        this.AddSuccess("Page updated successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the confirmation page for deleting a CMS page.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var page = await _pageService.GetByIdAsync(id);
        if (page == null)
            return NotFound();

        var viewModel = _mapper.Map<PageListItemViewModel>(page);
        return View(viewModel);
    }

    /// <summary>
    /// Performs a soft delete (moves the page to recycle bin).
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SoftDeleteConfirmed(Guid id)
    {
        var userId = _currentUserService.UserId ?? throw new InvalidOperationException("User not authenticated.");
        await _pageService.SoftDeleteAsync(id, userId);

        this.AddSuccess("Page moved to recycle bin.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the list of soft-deleted pages for administrators.
    /// </summary>
    [HttpGet]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Deleted()
    {
        var deletedPages = await _pageService.GetDeletedAsync();
        var viewModel = _mapper.Map<List<PageListItemViewModel>>(deletedPages);
        return View(viewModel);
    }

    /// <summary>
    /// Restores a soft-deleted page back to active status.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var userId = _currentUserService.UserId ?? throw new InvalidOperationException("User not authenticated.");
        await _pageService.RestoreAsync(id, userId);

        this.AddSuccess("Page restored successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Permanently deletes a page from the system.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        await _pageService.HardDeleteAsync(id);

        this.AddSuccess("Page permanently deleted.");
        return RedirectToAction(nameof(Deleted));
    }

    /// <summary>
    /// Helper method to populate language dropdown in views.
    /// </summary>
    private void SetLanguageList() =>
        ViewBag.LanguageList = _languageProvider.GetDisplayNames();
}
