using System;

using DarwinCMS.Shared.ViewModels.Interfaces;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Permissions
{
    /// <summary>
    /// ViewModel used for listing permissions in the admin panel and recycle bin.
    /// Includes soft-delete metadata for recycle bin functionality.
    /// </summary>
    public class PermissionListViewModel : ILogicalDeletableViewModel
    {
        /// <summary>
        /// Unique identifier for the permission record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Internal permission name (e.g. "manage_users").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Display name for showing in the UI (e.g. "Manage Users").
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the permission is protected and cannot be deleted.
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Indicates whether the permission is logically (soft) deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The UTC date and time when the permission was last modified (or deleted).
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// The ID of the user who last modified (or deleted) the permission.
        /// </summary>
        public Guid? ModifiedByUserId { get; set; }
    }
}
