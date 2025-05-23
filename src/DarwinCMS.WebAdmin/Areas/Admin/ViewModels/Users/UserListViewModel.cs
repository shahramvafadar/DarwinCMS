using DarwinCMS.Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Users
{
    /// <summary>
    /// ViewModel for listing users in the admin UI.
    /// Includes search, filtering, sorting, and paging options.
    /// </summary>
    public class UserListViewModel
    {
        /// <summary>
        /// The list of users to be displayed on the current page.
        /// </summary>
        public List<UserListDto> Users { get; set; } = new();

        /// <summary>
        /// Current search string (e.g. username or email).
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Current selected role filter (optional).
        /// </summary>
        public Guid? RoleFilterId { get; set; }

        /// <summary>
        /// All roles available for dropdown filter.
        /// </summary>
        public List<SelectListItem> Roles { get; set; } = new();

        /// <summary>
        /// Current page number (1-based).
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Total number of pages (calculated by controller).
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Number of users per page.
        /// Used for rendering row numbers in the list.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Name of the column to sort by (e.g. Username, Email, CreatedAt).
        /// </summary>
        public string? SortColumn { get; set; }

        /// <summary>
        /// Direction of sort: "asc" or "desc".
        /// </summary>
        public string? SortDirection { get; set; }

        /// <summary>
        /// Determines whether current list has any active filters.
        /// </summary>
        public bool HasFilter =>
            !string.IsNullOrWhiteSpace(SearchTerm) || RoleFilterId.HasValue;

        /// <summary>
        /// Returns the next sort direction for a given column.
        /// Used to generate sort links in the view.
        /// </summary>
        public string GetNextSortDirection(string column)
        {
            if (SortColumn?.Equals(column, StringComparison.OrdinalIgnoreCase) == true)
            {
                return SortDirection?.ToLowerInvariant() == "asc" ? "desc" : "asc";
            }

            return "asc";
        }

        /// <summary>
        /// Returns the Font Awesome icon class for the current sort state of a column.
        /// </summary>
        public string? GetSortIcon(string column)
        {
            if (!SortColumn?.Equals(column, StringComparison.OrdinalIgnoreCase) ?? true)
                return null;

            return SortDirection?.ToLowerInvariant() switch
            {
                "asc" => "fa-sort-up",
                "desc" => "fa-sort-down",
                _ => null
            };
        }
    }
}
