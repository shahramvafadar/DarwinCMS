﻿using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Repository for managing uploaded files and their metadata.
/// </summary>
public class StoredFileRepository : BaseRepository<StoredFile>, IStoredFileRepository
{
    /// <summary>
    /// Initializes a new instance of the StoredFile repository.
    /// </summary>
    /// <param name="db">Darwin CMS database context.</param>
    public StoredFileRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<List<StoredFile>> GetByFolderAsync(string? folder)
    {
        return await _set
            .Where(f => f.Folder == folder || (folder == null && f.Folder == null))
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<StoredFile?> GetByPathAsync(string filePath)
    {
        return await _set.FirstOrDefaultAsync(f => f.FilePath == filePath);
    }
}
