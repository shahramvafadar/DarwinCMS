using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Auth;

/// <summary>
/// Represents the form model used to request a password reset link.
/// Users must provide a valid email address to receive the reset token.
/// </summary>
public class ForgotPasswordViewModel
{
    /// <summary>
    /// The email address of the user requesting a password reset.
    /// Must be valid and match RFC 5322 format.
    /// </summary>
    [Required]
    [EmailAddress]
    [RegularExpression(
        @"^(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}|(?:\[(?:[01]?[0-9][0-9]?|2[0-4][0-9]|25[0-5])(?:\.(?:[01]?[0-9][0-9]?|2[0-4][0-9]|25[0-5])){3}\]))",
        ErrorMessage = "Invalid email format.")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;
}
