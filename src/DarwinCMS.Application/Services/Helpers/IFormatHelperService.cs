using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Services.Settings;

namespace DarwinCMS.Application.Services.Helpers;

/// <summary>
/// Provides helpers to format dates, times, and numbers using site settings.
/// </summary>
public interface IFormatHelperService
{
    /// <summary>
    /// Formats a DateTime using the configured timezone, calendar type, and date/time format.
    /// </summary>
    Task<string> FormatDateTimeAsync(DateTime date, CancellationToken cancellationToken = default);

    /// <summary>
    /// Formats a number using configured decimal and thousand separators.
    /// </summary>
    Task<string> FormatNumberAsync(decimal number, CancellationToken cancellationToken = default);
}
