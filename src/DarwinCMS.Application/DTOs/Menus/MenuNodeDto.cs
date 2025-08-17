using System.Collections.Generic;

namespace DarwinCMS.Application.DTOs.Menus
{
    /// <summary>
    /// Tree node used to render navigation menus.
    /// URL may be an absolute link or a CMS slug-based path depending on LinkType.
    /// </summary>
    public sealed class MenuNodeDto
    {
        /// <summary>Display label of the menu item.</summary>
        public string Title { get; set; } = default!;

        /// <summary>Absolute URL for external links; null for internal CMS pages.</summary>
        public string? Url { get; set; }

        /// <summary>Slug for internal CMS pages; null for external links.</summary>
        public string? Slug { get; set; }

        /// <summary>Link type as defined by the domain model (e.g., "Internal", "External", "Module").</summary>
        public string LinkType { get; set; } = "Internal";

        /// <summary>Child menu nodes.</summary>
        public List<MenuNodeDto> Children { get; set; } = new();
    }
}
