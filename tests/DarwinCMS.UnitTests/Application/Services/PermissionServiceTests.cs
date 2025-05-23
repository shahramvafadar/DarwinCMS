using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Permissions;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Services.Permissions;
using FluentAssertions;
using Moq;
using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for PermissionService.
/// </summary>
public class PermissionServiceTests
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly IPermissionService _permissionService;

    public PermissionServiceTests()
    {
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _permissionService = new PermissionService(_permissionRepositoryMock.Object);
    }

    [Fact(DisplayName = "Should return all permissions")]
    public async Task GetAllAsync_ShouldReturnPermissions()
    {
        // Arrange
        var list = new List<Permission>
        {
            new("ManageUsers"),
            new("EditContent")
        };

        _permissionRepositoryMock.Setup(x => x.GetAllAsync(null, It.IsAny<CancellationToken>()))
                                            .ReturnsAsync(list);


        // Act
        var result = await _permissionService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(p => p.Name).Should().Contain(new[] { "ManageUsers", "EditContent" });
    }

    [Fact(DisplayName = "Should return permission by id")]
    public async Task GetByIdAsync_ShouldReturnPermission()
    {
        // Arrange
        var permission = new Permission("ViewDashboard");

        _permissionRepositoryMock.Setup(x => x.GetByIdAsync(permission.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        var result = await _permissionService.GetByIdAsync(permission.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("ViewDashboard");
    }

    [Fact(DisplayName = "Should return permission by name")]
    public async Task GetByNameAsync_ShouldReturnPermission()
    {
        // Arrange
        var permission = new Permission("ExportData");

        _permissionRepositoryMock.Setup(x => x.GetByNameAsync("ExportData", It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        var result = await _permissionService.GetByNameAsync("ExportData");

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("ExportData");
    }

    [Fact(DisplayName = "Should delete permission if found")]
    public async Task DeleteAsync_ShouldCallDelete_WhenPermissionFound()
    {
        // Arrange: create a permission and assign ID using reflection
        var permission = new Permission("TempDelete");

        typeof(Permission).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
            .SetValue(permission, Guid.NewGuid());

        _permissionRepositoryMock.Setup(x => x.GetByIdAsync(permission.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        await _permissionService.DeleteAsync(permission.Id);

        // Assert
        _permissionRepositoryMock.Verify(x => x.Delete(permission), Times.Once);
        _permissionRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should not call delete if permission not found")]
    public async Task DeleteAsync_ShouldDoNothing_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _permissionRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission?)null);

        // Act
        await _permissionService.DeleteAsync(id);

        // Assert
        _permissionRepositoryMock.Verify(x => x.Delete(It.IsAny<Permission>()), Times.Never);
        _permissionRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
