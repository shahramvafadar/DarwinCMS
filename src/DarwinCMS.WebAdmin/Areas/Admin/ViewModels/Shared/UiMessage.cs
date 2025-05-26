namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Shared;

/// <summary>
/// View model for displaying structured user messages in the UI.
/// Used to represent success, warning, or error messages consistently.
/// </summary>
public class UiMessage
{
    /// <summary>
    /// The main message to be displayed to the user.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Optional reference ID (e.g., from logs) used for support and traceability.
    /// </summary>
    public string? ReferenceId { get; set; }

    /// <summary>
    /// Type of message: success, warning, or error. Default is 'error'.
    /// </summary>
    public string Type { get; set; } = "error";

    /// <summary>
    /// Optional URL to turn the message box into a clickable link.
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// Helper method to create an error message.
    /// </summary>
    public static UiMessage CreateError(string message, string? referenceId = null) => new()
    {
        Message = message,
        ReferenceId = referenceId,
        Type = "error"
    };

    /// <summary>
    /// Helper method to create a success message.
    /// </summary>
    public static UiMessage CreateSuccess(string message) => new()
    {
        Message = message,
        Type = "success"
    };

    /// <summary>
    /// Helper method to create a warning message.
    /// </summary>
    public static UiMessage CreateWarning(string message) => new()
    {
        Message = message,
        Type = "warning"
    };

    /// <summary>
    /// Helper method to create a message of any type with an optional link.
    /// </summary>
    public static UiMessage Create(string message, string type = "error", string? referenceId = null, string? link = null) => new()
    {
        Message = message,
        Type = type,
        ReferenceId = referenceId,
        Link = link
    };
}
