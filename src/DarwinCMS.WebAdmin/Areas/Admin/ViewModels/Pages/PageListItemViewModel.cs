using System;

using DarwinCMS.Shared.ViewModels.Interfaces;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Pages;

/// <summary>
/// View model used for listing CMS pages in the admin UI (Index and Recycle Bin).
/// </summary>
public class PageListItemViewModel : ILogicalDeletableViewModel
{
    /// <summary>
    /// Unique identifier of the page.
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
    /// Date and time when the page was last modified.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// The ID of the user who last modified (or deleted) the item.
    /// </summary>
    public Guid? ModifiedByUserId { get; set; }

    /// <summary>
    /// Indicates whether the page is logically (soft) deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
