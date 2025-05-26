namespace DarwinCMS.Application.DTOs.Permissions;

/// <summary>
/// Represents the data required to create a new permission.
/// </summary>
public class CreatePermissionRequest
{
    /// <summary>
    /// Internal name of the permission (must be unique).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display name shown to users in the admin panel.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
}
