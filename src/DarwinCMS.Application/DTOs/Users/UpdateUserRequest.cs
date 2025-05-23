using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.Application.DTOs.Users;

/// <summary>
/// Represents the data required to update an existing user profile, identity, and role assignments.
/// Typically used in admin interfaces or API endpoints.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user's username.
    /// Must be unique and non-empty.
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
    /// Gets or sets the user's gender.
    /// Values may be restricted by the UI or business rules.
    /// </summary>
    [Required]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's birth date.
    /// </summary>
    [Required]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the user's mobile phone number, if provided.
    /// </summary>
    public string? MobilePhone { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the mobile phone number has been verified.
    /// </summary>
    public bool IsMobileConfirmed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the email address has been confirmed.
    /// </summary>
    public bool IsEmailConfirmed { get; set; }

    /// <summary>
    /// Gets or sets the preferred language code for UI localization (e.g., "en", "de", "fa").
    /// </summary>
    [Required]
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Gets or sets the URL of the user's profile picture, if any.
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Gets or sets the list of role IDs assigned to the user.
    /// Must include at least one valid role.
    /// </summary>
    [Required]
    public List<Guid> RoleIds { get; set; } = new();
}
