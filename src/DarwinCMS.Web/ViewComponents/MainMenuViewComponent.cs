using System;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.Application.Services.Settings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace DarwinCMS.Web.ViewComponents
{
    /// <summary>
    /// Renders the main navigation menu using a short-lived in-memory cache (TTL from SiteSettings).
    /// </summary>
    public sealed class MainMenuViewComponent : ViewComponent
    {
        private readonly IMenuQueryService _menuQuery;
        private readonly ISiteSettingQueryService _settings;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of <see cref="MainMenuViewComponent"/>.
        /// </summary>
        public MainMenuViewComponent(
            IMenuQueryService menuQuery,
            ISiteSettingQueryService settings,
            IMemoryCache cache)
        {
            _menuQuery = menuQuery ?? throw new ArgumentNullException(nameof(menuQuery));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Invokes the menu rendering for the given position ("header" or "footer").
        /// Language is resolved from route values; defaults to "de".
        /// </summary>
        public async Task<IViewComponentResult> InvokeAsync(string position = "header", CancellationToken ct = default)
        {
            var lang = (RouteData.Values.TryGetValue("lang", out var l) ? l?.ToString() : null) ?? "de";
            var cacheKey = $"menu:{position}:{lang}";
            var ttl = await _settings.GetValueAsync("Navigation.MenuCacheTtlSeconds", ct);
            var ttlSeconds = 0;
            _ = int.TryParse(ttl, out ttlSeconds);
            if (ttlSeconds <= 0) ttlSeconds = 60;

            var menu = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttlSeconds);
                return await _menuQuery.GetTreeAsync(position, lang, ct);
            });

            return View(menu ?? new MenuNodeDto { Title = "Menu" });
        }
    }
}
