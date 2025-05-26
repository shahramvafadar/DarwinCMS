using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing uploaded media files.
/// </summary>
public interface IMediaFileRepository : IRepository<MediaFile>
{
    /// <summary>
    /// Returns all media files in a specific folder.
    /// </summary>
    Task<List<MediaFile>> GetByFolderAsync(string? folder);

    /// <summary>
    /// Gets a media file by its path (used for previews or editing).
    /// </summary>
    Task<MediaFile?> GetByPathAsync(string filePath);
}
