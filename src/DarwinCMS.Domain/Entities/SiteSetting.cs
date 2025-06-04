using System;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a configurable setting for the website. Stored as a key-value pair with support for category, language, caching, and soft deletion.
/// </summary>
public class SiteSetting : BaseEntity
{
    /// <summary>
    /// Unique key identifying the setting (e.g., "Site.Title").
    /// </summary>
    public string Key { get; private set; } = string.Empty;

    /// <summary>
    /// The actual value of the setting (e.g., string, JSON, HTML).
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Type of the value (e.g., string, boolean, html).
    /// </summary>
    public string ValueType { get; private set; } = "string";

    /// <summary>
    /// Optional category to group related settings.
    /// </summary>
    public string? Category { get; private set; }

    /// <summary>
    /// Language code for localization (e.g., "en", "de", "fa").
    /// </summary>
    public string? LanguageCode { get; private set; }

    /// <summary>
    /// Optional description for admin display.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Indicates if the setting is system-level and cannot be deleted.
    /// </summary>
    public bool IsSystem { get; private set; } = false;

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected SiteSetting() { }

    /// <summary>
    /// Creates a new site setting with required and optional details.
    /// </summary>
    public SiteSetting(string key, string value, string valueType, string? category, string? languageCode, string? description, bool isSystem, Guid createdByUserId)
    {
        SetKey(key, createdByUserId);
        SetValue(value, createdByUserId);
        SetValueType(valueType, createdByUserId);
        SetCategory(category, createdByUserId);
        SetLanguageCode(languageCode, createdByUserId);
        SetDescription(description, createdByUserId);
        if (isSystem) MarkAsSystem(createdByUserId);
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Sets the key of this setting.
    /// </summary>
    public void SetKey(string key, Guid? modifierId)
    {
        Key = string.IsNullOrWhiteSpace(key) ? throw new ArgumentException("Key is required.", nameof(key)) : key.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets or updates the value.
    /// </summary>
    public void SetValue(string value, Guid? modifierId)
    {
        Value = value?.Trim() ?? string.Empty;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the data type of the setting's value.
    /// </summary>
    public void SetValueType(string valueType, Guid? modifierId)
    {
        ValueType = string.IsNullOrWhiteSpace(valueType) ? "string" : valueType.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the optional category.
    /// </summary>
    public void SetCategory(string? category, Guid? modifierId)
    {
        Category = category?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the language code for localization.
    /// </summary>
    public void SetLanguageCode(string? code, Guid? modifierId)
    {
        LanguageCode = code?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Sets the description for admin display.
    /// </summary>
    public void SetDescription(string? description, Guid? modifierId)
    {
        Description = description?.Trim();
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Marks this setting as system-level (protected).
    /// </summary>
    public void MarkAsSystem(Guid? modifierId)
    {
        IsSystem = true;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Marks this setting as logically deleted.
    /// </summary>
    public void MarkAsDeleted(Guid? modifierId)
    {
        MarkAsModified(modifierId, true);
    }

    /// <summary>
    /// Restores this setting from a soft-deleted state.
    /// </summary>
    public void Restore(Guid? modifierId)
    {
        MarkAsModified(modifierId, false);
    }
}
