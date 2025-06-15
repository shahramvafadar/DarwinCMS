using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.AccessControl;
using DarwinCMS.Infrastructure.Services.AccessControl;

using FluentAssertions;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for the <see cref="AuthorizationService"/> class.
/// Covers both current user and specified user permission checks.
/// </summary>
public class AuthorizationServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserMock;
    private readonly Mock<IRolePermissionRepository> _rolePermissionRepoMock;
    private readonly Mock<IUserRoleRepository> _userRoleRepoMock;
    private readonly IAuthorizationService _service;

    /// <summary>
    /// Initializes the test by mocking dependencies and creating the service instance.
    /// </summary>
    public AuthorizationServiceTests()
    {
        _currentUserMock = new Mock<ICurrentUserService>();
        _rolePermissionRepoMock = new Mock<IRolePermissionRepository>();
        _userRoleRepoMock = new Mock<IUserRoleRepository>();

        _service = new AuthorizationService(
            _currentUserMock.Object,
            _rolePermissionRepoMock.Object,
            _userRoleRepoMock.Object);
    }

    /// <summary>
    /// Should return false if the user is not authenticated.
    /// </summary>
    [Fact(DisplayName = "Should return false if user is not authenticated")]
    public async Task HasPermission_ShouldReturnFalse_WhenUserIsNotAuthenticated()
    {
        _currentUserMock.SetupGet(c => c.IsAuthenticated).Returns(false);

        var result = await _service.HasPermission("any.permission");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Should return true if current user has the specified permission.
    /// </summary>
    [Fact(DisplayName = "Should return true if current user has permission")]
    public async Task HasPermission_ShouldReturnTrue_WhenPermissionGranted()
    {
        _currentUserMock.SetupGet(c => c.IsAuthenticated).Returns(true);
        _currentUserMock.Setup(c => c.HasPermission("dashboard.view")).Returns(true);

        var result = await _service.HasPermission("dashboard.view");

        result.Should().BeTrue();
    }

    /// <summary>
    /// Should return false if user has no roles assigned.
    /// </summary>
    [Fact(DisplayName = "Should return false if user has no roles")]
    public async Task HasPermissionAsync_ShouldReturnFalse_IfUserHasNoRoles()
    {
        var userId = Guid.NewGuid();
        _userRoleRepoMock.Setup(r => r.GetRoleIdsByUserIdAsync(userId))
            .ReturnsAsync(new List<Guid>());

        var result = await _service.HasPermissionAsync(userId, "admin.access");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Should return true if any of user's roles grant the permission.
    /// </summary>
    [Fact(DisplayName = "Should return true if permission exists for any role")]
    public async Task HasPermissionAsync_ShouldReturnTrue_IfAnyRoleHasPermission()
    {
        var userId = Guid.NewGuid();
        var roleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        _userRoleRepoMock.Setup(r => r.GetRoleIdsByUserIdAsync(userId))
            .ReturnsAsync(roleIds);

        _rolePermissionRepoMock
            .Setup(r => r.DoesAnyRoleHavePermissionAsync(
                roleIds, "admin.manage", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);


        var result = await _service.HasPermissionAsync(userId, "admin.manage");

        result.Should().BeTrue();
    }

    /// <summary>
    /// Should return false if no role has the required permission.
    /// </summary>
    [Fact(DisplayName = "Should return false if no role has permission")]
    public async Task HasPermissionAsync_ShouldReturnFalse_IfNoRoleHasPermission()
    {
        var userId = Guid.NewGuid();
        var roles = new List<Guid> { Guid.NewGuid() };

        _userRoleRepoMock.Setup(r => r.GetRoleIdsByUserIdAsync(userId))
            .ReturnsAsync(roles);

        _rolePermissionRepoMock
            .Setup(r => r.DoesAnyRoleHavePermissionAsync(
                roles,
                "settings.write",
                null, // optional parameter: module
                It.IsAny<CancellationToken>() // optional parameter: cancellationToken
            ))
            .ReturnsAsync(false);


        var result = await _service.HasPermissionAsync(userId, "settings.write");

        result.Should().BeFalse();
    }
}
