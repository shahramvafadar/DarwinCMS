using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Content;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Infrastructure.Services.Content;
using FluentAssertions;
using Moq;
using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for ContentItemService logic.
/// Covers creation, retrieval, update, deletion, and filtering.
/// </summary>
public class ContentItemServiceTests
{
    private readonly Mock<IContentItemRepository> _repositoryMock;
    private readonly IContentItemService _service;

    /// <summary>
    /// Initializes the test class and mocks.
    /// </summary>
    public ContentItemServiceTests()
    {
        _repositoryMock = new Mock<IContentItemRepository>();
        _service = new ContentItemService(_repositoryMock.Object);
    }

    /// <summary>
    /// Should add a new content item and return its ID.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldAddItem()
    {
        var item = CreateTestItem();

        _repositoryMock.Setup(r => r.AddAsync(item, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);


        var result = await _service.CreateAsync(item);

        result.Should().Be(item.Id);
        _repositoryMock.Verify(r => r.AddAsync(item, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Should return content item by ID when it exists.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnItem_WhenExists()
    {
        var item = CreateTestItem();
        _repositoryMock.Setup(r => r.GetByIdAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        var result = await _service.GetByIdAsync(item.Id);

        result.Should().NotBeNull();
        result?.Id.Should().Be(item.Id);
    }

    /// <summary>
    /// Should return null if item with given ID does not exist.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((ContentItem?)null);

        var result = await _service.GetByIdAsync(id);

        result.Should().BeNull();
    }

    /// <summary>
    /// Should return content item by slug and language when it exists.
    /// </summary>
    [Fact]
    public async Task GetBySlugAsync_ShouldReturnItem_WhenExists()
    {
        var slug = "sample-content";
        var lang = "en";
        var item = CreateTestItem(slug);
        _repositoryMock.Setup(r => r.GetBySlugAsync(slug, lang, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        var result = await _service.GetBySlugAsync(slug, lang);

        result.Should().NotBeNull();
        result?.Slug.Value.Should().Be(slug);
    }

    /// <summary>
    /// Should return list of content items (filtered or all).
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredItems()
    {
        var items = new List<ContentItem>
        {
            CreateTestItem("one"),
            CreateTestItem("two")
        };

        _repositoryMock.Setup(r => r.GetAllAsync(null, null, It.IsAny<CancellationToken>())).ReturnsAsync(items);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Should update existing content item and persist changes.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldModifyItem()
    {
        var item = CreateTestItem();
        _repositoryMock.Setup(r => r.Update(item)).Verifiable();
        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

        await _service.UpdateAsync(item);

        _repositoryMock.Verify(r => r.Update(item), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Should delete the content item by ID if found.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveItem()
    {
        var item = CreateTestItem();
        _repositoryMock.Setup(r => r.GetByIdAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync(item);
        _repositoryMock.Setup(r => r.Delete(item)).Verifiable();
        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

        await _service.DeleteAsync(item.Id);

        _repositoryMock.Verify(r => r.Delete(item), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Creates a sample ContentItem instance for use in tests.
    /// </summary>
    private static ContentItem CreateTestItem(string slug = "sample-content")
    {
        return new ContentItem(
            title: "Sample Title",
            slug: new Slug(slug),
            body: "Sample body",
            contentType: "Page",
            createdByUserId: Guid.NewGuid(),
            languageCode: "en",
            summary: "Summary",
            tags: new TagList("tag1,tag2"),
            metadataJson: "{\"seoTitle\":\"Sample\"}"
        );
    }
}
