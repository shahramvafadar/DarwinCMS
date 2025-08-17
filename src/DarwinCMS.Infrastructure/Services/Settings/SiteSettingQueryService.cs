using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Services.Settings;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Settings
{
    /// <summary>
    /// EF Core implementation of <see cref="ISiteSettingQueryService"/>.
    /// Uses direct DbContext queries; caching can be added later if required.
    /// </summary>
    public sealed class SiteSettingQueryService : ISiteSettingQueryService
    {
        private readonly DarwinDbContext _db;

        /// <summary>
        /// Initializes a new instance of <see cref="SiteSettingQueryService"/>.
        /// </summary>
        public SiteSettingQueryService(DarwinDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <inheritdoc/>
        public async Task<string?> GetValueAsync(string key, CancellationToken ct)
        {
            var setting = await _db.SiteSettings
                .AsNoTracking()
                .Where(s => s.Key == key && !s.IsDeleted)
                .OrderByDescending(s => s.ModifiedAt ?? s.CreatedAt)
                .FirstOrDefaultAsync(ct);

            return setting?.Value;
        }

        /// <inheritdoc/>
        public async Task<int> GetIntValueAsync(string key, int defaultValue, CancellationToken ct)
        {
            var raw = await GetValueAsync(key, ct);
            if (string.IsNullOrWhiteSpace(raw)) return defaultValue;

            // Allow both invariant and local integer formats
            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var val))
                return val;

            if (int.TryParse(raw, out val))
                return val;

            return defaultValue;
        }
    }
}
