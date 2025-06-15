using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Permissions;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Services.Permissions;

using FluentAssertions;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for the PermissionService covering core CRUD behaviors.
/// </summary>
public class PermissionServiceTests
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IPermissionService _permissionService;

    /// <summary>
    /// Initializes mocks and service under test.
    /// </summary>
    public PermissionServiceTests()
    {
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _mapperMock = new Mock<IMapper>();
        _permissionService = new PermissionService(_permissionRepositoryMock.Object, _mapperMock.Object);
    }

    /// <summary>
    /// Should return all available permissions in the system.
    /// </summary>
    [Fact(DisplayName = "Should return all permissions")]
    public async Task GetAllAsync_ShouldReturnPermissions()
    {
        // Arrange
        var list = new List<Permission>
        {
            new("ManageUsers", Guid.NewGuid()),
            new("EditContent", Guid.NewGuid())
        };

        _permissionRepositoryMock
            .Setup(x => x.GetAllAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        // Act
        var result = await _permissionService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(p => p.Name).Should().Contain(new[] { "ManageUsers", "EditContent" });
    }

    /// <summary>
    /// Should return a permission by ID when it exists.
    /// </summary>
    [Fact(DisplayName = "Should return permission by id")]
    public async Task GetByIdAsync_ShouldReturnPermission()
    {
        // Arrange
        var permission = new Permission("ViewDashboard", Guid.NewGuid());

        _permissionRepositoryMock
            .Setup(x => x.GetByIdAsync(permission.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        var result = await _permissionService.GetByIdAsync(permission.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("ViewDashboard");
    }

    /// <summary>
    /// Should return a permission by its unique name.
    /// </summary>
    [Fact(DisplayName = "Should return permission by name")]
    public async Task GetByNameAsync_ShouldReturnPermission()
    {
        // Arrange
        var permission = new Permission("ExportData", Guid.NewGuid());

        _permissionRepositoryMock
            .Setup(x => x.GetByNameAsync("ExportData", It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        var result = await _permissionService.GetByNameAsync("ExportData");

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("ExportData");
    }

    /// <summary>
    /// Should hard delete a permission if found by ID.
    /// </summary>
    [Fact(DisplayName = "Should hard delete permission if found")]
    public async Task HardDeleteAsync_ShouldDelete_WhenPermissionFound()
    {
        // Arrange
        var permission = new Permission("TempDelete", Guid.NewGuid());
        typeof(Permission).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
            .SetValue(permission, Guid.NewGuid());

        _permissionRepositoryMock
            .Setup(x => x.GetByIdAsync(permission.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        await _permissionService.HardDeleteAsync(permission.Id);

        // Assert
        _permissionRepositoryMock.Verify(x => x.Delete(permission), Times.Once);
        _permissionRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Should not attempt hard deletion if permission does not exist.
    /// </summary>
    [Fact(DisplayName = "Should not call hard delete if permission not found")]
    public async Task HardDeleteAsync_ShouldDoNothing_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _permissionRepositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission?)null);

        // Act
        await _permissionService.HardDeleteAsync(id);

        // Assert
        _permissionRepositoryMock.Verify(x => x.Delete(It.IsAny<Permission>()), Times.Never);
        _permissionRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
