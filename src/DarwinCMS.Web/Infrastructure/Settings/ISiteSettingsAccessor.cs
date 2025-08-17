using System.Threading.Tasks;

namespace DarwinCMS.Web.Infrastructure.Settings;

/// <summary>
/// Abstraction to read site settings with optional local caching and type conversion.
/// </summary>
public interface ISiteSettingsAccessor
{
    /// <summary>
    /// Gets a setting as a string. Returns <paramref name="fallback"/> if missing.
    /// </summary>
    Task<string> GetStringAsync(string key, string? languageCode = null, string? fallback = null);

    /// <summary>
    /// Gets a setting as an integer (e.g., cache TTL). Returns <paramref name="fallback"/> if missing or invalid.
    /// </summary>
    Task<int> GetIntAsync(string key, string? languageCode = null, int fallback = 0);
}
