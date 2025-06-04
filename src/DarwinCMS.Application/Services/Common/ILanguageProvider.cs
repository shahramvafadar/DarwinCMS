namespace DarwinCMS.Application.Services.Common;

/// <summary>
/// Provides a list of supported languages in the system.
/// Used for dropdowns, filtering, localization, and validation.
/// </summary>
public interface ILanguageProvider
{
    /// <summary>
    /// Returns all active language codes (e.g., "en", "de").
    /// </summary>
    List<string> GetAllLanguageCodes();

    /// <summary>
    /// Returns displayable language code pairs (code + name).
    /// </summary>
    List<(string Code, string DisplayName)> GetDisplayNames();
}
