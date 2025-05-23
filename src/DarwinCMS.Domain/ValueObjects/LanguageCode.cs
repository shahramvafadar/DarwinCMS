namespace DarwinCMS.Domain.ValueObjects;

/// <summary>
/// Represents a two-letter ISO language code (e.g., "en", "de", "fa").
/// Used for UI localization and content routing.
/// </summary>
public sealed class LanguageCode : IEquatable<LanguageCode>
{
    /// <summary>
    /// The two-letter language code in lowercase (e.g., "en").
    /// </summary>
    public string Value { get; private set; } = "en"; // Needed for EF Core binding

    /// <summary>
    /// Parameterless constructor required by EF Core.
    /// Should never be used directly.
    /// </summary>
    private LanguageCode() { }

    /// <summary>
    /// Initializes a new LanguageCode instance.
    /// </summary>
    /// <param name="code">Two-letter language code (e.g., "en").</param>
    /// <exception cref="ArgumentException">If the code is null, empty, or invalid.</exception>
    public LanguageCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Language code cannot be null or empty.", nameof(code));

        code = code.Trim().ToLowerInvariant();

        if (code.Length != 2)
            throw new ArgumentException("Language code must be exactly 2 characters.", nameof(code));

        Value = code;
    }

    /// <summary>
    /// Returns the language code as string.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Compares this code to another for equality.
    /// </summary>
    public bool Equals(LanguageCode? other) => other is not null && Value == other.Value;

    /// <summary>
    /// Compares this code to any object.
    /// </summary>
    public override bool Equals(object? obj) => obj is LanguageCode other && Equals(other);

    /// <summary>
    /// Returns the hash code of the value.
    /// </summary>
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(LanguageCode left, LanguageCode right) => Equals(left, right);

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(LanguageCode left, LanguageCode right) => !Equals(left, right);
}
