using System;

namespace DarwinCMS.Application.DTOs.Pages
{
    /// <summary>
    /// Lightweight page projection for indexing scenarios (e.g., sitemaps).
    /// </summary>
    public sealed class PageIndexItemDto
    {
        /// <summary>Language code (e.g., "de").</summary>
        public string LanguageCode { get; set; } = "de";

        /// <summary>URL-facing slug.</summary>
        public string Slug { get; set; } = default!;

        /// <summary>Last modification date (UTC) used by search engines for change detection.</summary>
        public DateTime? LastModifiedUtc { get; set; }
    }
}
