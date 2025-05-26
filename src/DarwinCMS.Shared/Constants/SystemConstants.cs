namespace DarwinCMS.Shared.Constants;

/// <summary>
/// Contains global system constants used across the application.
/// </summary>
public static class SystemConstants
{
    /// <summary>
    /// Represents the ID of the internal system actor used for seeding, migrations, or automated actions.
    /// This ID is reserved and must never be assigned to a real user.
    /// </summary>
    public static readonly Guid SystemUserId = new("00000000-0000-0000-0000-000000000001");

    /// <summary>
    /// System-level permission that allows unrestricted access to all admin operations.
    /// Roles with this permission bypass individual permission checks.
    /// </summary>
    public const string FullAdminAccessPermission = "full_admin_access";
}
