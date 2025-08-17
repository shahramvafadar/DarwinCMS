using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using DarwinCMS.Application.Services.Settings;

namespace DarwinCMS.Web.Infrastructure.Settings;

/// <summary>
/// Reads site settings using the existing ISiteSettingService and caches values in-memory per key+language.
/// </summary>
public sealed class SiteSettingsAccessor : ISiteSettingsAccessor
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Creates a new accessor that wraps ISiteSettingService and IMemoryCache for quick lookups.
    /// </summary>
    public SiteSettingsAccessor(ISiteSettingService siteSettingService, IMemoryCache cache)
    {
        _siteSettingService = siteSettingService;
        _cache = cache;
    }

    /// <inheritdoc />
    public async Task<string> GetStringAsync(string key, string? languageCode = null, string? fallback = null)
    {
        // Cache key includes language dimension because settings may be localized
        var cacheKey = $"settings:string:{key}:{languageCode ?? "_"}";

        if (_cache.TryGetValue(cacheKey, out string? cached))
            return cached ?? fallback ?? string.Empty;

        var value = await _siteSettingService.GetValueAsync(key, languageCode);
        var result = value ?? fallback ?? string.Empty;

        // Small default cache for settings to reduce DB hits; 60s is reasonable
        _cache.Set(cacheKey, result, TimeSpan.FromSeconds(60));
        return result;
    }

    /// <inheritdoc />
    public async Task<int> GetIntAsync(string key, string? languageCode = null, int fallback = 0)
    {
        var raw = await GetStringAsync(key, languageCode, null);
        return int.TryParse(raw, out var number) ? number : fallback;
    }
}
