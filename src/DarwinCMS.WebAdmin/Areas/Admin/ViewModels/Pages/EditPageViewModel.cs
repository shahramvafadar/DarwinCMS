using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Pages;

/// <summary>
/// ViewModel used for editing an existing CMS page in the admin panel.
/// </summary>
public class EditPageViewModel
{
    /// <summary>
    /// Unique identifier of the page.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Language code for the page (e.g., "en", "de").
    /// </summary>
    [Required]
    [Display(Name = "Language")]
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// The page title.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Slug used for page URL.
    /// </summary>
    [Required]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "Slug must contain lowercase letters, numbers, or dashes.")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional summary for SEO and preview.
    /// </summary>
    [Display(Name = "Summary")]
    [StringLength(300)]
    public string? Summary { get; set; }

    /// <summary>
    /// Main HTML content of the page.
    /// </summary>
    [Display(Name = "Content")]
    [DataType(DataType.MultilineText)]
    public string ContentHtml { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the page is published.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Optional UTC date when the page should be published.
    /// </summary>
    [Display(Name = "Publish Date (UTC)")]
    public DateTime? PublishDateUtc { get; set; }

    /// <summary>
    /// Optional UTC date when the page should expire.
    /// </summary>
    [Display(Name = "Expire Date (UTC)")]
    public DateTime? ExpireDateUtc { get; set; }

    /// <summary>
    /// Cover image URL used in previews and social sharing.
    /// </summary>
    [Display(Name = "Cover Image URL")]
    [Url]
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Optional category or tag for the page.
    /// </summary>
    [Display(Name = "Category / Tag")]
    public string? Category { get; set; }

    /// <summary>
    /// SEO: Meta title.
    /// </summary>
    [Display(Name = "Meta Title")]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO: Meta description.
    /// </summary>
    [Display(Name = "Meta Description")]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// SEO: Canonical URL.
    /// </summary>
    [Display(Name = "Canonical URL")]
    public string? CanonicalUrl { get; set; }

    /// <summary>
    /// Open Graph: Title.
    /// </summary>
    [Display(Name = "OG Title")]
    public string? OgTitle { get; set; }

    /// <summary>
    /// Open Graph: Image URL.
    /// </summary>
    [Display(Name = "OG Image URL")]
    public string? OgImage { get; set; }

    /// <summary>
    /// Open Graph: Description.
    /// </summary>
    [Display(Name = "OG Description")]
    public string? OgDescription { get; set; }

    /// <summary>
    /// Structured data in JSON-LD format.
    /// </summary>
    [Display(Name = "Structured Data (JSON-LD)")]
    [DataType(DataType.MultilineText)]
    public string? StructuredDataJsonLd { get; set; }
}
