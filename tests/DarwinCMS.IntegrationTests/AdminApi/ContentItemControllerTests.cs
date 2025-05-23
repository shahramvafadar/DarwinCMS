using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DarwinCMS.WebAdmin;
using DarwinCMS.Application.Services.Content;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DarwinCMS.IntegrationTests.AdminApi;

/// <summary>
/// Integration tests for ContentItemController endpoints.
/// </summary>
public class ContentItemControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IContentItemService> _serviceMock;

    public ContentItemControllerTests(WebApplicationFactory<Program> factory)
    {
        _serviceMock = new Mock<IContentItemService>();

        var app = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_serviceMock.Object);
            });
        });

        _client = app.CreateClient();
    }

    [Fact(DisplayName = "Returns item by ID when found")]
    public async Task GetById_ShouldReturnOk()
    {
        var id = Guid.NewGuid();
        var item = new ContentItem("Test", new Slug("test"), "Body", "Page", Guid.NewGuid());
        _serviceMock.Setup(s => s.GetByIdAsync(id, default)).ReturnsAsync(item);

        var response = await _client.GetAsync($"/api/admin/content-items/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(DisplayName = "Returns 404 if item not found")]
    public async Task GetById_ShouldReturnNotFound()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByIdAsync(id, default)).ReturnsAsync((ContentItem?)null);

        var response = await _client.GetAsync($"/api/admin/content-items/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Returns item by slug and language")]
    public async Task GetBySlug_ShouldReturnOk()
    {
        var slug = "test";
        var lang = "en";
        var item = new ContentItem("Test", new Slug(slug), "Body", "Page", Guid.NewGuid());
        _serviceMock.Setup(s => s.GetBySlugAsync(slug, lang, default)).ReturnsAsync(item);

        var response = await _client.GetAsync($"/api/admin/content-items/slug?slug={slug}&languageCode={lang}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(DisplayName = "Returns list of content items")]
    public async Task GetAll_ShouldReturnList()
    {
        var list = new List<ContentItem> { new("Title", new Slug("slug"), "Body", "Type", Guid.NewGuid()) };
        _serviceMock.Setup(s => s.GetAllAsync(null, null, default)).ReturnsAsync(list);

        var response = await _client.GetAsync("/api/admin/content-items");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(DisplayName = "Creates content item and returns ID")]
    public async Task Create_ShouldReturnCreated()
    {
        var item = new ContentItem("Title", new Slug("slug"), "Body", "Type", Guid.NewGuid());
        _serviceMock.Setup(s => s.CreateAsync(item, default)).ReturnsAsync(item.Id);

        var response = await _client.PostAsJsonAsync("/api/admin/content-items", item);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "Updates content item")]
    public async Task Update_ShouldReturnNoContent()
    {
        var item = new ContentItem("Title", new Slug("slug"), "Body", "Type", Guid.NewGuid());

        var response = await _client.PutAsJsonAsync("/api/admin/content-items", item);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "Deletes content item")]
    public async Task Delete_ShouldReturnNoContent()
    {
        var id = Guid.NewGuid();
        var response = await _client.DeleteAsync($"/api/admin/content-items/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
