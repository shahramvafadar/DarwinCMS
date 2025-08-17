using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Files;
using DarwinCMS.Application.Services.Files;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Files
{
    /// <summary>
    /// EF Core implementation of <see cref="IFileQueryService"/>.
    /// Serves files from physical storage using the StoredFile entity's FilePath/MimeType/FileName.
    /// </summary>
    public sealed class FileQueryService : IFileQueryService
    {
        private readonly DarwinDbContext _db;

        /// <summary>
        /// Initializes a new instance of <see cref="FileQueryService"/>.
        /// </summary>
        public FileQueryService(DarwinDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <inheritdoc/>
        public async Task<FileBinaryDto?> GetAsync(Guid id, CancellationToken ct)
        {
            var f = await _db.StoredFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (f is null || string.IsNullOrWhiteSpace(f.FilePath) || !File.Exists(f.FilePath))
                return null;

            var fi = new FileInfo(f.FilePath);
            var stream = new FileStream(f.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new FileBinaryDto
            {
                FileName = f.FileName,
                ContentType = string.IsNullOrWhiteSpace(f.MimeType) ? "application/octet-stream" : f.MimeType,
                Content = stream,
                LastModifiedUtc = fi.LastWriteTimeUtc,
                ETag = $"\"{fi.LastWriteTimeUtc.Ticks}-{fi.Length}\"",
                MaxAgeSeconds = 86400 // 1 day default; adjust via settings if needed
            };
        }
    }
}
