using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Services.Settings;

namespace DarwinCMS.WebAdmin.Infrastructure.Helpers;

/// <summary>
/// Provides helpers to format dates, times, and numbers using site settings.
/// </summary>
public static class FormatHelper
{
    /// <summary>
    /// Formats a DateTime using the configured timezone, calendar type, and date/time format.
    /// </summary>
    public static async Task<string> FormatDateTimeAsync(
        DateTime date,
        ISiteSettingService settings,
        CancellationToken cancellationToken = default)
    {
        // Load date/time format
        var dateFormat = await settings.GetValueAsync("Site.DateFormat", cancellationToken: cancellationToken);
        var timeFormat = await settings.GetValueAsync("Site.TimeFormat", cancellationToken: cancellationToken);

        // Load timezone
        var timeZoneId = await settings.GetValueAsync("Site.TimeZone", cancellationToken: cancellationToken);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        // Load calendar type
        var calendarType = await settings.GetValueAsync("Site.CalendarType", cancellationToken: cancellationToken);
        var culture = CultureInfo.InvariantCulture.Clone() as CultureInfo;

        // Choose calendar
        culture!.DateTimeFormat.Calendar = calendarType switch
        {
            "Persian" => new PersianCalendar(),
            "Hijri" => new HijriCalendar(),
            _ => new GregorianCalendar()
        };

        // Apply timezone
        var localTime = TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(), timeZone);

        return localTime.ToString($"{dateFormat} {timeFormat}", culture);
    }

    /// <summary>
    /// Formats a number using configured decimal and thousand separators.
    /// </summary>
    public static async Task<string> FormatNumberAsync(
        decimal number,
        ISiteSettingService settings,
        CancellationToken cancellationToken = default)
    {
        var decimalSep = await settings.GetValueAsync("Site.DecimalSeparator", cancellationToken: cancellationToken);
        var thousandSep = await settings.GetValueAsync("Site.ThousandSeparator", cancellationToken: cancellationToken);

        var numberFormat = (CultureInfo.InvariantCulture.Clone() as CultureInfo)!.NumberFormat;
        numberFormat.NumberDecimalSeparator = decimalSep;
        numberFormat.NumberGroupSeparator = thousandSep;

        return number.ToString("N", numberFormat);
    }
}
