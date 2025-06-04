using System;
using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.Application.DTOs.SiteSettings;

/// <summary>
/// DTO used when creating a new site setting.
/// Carries data from the admin UI to the service layer.
/// </summary>
public class CreateSiteSettingRequest
{
    /// <summary>
    /// Unique key of the setting (e.g., \"Site.Title\").
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Value of the setting.
    /// </summary>
    [Required]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Type of the value (e.g., \"string\", \"json\").
    /// </summary>
    [MaxLength(50)]
    public string ValueType { get; set; } = "string";

    /// <summary>
    /// Optional category for grouping (e.g., \"Branding\").
    /// </summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>
    /// Optional language code (e.g., \"en\", \"de\").
    /// </summary>
    [MaxLength(10)]
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Optional description for display in the admin panel.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this setting is protected and cannot be deleted.
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// User ID of the creator (for auditing).
    /// </summary>
    public Guid CreatedByUserId { get; set; }
}
