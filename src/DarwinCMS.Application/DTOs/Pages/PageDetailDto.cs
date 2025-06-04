namespace DarwinCMS.Application.DTOs.Pages;

/// <summary>
/// Full data used for editing or displaying a single CMS page.
/// </summary>
public class PageDetailDto : CreatePageDto
{
    /// <summary>
    /// Unique identifier of the page.
    /// </summary>
    public Guid Id { get; set; }
}
