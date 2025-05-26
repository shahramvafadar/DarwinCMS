namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a file uploaded and managed in the CMS.
/// Can be an image, video, document, or any static asset.
/// </summary>
public class MediaFile : BaseEntity
{
    /// <summary>
    /// Display name of the file (original file name or assigned title).
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Full virtual path where the file is stored (e.g. "/media/2025/05/img_01.jpg").
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// MIME type of the file (e.g. image/jpeg, video/mp4, application/pdf).
    /// </summary>
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// Size of the file in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Optional width of the file (for images and videos).
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Optional height of the file (for images and videos).
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Optional alternative text for SEO and accessibility.
    /// </summary>
    public string? AltText { get; set; }

    /// <summary>
    /// Optional caption or description shown near the media.
    /// </summary>
    public string? Caption { get; set; }

    /// <summary>
    /// Optional virtual folder name for grouping files (e.g. "blog", "products").
    /// </summary>
    public string? Folder { get; set; }

    /// <summary>
    /// Language code if the file is specific to a language (optional).
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// Indicates if the file is a system file and should not be deleted.
    /// </summary>
    public bool IsSystem { get; set; } = false;
}
