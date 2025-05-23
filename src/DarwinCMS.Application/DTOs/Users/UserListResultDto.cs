namespace DarwinCMS.Application.DTOs.Users;

/// <summary>
/// Contains a paginated result of users with metadata.
/// </summary>
public class UserListResultDto
{
    /// <summary>
    /// Total count of users (before pagination).
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Paged list of users.
    /// </summary>
    public List<UserListDto> Users { get; set; } = new();
}
