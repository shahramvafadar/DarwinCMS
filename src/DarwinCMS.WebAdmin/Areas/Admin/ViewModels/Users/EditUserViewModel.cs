using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Users
{
    /// <summary>
    /// ViewModel used for editing an existing user record in the admin panel.
    /// This includes identity, personal information, contact, preferences, and assigned roles.
    /// </summary>
    public class EditUserViewModel
    {
        /// <summary>
        /// Unique identifier of the user being edited.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// System-unique login name for the user.
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email address used for login and communication.
        /// </summary>
        [Required]
        [EmailAddress]
        [RegularExpression(@"^(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}|(?:\[(?:[01]?[0-9][0-9]?|2[0-4][0-9]|25[0-5])(?:\.(?:[01]?[0-9][0-9]?|2[0-4][0-9]|25[0-5])){3}\]))",
        ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;


        /// <summary>
        /// User's given name.
        /// </summary>
        [Required]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// User's family or last name.
        /// </summary>
        [Required]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gender label (e.g., Male, Female, Other).
        /// </summary>
        [Required]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Date of birth (ISO format).
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Optional mobile phone number in international format.
        /// </summary>
        public string? MobilePhone { get; set; }

        /// <summary>
        /// Whether the mobile number is confirmed.
        /// </summary>
        public bool IsMobileConfirmed { get; set; }

        /// <summary>
        /// Whether the email address is confirmed.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Preferred language in ISO 639-1 format.
        /// </summary>
        [Required]
        public string LanguageCode { get; set; } = "en";

        /// <summary>
        /// Optional URL to the user's profile picture.
        /// </summary>
        public string? ProfilePictureUrl { get; set; }

        /// <summary>
        /// List of selected role IDs assigned to this user.
        /// </summary>
        [Required]
        public List<Guid> RoleIds { get; set; } = new();

        /// <summary>
        /// List of available roles to populate the dropdown or multiselect.
        /// </summary>
        public List<SelectListItem> Roles { get; set; } = new();
    }
}
