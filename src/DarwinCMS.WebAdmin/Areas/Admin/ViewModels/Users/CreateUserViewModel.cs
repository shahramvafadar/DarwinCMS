using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Users;

/// <summary>
/// ViewModel for creating a new user in the admin panel.
/// </summary>
public class CreateUserViewModel
{
    /// <summary>
    /// Desired username for login. Must be unique.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User's primary email address.
    /// </summary>
    [Required]
    [EmailAddress]
    [RegularExpression(@"^(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}|(?:\[(?:[01]?[0-9][0-9]?|2[0-4][0-9]|25[0-5])(?:\.(?:[01]?[0-9][0-9]?|2[0-4][0-9]|25[0-5])){3}\]))", 
        ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;


    /// <summary>
    /// Password for login. Minimum 8 characters.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Password confirmation. Must match Password field.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// User's first name.
    /// </summary>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's gender. One of: Male, Female, Other.
    /// </summary>
    [Required]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// User's date of birth.
    /// </summary>
    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Optional mobile phone number (international format).
    /// </summary>
    public string? MobilePhone { get; set; }

    /// <summary>
    /// Whether the mobile phone is confirmed.
    /// </summary>
    public bool IsMobileConfirmed { get; set; }

    /// <summary>
    /// Whether the email is confirmed.
    /// </summary>
    public bool IsEmailConfirmed { get; set; }

    /// <summary>
    /// Preferred UI language (e.g., "en", "de", "fa").
    /// </summary>
    [Required]
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Optional profile picture URL.
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// List of selected Role IDs for this user.
    /// </summary>
    [Required]
    public List<Guid> RoleIds { get; set; } = new();

    /// <summary>
    /// Available roles for display in the form.
    /// </summary>
    public List<SelectListItem> Roles { get; set; } = new();

    /// <summary>
    /// Initializes the BirthDate to a reasonable default (1985-01-01).
    /// </summary>
    public CreateUserViewModel()
    {
        BirthDate = new DateTime(1985, 1, 1);
    }
}
