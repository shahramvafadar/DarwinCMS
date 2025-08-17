using System;
using System.IO;

namespace DarwinCMS.Application.DTOs.Files
{
    /// <summary>
    /// Binary payload + metadata for serving a stored file to clients.
    /// Stream is owned by the caller and must be disposed by the web layer after use.
    /// </summary>
    public sealed class FileBinaryDto
    {
        /// <summary>Original file name (for Content-Disposition or logging).</summary>
        public string FileName { get; set; } = "file";

        /// <summary>MIME content type (e.g., "image/jpeg").</summary>
        public string ContentType { get; set; } = "application/octet-stream";

        /// <summary>Binary content stream. The stream position is set to 0.</summary>
        public Stream Content { get; set; } = Stream.Null;

        /// <summary>ETag value for client-side caching (optional).</summary>
        public string? ETag { get; set; }

        /// <summary>Last modified timestamp for caching and conditional requests (UTC).</summary>
        public DateTime? LastModifiedUtc { get; set; }

        /// <summary>Optional cache max-age in seconds.</summary>
        public int? MaxAgeSeconds { get; set; }
    }
}
