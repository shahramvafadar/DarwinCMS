using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Services.Users;

using FluentAssertions;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for UserRoleService, validating role assignments, removals, and lookups.
/// </summary>
public class UserRoleServiceTests
{
    private readonly Mock<IUserRoleRepository> _userRoleRepoMock;
    private readonly IUserRoleService _userRoleService;

    /// <summary>
    /// Initializes UserRoleService and its repository mock.
    /// </summary>
    public UserRoleServiceTests()
    {
        _userRoleRepoMock = new Mock<IUserRoleRepository>();
        _userRoleService = new UserRoleService(_userRoleRepoMock.Object);
    }

    /// <summary>
    /// Should assign a role to a user only if not already assigned.
    /// </summary>
    [Fact(DisplayName = "Should assign role if not exists")]
    public async Task AssignRoleAsync_ShouldAdd_WhenNotExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        _userRoleRepoMock.Setup(x => x.GetAsync(userId, roleId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserRole?)null);

        // Act
        await _userRoleService.AssignRoleAsync(userId, roleId);

        // Assert
        _userRoleRepoMock.Verify(x => x.AddAsync(It.Is<UserRole>(ur => ur.UserId == userId && ur.RoleId == roleId), It.IsAny<CancellationToken>()), Times.Once);
        _userRoleRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Should not assign a role if already assigned.
    /// </summary>
    [Fact(DisplayName = "Should not assign if already exists")]
    public async Task AssignRoleAsync_ShouldNotAdd_WhenAlreadyExists()
    {
        // Arrange
        var existing = new UserRole(Guid.NewGuid(), Guid.NewGuid());

        _userRoleRepoMock.Setup(x => x.GetAsync(existing.UserId, existing.RoleId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        await _userRoleService.AssignRoleAsync(existing.UserId, existing.RoleId);

        // Assert
        _userRoleRepoMock.Verify(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRoleRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Should unassign role from user if assignment exists.
    /// </summary>
    [Fact(DisplayName = "Should delete role if exists")]
    public async Task UnassignRoleAsync_ShouldDelete_WhenExists()
    {
        // Arrange
        var role = new UserRole(Guid.NewGuid(), Guid.NewGuid());

        _userRoleRepoMock.Setup(x => x.GetAsync(role.UserId, role.RoleId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        // Act
        await _userRoleService.UnassignRoleAsync(role.UserId, role.RoleId);

        // Assert
        _userRoleRepoMock.Verify(x => x.Delete(role), Times.Once);
        _userRoleRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Should not attempt delete if role is not assigned.
    /// </summary>
    [Fact(DisplayName = "Should do nothing if not assigned")]
    public async Task UnassignRoleAsync_ShouldDoNothing_WhenNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        _userRoleRepoMock.Setup(x => x.GetAsync(userId, roleId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserRole?)null);

        // Act
        await _userRoleService.UnassignRoleAsync(userId, roleId);

        // Assert
        _userRoleRepoMock.Verify(x => x.Delete(It.IsAny<UserRole>()), Times.Never);
        _userRoleRepoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Should return all roles assigned to a given user.
    /// </summary>
    [Fact(DisplayName = "Should return all roles for user")]
    public async Task GetRolesForUserAsync_ShouldReturnList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var list = new List<UserRole>
        {
            new(userId, Guid.NewGuid()),
            new(userId, Guid.NewGuid())
        };

        _userRoleRepoMock.Setup(x => x.GetByUserIdAsync(userId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        // Act
        var result = await _userRoleService.GetRolesForUserAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Should return true when user has the given role.
    /// </summary>
    [Fact(DisplayName = "Should return true if user has role")]
    public async Task UserHasRoleAsync_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        _userRoleRepoMock.Setup(x => x.ExistsAsync(userId, roleId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _userRoleService.UserHasRoleAsync(userId, roleId);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Should return false when user does not have the given role.
    /// </summary>
    [Fact(DisplayName = "Should return false if user does not have role")]
    public async Task UserHasRoleAsync_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        _userRoleRepoMock.Setup(x => x.ExistsAsync(userId, roleId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _userRoleService.UserHasRoleAsync(userId, roleId);

        // Assert
        result.Should().BeFalse();
    }
}
