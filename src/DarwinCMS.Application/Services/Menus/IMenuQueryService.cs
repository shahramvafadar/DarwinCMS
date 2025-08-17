using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Menus;

namespace DarwinCMS.Application.Services.Menus
{
    /// <summary>
    /// Read-only query service for retrieving a navigational menu tree.
    /// </summary>
    public interface IMenuQueryService
    {
        /// <summary>
        /// Retrieves a published menu tree for the given position code ("header" / "footer") and language code.
        /// Returns null when menu is not found or inactive.
        /// </summary>
        Task<MenuNodeDto?> GetTreeAsync(string position, string languageCode, CancellationToken ct);
    }
}
