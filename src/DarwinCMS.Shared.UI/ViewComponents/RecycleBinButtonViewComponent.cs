using Microsoft.AspNetCore.Mvc;
using DarwinCMS.Application.Services.AccessControl; // Interface for permission checks

namespace DarwinCMS.Shared.UI.ViewComponents
{
    /// <summary>
    /// ViewComponent to render a "Recycle Bin" button if the current user has the appropriate permissions.
    /// This component ensures consistent styling and centralized permission logic.
    /// </summary>
    public class RecycleBinButtonViewComponent : ViewComponent
    {
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="currentUserService">Provides information about the current user and their permissions.</param>
        public RecycleBinButtonViewComponent(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Invokes the ViewComponent and decides whether to render the button based on permissions.
        /// </summary>
        /// <returns>The button HTML if authorized; otherwise, an empty result.</returns>
        public IViewComponentResult Invoke()
        {
            // Check if the user has the "recycle_bin_access" or "full_admin_access" permission.
            if (_currentUserService.HasPermission("recycle_bin_access") || _currentUserService.HasPermission("full_admin_access"))
            {
                // Return the View to render the button.
                return View();
            }

            // If the user lacks permissions, return an empty result (no button).
            return Content(string.Empty);
        }
    }
}
