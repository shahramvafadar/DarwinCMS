namespace DarwinCMS.Application.DTOs.Pages;

/// <summary>
/// Represents a single item in a paged list of CMS pages.
/// </summary>
public class PageListItemDto
{
    /// <summary>
    /// Unique identifier of the page.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Page title shown in list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Slug used for navigation or preview.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Published status.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Scheduled publication time (if set).
    /// </summary>
    public DateTime? PublishDateUtc { get; set; }

    /// <summary>
    /// Language code of the page.
    /// </summary>
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; } = 1;
}
