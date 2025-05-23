using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.Web.Areas.Admin.Controllers;

/// <summary>
/// Handles the main admin dashboard view.
/// </summary>
[Area("Admin")]
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
