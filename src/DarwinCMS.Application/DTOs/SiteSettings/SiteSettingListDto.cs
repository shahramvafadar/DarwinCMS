using System;

namespace DarwinCMS.Application.DTOs.SiteSettings;

/// <summary>
/// DTO for displaying site settings in the admin panel (list view).
/// Maps only required fields for performance and clarity.
/// </summary>
public class SiteSettingListDto
{
    /// <summary>
    /// Unique identifier of the setting.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Unique key of the setting (e.g., \"Site.Title\").
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Category for grouping (e.g., \"Branding\").
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Current value of the setting.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Optional language code (e.g., \"en\", \"de\").
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Indicates if this setting is protected and cannot be deleted.
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// Indicates if this setting is logically deleted (soft delete).
    /// </summary>
    public bool IsDeleted { get; set; }
}
