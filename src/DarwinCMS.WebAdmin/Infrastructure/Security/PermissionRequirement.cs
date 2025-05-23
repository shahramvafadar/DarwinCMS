using Microsoft.AspNetCore.Authorization;

namespace DarwinCMS.WebAdmin.Infrastructure.Security;

/// <summary>
/// Represents a permission requirement used by ASP.NET Core's policy-based authorization system.
/// This model defines the name of a specific permission that must be granted to the current user.
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the name of the required permission (e.g. "user.manage", "content.edit").
    /// </summary>
    public string Permission { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionRequirement"/> class.
    /// </summary>
    /// <param name="permission">The permission string required for authorization.</param>
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}
