using System;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles
{
    /// <summary>
    /// ViewModel used to confirm deletion of a role from the admin UI.
    /// This model is typically passed to the delete confirmation view.
    /// </summary>
    public class DeleteRoleViewModel
    {
        /// <summary>
        /// Unique identifier of the role to be deleted.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Friendly display name of the role, used in the confirmation message.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Optional description to help admin understand the purpose of the role before deleting.
        /// </summary>
        public string? Description { get; set; }
    }
}
