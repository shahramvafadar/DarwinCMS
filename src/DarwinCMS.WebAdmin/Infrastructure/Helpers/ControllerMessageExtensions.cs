using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Shared;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.WebAdmin.Infrastructure.Helpers;

/// <summary>
/// Extension methods to simplify adding user messages to ViewData from controllers.
/// Supports success, warning, and error messages with optional trace IDs.
/// </summary>
public static class ControllerMessageExtensions
{
    private const string Key = "Messages";

    /// <summary>
    /// Adds a user-facing message to the controller's ViewData dictionary.
    /// </summary>
    /// <param name="controller">The target controller.</param>
    /// <param name="message">The UiMessage to add.</param>
    public static void AddMessage(this Controller controller, UiMessage message)
    {
        if (controller.ViewData[Key] is not List<UiMessage> list)
        {
            list = new List<UiMessage>();
            controller.ViewData[Key] = list;
        }
        list.Add(message);
    }

    /// <summary>
    /// Adds a success message.
    /// </summary>
    public static void AddSuccess(this Controller controller, string message)
        => controller.AddMessage(UiMessage.CreateSuccess(message));

    /// <summary>
    /// Adds a warning message.
    /// </summary>
    public static void AddWarning(this Controller controller, string message)
        => controller.AddMessage(UiMessage.CreateWarning(message));

    /// <summary>
    /// Adds an error message with optional reference ID.
    /// </summary>
    public static void AddError(this Controller controller, string message, string? referenceId = null)
        => controller.AddMessage(UiMessage.CreateError(message, referenceId));
}
