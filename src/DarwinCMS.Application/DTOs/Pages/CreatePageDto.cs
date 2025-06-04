namespace DarwinCMS.Application.DTOs.Pages;

/// <summary>
/// Represents the input model for creating a new CMS page.
/// </summary>
public class CreatePageDto
{
    /// <summary>
    /// The language code of the page (e.g., "en", "fa", "de").
    /// </summary>
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Page title shown in navigation and metadata.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The unique URL-friendly slug (e.g., "about-us").
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional short summary used in SEO or previews.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// The main HTML content of the page.
    /// </summary>
    public string ContentHtml { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the page is published or draft.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Optional date/time when the page should be published.
    /// </summary>
    public DateTime? PublishDateUtc { get; set; }

    /// <summary>
    /// Optional date/time when the page should expire.
    /// </summary>
    public DateTime? ExpireDateUtc { get; set; }

    /// <summary>
    /// Cover image URL for previews or social sharing.
    /// </summary>
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Optional category or tag for filtering/grouping.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// SEO: Meta title for search engines.
    /// </summary>
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO: Meta description for search result previews.
    /// </summary>
    public string? MetaDescription { get; set; }

    /// <summary>
    /// SEO: Canonical URL to avoid duplicate content.
    /// </summary>
    public string? CanonicalUrl { get; set; }

    /// <summary>
    /// Open Graph: Social sharing title.
    /// </summary>
    public string? OgTitle { get; set; }

    /// <summary>
    /// Open Graph: Image shown in social previews.
    /// </summary>
    public string? OgImage { get; set; }

    /// <summary>
    /// Open Graph: Description shown on social platforms.
    /// </summary>
    public string? OgDescription { get; set; }

    /// <summary>
    /// JSON-LD structured data for SEO (as raw string).
    /// </summary>
    public string? StructuredDataJsonLd { get; set; }
}
