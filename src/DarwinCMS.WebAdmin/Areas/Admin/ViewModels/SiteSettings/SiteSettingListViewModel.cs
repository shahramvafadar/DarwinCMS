using System;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.SiteSettings;

/// <summary>
/// ViewModel for listing site settings in the admin panel.
/// </summary>
public class SiteSettingListViewModel
{
    /// <summary>
    /// The unique identifier of the setting.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique key of the setting.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The human-friendly category for grouping.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// The stored value of the setting.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Optional language code.
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Indicates whether this setting is protected.
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// Indicates whether this setting has been logically deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
