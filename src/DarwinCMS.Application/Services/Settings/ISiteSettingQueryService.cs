using System.Threading;
using System.Threading.Tasks;

namespace DarwinCMS.Application.Services.Settings
{
    /// <summary>
    /// Read-only access to site settings (key-value) for the public website.
    /// </summary>
    public interface ISiteSettingQueryService
    {
        /// <summary>
        /// Gets the raw string value of a setting by key (e.g., "Site.Title").
        /// Returns null when the key is not found.
        /// </summary>
        Task<string?> GetValueAsync(string key, CancellationToken ct);

        /// <summary>
        /// Gets an integer value by key. Returns defaultValue when missing or invalid.
        /// </summary>
        Task<int> GetIntValueAsync(string key, int defaultValue, CancellationToken ct);
    }
}
