using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using DarwinCMS.Infrastructure.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Implementation of ISiteSettingRepository using EF Core.
/// Supports basic CRUD, soft delete, restore, and listing deleted items.
/// </summary>
public class SiteSettingRepository : BaseRepository<SiteSetting>, ISiteSettingRepository
{
    /// <summary>
    /// Initializes the repository with the EF DbContext.
    /// </summary>
    public SiteSettingRepository(DarwinDbContext db) : base(db) { }

    /// <inheritdoc />
    public async Task<SiteSetting?> GetByKeyAsync(string key, string? languageCode, CancellationToken cancellationToken = default)
    {
        return await _set.FirstOrDefaultAsync(
            s => s.Key == key && s.LanguageCode == languageCode,
            cancellationToken);
    }


    /// <inheritdoc />
    public async Task<List<SiteSetting>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _set.Where(s => s.Category == category && !s.IsDeleted)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Marks a setting as logically deleted.
    /// </summary>
    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var setting = await _set.FindAsync(new object[] { id }, cancellationToken);
        if (setting != null)
        {
            setting.MarkAsDeleted();
            _set.Update(setting);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Restores a previously soft-deleted setting.
    /// </summary>
    public async Task RestoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var setting = await _set.FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted, cancellationToken);
        if (setting != null)
        {
            setting.Restore();
            _set.Update(setting);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Permanently deletes a setting from the database.
    /// </summary>
    public async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var setting = await _set.FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted, cancellationToken);
        if (setting != null)
        {
            _set.Remove(setting);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Retrieves all logically deleted settings.
    /// </summary>
    public async Task<List<SiteSetting>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _set.Where(s => s.IsDeleted)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync(IQueryable<SiteSetting> query, CancellationToken cancellationToken = default)
    {
        return await query.CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<SiteSetting>> ToListAsync(IQueryable<SiteSetting> query, CancellationToken cancellationToken = default)
    {
        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

}
