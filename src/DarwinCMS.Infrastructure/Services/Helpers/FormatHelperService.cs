using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Services.Helpers;
using DarwinCMS.Application.Services.Settings;

namespace DarwinCMS.Infrastructure.Services.Helpers;

/// <summary>
/// Implementation of IFormatHelperService using ISiteSettingService.
/// Responsible for formatting dates, times, and numbers based on site-wide settings.
/// </summary>
public class FormatHelperService : IFormatHelperService
{
    private readonly ISiteSettingService _siteSettingService;

    /// <summary>
    /// Initializes the FormatHelperService with required dependencies.
    /// </summary>
    /// <param name="siteSettingService">Service to retrieve site-wide settings (cached).</param>
    public FormatHelperService(ISiteSettingService siteSettingService)
    {
        _siteSettingService = siteSettingService;
    }

    /// <inheritdoc/>
    public async Task<string> FormatDateTimeAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        // Load configured date and time formats
        var dateFormat = await _siteSettingService.GetValueAsync("Site.DateFormat", cancellationToken: cancellationToken);
        var timeFormat = await _siteSettingService.GetValueAsync("Site.TimeFormat", cancellationToken: cancellationToken);

        // Load configured timezone (IANA)
        var timeZoneId = await _siteSettingService.GetValueAsync("Site.TimeZone", cancellationToken: cancellationToken);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        // Load calendar type (Gregorian, Persian, Hijri, etc.)
        var calendarType = await _siteSettingService.GetValueAsync("Site.CalendarType", cancellationToken: cancellationToken);

        // Create a culture info with the selected calendar type
        var culture = (CultureInfo.InvariantCulture.Clone() as CultureInfo)!;
        culture.DateTimeFormat.Calendar = calendarType switch
        {
            "Persian" => new PersianCalendar(),
            "Hijri" => new HijriCalendar(),
            _ => new GregorianCalendar()
        };

        // Convert UTC time to local time based on timezone
        var localTime = TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(), timeZone);

        // Format and return the final result
        return localTime.ToString($"{dateFormat} {timeFormat}", culture);
    }

    /// <inheritdoc/>
    public async Task<string> FormatNumberAsync(decimal number, CancellationToken cancellationToken = default)
    {
        // Load configured decimal and thousand separators
        var decimalSep = await _siteSettingService.GetValueAsync("Site.DecimalSeparator", cancellationToken: cancellationToken);
        var thousandSep = await _siteSettingService.GetValueAsync("Site.ThousandSeparator", cancellationToken: cancellationToken);

        // Create a culture info with customized number formatting
        var numberFormat = (CultureInfo.InvariantCulture.Clone() as CultureInfo)!.NumberFormat;
        numberFormat.NumberDecimalSeparator = decimalSep;
        numberFormat.NumberGroupSeparator = thousandSep;

        // Format and return the final number as string
        return number.ToString("N", numberFormat);
    }
}
