using System;
using System.Collections.Generic;

namespace DarwinCMS.Application.DTOs.Users
{
    /// <summary>
    /// Data Transfer Object used for listing users in a simple and clean format.
    /// This DTO is used in service layer and passed to view models.
    /// </summary>
    public class UserListDto
    {
        /// <summary>
        /// Unique identifier of the user (GUID).
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Username or login name of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Primary email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// List of roles assigned to this user (e.g., ["Admin", "Editor"]).
        /// Displayed as comma-separated string in UI.
        /// </summary>
        public List<string> RoleNames {  get; set; } = new();

        /// <summary>
        /// Date and time the user was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
