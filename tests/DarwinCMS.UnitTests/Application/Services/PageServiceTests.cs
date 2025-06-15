using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Pages;
using DarwinCMS.Application.Services.Pages;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Infrastructure.Services.Pages;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services
{
    /// <summary>
    /// Unit tests for the PageService class.
    /// Verifies functionality for listing, retrieving, creating, updating, deleting, and restoring CMS pages.
    /// </summary>
    public class PageServiceTests
    {
        private readonly Mock<IPageRepository> _pageRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IPageService _service;

        /// <summary>
        /// Initializes mocks and service instance.
        /// </summary>
        public PageServiceTests()
        {
            _pageRepositoryMock = new Mock<IPageRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new PageService(_pageRepositoryMock.Object, _mapperMock.Object);
        }

        /// <summary>
        /// Verifies GetListAsync returns filtered paged list.
        /// </summary>
        [Fact(DisplayName = "Should return filtered list of pages")]
        public async Task GetListAsync_ShouldReturnPaged()
        {
            var createdBy = Guid.NewGuid();
            var data = new List<Page>
            {
                new Page("About", new Slug("about"), "en", "<p>About</p>", true, createdBy)
            }.AsQueryable();

            var dtoList = new List<PageListItemDto> { new() { Title = "About" } };

            _pageRepositoryMock.Setup(r => r.Query()).Returns(data);
            _mapperMock.Setup(m => m.ConfigurationProvider).Returns(new MapperConfiguration(cfg => { }));
            _mapperMock.Setup(m => m.Map<List<PageListItemDto>>(It.IsAny<List<Page>>())).Returns(dtoList);

            var filter = new PageFilterDto { Page = 1, PageSize = 10 };
            var result = await _service.GetListAsync(filter);

            result.Should().NotBeNull();
        }

        /// <summary>
        /// Verifies retrieval of a page by ID.
        /// </summary>
        [Fact(DisplayName = "Should return page by id")]
        public async Task GetByIdAsync_ShouldReturnPage()
        {
            var id = Guid.NewGuid();
            var entity = new Page("About", new Slug("about"), "en", "<p>About</p>", true, Guid.NewGuid());
            var dto = new PageDetailDto { Title = "About" };

            _pageRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<PageDetailDto>(entity)).Returns(dto);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result?.Title.Should().Be("About");
        }

        /// <summary>
        /// Verifies that creating a page works and returns a new ID.
        /// </summary>
        [Fact(DisplayName = "Should create new page")]
        public async Task CreateAsync_ShouldAddPage()
        {
            var dto = new CreatePageDto { Title = "New Page", Slug = "new-page", LanguageCode = "en", ContentHtml = "<p>New</p>", IsPublished = true };
            var page = new Page(dto.Title, new Slug(dto.Slug), dto.LanguageCode, dto.ContentHtml, dto.IsPublished, Guid.NewGuid());

            _mapperMock.Setup(m => m.Map<Page>(dto)).Returns(page);
            _pageRepositoryMock.Setup(r => r.IsSlugUniqueAsync(dto.Slug, dto.LanguageCode, null)).ReturnsAsync(true);
            _pageRepositoryMock.Setup(r => r.AddAsync(page, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _pageRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto, page.CreatedByUserId ?? Guid.NewGuid());

            result.Should().Be(page.Id);
        }

        /// <summary>
        /// Verifies page update applies changes.
        /// </summary>
        [Fact(DisplayName = "Should update existing page")]
        public async Task UpdateAsync_ShouldUpdatePage()
        {
            var id = Guid.NewGuid();
            var dto = new UpdatePageDto { Title = "Updated", Slug = "updated", LanguageCode = "en" };
            var entity = new Page("Old", new Slug("old"), "en", "<p>Old</p>", true, Guid.NewGuid());

            _pageRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _pageRepositoryMock.Setup(r => r.IsSlugUniqueAsync(dto.Slug, dto.LanguageCode, id)).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map(dto, entity));
            _pageRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.UpdateAsync(id, dto, Guid.NewGuid());

            _pageRepositoryMock.Verify(r => r.Update(entity), Times.Once);
        }

        /// <summary>
        /// Verifies soft delete of a page.
        /// </summary>
        [Fact(DisplayName = "Should soft delete page")]
        public async Task SoftDeleteAsync_ShouldCallRepo()
        {
            await _service.SoftDeleteAsync(Guid.NewGuid(), Guid.NewGuid());

            _pageRepositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies permanent deletion of a page.
        /// </summary>
        [Fact(DisplayName = "Should hard delete page")]
        public async Task HardDeleteAsync_ShouldCallRepo()
        {
            await _service.HardDeleteAsync(Guid.NewGuid());

            _pageRepositoryMock.Verify(r => r.HardDeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Ensures soft-deleted pages are returned.
        /// </summary>
        [Fact(DisplayName = "Should return deleted pages")]
        public async Task GetDeletedAsync_ShouldReturn()
        {
            var list = new List<Page>
            {
                new Page("Trashed", new Slug("trashed"), "en", "<p>Deleted</p>", true, Guid.NewGuid())
            };

            var dtoList = new List<PageListItemDto> { new PageListItemDto { Title = "Trashed" } };

            _pageRepositoryMock.Setup(r => r.GetDeletedAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<PageListItemDto>>(list)).Returns(dtoList);

            var result = await _service.GetDeletedAsync();

            result.Should().HaveCount(1);
        }

        /// <summary>
        /// Verifies restoration of a soft-deleted page.
        /// </summary>
        [Fact(DisplayName = "Should restore page")]
        public async Task RestoreAsync_ShouldCallRepo()
        {
            await _service.RestoreAsync(Guid.NewGuid(), Guid.NewGuid());

            _pageRepositoryMock.Verify(r => r.RestoreAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
