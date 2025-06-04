using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Threading.Tasks;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Shared;

namespace DarwinCMS.WebAdmin.Infrastructure.Middleware;

/// <summary>
/// Middleware to ensure ViewData["Messages"] is always initialized for Razor Views.
/// Prevents null reference errors when rendering the _UiMessage partial in layouts or views.
/// </summary>
public class ViewDataInitializerMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes the middleware with the next delegate in the pipeline.
    /// </summary>
    /// <param name="next">The next middleware to invoke.</param>
    public ViewDataInitializerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Ensures ViewData["Messages"] is initialized as an empty list for all MVC views.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if ViewData is present (typically only in MVC view rendering)
        if (context.Items.TryGetValue("ViewData", out var viewDataObj) &&
            viewDataObj is ViewDataDictionary viewData)
        {
            // Initialize "Messages" if it's missing
            if (viewData["Messages"] == null)
            {
                viewData["Messages"] = new List<UiMessage>();
            }
        }

        await _next(context);
    }
}
