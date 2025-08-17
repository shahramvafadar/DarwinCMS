using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Files;

namespace DarwinCMS.Application.Services.Files
{
    /// <summary>
    /// Read-only service for serving stored files (e.g., cover/OG images) to the public website.
    /// </summary>
    public interface IFileQueryService
    {
        /// <summary>
        /// Retrieves a binary file (stream + metadata) by its identifier.
        /// Returns null if not found or not available for public serving.
        /// </summary>
        Task<FileBinaryDto?> GetAsync(Guid id, CancellationToken ct);
    }
}
