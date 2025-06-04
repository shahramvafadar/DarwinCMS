using DarwinCMS.Application.Services.Common;

namespace DarwinCMS.Infrastructure.Services.Common;

/// <summary>
/// Static implementation of ILanguageProvider using a hardcoded list.
/// Can later be replaced with a DB-based or settings-driven version.
/// </summary>
public class StaticLanguageProvider : ILanguageProvider
{
    private static readonly List<(string Code, string DisplayName)> _languages = new()
    {
        ("en", "English"),
        ("de", "Deutsch"),
        ("fa", "فارسی")
    };

    /// <inheritdoc />
    public List<string> GetAllLanguageCodes()
    {
        return _languages.Select(x => x.Code).ToList();
    }

    /// <inheritdoc />
    public List<(string Code, string DisplayName)> GetDisplayNames()
    {
        return _languages;
    }
}
