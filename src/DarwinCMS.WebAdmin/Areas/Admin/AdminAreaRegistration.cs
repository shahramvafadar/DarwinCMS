using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Areas.Admin;

/// <summary>
/// Marker attribute used to explicitly declare the "Admin" area for controllers.
/// This avoids repeating [Area("Admin")] in every controller manually.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AdminAreaRegistration : AreaAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdminAreaRegistration"/> class with "Admin" as the area name.
    /// </summary>
    public AdminAreaRegistration() : base("Admin") { }
}
