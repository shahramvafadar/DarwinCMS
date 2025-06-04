using System;
using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.Application.DTOs.SiteSettings;

/// <summary>
/// DTO used when updating an existing site setting.
/// Carries data from the admin UI to the service layer.
/// </summary>
public class UpdateSiteSettingRequest
{
    /// <summary>
    /// Unique identifier of the setting to be updated.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// New key of the setting.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Updated value of the setting.
    /// </summary>
    [Required]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Updated type of the value.
    /// </summary>
    [MaxLength(50)]
    public string ValueType { get; set; } = "string";

    /// <summary>
    /// Optional updated category.
    /// </summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>
    /// Optional updated language code.
    /// </summary>
    [MaxLength(10)]
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Optional updated description.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// User ID of the last modifier (for auditing).
    /// </summary>
    public Guid ModifiedByUserId { get; set; }
}
