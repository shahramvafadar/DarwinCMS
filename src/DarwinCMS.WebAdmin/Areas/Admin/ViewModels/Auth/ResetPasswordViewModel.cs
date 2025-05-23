using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Auth;

/// <summary>
/// Represents the reset password form used in the Admin panel.
/// This model captures the reset token, email, and new password values.
/// </summary>
public class ResetPasswordViewModel
{
    /// <summary>
    /// Token used to validate the password reset request.
    /// This is usually generated and sent via email.
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the user requesting the password reset.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The new password chosen by the user.
    /// Must be at least 6 characters and no more than 100.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation of the new password.
    /// Must match the NewPassword field.
    /// </summary>
    [Required]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
