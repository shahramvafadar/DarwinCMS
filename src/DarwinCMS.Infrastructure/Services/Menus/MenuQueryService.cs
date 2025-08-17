using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Infrastructure.EF;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Menus
{
    /// <summary>
    /// EF Core implementation of <see cref="IMenuQueryService"/>.
    /// Loads a menu by position (e.g., "header"/"footer") and language, then builds a hierarchical tree ordered by DisplayOrder.
    /// </summary>
    public sealed class MenuQueryService : IMenuQueryService
    {
        private readonly DarwinDbContext _db;

        /// <summary>
        /// Initializes a new instance of <see cref="MenuQueryService"/>.
        /// </summary>
        public MenuQueryService(DarwinDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <inheritdoc/>
        public async Task<MenuNodeDto?> GetTreeAsync(string position, string languageCode, CancellationToken ct)
        {
            var menu = await _db.Menus
                .AsNoTracking()
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m =>
                    m.Position == position &&            // Position is string: "header"/"footer"
                    m.LanguageCode == languageCode &&
                    m.IsActive, ct);

            if (menu is null)
                return null;

            // All active items ordered by DisplayOrder
            var items = menu.Items
                .Where(i => i.IsActive)
                .OrderBy(i => i.DisplayOrder)
                .ToList();

            // Preload slugs for internal links to avoid N+1
            var internalPageIds = items
                .Where(i => i.PageId.HasValue && i.LinkType == LinkType.Internal)
                .Select(i => i.PageId!.Value)
                .Distinct()
                .ToList();

            var slugByPageId = await _db.Pages
                .AsNoTracking()
                .Where(p => internalPageIds.Contains(p.Id))
                .Select(p => new { p.Id, p.SlugValue })
                .ToDictionaryAsync(x => x.Id, x => x.SlugValue, ct);

            // Split roots (ParentId == null) and build children lookup keyed by non-null Guid
            var rootItems = items.Where(i => i.ParentId == null).OrderBy(i => i.DisplayOrder).ToList();
            var childrenLookup = items
                .Where(i => i.ParentId != null)
                .GroupBy(i => i.ParentId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            MenuNodeDto MapNode(Guid itemId)
            {
                var item = items.First(x => x.Id == itemId);

                var node = new MenuNodeDto
                {
                    Title = item.Title,
                    LinkType = item.LinkType.Value, // "internal" | "external" | "module"
                    Url = item.LinkType == LinkType.External ? item.Url : null,
                    Slug = (item.LinkType == LinkType.Internal && item.PageId.HasValue && slugByPageId.TryGetValue(item.PageId.Value, out var slug))
                        ? slug
                        : null,
                    Children = new List<MenuNodeDto>()
                };

                if (childrenLookup.TryGetValue(item.Id, out var kids))
                {
                    foreach (var child in kids.OrderBy(c => c.DisplayOrder))
                        node.Children.Add(MapNode(child.Id));
                }

                return node;
            }

            var root = new MenuNodeDto
            {
                Title = menu.Title,
                LinkType = "root",
                Children = new List<MenuNodeDto>()
            };

            foreach (var top in rootItems)
                root.Children.Add(MapNode(top.Id));

            return root;
        }
    }
}
