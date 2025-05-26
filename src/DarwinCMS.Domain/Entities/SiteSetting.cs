namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a configurable setting for the website.
/// Stored as a key-value pair with support for category, language, and caching.
/// </summary>
public class SiteSetting : BaseEntity
{
    /// <summary>
    /// Unique key identifying the setting (e.g. "Site.Title", "SEO.MetaDescription").
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Value of the setting as a plain string. Can be interpreted as JSON, HTML, etc.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Optional category for grouping related settings (e.g. Branding, SEO, Scripts).
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Language code (e.g. "en", "de", "fa") if this is a localized setting.
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Type of the value stored (e.g. string, boolean, json, html, image).
    /// Used for validation and rendering in admin UI.
    /// </summary>
    public string ValueType { get; set; } = "string";

    /// <summary>
    /// Optional description for display in the admin panel.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether this setting is protected and cannot be deleted.
    /// </summary>
    public bool IsSystem { get; set; } = false;
}
