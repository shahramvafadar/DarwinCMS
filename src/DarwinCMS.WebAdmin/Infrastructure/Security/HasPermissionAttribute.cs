using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DarwinCMS.WebAdmin.Infrastructure.Security;

/// <summary>
/// Attribute to enforce permission-based access control.
/// Internally maps to HasPermissionFilter to perform real-time permission evaluation.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Constructs the permission-based filter wrapper using the given permission key.
    /// </summary>
    /// <param name="permission">The technical permission name (e.g. "manage_users")</param>
    public HasPermissionAttribute(string permission)
        : base(typeof(HasPermissionFilter))
    {
        Arguments = new object[] { permission };
    }
}
