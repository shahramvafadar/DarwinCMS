using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Services.Roles;
using FluentAssertions;
using Moq;
using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for RoleService.
/// </summary>
public class RoleServiceTests
{
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly IRoleService _roleService;

    public RoleServiceTests()
    {
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _roleService = new RoleService(_roleRepositoryMock.Object);
    }

    [Fact(DisplayName = "Should return all active roles")]
    public async Task GetAllAsync_ShouldReturnRoles()
    {
        // Arrange: create fake list of roles
        var roles = new List<Role>
        {
            new("Admin"),
            new("Editor")
        };

        _roleRepositoryMock.Setup(x => x.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(roles);

        // Act
        var result = await _roleService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(r => r.Name).Should().Contain(new[] { "Admin", "Editor" });
    }

    [Fact(DisplayName = "Should return role by id")]
    public async Task GetByIdAsync_ShouldReturnRole()
    {
        // Arrange
        var role = new Role("Support");

        _roleRepositoryMock.Setup(x => x.GetByIdAsync(role.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        // Act
        var result = await _roleService.GetByIdAsync(role.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Support");
    }

    [Fact(DisplayName = "Should return role by name")]
    public async Task GetByNameAsync_ShouldReturnRole()
    {
        // Arrange
        var role = new Role("Manager");

        _roleRepositoryMock.Setup(x => x.GetByNameAsync("Manager", It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        // Act
        var result = await _roleService.GetByNameAsync("Manager");

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Manager");
    }

    [Fact(DisplayName = "Should delete role if exists")]
    public async Task DeleteAsync_ShouldCallRepository_WhenRoleFound()
    {
        // Arrange: create role and assign ID using reflection
        var role = new Role("Temp");

        // EF Core restricts setting Id directly, so we use reflection for testing
        typeof(Role).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
            .SetValue(role, Guid.NewGuid());

        _roleRepositoryMock.Setup(x => x.GetByIdAsync(role.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        // Act
        await _roleService.DeleteAsync(role.Id);

        // Assert
        _roleRepositoryMock.Verify(x => x.Delete(role), Times.Once);
        _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should do nothing if role not found")]
    public async Task DeleteAsync_ShouldDoNothing_WhenRoleNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _roleRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role?)null);

        // Act
        await _roleService.DeleteAsync(id);

        // Assert
        _roleRepositoryMock.Verify(x => x.Delete(It.IsAny<Role>()), Times.Never);
        _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
