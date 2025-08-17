using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Services.Pages;
using DarwinCMS.Application.Services.Seo;
using DarwinCMS.Application.Services.Settings;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.Web.Controllers
{
    /// <summary>
    /// Handles public content pages addressed by language prefix and slug.
    /// </summary>
    public sealed class PageController : Controller
    {
        private readonly IPageQueryService _pages;
        private readonly ISiteSettingQueryService _settings;
        private readonly IHreflangQueryService _hreflang;

        /// <summary>
        /// Initializes a new instance of <see cref="PageController"/>.
        /// </summary>
        public PageController(
            IPageQueryService pages,
            ISiteSettingQueryService settings,
            IHreflangQueryService hreflang)
        {
            _pages = pages ?? throw new ArgumentNullException(nameof(pages));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _hreflang = hreflang ?? throw new ArgumentNullException(nameof(hreflang));
        }

        /// <summary>
        /// Matches /{slug} without language. Redirects to the same page under the default site language.
        /// </summary>
        [HttpGet("{slug:length(1,200)}")]
        public async Task<IActionResult> DetailsNoLang(string slug, CancellationToken ct)
        {
            var lang = (await _settings.GetValueAsync("Site.DefaultLanguage", ct)) ?? "de";
            return RedirectToActionPermanent(nameof(Details), new { lang, slug });
        }

        /// <summary>
        /// Matches /{lang}/{slug}. Allowed languages are constrained by the regex.
        /// Returns 404 when the page is not found or not visible.
        /// Also sets canonical and hreflang data into ViewData for the SEO tag helper.
        /// </summary>
        [HttpGet("{lang:regex(^en|de|fa$)}/{slug:length(1,200)}")]
        public async Task<IActionResult> Details(string lang, string slug, CancellationToken ct)
        {
            var dto = await _pages.GetBySlugAsync(lang, slug, ct);
            if (dto is null || !dto.IsPublished) return NotFound();

            // Title/description for SEO
            ViewData["Title"] = string.IsNullOrWhiteSpace(dto.SeoTitle) ? dto.Title : dto.SeoTitle;
            ViewData["Description"] = dto.SeoDescription ?? string.Empty;

            // Canonical: absolute URL for the current page
            var canonical = Url.Action(nameof(Details), "Page", new { lang, slug }, Request.Scheme);
            ViewData["CanonicalUrl"] = canonical;

            // Hreflang alternates (if there are versions of the same slug in other languages)
            var alternates = await _hreflang.GetAlternatesBySlugAsync(slug, ct);
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var a in alternates)
            {
                map[a.LanguageCode] = $"{baseUrl}/{a.LanguageCode}/{Uri.EscapeDataString(a.Slug)}";
            }

            // Optionally include x-default pointing to the site's default language
            var defaultLang = (await _settings.GetValueAsync("Site.DefaultLanguage", ct)) ?? "de";
            if (!map.ContainsKey("x-default"))
                map["x-default"] = $"{baseUrl}/{defaultLang}/{Uri.EscapeDataString(slug)}";

            ViewData["Hreflang"] = map;

            return View(dto);
        }
    }
}
