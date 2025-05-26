using System.Collections.Generic;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Permissions;

/// <summary>
/// ViewModel used to display a paginated and sortable list of permissions.
/// </summary>
public class PermissionIndexViewModel
{
    /// <summary>
    /// List of permissions for the current page.
    /// </summary>
    public List<PermissionListViewModel> Permissions { get; set; } = new();

    /// <summary>
    /// Total number of pages available.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Current page number being viewed.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Term used for filtering the list by name or display name.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Column currently being sorted.
    /// </summary>
    public string? SortColumn { get; set; }

    /// <summary>
    /// Direction of sort: "asc" or "desc".
    /// </summary>
    public string? SortDirection { get; set; }
}
