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
using DarwinCMS.Shared.Exceptions;
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

    /// <summary>
    /// Initializes test class and dependencies.
    /// </summary>
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

    /// <summary>
    /// Tests that a new user is successfully created when valid data is provided.
    /// </summary>
    [Fact(DisplayName = "Should create user with valid data")]
    public async Task CreateAsync_ShouldSucceed_WhenDataIsValid()
    {
        // Arrange
        var testRoleId = Guid.NewGuid();
        var request = _fixture.Build<CreateUserRequest>()
            .With(x => x.Username, "daniel90")
            .With(x => x.Email, "daniel@example.com")
            .With(x => x.Password, "Test1234!")
            .With(x => x.LanguageCode, "en")
            .With(x => x.FirstName, "Daniel")
            .With(x => x.LastName, "Morgan")
            .With(x => x.Gender, "Male")
            .With(x => x.BirthDate, new DateTime(1990, 1, 1))
            .With(x => x.RoleIds, new List<Guid> { testRoleId })
            .Create();

        _userRepositoryMock.Setup(x => x.GetByUsernameOrEmailAsync(request.Username, request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        _roleRepositoryMock.Setup(x => x.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Role> { new Role("Admin", Guid.NewGuid(), "Admin", "", "", 0) { Id = testRoleId } });

        _userRoleRepositoryMock.Setup(x => x.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _userService.CreateAsync(request, Guid.NewGuid());

        // Assert
        result.Should().NotBeNull();
        result.Email.Value.Should().Be(request.Email);
        result.Username.Should().Be(request.Username);
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Ensures that the system prevents creation of users with existing username/email.
    /// </summary>
    [Fact(DisplayName = "Should throw when username is already taken")]
    public async Task CreateAsync_ShouldThrow_WhenUsernameExists()
    {
        var request = _fixture.Build<CreateUserRequest>()
            .With(x => x.Email, "taken@example.com")
            .With(x => x.Username, "existinguser")
            .With(x => x.FirstName, "Elena")
            .With(x => x.LastName, "Stone")
            .Create();

        _userRepositoryMock.Setup(x => x.GetByUsernameOrEmailAsync(request.Username, request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User("Elena", "Stone", "Female", DateTime.Today.AddYears(-30), "existinguser", new Email("taken@example.com"), "hashed", Guid.NewGuid()));

        var act = () => _userService.CreateAsync(request, Guid.NewGuid());

        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Username or Email is already taken.");
    }


    /// <summary>
    /// Disables an active user and confirms status change.
    /// </summary>
    [Fact(DisplayName = "Should disable user if exists")]
    public async Task DisableUserAsync_ShouldSetInactive()
    {
        var user = new User("Michael", "Stone", "Male", new DateTime(1985, 1, 1), "mike", new Email("mike@example.com"), "hash123", Guid.NewGuid());
        user.IsActive.Should().BeTrue();

        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        await _userService.DisableUserAsync(user.Id, Guid.NewGuid());

        user.IsActive.Should().BeFalse();
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Hashes a new password and updates the stored hash.
    /// </summary>
    [Fact(DisplayName = "Should hash and update password")]
    public async Task ChangePasswordAsync_ShouldHashAndSave()
    {
        var user = new User("Emma", "Turner", "Female", new DateTime(1990, 6, 1), "emma90", new Email("emma@example.com"), "oldhash", Guid.NewGuid());

        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var newRawPassword = "NewSecure123!";

        await _userService.ChangePasswordAsync(user.Id, newRawPassword, Guid.NewGuid());

        user.PasswordHash.Should().NotBe("oldhash");
        user.PasswordHash.Should().NotBe(newRawPassword);
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Enables an inactive user account.
    /// </summary>
    [Fact(DisplayName = "Should enable user if inactive")]
    public async Task EnableUserAsync_ShouldSetActive()
    {
        var user = new User("Daniel", "Ford", "Male", new DateTime(1980, 3, 20), "daniel88", new Email("daniel@example.com"), "hash", Guid.NewGuid());
        user.Deactivate();
        user.IsActive.Should().BeFalse();

        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        await _userService.EnableUserAsync(user.Id, Guid.NewGuid());

        user.IsActive.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Verifies that a user is successfully updated with assigned roles.
    /// Ensures Delete is called for existing roles, then AddAsync is called for new roles.
    /// </summary>
    [Fact(DisplayName = "Should update user and assigned roles")]
    public async Task UpdateAsync_ShouldUpdateUserDetailsAndRoles()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var oldRoleId = Guid.NewGuid();
        var newRole1 = Guid.NewGuid();
        var newRole2 = Guid.NewGuid();

        var user = new User("Test", "User", "Other", new DateTime(2000, 1, 1), "testuser", new Email("test@example.com"), "hash", Guid.NewGuid());
        typeof(User).GetProperty("Id")!.SetValue(user, userId);

        var oldUserRole = new UserRole(userId, oldRoleId);
        typeof(UserRole).GetProperty("Id")!.SetValue(oldUserRole, Guid.NewGuid());

        var userRoles = new List<UserRole> { oldUserRole };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userRoleRepositoryMock.Setup(r => r.GetByUserIdAsync(userId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userRoles);

        var deletedRoles = new List<UserRole>();

        _userRoleRepositoryMock.Setup(r => r.Delete(It.IsAny<UserRole>()))
            .Callback<UserRole>(ur => deletedRoles.Add(ur));

        _userRoleRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _userRoleRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _userRepositoryMock.Setup(r => r.Update(It.IsAny<User>()));
        _userRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var updateRequest = new UpdateUserRequest
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Gender = "Other",
            BirthDate = new DateTime(2000, 1, 1),
            LanguageCode = "en",
            ProfilePictureUrl = null,
            MobilePhone = null,
            IsEmailConfirmed = true,
            IsMobileConfirmed = true,
            RoleIds = new List<Guid> { newRole1, newRole2 }
        };

        // Act
        await _userService.UpdateAsync(updateRequest, Guid.NewGuid());

        // Assert
        deletedRoles.Should().ContainSingle(ur =>
            ur.UserId == userId && ur.RoleId == oldRoleId,
            "the service should delete old roles before assigning new ones");

        _userRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
        _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _userRoleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _userRoleRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Should hard delete user and their roles.
    /// </summary>
    [Fact(DisplayName = "Should hard delete user and their roles")]
    public async Task HardDeleteAsync_ShouldRemoveUserAndRoles()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("Sophie", "Martin", "Female", new DateTime(1991, 5, 20), "sophiem", new Email("sophie@test.com"), "hash", Guid.NewGuid());

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        _userRoleRepositoryMock.Setup(r => r.GetByUserIdAsync(userId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserRole> { new(userId, Guid.NewGuid()) });

        // Act
        await _userService.HardDeleteAsync(userId);

        // Assert
        _userRoleRepositoryMock.Verify(r => r.Delete(It.IsAny<UserRole>()), Times.AtLeastOnce);
        _userRepositoryMock.Verify(r => r.HardDeleteAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        // ❌ این Verify رو دیگه حذف می‌کنیم چون SaveChangesAsync در Repository انجام میشه
        // _userRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


}
