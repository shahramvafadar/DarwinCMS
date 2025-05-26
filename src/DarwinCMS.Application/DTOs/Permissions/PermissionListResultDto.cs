using System.Collections.Generic;

namespace DarwinCMS.Application.DTOs.Permissions;

/// <summary>
/// Represents the paginated result of permission records.
/// </summary>
public class PermissionListResultDto
{
    /// <summary>
    /// Total number of permissions matching the filter.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// List of permissions for the current page.
    /// </summary>
    public List<PermissionListDto> Permissions { get; set; } = new();
}
