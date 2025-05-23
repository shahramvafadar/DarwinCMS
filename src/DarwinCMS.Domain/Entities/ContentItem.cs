using System.Text.Json;

using DarwinCMS.Domain.Enums;
using DarwinCMS.Domain.Interfaces;
using DarwinCMS.Domain.ValueObjects;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents any type of content in the CMS, such as articles, pages, products, etc.
/// Designed to be flexible, multilingual, and SEO-aware.
/// </summary>
public class ContentItem : BaseEntity, IAuditableEntity
{
    /// <summary>
    /// Title of the content.
    /// Displayed in lists and detail pages.
    /// Example: "How to Use Darwin CMS"
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Short summary or excerpt of the content.
    /// Used in content listings and SEO meta description.
    /// Example: "This article explains how to get started with Darwin CMS..."
    /// </summary>
    public string Summary { get; private set; } = string.Empty;

    /// <summary>
    /// The full body content in Markdown or HTML.
    /// Rendered in the content detail view.
    /// </summary>
    public string Body { get; private set; } = string.Empty;

    /// <summary>
    /// Unique slug used in the URL.
    /// Must be unique per language.
    /// Example: "getting-started", "products/darwin-cms"
    /// Now enforced via the Slug Value Object.
    /// </summary>
    public Slug Slug { get; private set; } = Slug.CreatePlaceholder();


    /// <summary>
    /// Content type identifier (e.g., Page, Post, Product).
    /// Helps group and route different content modules.
    /// Example: "Page", "Blog", "Product"
    /// </summary>
    public string ContentType { get; private set; } = string.Empty;

    /// <summary>
    /// List of tags associated with the content.
    /// Used for filtering, search, and categorization.
    /// Internally stored as normalized, lowercase, comma-separated string.
    /// Example: "cms,dotnet,content"
    /// Initialized with an empty tag list by default to satisfy nullable rules.
    /// </summary>
    public TagList Tags { get; private set; } = new();

    /// <summary>
    /// Two-letter language code.
    /// Used for multilingual UI and URL routing.
    /// Example: "en", "de", "fa"
    /// </summary>
    public string LanguageCode { get; private set; } = "en";

    /// <summary>
    /// The UTC time when the content was published.
    /// May be null if content is still in draft mode.
    /// </summary>
    public DateTime? PublishedAt { get; private set; }

    /// <summary>
    /// ID of the user who originally created the content.
    /// Used for auditing and author attribution.
    /// </summary>
    public Guid CreatedByUserId { get; private set; }

    /// <summary>
    /// ID of the user who last modified the content.
    /// Null if never modified after creation.
    /// </summary>
    public Guid? ModifiedByUserId { get; private set; }

    /// <summary>
    /// Optional metadata stored as JSON string.
    /// Can include SEO info, OpenGraph, display settings, etc.
    /// Example: {"seoTitle":"...", "thumbnailUrl":"..."}
    /// </summary>
    public string MetadataJson { get; private set; } = "{}";

    /// <summary>
    /// Current status of the content item (Draft, Published, etc).
    /// Replaces the old IsPublished boolean for better extensibility.
    /// </summary>
    public ContentItemStatus Status { get; private set; } = ContentItemStatus.Draft;

    /// <summary>
    /// Returns true if the content is marked as Published.
    /// </summary>
    public bool IsPublished => Status == ContentItemStatus.Published;

    /// <summary>
    /// Constructor required for EF Core.
    /// </summary>
    protected ContentItem()
    {
        // For EF Core only — fields already initialized
    }

    /// <summary>
    /// Creates a new content item with all essential fields.
    /// </summary>
    public ContentItem(
        string title,
        Slug slug,
        string body,
        string contentType,
        Guid createdByUserId,
        string languageCode = "en",
        string summary = "",
        TagList? tags = null,
        string? metadataJson = null)
    {
        SetTitle(title);
        SetSlug(slug);
        SetBody(body);
        SetContentType(contentType);
        SetLanguage(languageCode);
        SetSummary(summary);
        SetTags(tags ?? new TagList());
        SetMetadataJson(metadataJson);
        CreatedByUserId = createdByUserId;
    }

    /// <summary>
    /// Sets the title of the content item.
    /// Throws if empty.
    /// </summary>
    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        Title = title.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Sets the slug using a validated Slug value object.
    /// </summary>
    public void SetSlug(Slug slug)
    {
        Slug = slug;
        MarkAsModified();
    }

    /// <summary>
    /// Sets the body content (Markdown or HTML).
    /// Null values are converted to empty.
    /// </summary>
    public void SetBody(string body)
    {
        Body = body?.Trim() ?? string.Empty;
        MarkAsModified();
    }

    /// <summary>
    /// Sets the content type (e.g., Page, Post).
    /// Throws if empty.
    /// </summary>
    public void SetContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Content type is required.", nameof(contentType));

        ContentType = contentType.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Sets the language code for the content.
    /// Defaults to "en" if null or empty.
    /// </summary>
    public void SetLanguage(string code)
    {
        LanguageCode = string.IsNullOrWhiteSpace(code) ? "en" : code.Trim().ToLowerInvariant();
        MarkAsModified();
    }

    /// <summary>
    /// Sets the content summary.
    /// </summary>
    public void SetSummary(string summary)
    {
        Summary = summary?.Trim() ?? string.Empty;
        MarkAsModified();
    }

    /// <summary>
    /// Sets the list of tags for the content.
    /// </summary>
    public void SetTags(TagList tagList)
    {
        Tags = tagList;
        MarkAsModified();
    }

    /// <summary>
    /// Sets the metadata JSON string.
    /// Throws if invalid JSON.
    /// </summary>
    public void SetMetadataJson(string? json)
    {
        MetadataJson = IsValidJson(json) ? json ?? "{}" : throw new ArgumentException("Metadata is not a valid JSON string.");
        MarkAsModified();
    }

    /// <summary>
    /// Publishes the content and sets the PublishedAt timestamp.
    /// </summary>
    public void Publish()
    {
        Status = ContentItemStatus.Published;
        PublishedAt ??= DateTime.UtcNow;
        MarkAsModified();
    }

    /// <summary>
    /// Marks the content as archived (not visible).
    /// </summary>
    public void Archive()
    {
        Status = ContentItemStatus.Archived;
        MarkAsModified();
    }

    /// <summary>
    /// Schedules the content for future publishing.
    /// </summary>
    public void Schedule()
    {
        Status = ContentItemStatus.Scheduled;
        MarkAsModified();
    }

    /// <summary>
    /// Reverts the content to draft status.
    /// </summary>
    public void MarkAsDraft()
    {
        Status = ContentItemStatus.Draft;
        MarkAsModified();
    }

    /// <summary>
    /// Sets the ID of the user who last modified this item.
    /// </summary>
    public void SetModifiedBy(Guid userId)
    {
        ModifiedByUserId = userId;
        MarkAsModified();
    }

    /// <summary>
    /// Validates whether the given string is valid JSON.
    /// </summary>
    private static bool IsValidJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return true;
        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Returns a readable summary for debugging.
    /// </summary>
    public override string ToString() =>
        $"[{LanguageCode}] {ContentType}: {Title} (Status: {Status})";
}
