using System;
using System.Threading;
using System.Threading.Tasks;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Auth;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.Services.Auth;

using FluentAssertions;

using Moq;

using Xunit;

namespace DarwinCMS.UnitTests.Application.Services;

/// <summary>
/// Unit tests for the PasswordResetService.
/// Covers token generation, validation, and invalidation behavior.
/// </summary>
public class PasswordResetServiceTests
{
    private readonly Mock<IPasswordResetTokenRepository> _tokenRepoMock;
    private readonly IPasswordResetService _service;

    /// <summary>
    /// Initializes mock repository and service instance.
    /// </summary>
    public PasswordResetServiceTests()
    {
        _tokenRepoMock = new Mock<IPasswordResetTokenRepository>();
        _service = new PasswordResetService(_tokenRepoMock.Object);
    }

    /// <summary>
    /// Should generate a token and store it with correct expiration and fields.
    /// </summary>
    [Fact(DisplayName = "Should generate new token")]
    public async Task GenerateTokenAsync_ShouldCreateValidToken()
    {
        var email = "user@example.com";
        var userId = Guid.NewGuid();

        PasswordResetToken? captured = null;

        _tokenRepoMock
            .Setup(r => r.AddAsync(It.IsAny<PasswordResetToken>(), It.IsAny<CancellationToken>()))
            .Callback<PasswordResetToken, CancellationToken>((t, _) => captured = t)
            .Returns(Task.CompletedTask);

        _tokenRepoMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.GenerateTokenAsync(email, userId);

        result.Should().NotBeNull();
        result.Email.Should().Be(email);
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(25));
        captured.Should().NotBeNull();
        captured!.CreatedByUserId.Should().Be(userId);
    }

    /// <summary>
    /// Should return valid token object if token exists and is usable.
    /// </summary>
    [Fact(DisplayName = "Should validate usable token")]
    public async Task ValidateTokenAsync_ShouldReturnIfValid()
    {
        var token = "abc123";
        var mockToken = new PasswordResetToken("user@example.com", token, DateTime.UtcNow.AddMinutes(10), Guid.NewGuid());

        _tokenRepoMock
            .Setup(r => r.GetByTokenAsync(It.Is<string>(s => s == token), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockToken);


        var result = await _service.ValidateTokenAsync(token);

        result.Should().NotBeNull();
        result!.Token.Should().Be(token);
    }

    /// <summary>
    /// Should return null if token is not found in repository.
    /// </summary>
    [Fact(DisplayName = "Should return null if token not found")]
    public async Task ValidateTokenAsync_ShouldReturnNullIfMissing()
    {
        _tokenRepoMock
            .Setup(r => r.GetByTokenAsync(It.Is<string>(s => s == "invalid"), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PasswordResetToken?)null);


        var result = await _service.ValidateTokenAsync("invalid");

        result.Should().BeNull();
    }

    /// <summary>
    /// Should return null if token is already used or expired.
    /// </summary>
    [Fact(DisplayName = "Should return null if token invalid")]
    public async Task ValidateTokenAsync_ShouldReturnNullIfExpiredOrUsed()
    {
        var expired = new PasswordResetToken("test@example.com", "expired", DateTime.UtcNow.AddMinutes(-1), Guid.NewGuid());
        expired.MarkAsUsed(Guid.NewGuid());

        _tokenRepoMock
            .Setup(r => r.GetByTokenAsync(It.Is<string>(s => s == "expired"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expired);


        var result = await _service.ValidateTokenAsync("expired");

        result.Should().BeNull();
    }

    /// <summary>
    /// Should mark token as used and persist change to database.
    /// </summary>
    [Fact(DisplayName = "Should invalidate token")]
    public async Task InvalidateTokenAsync_ShouldMarkAndSave()
    {
        var token = new PasswordResetToken("test@example.com", "abc123", DateTime.UtcNow.AddMinutes(10), Guid.NewGuid());
        var modifierId = Guid.NewGuid();

        _tokenRepoMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _tokenRepoMock
            .Setup(r => r.Update(It.IsAny<PasswordResetToken>()));

        await _service.InvalidateTokenAsync(token, modifierId);

        token.IsUsed.Should().BeTrue();

        _tokenRepoMock.Verify(r => r.Update(token), Times.Once);
        _tokenRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
