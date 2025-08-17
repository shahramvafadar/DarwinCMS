using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Services.Pages;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.Web.Controllers
{
    /// <summary>
    /// Generates a simple XML sitemap for published pages.
    /// </summary>
    public sealed class SitemapController : Controller
    {
        private readonly IPageQueryService _pages;

        /// <summary>
        /// Initializes a new instance of <see cref="SitemapController"/>.
        /// </summary>
        public SitemapController(IPageQueryService pages)
        {
            _pages = pages ?? throw new ArgumentNullException(nameof(pages));
        }

        /// <summary>
        /// Returns /sitemap.xml containing all visible pages across languages.
        /// </summary>
        [HttpGet("sitemap.xml")]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var items = await _pages.GetPublishedAsync(languageCode: null, ct);

            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

            var sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

            foreach (var it in items)
            {
                var loc = $"{baseUrl}/{it.LanguageCode}/{Uri.EscapeDataString(it.Slug)}";
                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(loc)}</loc>");
                if (it.LastModifiedUtc.HasValue)
                    sb.AppendLine($"    <lastmod>{it.LastModifiedUtc.Value.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}</lastmod>");
                sb.AppendLine("    <changefreq>weekly</changefreq>");
                sb.AppendLine("    <priority>0.6</priority>");
                sb.AppendLine("  </url>");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
