using System;

namespace DarwinCMS.Application.DTOs.Pages
{
    /// <summary>
    /// Public-facing projection for a CMS page that is safe to expose to the website.
    /// </summary>
    public sealed class PagePublicDto
    {
        /// <summary>Display title of the page.</summary>
        public string Title { get; set; } = default!;

        /// <summary>Unique slug used in public routing.</summary>
        public string Slug { get; set; } = default!;

        /// <summary>Optional short summary for previews/snippets.</summary>
        public string? Summary { get; set; }

        /// <summary>HTML content already sanitized/ready for rendering.</summary>
        public string ContentHtml { get; set; } = default!;

        /// <summary>Publication timestamp (UTC) for display and sitemaps.</summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>SEO title override; falls back to Title when null/empty.</summary>
        public string? SeoTitle { get; set; }

        /// <summary>SEO description for meta description tag.</summary>
        public string? SeoDescription { get; set; }

        /// <summary>Language code (e.g., "de", "en", "fa") for routing and hreflang.</summary>
        public string LanguageCode { get; set; } = "de";

        /// <summary>
        /// Indicates publication visibility to the public website; for query results this is always true.
        /// </summary>
        public bool IsPublished { get; set; } = true;
    }
}
