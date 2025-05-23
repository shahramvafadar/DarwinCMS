namespace DarwinCMS.Application.DTOs.Roles;

/// <summary>
/// Contains total count and current page of roles, typically used for paginated list results.
/// </summary>
public class RoleListResultDto
{
    /// <summary>
    /// Total number of roles matching the query (for pagination).
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// List of roles for the current page.
    /// </summary>
    public List<RoleDto> Roles { get; set; } = new();
}
