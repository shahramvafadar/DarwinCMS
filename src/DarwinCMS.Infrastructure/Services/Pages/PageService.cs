using AutoMapper;
using AutoMapper.QueryableExtensions;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Pages;
using DarwinCMS.Application.Services.Pages;
using DarwinCMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Pages;

/// <summary>
/// Application service for managing CMS Page entities.
/// Handles creation, updating, deletion, soft deletion, restoration, and filtering.
/// </summary>
public class PageService : IPageService
{
    private readonly IPageRepository _pageRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the PageService class.
    /// </summary>
    /// <param name="pageRepository">Page repository instance for persistence.</param>
    /// <param name="mapper">AutoMapper instance for object mapping.</param>
    public PageService(IPageRepository pageRepository, IMapper mapper)
    {
        _pageRepository = pageRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<List<PageListItemDto>> GetListAsync(PageFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = _pageRepository.Query().Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.LanguageCode))
            query = query.Where(p => p.LanguageCode == filter.LanguageCode);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.ToLowerInvariant();
            query = query.Where(p =>
                p.Title.ToLowerInvariant().Contains(term) ||
                p.Slug.Value.ToLowerInvariant().Contains(term));
        }

        if (filter.IsPublished != null)
            query = query.Where(p => p.IsPublished == filter.IsPublished);

        // Sort by publish date descending by default
        query = query.OrderByDescending(p => p.PublishDateUtc ?? p.CreatedAt);

        var skip = (filter.Page - 1) * filter.PageSize;

        return await query
            .Skip(skip)
            .Take(filter.PageSize)
            .ProjectTo<PageListItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PageDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _pageRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<PageDetailDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateAsync(CreatePageDto input, Guid createdByUserId, CancellationToken cancellationToken = default)
    {
        var page = _mapper.Map<Page>(input);
        page.MarkAsCreated(createdByUserId);

        var isUnique = await _pageRepository.IsSlugUniqueAsync(page.Slug.Value, page.LanguageCode);
        if (!isUnique)
            throw new InvalidOperationException("A page with the same slug already exists.");

        await _pageRepository.AddAsync(page, cancellationToken);
        await _pageRepository.SaveChangesAsync(cancellationToken);

        return page.Id;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Guid id, UpdatePageDto input, Guid modifiedByUserId, CancellationToken cancellationToken = default)
    {
        var existing = await _pageRepository.GetByIdAsync(id, cancellationToken);
        if (existing == null)
            throw new InvalidOperationException("Page not found.");

        var isUnique = await _pageRepository.IsSlugUniqueAsync(input.Slug, input.LanguageCode, id);
        if (!isUnique)
            throw new InvalidOperationException("Slug must be unique.");

        _mapper.Map(input, existing);
        existing.SetModifiedBy(modifiedByUserId);

        _pageRepository.Update(existing);
        await _pageRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SoftDeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        await _pageRepository.SoftDeleteAsync(id, userId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _pageRepository.HardDeleteAsync(id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<PageListItemDto>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        var deletedPages = await _pageRepository.GetDeletedAsync(cancellationToken);
        return _mapper.Map<List<PageListItemDto>>(deletedPages);
    }

    /// <inheritdoc/>
    public async Task RestoreAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        await _pageRepository.RestoreAsync(id, userId, cancellationToken);
    }
}
