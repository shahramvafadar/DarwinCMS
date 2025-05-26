using System;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a CMS page that contains structured content for public display.
/// </summary>
public class Page : BaseEntity
{
    /// <summary>
    /// The language code (e.g. en, de, fa) of this version of the page.
    /// </summary>
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Title of the page for display and SEO.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Short summary or description of the page.
    /// Useful for meta tags or previews.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// The unique slug used for the URL (e.g. "/about-us").
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// The main content body of the page (HTML formatted).
    /// </summary>
    public string ContentHtml { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the page is currently published.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Optional date and time when the page should become visible.
    /// </summary>
    public DateTime? PublishDateUtc { get; set; }

    /// <summary>
    /// Optional expiration date after which the page is no longer active.
    /// </summary>
    public DateTime? ExpireDateUtc { get; set; }

    /// <summary>
    /// SEO: Meta title for the page.
    /// </summary>
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO: Meta description for search engines.
    /// </summary>
    public string? MetaDescription { get; set; }

    /// <summary>
    /// SEO: Canonical URL to prevent duplicate indexing.
    /// </summary>
    public string? CanonicalUrl { get; set; }

    /// <summary>
    /// SEO: Open Graph title for social sharing.
    /// </summary>
    public string? OgTitle { get; set; }

    /// <summary>
    /// SEO: Open Graph image (URL or file ref).
    /// </summary>
    public string? OgImage { get; set; }

    /// <summary>
    /// SEO: Open Graph description.
    /// </summary>
    public string? OgDescription { get; set; }

    /// <summary>
    /// JSON-LD structured data for advanced SEO.
    /// </summary>
    public string? StructuredDataJsonLd { get; set; }

    /// <summary>
    /// URL of the cover image or thumbnail used in previews.
    /// </summary>
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Optional category or tag for grouping pages.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Indicates if this page is a system page (cannot be deleted).
    /// </summary>
    public bool IsSystem { get; set; } = false;

    /// <summary>
    /// Optional reference to the parent page (for hierarchy).
    /// </summary>
    public Guid? ParentPageId { get; set; }

    /// <summary>
    /// Optional display order when rendering in lists or menus.
    /// </summary>
    public int DisplayOrder { get; set; } = 0;
}
