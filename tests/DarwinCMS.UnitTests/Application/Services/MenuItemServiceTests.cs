using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Infrastructure.Services.Menus;

using FluentAssertions;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services
{
    /// <summary>
    /// Unit tests for the MenuItemService class.
    /// Covers core operations including tree building, retrieval, creation, update, deletion, and restoration.
    /// </summary>
    public class MenuItemServiceTests
    {
        /// <summary>
        /// Mock repository for simulating database access.
        /// </summary>
        private readonly Mock<IMenuItemRepository> _menuItemRepositoryMock;

        /// <summary>
        /// Mock mapper for transforming entities to DTOs.
        /// </summary>
        private readonly Mock<IMapper> _mapperMock;

        /// <summary>
        /// The service under test.
        /// </summary>
        private readonly IMenuItemService _service;

        /// <summary>
        /// Initializes all mocked dependencies and the service under test.
        /// </summary>
        public MenuItemServiceTests()
        {
            _menuItemRepositoryMock = new Mock<IMenuItemRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new MenuItemService(_menuItemRepositoryMock.Object, _mapperMock.Object);
        }

        /// <summary>
        /// Verifies that tree structure is returned correctly based on menu ID.
        /// </summary>
        [Fact(DisplayName = "Should return tree of menu items by menuId")]
        public async Task GetItemsByMenuIdAsync_ShouldReturnTree()
        {
            var menuId = Guid.NewGuid();
            var item = new MenuItem(menuId, "Home", LinkType.Internal, "/home", "fa fa-home", 1, "always", true, Guid.NewGuid());
            var dto = new MenuItemDto { Title = "Home" };

            _menuItemRepositoryMock.Setup(r => r.GetByMenuIdAsync(menuId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MenuItem> { item });
            _mapperMock.Setup(m => m.Map<MenuItemDto>(item)).Returns(dto);

            var result = await _service.GetItemsByMenuIdAsync(menuId);

            result.Should().ContainSingle();
        }

        /// <summary>
        /// Verifies correct retrieval of item by ID.
        /// </summary>
        [Fact(DisplayName = "Should return menu item by id")]
        public async Task GetByIdAsync_ShouldReturnItem()
        {
            var id = Guid.NewGuid();
            var item = new MenuItem(Guid.NewGuid(), "Home", LinkType.Internal, "/home", "fa fa-home", 1, "always", true, Guid.NewGuid());
            var dto = new MenuItemDto { Title = "Home" };

            _menuItemRepositoryMock.Setup(r => r.GetWithChildrenAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(item);
            _mapperMock.Setup(m => m.Map<MenuItemDto>(item)).Returns(dto);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
        }

        /// <summary>
        /// Ensures menu item is added correctly.
        /// </summary>
        [Fact(DisplayName = "Should create new menu item")]
        public async Task CreateAsync_ShouldAdd()
        {
            var dto = new CreateMenuItemDto { Title = "Home" };
            var entity = new MenuItem(Guid.NewGuid(), "Home", LinkType.Internal, "/home", "fa fa-home", 1, "always", true, Guid.NewGuid());

            _mapperMock.Setup(m => m.Map<MenuItem>(dto)).Returns(entity);
            _menuItemRepositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _menuItemRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var id = await _service.CreateAsync(dto, Guid.NewGuid());

            id.Should().Be(entity.Id);
        }

        /// <summary>
        /// Verifies update is applied to an existing menu item.
        /// </summary>
        [Fact(DisplayName = "Should update menu item")]
        public async Task UpdateAsync_ShouldUpdate()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateMenuItemDto { Id = id, Title = "Updated" };
            var entity = new MenuItem(Guid.NewGuid(), "Old", LinkType.Internal, "/old", "fa fa-edit", 2, "always", true, Guid.NewGuid());

            _menuItemRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _menuItemRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.UpdateAsync(dto, Guid.NewGuid());

            _menuItemRepositoryMock.Verify(r => r.Update(entity), Times.Once);
        }

        /// <summary>
        /// Verifies soft delete method on repository is called.
        /// </summary>
        [Fact(DisplayName = "Should soft delete item")]
        public async Task SoftDeleteAsync_ShouldCallRepo()
        {
            _menuItemRepositoryMock.Setup(r => r.SoftDeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.SoftDeleteAsync(Guid.NewGuid(), Guid.NewGuid());

            _menuItemRepositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies restoration of a soft-deleted item.
        /// </summary>
        [Fact(DisplayName = "Should restore item")]
        public async Task RestoreAsync_ShouldCallRepo()
        {
            _menuItemRepositoryMock.Setup(r => r.RestoreAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.RestoreAsync(Guid.NewGuid(), Guid.NewGuid());

            _menuItemRepositoryMock.Verify(r => r.RestoreAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies permanent deletion of a menu item.
        /// </summary>
        [Fact(DisplayName = "Should hard delete item")]
        public async Task HardDeleteAsync_ShouldCallRepo()
        {
            _menuItemRepositoryMock.Setup(r => r.HardDeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.HardDeleteAsync(Guid.NewGuid());

            _menuItemRepositoryMock.Verify(r => r.HardDeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Ensures retrieval of all soft-deleted items.
        /// </summary>
        [Fact(DisplayName = "Should return deleted items")]
        public async Task GetDeletedAsync_ShouldReturn()
        {
            var items = new List<MenuItem> { new(Guid.NewGuid(), "Trash", LinkType.Internal, "/trash", "fa fa-trash", 3, "always", false, Guid.NewGuid()) };
            _menuItemRepositoryMock.Setup(r => r.GetDeletedAsync(It.IsAny<CancellationToken>())).ReturnsAsync(items);
            _mapperMock.Setup(m => m.Map<List<MenuItemDto>>(items)).Returns(new List<MenuItemDto>());

            var result = await _service.GetDeletedAsync();

            result.Should().NotBeNull();
        }
    }
}
