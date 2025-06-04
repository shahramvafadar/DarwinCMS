using System;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a file uploaded and managed in the CMS.
/// Can be an image, video, document, script, or any static asset.
/// </summary>
public class StoredFile : BaseEntity
{
    /// <summary>
    /// Display name of the file (original file name or assigned title).
    /// </summary>
    public string FileName { get; private set; } = string.Empty;

    /// <summary>
    /// Full virtual path where the file is stored (e.g. "/media/2025/05/img_01.jpg").
    /// </summary>
    public string FilePath { get; private set; } = string.Empty;

    /// <summary>
    /// MIME type of the file (e.g. image/jpeg, video/mp4, application/pdf).
    /// </summary>
    public string MimeType { get; private set; } = string.Empty;

    /// <summary>
    /// Size of the file in bytes.
    /// </summary>
    public long FileSize { get; private set; }

    /// <summary>
    /// Optional width of the file (for images and videos).
    /// </summary>
    public int? Width { get; private set; }

    /// <summary>
    /// Optional height of the file (for images and videos).
    /// </summary>
    public int? Height { get; private set; }

    /// <summary>
    /// Optional alternative text for SEO and accessibility.
    /// </summary>
    public string? AltText { get; private set; }

    /// <summary>
    /// Optional caption or description shown near the media.
    /// </summary>
    public string? Caption { get; private set; }

    /// <summary>
    /// Optional virtual folder name for grouping files (e.g. "blog", "products").
    /// </summary>
    public string? Folder { get; private set; }

    /// <summary>
    /// Language code if the file is specific to a language (optional).
    /// </summary>
    public string? LanguageCode { get; private set; }

    /// <summary>
    /// Indicates if the file is a system file and should not be deleted.
    /// </summary>
    public bool IsSystem { get; private set; } = false;

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected StoredFile() { }

    /// <summary>
    /// Initializes a new StoredFile with mandatory fields.
    /// </summary>
    /// <param name="fileName">Original or display name of the file</param>
    /// <param name="filePath">Virtual path where the file is stored</param>
    /// <param name="mimeType">MIME type of the file</param>
    /// <param name="fileSize">File size in bytes</param>
    /// <param name="createdByUserId">ID of the user who uploaded the file</param>
    public StoredFile(string fileName, string filePath, string mimeType, long fileSize, Guid createdByUserId)
    {
        SetFileName(fileName);
        SetFilePath(filePath);
        SetMimeType(mimeType);
        FileSize = fileSize;
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Sets or updates the file name.
    /// </summary>
    public void SetFileName(string name)
    {
        FileName = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Required", nameof(name)) : name.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets or updates the file path.
    /// </summary>
    public void SetFilePath(string path)
    {
        FilePath = string.IsNullOrWhiteSpace(path) ? throw new ArgumentException("Required", nameof(path)) : path.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets or updates the MIME type of the file.
    /// </summary>
    public void SetMimeType(string mime)
    {
        MimeType = string.IsNullOrWhiteSpace(mime) ? throw new ArgumentException("Required", nameof(mime)) : mime.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the width and height for media files (images/videos).
    /// </summary>
    public void SetDimensions(int? width, int? height)
    {
        Width = width;
        Height = height;
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the alternative text used for accessibility or SEO.
    /// </summary>
    public void SetAltText(string? text)
    {
        AltText = text?.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the optional caption shown alongside the file.
    /// </summary>
    public void SetCaption(string? caption)
    {
        Caption = caption?.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the virtual folder used for grouping or categorization.
    /// </summary>
    public void SetFolder(string? folder)
    {
        Folder = folder?.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the language code if the file is language-specific.
    /// </summary>
    public void SetLanguageCode(string? code)
    {
        LanguageCode = code?.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Marks the file as a system-level asset that should not be deleted.
    /// </summary>
    public void MarkAsSystem(Guid? modifierUserId = null)
    {
        IsSystem = true;
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Marks this file as logically deleted.
    /// </summary>
    /// <param name="modifierUserId">ID of the user deleting</param>
    public void MarkAsDeleted(Guid? modifierUserId)
    {
        IsDeleted = true;
        MarkAsModified(modifierUserId, isDeleted: true);
    }

    /// <summary>
    /// Restores this file from a logically deleted state.
    /// </summary>
    /// <param name="modifierUserId">ID of the user restoring</param>
    public void Restore(Guid? modifierUserId)
    {
        IsDeleted = false;
        MarkAsModified(modifierUserId, isDeleted: false);
    }

    /// <summary>
    /// Sets the last modifying user's ID.
    /// </summary>
    public void SetModifiedBy(Guid modifierId)
    {
        ModifiedByUserId = modifierId;
        MarkAsModified(modifierId);
    }
}
