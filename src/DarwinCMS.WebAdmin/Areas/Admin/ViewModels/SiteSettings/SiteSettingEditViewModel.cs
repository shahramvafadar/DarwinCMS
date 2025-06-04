using System;
using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.SiteSettings;

/// <summary>
/// ViewModel for creating or editing a site setting in the admin panel.
/// Includes all fields used for display and validation.
/// </summary>
public class SiteSettingEditViewModel
{
    /// <summary>
    /// Unique identifier of the setting.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Unique key for the setting (e.g., "Site.Title").
    /// </summary>
    [Required]
    [MaxLength(200)]
    [Display(Name = "Key")]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The current value of the setting.
    /// </summary>
    [Required]
    [Display(Name = "Value")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Data type of the setting (e.g., string, html, json).
    /// </summary>
    [MaxLength(50)]
    [Display(Name = "Value Type")]
    public string ValueType { get; set; } = "string";

    /// <summary>
    /// Optional category for organizing settings.
    /// </summary>
    [MaxLength(100)]
    [Display(Name = "Category")]
    public string? Category { get; set; }

    /// <summary>
    /// Optional language code for localized settings.
    /// </summary>
    [MaxLength(10)]
    [Display(Name = "Language Code")]
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Optional description for display in the admin panel.
    /// </summary>
    [MaxLength(500)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether this setting is system-level (protected).
    /// </summary>
    [Display(Name = "Is System")]
    public bool IsSystem { get; set; }

    /// <summary>
    /// Old key used for composite key lookup during update (hidden field in form).
    /// </summary>
    public string OldKey { get; set; } = string.Empty;

    /// <summary>
    /// Old language code used for composite key lookup during update (hidden field in form).
    /// </summary>
    public string? OldLanguageCode { get; set; }
}
