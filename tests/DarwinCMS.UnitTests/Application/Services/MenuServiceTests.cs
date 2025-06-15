using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.Services.Menus;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Services.Menus;
using DarwinCMS.UnitTests.Helpers;

using FluentAssertions;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services
{
    /// <summary>
    /// Unit tests for the MenuService class.
    /// Covers creation, update, retrieval, soft deletion, restoration, and hard deletion.
    /// </summary>
    public class MenuServiceTests
    {
        private readonly Mock<IMenuRepository> _menuRepoMock;
        private readonly Mock<IMenuItemRepository> _menuItemRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IMenuService _menuService;

        /// <summary>
        /// Initializes all required dependencies.
        /// </summary>
        public MenuServiceTests()
        {
            _menuRepoMock = new Mock<IMenuRepository>();
            _menuItemRepoMock = new Mock<IMenuItemRepository>();
            _mapperMock = new Mock<IMapper>();

            _menuService = new MenuService(
                _menuRepoMock.Object,
                _menuItemRepoMock.Object,
                _mapperMock.Object);
        }

        /// <summary>
        /// Ensures that the service returns all active (non-deleted) menus using mocked IQueryable Menu>.
        /// </summary>
        [Fact(DisplayName = "Should return all active menus")]
        public async Task GetAllAsync_ShouldReturnMenus()
        {
            // Arrange: real domain objects
            var menuList = new List<Menu>
            {
                new Menu("Main", "top", "en", Guid.NewGuid())
            };

            _menuRepoMock.Setup(r => r.Query())
                .Returns(menuList.AsQueryable().BuildMockDbSet().Object); // must be domain, not DTO

            _mapperMock.Setup(m => m.Map<MenuListItemDto>(It.IsAny<Menu>()))
                .Returns((Menu m) => new MenuListItemDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Position = m.Position,
                    LanguageCode = m.LanguageCode,
                    IsActive = m.IsActive
                });

            // Act
            var result = await _menuService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Title.Should().Be("Main");
        }

        /// <summary>
        /// Ensures GetByIdAsync returns mapped menu including ordered items.
        /// </summary>
        [Fact(DisplayName = "Should return menu detail by id")]
        public async Task GetByIdAsync_ShouldReturnMenu()
        {
            var id = Guid.NewGuid();
            var menu = new Menu("Main", "top", "en", Guid.NewGuid());
            _menuRepoMock.Setup(r => r.Query())
                .Returns(new List<Menu> { menu }.AsQueryable().BuildMockDbSet().Object);

            _mapperMock.Setup(m => m.Map<MenuDetailDto>(menu)).Returns(new MenuDetailDto());

            var result = await _menuService.GetByIdAsync(id);

            result.Should().NotBeNull();
        }

        /// <summary>
        /// Ensures CreateAsync creates and saves a new menu.
        /// </summary>
        [Fact(DisplayName = "Should create new menu")]
        public async Task CreateAsync_ShouldAddMenu()
        {
            var dto = new CreateMenuDto { Title = "Footer", Position = "bottom", LanguageCode = "en" };
            var menu = new Menu("Footer", "bottom", "en", Guid.NewGuid());

            _mapperMock.Setup(m => m.Map<Menu>(dto)).Returns(menu);
            _menuRepoMock.Setup(r => r.AddAsync(menu, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _menuRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var id = await _menuService.CreateAsync(dto, Guid.NewGuid());

            id.Should().Be(menu.Id);
        }

        /// <summary>
        /// Ensures UpdateAsync modifies menu and replaces items.
        /// </summary>
        [Fact(DisplayName = "Should update menu and items")]
        public async Task UpdateAsync_ShouldModifyMenu()
        {
            var id = Guid.NewGuid();
            var menu = new Menu("Main", "top", "en", Guid.NewGuid());
            var dto = new UpdateMenuDto { Title = "Main", Position = "top", LanguageCode = "en", Items = [] };

            _menuRepoMock.Setup(r => r.Query())
                .Returns(new List<Menu> { menu }.AsQueryable().BuildMockDbSet().Object);

            _mapperMock.Setup(m => m.Map<List<MenuItem>>(dto.Items)).Returns(new List<MenuItem>());
            _menuRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _menuService.UpdateAsync(id, dto, Guid.NewGuid());

            _menuRepoMock.Verify(r => r.Update(menu), Times.Once);
        }

        /// <summary>
        /// Ensures soft delete delegates to repository.
        /// </summary>
        [Fact(DisplayName = "Should soft delete menu")]
        public async Task SoftDeleteAsync_ShouldCallRepo()
        {
            _menuRepoMock.Setup(r => r.SoftDeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _menuService.SoftDeleteAsync(Guid.NewGuid(), Guid.NewGuid());

            _menuRepoMock.Verify(r => r.SoftDeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Ensures restoration of soft-deleted menu.
        /// </summary>
        [Fact(DisplayName = "Should restore menu")]
        public async Task RestoreAsync_ShouldCallRepo()
        {
            _menuRepoMock.Setup(r => r.RestoreAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _menuService.RestoreAsync(Guid.NewGuid(), Guid.NewGuid());

            _menuRepoMock.Verify(r => r.RestoreAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Ensures permanent deletion is invoked.
        /// </summary>
        [Fact(DisplayName = "Should hard delete menu")]
        public async Task HardDeleteAsync_ShouldCallRepo()
        {
            _menuRepoMock.Setup(r => r.HardDeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _menuService.HardDeleteAsync(Guid.NewGuid());

            _menuRepoMock.Verify(r => r.HardDeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Ensures retrieval of logically deleted menus.
        /// </summary>
        [Fact(DisplayName = "Should return deleted menus")]
        public async Task GetDeletedAsync_ShouldReturnList()
        {
            var menus = new List<Menu> { new("Trash", "bottom", "en", Guid.NewGuid()) };
            _menuRepoMock.Setup(r => r.GetDeletedAsync(It.IsAny<CancellationToken>())).ReturnsAsync(menus);
            _mapperMock.Setup(m => m.Map<List<MenuListItemDto>>(menus)).Returns(new List<MenuListItemDto>());

            var result = await _menuService.GetDeletedAsync();

            result.Should().NotBeNull();
        }
    }
}
