using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Repository interface for managing uploaded media files.
/// </summary>
public interface IStoredFileRepository : IRepository<StoredFile>
{
    /// <summary>
    /// Returns all media files in a specific folder.
    /// </summary>
    Task<List<StoredFile>> GetByFolderAsync(string? folder);

    /// <summary>
    /// Gets a media file by its path (used for previews or editing).
    /// </summary>
    Task<StoredFile?> GetByPathAsync(string filePath);
}
