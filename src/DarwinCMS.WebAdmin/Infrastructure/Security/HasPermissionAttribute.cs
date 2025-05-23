using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DarwinCMS.WebAdmin.Infrastructure.Security;

/// <summary>
/// Attribute to enforce permission-based access control.
/// Internally translates into an ASP.NET Core policy for cleaner syntax and reusability.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Constructs the permission-based filter.
    /// This will trigger policy evaluation using ASP.NET Core's authorization system.
    /// </summary>
    /// <param name="permission">The technical permission name (e.g. "user.manage")</param>
    public HasPermissionAttribute(string permission)
        : base(typeof(HasPermissionFilter))
    {
        Arguments = new object[] { permission };
    }
}



