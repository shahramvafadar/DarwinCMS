namespace DarwinCMS.Domain.ValueObjects;

/// <summary>
/// ValueObject representing the type of a menu link.
/// Used to distinguish between links to internal CMS pages, external URLs, or module-specific routes.
/// </summary>
public readonly struct LinkType : IEquatable<LinkType>
{
    /// <summary>
    /// Gets the string value representation (e.g. "internal", "external", "module").
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new instance of <see cref="LinkType"/> with the specified value.
    /// </summary>
    private LinkType(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Link to a CMS internal page (PageId must be set).
    /// </summary>
    public static LinkType Internal => new("internal");

    /// <summary>
    /// Link to an external URL (e.g., https://example.com).
    /// </summary>
    public static LinkType External => new("external");

    /// <summary>
    /// Link to a module route (e.g., /shop/categories).
    /// </summary>
    public static LinkType Module => new("module");

    /// <summary>
    /// Returns all allowed values (used in dropdowns, validation, etc.).
    /// </summary>
    public static IEnumerable<LinkType> All => new[] { Internal, External, Module };

    /// <inheritdoc/>
    public bool Equals(LinkType other) => Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is LinkType other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Returns the string representation of the value.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Creates a LinkType from a string value.
    /// Throws an exception if value is invalid.
    /// </summary>
    public static LinkType From(string value)
    {
        if (string.Equals(value, "internal", StringComparison.OrdinalIgnoreCase)) return Internal;
        if (string.Equals(value, "external", StringComparison.OrdinalIgnoreCase)) return External;
        if (string.Equals(value, "module", StringComparison.OrdinalIgnoreCase)) return Module;

        throw new ArgumentException($"Invalid LinkType: '{value}'");
    }


    /// <summary>
    /// Compares two <see cref="LinkType"/> values for equality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns><c>true</c> if both values are equal; otherwise <c>false</c>.</returns>
    public static bool operator ==(LinkType left, LinkType right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="LinkType"/> values for inequality.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns><c>true</c> if the values are not equal; otherwise <c>false</c>.</returns>
    public static bool operator !=(LinkType left, LinkType right) => !(left == right);

}
