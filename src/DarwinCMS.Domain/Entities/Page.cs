using System;

using DarwinCMS.Domain.Interfaces;
using DarwinCMS.Domain.ValueObjects;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a CMS page that contains structured content for public display.
/// Includes SEO fields, localization, and support for soft deletion.
/// </summary>
public class Page : BaseEntity
{
    /// <summary>
    /// The language code (e.g., "en", "de", "fa") of this version of the page.
    /// </summary>
    public string LanguageCode { get; private set; } = "en";

    /// <summary>
    /// Title of the page for display and SEO.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Short summary or description of the page.
    /// Useful for meta tags or previews.
    /// </summary>
    public string? Summary { get; private set; }

    /// <summary>
    /// The Slug value object used for SEO-friendly URLs.
    /// EF Core will store only SlugValue.
    /// </summary>
    public Slug Slug { get; private set; } = Slug.CreatePlaceholder();

    /// <summary>
    /// The actual Slug string stored in the database.
    /// </summary>
    public string SlugValue
    {
        get => Slug.ToString();
        private set => Slug = new Slug(value);
    }

    /// <summary>
    /// The main content body of the page (HTML formatted).
    /// </summary>
    public string ContentHtml { get; private set; } = string.Empty;

    /// <summary>
    /// Indicates whether the page is currently published.
    /// </summary>
    public bool IsPublished { get; private set; }

    /// <summary>
    /// Optional date and time when the page should become visible.
    /// </summary>
    public DateTime? PublishDateUtc { get; private set; }

    /// <summary>
    /// Optional expiration date after which the page is no longer active.
    /// </summary>
    public DateTime? ExpireDateUtc { get; private set; }

    /// <summary>
    /// SEO: Meta title for the page.
    /// </summary>
    public string? MetaTitle { get; private set; }

    /// <summary>
    /// SEO: Meta description for search engines.
    /// </summary>
    public string? MetaDescription { get; private set; }

    /// <summary>
    /// SEO: Canonical URL to prevent duplicate indexing.
    /// </summary>
    public string? CanonicalUrl { get; private set; }

    /// <summary>
    /// SEO: Open Graph title for social sharing.
    /// </summary>
    public string? OgTitle { get; private set; }

    /// <summary>
    /// SEO: Open Graph image (URL or file ref).
    /// </summary>
    public string? OgImage { get; private set; }

    /// <summary>
    /// SEO: Open Graph description.
    /// </summary>
    public string? OgDescription { get; private set; }

    /// <summary>
    /// JSON-LD structured data for advanced SEO.
    /// </summary>
    public string? StructuredDataJsonLd { get; private set; }

    /// <summary>
    /// URL of the cover image or thumbnail used in previews.
    /// </summary>
    public string? CoverImageUrl { get; private set; }

    /// <summary>
    /// Optional category or tag for grouping pages.
    /// </summary>
    public string? Category { get; private set; }

    /// <summary>
    /// Indicates if this page is a system page (cannot be deleted).
    /// </summary>
    public bool IsSystem { get; private set; } = false;

    /// <summary>
    /// Optional reference to the parent page (for hierarchy).
    /// </summary>
    public Guid? ParentPageId { get; private set; }

    /// <summary>
    /// Optional display order when rendering in lists or menus.
    /// </summary>
    public int DisplayOrder { get; private set; } = 0;

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected Page() { }

    /// <summary>
    /// Initializes a new Page entity with required fields.
    /// </summary>
    public Page(string title, Slug slug, string languageCode, string contentHtml, bool isPublished, Guid createdByUserId)
    {
        SetTitle(title, createdByUserId);
        SetSlug(slug, createdByUserId);
        SetLanguage(languageCode, createdByUserId);
        SetContent(contentHtml, createdByUserId);
        IsPublished = isPublished;
        CreatedByUserId = createdByUserId;
    }

    /// <summary>
    /// Updates the title of the page.
    /// </summary>
    public void SetTitle(string title, Guid? modifierId)
    {
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title is required.", nameof(title)) : title.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Updates the slug of the page.
    /// </summary>
    public void SetSlug(Slug slug, Guid? modifierId)
    {
        Slug = slug ?? throw new ArgumentNullException(nameof(slug));
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the language code (e.g., "en", "fa").
    /// </summary>
    public void SetLanguage(string code, Guid? modifierId)
    {
        LanguageCode = string.IsNullOrWhiteSpace(code) ? "en" : code.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Updates the main content of the page.
    /// </summary>
    public void SetContent(string html, Guid? modifierId)
    {
        ContentHtml = html ?? string.Empty;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Updates the summary of the page.
    /// </summary>
    public void SetSummary(string? summary, Guid? modifierId)
    {
        Summary = summary?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the publication status and dates.
    /// </summary>
    /// <param name="isPublished">Whether the page is published.</param>
    /// <param name="publishDateUtc">Optional publish date.</param>
    /// <param name="expireDateUtc">Optional expiration date.</param>
    /// <param name="modifierId">The user who made the change.</param>
    public void SetPublication(bool isPublished, DateTime? publishDateUtc = null, DateTime? expireDateUtc = null, Guid? modifierId = null)
    {
        IsPublished = isPublished;
        PublishDateUtc = publishDateUtc;
        ExpireDateUtc = expireDateUtc;
        MarkAsModified(modifierId);
    }


    /// <summary>
    /// Sets SEO-related fields for better search engine indexing.
    /// </summary>
    public void SetSeoFields(string? metaTitle, string? metaDescription, string? canonicalUrl, Guid? modifierId)
    {
        MetaTitle = metaTitle?.Trim();
        MetaDescription = metaDescription?.Trim();
        CanonicalUrl = canonicalUrl?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets Open Graph fields for social media sharing.
    /// </summary>
    public void SetOpenGraphFields(string? ogTitle, string? ogDescription, string? ogImage, Guid? modifierId)
    {
        OgTitle = ogTitle?.Trim();
        OgDescription = ogDescription?.Trim();
        OgImage = ogImage?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets structured data for advanced SEO (JSON-LD).
    /// </summary>
    public void SetStructuredData(string? jsonLd, Guid? modifierId)
    {
        StructuredDataJsonLd = jsonLd?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets cover image URL.
    /// </summary>
    public void SetCoverImage(string? url, Guid? modifierId)
    {
        CoverImageUrl = url?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets category for the page.
    /// </summary>
    public void SetCategory(string? category, Guid? modifierId)
    {
        Category = category?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Marks this page as a system-protected item.
    /// </summary>
    public void MarkAsSystem(Guid? modifierId)
    {
        IsSystem = true;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Marks this page as logically deleted (soft delete).
    /// </summary>
    public void MarkAsDeleted(Guid? modifierId)
    {
        IsDeleted = true;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Restores a previously soft-deleted page.
    /// </summary>
    public void Restore(Guid? modifierId)
    {
        IsDeleted = false;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the last modifying user's ID.
    /// </summary>
    public void SetModifiedBy(Guid modifierId)
    {
        ModifiedByUserId = modifierId;
        MarkAsModified(modifierId);
    }

}
