using System.Collections.Generic;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles;

/// <summary>
/// ViewModel used for displaying a list of roles in the admin panel.
/// Contains paginated and filterable list data.
/// </summary>
public class RoleListViewModel
{
    /// <summary>
    /// List of role items shown in the table.
    /// </summary>
    public List<RoleListItemViewModel> Roles { get; set; } = new();

    /// <summary>
    /// Optional search term entered by admin for filtering.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// Total number of pages based on total count and page size.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Column currently used for sorting.
    /// </summary>
    public string? SortColumn { get; set; }

    /// <summary>
    /// Sort direction: "asc" or "desc".
    /// </summary>
    public string? SortDirection { get; set; }
}
