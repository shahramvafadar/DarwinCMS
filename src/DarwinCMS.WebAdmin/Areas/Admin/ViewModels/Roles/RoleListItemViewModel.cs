namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles
{
    /// <summary>
    /// Simplified ViewModel used to display role in list/grid.
    /// </summary>
    public class RoleListItemViewModel
    {
        /// <summary>
        /// Role identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Technical system name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Friendly display name or fallback to system name.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Short description of the role.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Status flag (true = active).
        /// </summary>
        public bool IsActive { get; set; }
    }
}
