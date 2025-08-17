using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Pages;
using DarwinCMS.Application.Services.Pages;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Pages
{
    /// <summary>
    /// EF Core implementation of <see cref="IPageQueryService"/>.
    /// Uses language + slug and the publication window (PublishDateUtc/ExpireDateUtc) with IsPublished flag.
    /// </summary>
    public sealed class PageQueryService : IPageQueryService
    {
        private readonly DarwinDbContext _db;

        /// <summary>
        /// Initializes a new instance of <see cref="PageQueryService"/>.
        /// </summary>
        public PageQueryService(DarwinDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <inheritdoc/>
        public async Task<PagePublicDto?> GetBySlugAsync(string languageCode, string slug, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var page = await _db.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(p =>
                    p.LanguageCode == languageCode &&
                    p.SlugValue == slug &&
                    p.IsPublished &&
                    (p.PublishDateUtc == null || p.PublishDateUtc <= now) &&
                    (p.ExpireDateUtc == null || p.ExpireDateUtc > now),
                    ct);

            if (page is null) return null;

            return new PagePublicDto
            {
                Title = page.Title,
                Slug = page.SlugValue,
                Summary = page.Summary,
                ContentHtml = page.ContentHtml,
                PublishedAt = page.PublishDateUtc,
                SeoTitle = page.MetaTitle,
                SeoDescription = page.MetaDescription,
                LanguageCode = page.LanguageCode,
                IsPublished = true
            };
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<PageIndexItemDto>> GetPublishedAsync(string? languageCode, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var query = _db.Pages.AsNoTracking().Where(p =>
                p.IsPublished &&
                (p.PublishDateUtc == null || p.PublishDateUtc <= now) &&
                (p.ExpireDateUtc == null || p.ExpireDateUtc > now));

            if (!string.IsNullOrWhiteSpace(languageCode))
                query = query.Where(p => p.LanguageCode == languageCode);

            var items = await query
                .Select(p => new PageIndexItemDto
                {
                    LanguageCode = p.LanguageCode,
                    Slug = p.SlugValue,
                    LastModifiedUtc = p.ModifiedAt ?? p.PublishDateUtc ?? p.CreatedAt
                })
                .OrderBy(p => p.LanguageCode).ThenBy(p => p.Slug)
                .ToListAsync(ct);

            return items;
        }
    }
}
