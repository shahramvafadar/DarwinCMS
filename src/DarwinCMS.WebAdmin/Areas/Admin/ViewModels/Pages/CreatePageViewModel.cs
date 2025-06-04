using System.ComponentModel.DataAnnotations;

using DarwinCMS.Application.DTOs.Pages;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Pages;

/// <summary>
/// ViewModel used for creating a new CMS page from the admin panel.
/// This model maps to <see cref="CreatePageDto"/> and is bound to the form in Create.cshtml.
/// </summary>
public class CreatePageViewModel
{
    /// <summary>
    /// The language code for the page (e.g., "en", "fa").
    /// </summary>
    [Required]
    [Display(Name = "Language")]
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// The page title, used in navigation and headings.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The unique slug used for URL routing (e.g., "about-us").
    /// </summary>
    [Required]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "Slug must contain lowercase letters, numbers, or dashes.")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional short summary for SEO or previews.
    /// </summary>
    [Display(Name = "Summary")]
    [StringLength(300)]
    public string? Summary { get; set; }

    /// <summary>
    /// The main HTML content of the page.
    /// </summary>
    [Display(Name = "Content")]
    [DataType(DataType.MultilineText)]
    public string ContentHtml { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the page is published.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Optional publish date in UTC.
    /// </summary>
    [Display(Name = "Publish Date (UTC)")]
    public DateTime? PublishDateUtc { get; set; }

    /// <summary>
    /// Optional expiration date in UTC.
    /// </summary>
    [Display(Name = "Expire Date (UTC)")]
    public DateTime? ExpireDateUtc { get; set; }

    /// <summary>
    /// Optional cover image for social previews.
    /// </summary>
    [Display(Name = "Cover Image URL")]
    [Url]
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Optional category for filtering or grouping.
    /// </summary>
    [Display(Name = "Category / Tag")]
    public string? Category { get; set; }

    /// <summary>
    /// SEO meta title for search engines.
    /// </summary>
    [Display(Name = "Meta Title")]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO meta description for search result previews.
    /// </summary>
    [Display(Name = "Meta Description")]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Canonical URL to avoid duplicate indexing.
    /// </summary>
    [Display(Name = "Canonical URL")]
    public string? CanonicalUrl { get; set; }

    /// <summary>
    /// Open Graph title for social sharing.
    /// </summary>
    [Display(Name = "OG Title")]
    public string? OgTitle { get; set; }

    /// <summary>
    /// Open Graph image URL for social sharing.
    /// </summary>
    [Display(Name = "OG Image URL")]
    public string? OgImage { get; set; }

    /// <summary>
    /// Open Graph description for social previews.
    /// </summary>
    [Display(Name = "OG Description")]
    public string? OgDescription { get; set; }

    /// <summary>
    /// Structured data for SEO (as raw JSON-LD string).
    /// </summary>
    [Display(Name = "Structured Data (JSON-LD)")]
    [DataType(DataType.MultilineText)]
    public string? StructuredDataJsonLd { get; set; }
}
