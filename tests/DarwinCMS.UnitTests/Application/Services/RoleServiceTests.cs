using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Roles;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Services.Roles;
using DarwinCMS.Shared.Exceptions;

using FluentAssertions;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for RoleService, focused on retrieval and hard deletion operations.
/// </summary>
public class RoleServiceTests
{
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IRoleService _roleService;

    /// <summary>
    /// Initializes all required mocks and service under test.
    /// </summary>
    public RoleServiceTests()
    {
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _userRoleRepositoryMock = new Mock<IUserRoleRepository>();
        _mapperMock = new Mock<IMapper>();

        _roleService = new RoleService(
            _roleRepositoryMock.Object,
            _userRoleRepositoryMock.Object,
            _mapperMock.Object);
    }

    /// <summary>
    /// Should return role when found by ID.
    /// </summary>
    [Fact(DisplayName = "Should return role by id")]
    public async Task GetByIdAsync_ShouldReturnRole()
    {
        // Arrange
        var role = new Role("Support", Guid.NewGuid());
        _roleRepositoryMock.Setup(x => x.GetByIdAsync(role.Id, It.IsAny<CancellationToken>())).ReturnsAsync(role);

        // Act
        var result = await _roleService.GetByIdAsync(role.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Support");
    }

    /// <summary>
    /// Should return role when found by exact name.
    /// </summary>
    [Fact(DisplayName = "Should return role by name")]
    public async Task GetByNameAsync_ShouldReturnRole()
    {
        // Arrange
        var role = new Role("Manager", Guid.NewGuid());
        _roleRepositoryMock.Setup(x => x.GetByNameAsync("Manager", It.IsAny<CancellationToken>())).ReturnsAsync(role);

        // Act
        var result = await _roleService.GetByNameAsync("Manager");

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Manager");
    }

    /// <summary>
    /// Should hard delete role if found.
    /// </summary>
    [Fact(DisplayName = "Should hard delete role if found")]
    public async Task HardDeleteAsync_ShouldDelete_WhenRoleFound()
    {
        // Arrange
        var role = new Role("Temp", Guid.NewGuid());
        typeof(Role).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
            .SetValue(role, Guid.NewGuid());

        _roleRepositoryMock.Setup(x => x.GetByIdAsync(role.Id, It.IsAny<CancellationToken>())).ReturnsAsync(role);

        // Act
        await _roleService.HardDeleteAsync(role.Id);

        // Assert
        _roleRepositoryMock.Verify(x => x.Delete(role), Times.Once);
        _roleRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Should throw BusinessRuleException when role not found for hard delete.
    /// </summary>
    [Fact(DisplayName = "Should throw exception if role not found for hard delete")]
    public async Task HardDeleteAsync_ShouldThrow_WhenRoleNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _roleRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role?)null);

        // Act
        var act = async () => await _roleService.HardDeleteAsync(id);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Role not found.");
    }

}
