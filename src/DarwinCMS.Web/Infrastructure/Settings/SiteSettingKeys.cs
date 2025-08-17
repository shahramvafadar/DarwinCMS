namespace DarwinCMS.Web.Infrastructure.Settings;

/// <summary>
/// Strongly-typed keys for site settings to avoid magic strings.
/// </summary>
public static class SiteSettingKeys
{
    /// <summary>
    /// Branding
    /// </summary>
    public const string SiteTitle = "Site.Title";

    /// <summary>
    /// Navigation / caching
    /// </summary>
    public const string NavigationMenuCacheTtlSeconds = "Navigation.MenuCacheTtlSeconds";

    /// <summary>
    /// Localization
    /// </summary>
    public const string SiteDefaultLanguage = "Site.DefaultLanguage";

    /// <summary>
    /// Homepage
    /// </summary>
    public const string SiteHomeSlug = "Site.HomeSlug";
}
