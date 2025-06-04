using DarwinCMS.Application.Services.Settings;
using DarwinCMS.WebAdmin.Infrastructure.Helpers;

using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DarwinCMS.WebAdmin.Infrastructure.TagHelpers;

/// <summary>
/// TagHelper to render a date and time formatted using the site's settings (date, time format, calendar).
/// Usage in Razor Views:
/// when need using: @addTagHelper *, DarwinCMS.WebAdmin
/// <format-date value="2025-05-30T14:00:00Z"></format-date>
/// </summary>
[HtmlTargetElement("format-date")]
public class FormatDateTimeTagHelper : TagHelper
{
    private readonly ISiteSettingService _siteSettingService;

    /// <summary>
    /// Initializes a new instance of the FormatDateTimeTagHelper.
    /// </summary>
    /// <param name="siteSettingService">Service for retrieving site settings.</param>
    public FormatDateTimeTagHelper(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    /// <summary>
    /// The UTC DateTime to format and render.
    /// </summary>
    [HtmlAttributeName("value")]
    public DateTime? Value { get; set; }

    /// <summary>
    /// Renders the formatted date/time based on the site's localization settings.
    /// </summary>
    /// <param name="context">The context for this TagHelper execution.</param>
    /// <param name="output">The output writer for the TagHelper's HTML content.</param>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Format the date using the FormatHelper
        string formatted = await FormatHelper.FormatDateTimeAsync(
            Value ?? DateTime.UtcNow,
            _siteSettingService);

        // Render the result
        output.TagName = "span";
        output.Content.SetContent(formatted);
    }
}
