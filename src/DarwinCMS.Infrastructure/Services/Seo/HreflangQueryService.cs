using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Seo;
using DarwinCMS.Application.Services.Seo;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Seo
{
    /// <summary>
    /// EF Core implementation of <see cref="IHreflangQueryService"/>.
    /// Selects all published pages that share the same SlugValue across languages.
    /// </summary>
    public sealed class HreflangQueryService : IHreflangQueryService
    {
        private readonly DarwinDbContext _db;

        /// <summary>
        /// Initializes a new instance of <see cref="HreflangQueryService"/>.
        /// </summary>
        public HreflangQueryService(DarwinDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<HreflangAlternateDto>> GetAlternatesBySlugAsync(string slug, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var items = await _db.Pages
                .AsNoTracking()
                .Where(p =>
                    p.SlugValue == slug &&
                    p.IsPublished &&
                    (p.PublishDateUtc == null || p.PublishDateUtc <= now) &&
                    (p.ExpireDateUtc == null || p.ExpireDateUtc > now))
                .Select(p => new HreflangAlternateDto
                {
                    LanguageCode = p.LanguageCode,
                    Slug = p.SlugValue
                })
                .OrderBy(p => p.LanguageCode)
                .ToListAsync(ct);

            return items;
        }
    }
}
