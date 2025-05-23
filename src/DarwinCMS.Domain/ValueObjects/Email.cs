using System.Text.RegularExpressions;

namespace DarwinCMS.Domain.ValueObjects;

/// <summary>
/// Represents a validated email address in the system.
/// </summary>
public sealed class Email : IEquatable<Email>
{
    /// <summary>
    /// Static regex to validate email addresses.
    /// </summary>
    private static readonly Regex _emailRegex = new(
        pattern: @"^[^\s@]+@[^\s@]+\.[^\s@]+$",
        options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Gets the normalized email address string.
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// Do not use directly. EF requires it for materialization.
    /// </summary>
    private Email() { }

    /// <summary>
    /// Initializes a new instance of the Email value object.
    /// Throws exception if format is invalid.
    /// </summary>
    /// <param name="value">Input email address</param>
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty.", nameof(value));

        value = value.Trim().ToLowerInvariant();

        if (!_emailRegex.IsMatch(value))
            throw new ArgumentException($"Invalid email format: '{value}'", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Returns a placeholder email address used to satisfy nullable warnings.
    /// Use only when the actual email is not yet assigned (e.g. during EF materialization).
    /// </summary>
    public static Email CreatePlaceholder()
    {
        return new Email("placeholder@system.local");
    }

    /// <summary>
    /// Returns the email as string.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Compares this email to another for equality.
    /// </summary>
    public bool Equals(Email? other) => other is not null && Value == other.Value;

    /// <summary>
    /// Compares this email to any object.
    /// </summary>
    public override bool Equals(object? obj) => obj is Email other && Equals(other);

    /// <summary>
    /// Returns the hash code of the email.
    /// </summary>
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(Email left, Email right) => Equals(left, right);

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(Email left, Email right) => !Equals(left, right);
}
