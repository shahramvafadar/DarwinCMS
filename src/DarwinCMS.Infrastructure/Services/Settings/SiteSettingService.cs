﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.GuardClauses;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.SiteSettings;
using DarwinCMS.Application.Services.Settings;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Repositories;
using DarwinCMS.Shared.Exceptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DarwinCMS.Infrastructure.Services.Settings;

/// <summary>
/// Service implementation for managing site-wide configuration settings.
/// Includes CRUD, soft deletion, restoration, hard deletion, and caching.
/// </summary>
public class SiteSettingService : ISiteSettingService
{
    private readonly ISiteSettingRepository _repository;
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="SiteSettingService"/> class.
    /// </summary>
    public SiteSettingService(ISiteSettingRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    /// <inheritdoc/>
    public async Task<string> GetValueAsync(string key, string? languageCode = null, CancellationToken cancellationToken = default)
    {
        var setting = await GetCachedSettingAsync(key, languageCode, cancellationToken);
        if (setting == null)
            throw new NotFoundException("SiteSetting", key);

        return setting.Value;
    }

    /// <inheritdoc/>
    public async Task<T> GetValueAsAsync<T>(string key, string? languageCode = null, CancellationToken cancellationToken = default)
    {
        var value = await GetValueAsync(key, languageCode, cancellationToken);

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return default!;
        }
    }

    /// <inheritdoc/>
    public async Task<List<SiteSetting>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.Query().Where(s => !s.IsDeleted).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<SiteSetting>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetDeletedAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateValueAsync(
        string oldKey,
        string? oldLanguageCode,
        string newKey,
        string? newLanguageCode,
        string newValue,
        Guid modifiedByUserId,
        CancellationToken cancellationToken = default)
    {
        var setting = await _repository.GetByKeyAsync(oldKey, oldLanguageCode, cancellationToken)
            ?? throw new NotFoundException($"Setting not found with Key='{oldKey}' and Language='{oldLanguageCode}'", newKey);

        setting.SetKey(newKey, modifiedByUserId);
        setting.SetLanguageCode(newLanguageCode, modifiedByUserId);
        setting.SetValue(newValue, modifiedByUserId);

        _repository.Update(setting);
        await _repository.SaveChangesAsync(cancellationToken);
        // Invalidate old and new cache entries
        _cache.Remove(SiteSettingCacheKey(oldKey, oldLanguageCode));
        _cache.Remove(SiteSettingCacheKey(newKey, newLanguageCode));
    }

    /// <inheritdoc/>
    public async Task CreateAsync(SiteSetting newSetting, CancellationToken cancellationToken = default)
    {
        await _repository.AddAsync(newSetting, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);
        // Invalidate cache
        _cache.Remove(SiteSettingCacheKey(newSetting.Key, newSetting.LanguageCode));
    }

    /// <inheritdoc/>
    public async Task SoftDeleteAsync(Guid id, Guid deletedByUserId, CancellationToken cancellationToken = default)
    {
        await _repository.SoftDeleteAsync(id, deletedByUserId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        await _repository.RestoreAsync(id, userId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _repository.HardDeleteAsync(id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<SiteSettingListResultDto> GetPagedListAsync(
        string? searchTerm,
        string? sortColumn,
        string? sortDirection,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(s => !s.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s =>
                s.Key.Contains(searchTerm) ||
                (s.Category != null && s.Category.Contains(searchTerm)));
        }

        query = (sortColumn?.ToLower(), sortDirection?.ToLower()) switch
        {
            ("category", "desc") => query.OrderByDescending(s => s.Category),
            ("category", _) => query.OrderBy(s => s.Category),
            ("value", "desc") => query.OrderByDescending(s => s.Value),
            ("value", _) => query.OrderBy(s => s.Value),
            (_, "desc") => query.OrderByDescending(s => s.Key),
            _ => query.OrderBy(s => s.Key)
        };

        var totalCount = await _repository.CountAsync(query, cancellationToken);
        var items = await _repository.ToListAsync(query.Skip(skip).Take(take), cancellationToken);

        var result = new SiteSettingListResultDto
        {
            TotalCount = totalCount,
            SiteSettings = items.Select(s => new SiteSettingListDto
            {
                Id = s.Id,
                Key = s.Key,
                Category = s.Category,
                Value = s.Value,
                LanguageCode = s.LanguageCode,
                IsSystem = s.IsSystem,
                IsDeleted = s.IsDeleted
            }).ToList()
        };

        return result;
    }

    /// <summary>
    /// Retrieves a setting from cache or loads from repository.
    /// </summary>
    private async Task<SiteSetting?> GetCachedSettingAsync(string key, string? languageCode, CancellationToken cancellationToken)
    {
        string cacheKey = SiteSettingCacheKey(key, languageCode);
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return await _repository.GetByKeyAsync(key, languageCode, cancellationToken);
        });
    }

    /// <summary>
    /// Generates a unique cache key based on the setting key and language.
    /// </summary>
    private string SiteSettingCacheKey(string key, string? lang = null)
        => $"SiteSetting::{key}::{lang?.ToLower() ?? "default"}";
}
