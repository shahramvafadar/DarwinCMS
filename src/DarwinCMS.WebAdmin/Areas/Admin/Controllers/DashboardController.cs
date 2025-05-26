using Microsoft.AspNetCore.Mvc;
using DarwinCMS.WebAdmin.Infrastructure.Security;

namespace DarwinCMS.WebAdmin.Areas.Admin.Controllers;

/// <summary>
/// Handles the main admin dashboard view.
/// </summary>
[Area("Admin")]
[HasPermission("access_admin_panel")]
public class DashboardController : Controller
{
    /// <summary>
    /// Displays the admin dashboard overview.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }
}
