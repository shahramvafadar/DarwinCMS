namespace DarwinCMS.Application.DTOs.Seo
{
    /// <summary>
    /// Represents an alternate-language version of a page for hreflang annotations.
    /// </summary>
    public sealed class HreflangAlternateDto
    {
        /// <summary>Language code of the alternate page (e.g., "de", "en", "fa").</summary>
        public string LanguageCode { get; set; } = "de";

        /// <summary>Slug used to construct the alternate URL.</summary>
        public string Slug { get; set; } = default!;
    }
}
