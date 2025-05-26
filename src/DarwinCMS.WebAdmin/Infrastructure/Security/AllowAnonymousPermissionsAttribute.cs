namespace DarwinCMS.WebAdmin.Infrastructure.Security;

/// <summary>
/// Indicates that the controller or action should skip AdminAccessEnforcerFilter permission checks.
/// Typically used for login, logout, reset password, etc.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AllowAnonymousPermissionsAttribute : Attribute
{
}
