using System;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Pages;
using DarwinCMS.Application.Services.Pages;
using DarwinCMS.Application.Services.Settings;
using DarwinCMS.Web.Infrastructure.Settings;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.Web.Controllers
{
    /// <summary>
    /// Public website home page.
    /// </summary>
    public sealed class HomeController : Controller
    {
        private readonly IPageQueryService _pages;
        private readonly ISiteSettingQueryService _settings;

        /// <summary>
        /// Initializes a new instance of <see cref="HomeController"/>.
        /// </summary>
        public HomeController(IPageQueryService pages, ISiteSettingQueryService settings)
        {
            _pages = pages ?? throw new ArgumentNullException(nameof(pages));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Renders the homepage using the configured slug and default language.
        /// Falls back to "home" (slug) and "de" (language) when settings are not present.
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var lang = (await _settings.GetValueAsync(SiteSettingKeys.SiteDefaultLanguage, ct)) ?? "de";
            var homeSlug = (await _settings.GetValueAsync(SiteSettingKeys.SiteHomeSlug, ct)) ?? "home";

            PagePublicDto? dto = await _pages.GetBySlugAsync(lang, homeSlug, ct);

            if (dto is null)
            {
                ViewData["Title"] = "Home";
                ViewData["Description"] = "Homepage is not configured yet.";
                return View(model: null);
            }

            ViewData["Title"] = string.IsNullOrWhiteSpace(dto.SeoTitle) ? dto.Title : dto.SeoTitle;
            ViewData["Description"] = dto.SeoDescription ?? string.Empty;
            return View(dto);
        }
    }
}
