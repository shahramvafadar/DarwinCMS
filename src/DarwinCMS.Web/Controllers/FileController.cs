using System;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Services.Files;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DarwinCMS.Web.Controllers
{
    /// <summary>
    /// Serves stored files (e.g., cover images) with cache-friendly headers.
    /// </summary>
    public sealed class FileController : Controller
    {
        private readonly IFileQueryService _files;

        /// <summary>
        /// Initializes a new instance of <see cref="FileController"/>.
        /// </summary>
        public FileController(IFileQueryService files)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
        }

        /// <summary>
        /// Serves a file by id. Example: /file/{id}
        /// </summary>
        [HttpGet("file/{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var dto = await _files.GetAsync(id, ct);
            if (dto is null) return NotFound();

            if (!string.IsNullOrEmpty(dto.ETag))
                Response.Headers[HeaderNames.ETag] = dto.ETag;
            if (dto.LastModifiedUtc.HasValue)
                Response.Headers[HeaderNames.LastModified] = dto.LastModifiedUtc.Value.ToString("R");
            if (dto.MaxAgeSeconds.HasValue)
                Response.Headers[HeaderNames.CacheControl] = $"public, max-age={dto.MaxAgeSeconds.Value}";

            return File(dto.Content, dto.ContentType, enableRangeProcessing: false);
        }
    }
}
