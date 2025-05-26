using System;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Permissions
{
    /// <summary>
    /// ViewModel used for listing permissions in the admin panel.
    /// </summary>
    public class PermissionListViewModel
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
    }
}

