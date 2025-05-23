using System.Text.RegularExpressions;

namespace DarwinCMS.Domain.ValueObjects;

/// <summary>
/// Represents a URL-safe, normalized identifier used for routing and SEO.
/// Must follow lowercase alphanumeric-kebab-case (e.g., "hello-world").
/// </summary>
public class Slug : IEquatable<Slug>
{
    private static readonly Regex _validSlugRegex = new("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);

    /// <summary>
    /// Gets or sets the normalized slug value.
    /// EF Core sets this value through the setter when reading from the database.
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Parameterless constructor required for EF Core.
    /// Initializes with an empty (invalid) slug.
    /// </summary>
    public Slug()
    {
        // For EF Core only
    }

    /// <summary>
    /// Creates a placeholder slug value for EF Core materialization.
    /// </summary>
    public static Slug CreatePlaceholder()
    {
        return new Slug("placeholder-slug");
    }


    /// <summary>
    /// Initializes a new Slug value object.
    /// Trims, lowercases, and validates the format.
    /// </summary>
    /// <param name="value">A valid, normalized slug string</param>
    /// <exception cref="ArgumentException">Thrown when format is invalid</exception>
    public Slug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Slug cannot be null or empty.", nameof(value));

        value = value.Trim().ToLowerInvariant();

        if (!_validSlugRegex.IsMatch(value))
            throw new ArgumentException($"Invalid slug format: '{value}'", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Returns the string representation of the slug.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Determines equality by comparing slug value.
    /// </summary>
    public bool Equals(Slug? other) => other is not null && Value == other.Value;

    /// <summary>
    /// Determines equality with any object.
    /// </summary>
    public override bool Equals(object? obj) => obj is Slug other && Equals(other);

    /// <summary>
    /// Gets the hash code for the slug.
    /// </summary>
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(Slug? left, Slug? right) => Equals(left, right);

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(Slug? left, Slug? right) => !(left == right);
}
