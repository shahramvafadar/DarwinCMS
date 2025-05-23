using System.Text;

namespace DarwinCMS.Domain.ValueObjects;

/// <summary>
/// Represents a normalized list of comma-separated tags (e.g., for filtering, categorization).
/// Internally stored as string[], externally exposed as comma-separated string.
/// </summary>
public class TagList : IEquatable<TagList>
{
    private string[] _tags = Array.Empty<string>();

    /// <summary>
    /// Gets or sets the normalized, comma-separated string of tags.
    /// This property is used by EF Core when mapping the value to the database.
    /// Example: "cms,dotnet,headless"
    /// </summary>
    public string Value
    {
        get => string.Join(",", _tags);
        private set => _tags = NormalizeTags(value?.Split(',') ?? Array.Empty<string>());
    }

    /// <summary>
    /// Gets the tags as a read-only list.
    /// </summary>
    public IReadOnlyList<string> Tags => _tags;

    /// <summary>
    /// Parameterless constructor required for EF Core.
    /// Initializes with an empty tag list.
    /// </summary>
    public TagList()
    {
        _tags = Array.Empty<string>();
    }

    /// <summary>
    /// Initializes a TagList from an enumerable of strings.
    /// Trims, lowercases, and de-duplicates all values.
    /// </summary>
    /// <param name="tags">Collection of tags</param>
    public TagList(IEnumerable<string> tags)
    {
        _tags = NormalizeTags(tags);
    }

    /// <summary>
    /// Initializes a TagList from a comma-separated string.
    /// Example input: "cms,dotnet,seo"
    /// </summary>
    public TagList(string? commaSeparated)
        : this((commaSeparated ?? string.Empty).Split(','))
    {
    }

    /// <summary>
    /// Returns the comma-separated string representation.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Returns true if this tag list equals another one (ignoring order).
    /// </summary>
    public bool Equals(TagList? other)
    {
        if (other is null || _tags.Length != other._tags.Length)
            return false;

        return !_tags.Except(other._tags).Any();
    }

    /// <summary>
    /// Compares with any object.
    /// </summary>
    public override bool Equals(object? obj) => obj is TagList other && Equals(other);

    /// <summary>
    /// Returns a hash code based on sorted tag values.
    /// </summary>
    public override int GetHashCode()
    {
        var sb = new StringBuilder();
        foreach (var tag in _tags.OrderBy(t => t))
        {
            sb.Append(tag);
        }
        return sb.ToString().GetHashCode();
    }

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(TagList? left, TagList? right) => Equals(left, right);

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(TagList? left, TagList? right) => !(left == right);

    /// <summary>
    /// Normalizes a list of tags by trimming, lowercasing, and removing duplicates.
    /// </summary>
    private static string[] NormalizeTags(IEnumerable<string> tags)
    {
        return tags
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim().ToLowerInvariant())
            .Distinct()
            .ToArray();
    }
}
