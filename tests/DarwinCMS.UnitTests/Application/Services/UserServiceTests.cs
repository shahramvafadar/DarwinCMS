using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Users;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Infrastructure.Services.Users;
using DarwinCMS.Shared.Security;
using FluentAssertions;
using Moq;
using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for the UserService, validating creation, update, deletion,
/// and account actions like enabling, disabling, and password changes.
/// </summary>
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly Fixture _fixture = new();

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _userRoleRepositoryMock = new Mock<IUserRoleRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserRequest, User>();
            cfg.CreateMap<UpdateUserRequest, User>();
        });
        _mapper = config.CreateMapper();

        _userService = new UserService(
            _userRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _userRoleRepositoryMock.Object,
            _mapper);
    }

    [Fact(DisplayName = "Should create user with valid data")]
    public async Task CreateAsync_ShouldSucceed_WhenDataIsValid()
    {
        var request = _fixture.Build<CreateUserRequest>()
            .With(x => x.Username, "testuser")
            .With(x => x.Email, "test@example.com")
            .With(x => x.Password, "Test1234!")
            .With(x => x.LanguageCode, "en")
            .With(x => x.RoleId, Guid.NewGuid())
            .Create();

        _userRepositoryMock.Setup(x => x.GetByUsernameOrEmailAsync(request.Username, request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        _roleRepositoryMock.Setup(x => x.GetByIdAsync(request.RoleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Role("User"));

        var result = await _userService.CreateAsync(request, Guid.NewGuid());

        result.Should().NotBeNull();
        result.Email.Value.Should().Be(request.Email);
        result.Username.Should().Be(request.Username);
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should throw when username is already taken")]
    public async Task CreateAsync_ShouldThrow_WhenUsernameExists()
    {
        var request = _fixture.Build<CreateUserRequest>()
            .With(x => x.Email, "taken@example.com")
            .With(x => x.Username, "existinguser")
            .With(x => x.RoleId, Guid.NewGuid())
            .Create();

        _userRepositoryMock.Setup(x => x.GetByUsernameOrEmailAsync(request.Username, request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User("Darwin", "Vafadar", "Male", DateTime.Today, "existinguser", new Email("darwin@example.com"), "hashed", Guid.NewGuid()));

        var act = () => _userService.CreateAsync(request, Guid.NewGuid());

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Username or Email is already taken.");
    }

    [Fact(DisplayName = "Should disable user if exists")]
    public async Task DisableUserAsync_ShouldSetInactive()
    {
        var user = new User("Shahram", "Vafadar", "Male", new DateTime(1985, 1, 1), "shahramv", new Email("shahram@example.com"), "hash123", Guid.NewGuid());
        user.IsActive.Should().BeTrue();

        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        await _userService.DisableUserAsync(user.Id, Guid.NewGuid());

        user.IsActive.Should().BeFalse();
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should hash and update password")]
    public async Task ChangePasswordAsync_ShouldHashAndSave()
    {
        var user = new User("Nora", "Davoodi", "Female", new DateTime(1990, 6, 1), "nora90", new Email("nora@example.com"), "oldhash", Guid.NewGuid());

        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var newRawPassword = "NewSecure123!";

        await _userService.ChangePasswordAsync(user.Id, newRawPassword, Guid.NewGuid());

        user.PasswordHash.Should().NotBe("oldhash");
        user.PasswordHash.Should().NotBe(newRawPassword);
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should enable user if inactive")]
    public async Task EnableUserAsync_ShouldSetActive()
    {
        var user = new User("Daniel", "zhozho", "Male", new DateTime(1980, 3, 20), "daniel88", new Email("daniel@example.com"), "hash", Guid.NewGuid());
        user.Deactivate();
        user.IsActive.Should().BeFalse();

        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        await _userService.EnableUserAsync(user.Id, Guid.NewGuid());

        user.IsActive.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should update user and assigned roles")]
    public async Task UpdateAsync_ShouldUpdateUserDetailsAndRoles()
    {
        var userId = Guid.NewGuid();
        var existingUser = new User("Kia", "Berliner", "Male", new DateTime(1990, 1, 1), "kianbi", new Email("kia@test.com"), "hash", Guid.NewGuid());

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _userRoleRepositoryMock.Setup(r => r.GetByUserIdAsync(userId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserRole> { new(userId, Guid.NewGuid()) });

        var updateRequest = new UpdateUserRequest
        {
            Id = userId,
            Username = "kianbi",
            Email = "kia@test.com",
            FirstName = "Kia",
            LastName = "Berliner",
            Gender = "Male",
            BirthDate = new DateTime(1990, 1, 1),
            LanguageCode = "en",
            ProfilePictureUrl = null,
            MobilePhone = "+491234567890",
            IsEmailConfirmed = true,
            IsMobileConfirmed = true,
            RoleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };

        await _userService.UpdateAsync(updateRequest, Guid.NewGuid());

        _userRepositoryMock.Verify(r => r.Update(existingUser), Times.Once);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _userRoleRepositoryMock.Verify(r => r.Delete(It.IsAny<UserRole>()), Times.AtLeastOnce);
        _userRoleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact(DisplayName = "Should delete user and their roles")]
    public async Task DeleteUserAsync_ShouldRemoveUserAndRoles()
    {
        var userId = Guid.NewGuid();
        var user = new User("Sara", "Kazemi", "Female", new DateTime(1991, 5, 20), "sarakz", new Email("sara@test.com"), "hash", Guid.NewGuid());

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        _userRoleRepositoryMock.Setup(r => r.GetByUserIdAsync(userId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserRole> { new(userId, Guid.NewGuid()) });

        await _userService.DeleteUserAsync(userId);

        _userRoleRepositoryMock.Verify(r => r.Delete(It.IsAny<UserRole>()), Times.AtLeastOnce);
        _userRepositoryMock.Verify(r => r.Delete(user), Times.Once);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
