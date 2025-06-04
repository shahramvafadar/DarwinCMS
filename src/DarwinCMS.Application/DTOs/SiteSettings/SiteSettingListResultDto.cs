using System.Collections.Generic;

namespace DarwinCMS.Application.DTOs.SiteSettings;

/// <summary>
/// DTO used to return a paginated list of site settings.
/// </summary>
public class SiteSettingListResultDto
{
    /// <summary>
    /// Total number of items matching the search/filter.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// List of site settings for the current page.
    /// </summary>
    public List<SiteSettingListDto> SiteSettings { get; set; } = new();
}
