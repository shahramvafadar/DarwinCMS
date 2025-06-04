namespace DarwinCMS.Application.DTOs.Pages;

/// <summary>
/// Represents filter options for querying a list of pages.
/// </summary>
public class PageFilterDto
{
    /// <summary>
    /// Optional language code to filter results.
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Optional search term (matches title or slug).
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Optional published status filter.
    /// </summary>
    public bool? IsPublished { get; set; }

    /// <summary>
    /// Page number for paging.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Page size for paging.
    /// </summary>
    public int PageSize { get; set; } = 20;
    /// <summary>
    /// Gets or sets the name of the column used for sorting data.
    /// </summary>
    public string? SortColumn { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the sorting order is descending.
    /// </summary>
    public bool SortDescending { get; set; }
}
