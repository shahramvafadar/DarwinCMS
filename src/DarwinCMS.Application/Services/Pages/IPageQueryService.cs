using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Pages;

namespace DarwinCMS.Application.Services.Pages
{
    /// <summary>
    /// Read-only query service for retrieving published pages for the public website.
    /// </summary>
    public interface IPageQueryService
    {
        /// <summary>
        /// Retrieves a published page by language and slug.
        /// Returns null when not found or not visible.
        /// </summary>
        Task<PagePublicDto?> GetBySlugAsync(string languageCode, string slug, CancellationToken ct);

        /// <summary>
        /// Returns all currently visible (published) pages.
        /// When languageCode is provided, the result is filtered by language; otherwise, it returns all languages.
        /// </summary>
        Task<IReadOnlyList<PageIndexItemDto>> GetPublishedAsync(string? languageCode, CancellationToken ct);
    }
}
