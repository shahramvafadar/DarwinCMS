using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.Web.Controllers
{
    /// <summary>
    /// Handles HTTP status code re-execution (e.g., 404) to render friendly pages.
    /// </summary>
    public sealed class ErrorController : Controller
    {
        /// <summary>
        /// Renders a specific status code page (e.g., /error/404).
        /// </summary>
        [HttpGet("error/{code:int}")]
        public IActionResult Status(int code)
        {
            ViewData["StatusCode"] = code;
            return View(code == 404 ? "404" : "Error");
        }
    }
}
