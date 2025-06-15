using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.SiteSettings;
using DarwinCMS.Application.Services.Settings;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Services.Settings;
using DarwinCMS.UnitTests.Helpers;

using FluentAssertions;

using Microsoft.Extensions.Caching.Memory;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services
{
    /// <summary>
    /// Unit tests for the SiteSettingService class, covering all core operations including CRUD, caching, and logical deletion.
    /// </summary>
    public class SiteSettingServiceTests
    {
        private readonly Mock<ISiteSettingRepository> _repositoryMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly ISiteSettingService _service;

        /// <summary>
        /// Initializes mocks and the service under test.
        /// </summary>
        public SiteSettingServiceTests()
        {
            _repositoryMock = new Mock<ISiteSettingRepository>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _service = new SiteSettingService(_repositoryMock.Object, _memoryCacheMock.Object);
        }

        /// <summary>
        /// Ensures value is retrieved from repository and returned correctly via cache.
        /// </summary>
        [Fact(DisplayName = "Should return value for existing key")]
        public async Task GetValueAsync_ShouldReturnValue()
        {
            var key = "SiteTitle";
            var setting = new SiteSetting(key, "Darwin CMS", "string", null, null, null, false, Guid.NewGuid());

            object? outValue = null;
            _memoryCacheMock.Setup(m => m.TryGetValue(It.IsAny<object>(), out outValue)).Returns(false);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            _repositoryMock.Setup(r => r.GetByKeyAsync(key, null, It.IsAny<CancellationToken>())).ReturnsAsync(setting);

            var result = await _service.GetValueAsync(key);

            result.Should().Be("Darwin CMS");
        }

        /// <summary>
        /// Ensures strongly typed value conversion returns expected result.
        /// </summary>
        [Fact(DisplayName = "Should convert value to correct type")]
        public async Task GetValueAsAsync_ShouldConvertToCorrectType()
        {
            var key = "MaxItems";
            var setting = new SiteSetting(key, "15", "string", null, null, null, false, Guid.NewGuid());

            object? outValue = null;
            _memoryCacheMock.Setup(m => m.TryGetValue(It.IsAny<object>(), out outValue)).Returns(false);
            var cacheEntryMock = new Mock<ICacheEntry>();
            _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            _repositoryMock.Setup(r => r.GetByKeyAsync(key, null, It.IsAny<CancellationToken>())).ReturnsAsync(setting);

            var result = await _service.GetValueAsAsync<int>(key);

            result.Should().Be(15);
        }

        /// <summary>
        /// Should add and save a new setting.
        /// </summary>
        [Fact(DisplayName = "Should create new setting")]
        public async Task CreateAsync_ShouldAddNewSetting()
        {
            var setting = new SiteSetting("Theme", "dark", "string", null, null, null, false, Guid.NewGuid());

            _repositoryMock.Setup(r => r.AddAsync(setting, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.CreateAsync(setting);

            _repositoryMock.Verify(r => r.AddAsync(setting, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Should update setting and invalidate cache.
        /// </summary>
        [Fact(DisplayName = "Should update and invalidate cache")]
        public async Task UpdateValueAsync_ShouldUpdate()
        {
            var oldKey = "SiteName";
            var newKey = "SiteTitle";
            var setting = new SiteSetting(oldKey, "Old Title", "string", null, null, null, false, Guid.NewGuid());

            _repositoryMock.Setup(r => r.GetByKeyAsync(oldKey, null, It.IsAny<CancellationToken>())).ReturnsAsync(setting);
            _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.UpdateValueAsync(oldKey, null, newKey, null, "New Title", Guid.NewGuid());

            _repositoryMock.Verify(r => r.Update(setting), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Should return all non-deleted settings.
        /// </summary>
        [Fact(DisplayName = "Should return all active settings")]
        public async Task GetAllAsync_ShouldReturnSettings()
        {
            var settings = new List<SiteSetting>
            {
                new("Key1", "Val1", "string", null, null, null, false, Guid.NewGuid())
            };

            //_repositoryMock.Setup(r => r.Query()).Returns(settings.AsQueryable());
            _repositoryMock.Setup(r => r.Query()).Returns(new TestAsyncEnumerable<SiteSetting>(settings));

            var result = await _service.GetAllAsync();
            result.Should().HaveCount(1);
        }

        /// <summary>
        /// Should soft delete a setting.
        /// </summary>
        [Fact(DisplayName = "Should soft delete setting")]
        public async Task SoftDeleteAsync_ShouldCallRepo()
        {
            _repositoryMock.Setup(r => r.SoftDeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.SoftDeleteAsync(Guid.NewGuid(), Guid.NewGuid());

            _repositoryMock.Verify(r => r.SoftDeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Should restore a soft deleted setting.
        /// </summary>
        [Fact(DisplayName = "Should restore soft-deleted setting")]
        public async Task RestoreAsync_ShouldCallRepo()
        {
            _repositoryMock.Setup(r => r.RestoreAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.RestoreAsync(Guid.NewGuid(), Guid.NewGuid());

            _repositoryMock.Verify(r => r.RestoreAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Should permanently delete a setting.
        /// </summary>
        [Fact(DisplayName = "Should hard delete setting")]
        public async Task HardDeleteAsync_ShouldCallRepo()
        {
            _repositoryMock.Setup(r => r.HardDeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _service.HardDeleteAsync(Guid.NewGuid());

            _repositoryMock.Verify(r => r.HardDeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
