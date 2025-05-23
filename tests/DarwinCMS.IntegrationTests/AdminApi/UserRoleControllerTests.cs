using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DarwinCMS.AdminApi;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DarwinCMS.IntegrationTests.AdminApi;

/// <summary>
/// Integration tests for UserRoleController API endpoints.
/// </summary>
public class UserRoleControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IUserRoleService> _serviceMock;

    /// <summary>
    /// Initializes test environment with mocked IUserRoleService.
    /// </summary>
    public UserRoleControllerTests(WebApplicationFactory<Program> factory)
    {
        _serviceMock = new Mock<IUserRoleService>();

        var app = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Inject mock implementation of IUserRoleService
                services.AddSingleton(_serviceMock.Object);
            });
        });

        _client = app.CreateClient();
    }

    /// <summary>
    /// Verifies that assigning a role returns HTTP 204.
    /// </summary>
    [Fact(DisplayName = "Assigns role successfully")]
    public async Task AssignRole_ShouldReturnNoContent()
    {
        // Act
        var response = await _client.PostAsync($"/api/admin/user-roles/assign?userId={Guid.NewGuid()}&roleId={Guid.NewGuid()}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Verifies that unassigning a role returns HTTP 204.
    /// </summary>
    [Fact(DisplayName = "Unassigns role successfully")]
    public async Task UnassignRole_ShouldReturnNoContent()
    {
        // Act
        var response = await _client.PostAsync($"/api/admin/user-roles/unassign?userId={Guid.NewGuid()}&roleId={Guid.NewGuid()}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Verifies that the endpoint returns the list of roles assigned to a user.
    /// </summary>
    [Fact(DisplayName = "Gets assigned roles")]
    public async Task GetRoles_ShouldReturnRoles()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roles = new List<UserRole> { new(userId, Guid.NewGuid()) };

        _serviceMock.Setup(s => s.GetRolesForUserAsync(userId, null, default)).ReturnsAsync(roles);

        // Act
        var response = await _client.GetAsync($"/api/admin/user-roles/by-user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<UserRole>>();
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

    /// <summary>
    /// Verifies that the endpoint returns true if user has the role.
    /// </summary>
    [Fact(DisplayName = "Checks user role assignment")]
    public async Task HasRole_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        _serviceMock.Setup(s => s.UserHasRoleAsync(userId, roleId, null, default)).ReturnsAsync(true);

        // Act
        var response = await _client.GetAsync($"/api/admin/user-roles/has-role?userId={userId}&roleId={roleId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<bool>();
        result.Should().BeTrue();
    }
}
