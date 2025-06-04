using System;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Pages;

/// <summary>
/// View model used for listing CMS pages in the admin UI.
/// This is mapped from PageListItemDto and displayed in the Index.cshtml and Recycle Bin.
/// </summary>
public class PageListItemViewModel
{
    /// <summary>
    /// Unique identifier of the page.
    /// Used for linking to Edit/Delete actions.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The title of the page, displayed in the admin list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Slug used in page URLs.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// The language code of the page (e.g., "en", "de", "fa").
    /// </summary>
    public string LanguageCode { get; set; } = "en";

    /// <summary>
    /// Indicates whether the page is currently published.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Optional publish date in UTC. May be null if not scheduled.
    /// </summary>
    public DateTime? PublishDateUtc { get; set; }

    /// <summary>
    /// Date and time when the page was last modified (used for showing delete time in Recycle Bin).
    /// </summary>
    public DateTime? ModifiedAt { get; set; }
}
