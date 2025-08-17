using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DarwinCMS.Web.TagHelpers
{
    /// <summary>
    /// Renders SEO meta tags (<title/>, description, OpenGraph/Twitter) in the document head.
    /// Intended to be placed inside _Layout.cshtml. Page views/controllers set attributes via ViewData or model.
    /// Usage: <seo-meta title="..." description="..." canonical-url="..." og-image-url="..." />
    /// </summary>
    [HtmlTargetElement("seo-meta", TagStructure = TagStructure.NormalOrSelfClosing)]
    public sealed class SeoMetaTagHelper : TagHelper
    {
        /// <summary>Page title. When omitted, falls back to ViewData["Title"].</summary>
        public string? Title { get; set; }

        /// <summary>Meta description. When omitted, falls back to ViewData["Description"].</summary>
        public string? Description { get; set; }

        /// <summary>Absolute canonical URL for the current page (optional).</summary>
        public string? CanonicalUrl { get; set; }

        /// <summary>Absolute URL to an OpenGraph/Twitter image (optional).</summary>
        public string? OgImageUrl { get; set; }

        /// <summary>
        /// Provides access to the current view context (ViewData, HttpContext).
        /// Not bound from HTML attributes.
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        /// <summary>
        /// Builds the set of SEO-related tags, using explicit attributes first,
        /// then falling back to ViewData when attributes are missing.
        /// Also renders canonical and hreflang link tags when provided via ViewData.
        /// </summary>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "meta";
            output.TagMode = TagMode.StartTagAndEndTag;

            var viewData = ViewContext?.ViewData;

            var effectiveTitle = !string.IsNullOrWhiteSpace(Title)
                ? Title
                : viewData?["Title"]?.ToString() ?? string.Empty;

            var effectiveDescription = !string.IsNullOrWhiteSpace(Description)
                ? Description
                : viewData?["Description"]?.ToString() ?? string.Empty;

            var effectiveCanonical = !string.IsNullOrWhiteSpace(CanonicalUrl)
                ? CanonicalUrl
                : viewData?["CanonicalUrl"]?.ToString();

            // Hreflang alternates are provided in ViewData["Hreflang"] as Dictionary<string, string>.
            var hreflangMap = viewData?["Hreflang"] as IDictionary<string, string>;

            var html = $@"
<title>{System.Net.WebUtility.HtmlEncode(effectiveTitle)}</title>
<meta name=""description"" content=""{System.Net.WebUtility.HtmlEncode(effectiveDescription)}"">
<meta property=""og:title"" content=""{System.Net.WebUtility.HtmlEncode(effectiveTitle)}"">
<meta property=""og:description"" content=""{System.Net.WebUtility.HtmlEncode(effectiveDescription)}"">
{(string.IsNullOrWhiteSpace(OgImageUrl) ? "" : $@"<meta property=""og:image"" content=""{OgImageUrl}"">")}
{(string.IsNullOrWhiteSpace(effectiveCanonical) ? "" : $@"<link rel=""canonical"" href=""{effectiveCanonical}"">")}
<meta name=""twitter:card"" content=""summary_large_image"">
<meta name=""twitter:title"" content=""{System.Net.WebUtility.HtmlEncode(effectiveTitle)}"">
<meta name=""twitter:description"" content=""{System.Net.WebUtility.HtmlEncode(effectiveDescription)}"">
{(string.IsNullOrWhiteSpace(OgImageUrl) ? "" : $@"<meta name=""twitter:image"" content=""{OgImageUrl}"">")}
";

            if (hreflangMap is not null)
            {
                foreach (var kv in hreflangMap)
                {
                    var lang = kv.Key;
                    var href = kv.Value;
                    html += $@"<link rel=""alternate"" hreflang=""{System.Net.WebUtility.HtmlEncode(lang)}"" href=""{href}"">" + "\n";
                }
            }

            output.Content.SetHtmlContent(html);
        }
    }
}
