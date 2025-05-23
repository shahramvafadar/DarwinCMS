using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DarwinCMS.Application.DTOs.Users;

/// <summary>
/// Represents the input required to register a new user.
/// Used in admin panel or API to submit basic profile, credentials, and role assignments.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Gets or sets the unique username for the user.
    /// This is typically used for login or display purposes.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's email address.
    /// Must be a valid email format.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's password.
    /// Must be at least 6 characters long.
    /// </summary>
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gender of the user.
    /// Expected values: "Male", "Female", or custom values depending on UI input.
    /// </summary>
    [Required]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's birth date.
    /// </summary>
    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the mobile phone number of the user.
    /// Can be null if not provided.
    /// </summary>
    public string? MobilePhone { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the mobile number is confirmed.
    /// </summary>
    public bool IsMobileConfirmed { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the email is confirmed.
    /// </summary>
    public bool IsEmailConfirmed { get; set; } = false;

    /// <summary>
    /// Gets or sets the preferred language code (e.g., "en", "de", "fa") for UI localization.
    /// Default is "en".
    /// </summary>
    [Required]
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Gets or sets the URL of the user's profile picture, if any.
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Gets or sets the list of role IDs to assign to the user.
    /// At least one role is typically required.
    /// </summary>
    [Required]
    public List<Guid> RoleIds { get; set; } = new();
}
