using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Seo;

namespace DarwinCMS.Application.Services.Seo
{
    /// <summary>
    /// Read-only service that provides alternate-language relations for hreflang rendering.
    /// </summary>
    public interface IHreflangQueryService
    {
        /// <summary>
        /// Returns all currently visible alternates that share the same slug across languages.
        /// If your content uses different slugs per language, this will naturally return fewer items.
        /// </summary>
        Task<IReadOnlyList<HreflangAlternateDto>> GetAlternatesBySlugAsync(string slug, CancellationToken ct);
    }
}
