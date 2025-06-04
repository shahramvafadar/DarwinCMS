using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Pages;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Pages;

/// <summary>
/// ViewModel for listing pages in the admin UI.
/// Includes search, language filtering, sorting, and pagination metadata.
/// </summary>
public class PageListViewModel
{
    /// <summary>
    /// The list of pages shown in the table.
    /// </summary>
    public List<PageListItemViewModel> Pages { get; set; } = new();

    /// <summary>
    /// Optional search term used to filter by title or slug.
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Filter by language code (e.g. "en", "de").
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Filter by published/draft status.
    /// </summary>
    public bool? IsPublished { get; set; }

    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// Total number of available pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Name of the column to sort by (e.g. Title, Slug, PublishDateUtc).
    /// </summary>
    public string? SortColumn { get; set; }

    /// <summary>
    /// Sort direction: "asc" or "desc".
    /// </summary>
    public string? SortDirection { get; set; }

    /// <summary>
    /// List of all available languages to show in the filter dropdown.
    /// </summary>
    public List<(string Code, string DisplayName)> LanguageList { get; set; } = new();

    /// <summary>
    /// Whether any filters are active.
    /// </summary>
    public bool HasFilter =>
        !string.IsNullOrWhiteSpace(Search) ||
        !string.IsNullOrWhiteSpace(LanguageCode) ||
        IsPublished.HasValue;

    /// <summary>
    /// Returns the next sort direction for the given column.
    /// Used to toggle asc/desc on column headers.
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
    /// Returns the FontAwesome icon class based on current sort state of a column.
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
