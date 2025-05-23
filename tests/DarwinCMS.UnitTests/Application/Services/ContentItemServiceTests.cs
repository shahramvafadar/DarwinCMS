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
/// </summary>
public class ContentItemServiceTests
{
    private readonly Mock<IContentItemRepository> _repositoryMock;
    private readonly IContentItemService _service;

    public ContentItemServiceTests()
    {
        _repositoryMock = new Mock<IContentItemRepository>();
        _service = new ContentItemService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddItem()
    {
        // Arrange
        var item = CreateTestItem();

        _repositoryMock.Setup(r => r.AddAsync(item, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(item);

        // Assert
        result.Should().Be(item.Id);
        _repositoryMock.Verify(r => r.AddAsync(item, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnItem_WhenExists()
    {
        // Arrange
        var item = CreateTestItem();
        _repositoryMock.Setup(r => r.GetByIdAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        // Act
        var result = await _service.GetByIdAsync(item.Id);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(item.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((ContentItem?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnItem_WhenExists()
    {
        // Arrange
        var slug = "sample-content";
        var lang = "en";
        var item = CreateTestItem(slug);
        _repositoryMock.Setup(r => r.GetBySlugAsync(slug, lang, It.IsAny<CancellationToken>())).ReturnsAsync(item);

        // Act
        var result = await _service.GetBySlugAsync(slug, lang);

        // Assert
        result.Should().NotBeNull();
        result?.Slug.Value.Should().Be(slug);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFilteredItems()
    {
        // Arrange
        var items = new List<ContentItem>
        {
            CreateTestItem("one"),
            CreateTestItem("two")
        };

        _repositoryMock.Setup(r => r.GetAllAsync(null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyItem()
    {
        // Arrange
        var item = CreateTestItem();
        _repositoryMock.Setup(r => r.Update(item)).Verifiable();
        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await _service.UpdateAsync(item);

        // Assert
        _repositoryMock.Verify(r => r.Update(item), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveItem()
    {
        // Arrange
        var item = CreateTestItem();
        _repositoryMock.Setup(r => r.GetByIdAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync(item);
        _repositoryMock.Setup(r => r.Delete(item)).Verifiable();
        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await _service.DeleteAsync(item.Id);

        // Assert
        _repositoryMock.Verify(r => r.Delete(item), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

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
