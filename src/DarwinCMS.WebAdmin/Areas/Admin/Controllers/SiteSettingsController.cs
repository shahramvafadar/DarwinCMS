using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using DarwinCMS.Application.DTOs.SiteSettings;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Application.Services.Settings;
using DarwinCMS.Shared.Exceptions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.SiteSettings;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;
using DarwinCMS.WebAdmin.Infrastructure.Security;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Controller for managing site settings in the admin panel.
/// </summary>
[Area("Admin")]
[HasPermission("manage_site_settings")]
public class SiteSettingsController : Controller
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="SiteSettingsController"/> class.
    /// </summary>
    public SiteSettingsController(
        ISiteSettingService siteSettingService,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _siteSettingService = siteSettingService;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Lists site settings with optional search, sorting, and pagination.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(
        string? searchTerm,
        string? sortColumn,
        string? sortDirection,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        const int PageSize = 10;
        int skip = (page - 1) * PageSize;

        var result = await _siteSettingService.GetPagedListAsync(
            searchTerm,
            sortColumn,
            sortDirection,
            skip,
            PageSize,
            cancellationToken);

        var viewModel = new SiteSettingIndexViewModel
        {
            SiteSettings = result.SiteSettings.Select(_mapper.Map<SiteSettingListViewModel>).ToList(),
            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)PageSize),
            CurrentPage = page,
            SearchTerm = searchTerm,
            SortColumn = sortColumn,
            SortDirection = sortDirection
        };

        return View(viewModel);
    }

    /// <summary>
    /// Shows the form to create a new setting.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View(new SiteSettingEditViewModel());
    }

    /// <summary>
    /// Processes creation of a new site setting.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SiteSettingEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = _mapper.Map<CreateSiteSettingRequest>(model);
        dto.CreatedByUserId = _currentUser.UserId ?? Guid.Empty;

        await _siteSettingService.CreateAsync(_mapper.Map<Domain.Entities.SiteSetting>(dto), cancellationToken);

        this.AddSuccess("Setting created successfully.");
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Shows the form to edit an existing site setting.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var setting = await _siteSettingService.GetAllAsync(cancellationToken)
            .ContinueWith(t => t.Result.FirstOrDefault(s => s.Id == id), cancellationToken);

        if (setting == null)
            return NotFound();

        var viewModel = _mapper.Map<SiteSettingEditViewModel>(setting);
        viewModel.OldKey = setting.Key;
        viewModel.OldLanguageCode = setting.LanguageCode;

        return View(viewModel);
    }

    /// <summary>
    /// Processes update of an existing site setting.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SiteSettingEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var dto = _mapper.Map<UpdateSiteSettingRequest>(model);
            dto.ModifiedByUserId = _currentUser.UserId ?? Guid.Empty;

            await _siteSettingService.UpdateValueAsync(
                model.OldKey,
                model.OldLanguageCode,
                model.Key,
                model.LanguageCode,
                dto.Value,
                dto.ModifiedByUserId,
                cancellationToken);

            this.AddSuccess("Setting updated successfully.");
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            this.AddError(ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// Shows a confirmation page before logically deleting a site setting.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var setting = await _siteSettingService.GetAllAsync(cancellationToken)
            .ContinueWith(t => t.Result.FirstOrDefault(s => s.Id == id), cancellationToken);

        if (setting == null)
            return NotFound();

        var viewModel = _mapper.Map<SiteSettingListViewModel>(setting);
        return View(viewModel);
    }

    /// <summary>
    /// Confirms logical deletion of the setting.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SoftDeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _siteSettingService.SoftDeleteAsync(id, _currentUser.UserId ?? Guid.Empty, cancellationToken);
            this.AddSuccess("Setting moved to recycle bin.");
        }
        catch (BusinessRuleException ex)
        {
            this.AddError(ex.Message);
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Shows deleted items (recycle bin).
    /// </summary>
    [HttpGet]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Deleted(CancellationToken cancellationToken)
    {
        var settings = await _siteSettingService.GetDeletedAsync(cancellationToken);
        var viewModel = settings.Select(_mapper.Map<SiteSettingListViewModel>).ToList();
        return View(viewModel);
    }

    /// <summary>
    /// Restores a logically deleted setting.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        await _siteSettingService.RestoreAsync(id, _currentUser.UserId ?? Guid.Empty, cancellationToken);
        this.AddSuccess("Setting restored successfully.");
        return RedirectToAction(nameof(Deleted));
    }

    /// <summary>
    /// Permanently deletes a site setting.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasPermission("recycle_bin_access")]
    public async Task<IActionResult> HardDelete(Guid id, CancellationToken cancellationToken)
    {
        await _siteSettingService.HardDeleteAsync(id, cancellationToken);
        this.AddSuccess("Setting permanently deleted.");
        return RedirectToAction(nameof(Deleted));
    }
}
