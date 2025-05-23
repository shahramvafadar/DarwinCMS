using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.Services.Auth;
using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Infrastructure.Services.Auth;

/// <summary>
/// Provides implementation for password reset token generation and validation.
/// </summary>
public class PasswordResetService : IPasswordResetService
{
    private readonly IPasswordResetTokenRepository _tokenRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordResetService"/> class.
    /// </summary>
    /// <param name="tokenRepository">The repository used to manage password reset tokens. This parameter cannot be null.</param>
    public PasswordResetService(IPasswordResetTokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    /// <summary>
    /// Generates and stores a unique reset token with 30-minute expiration.
    /// </summary>
    public async Task<PasswordResetToken> GenerateTokenAsync(string email)
    {
        var token = Guid.NewGuid().ToString("N");
        var expires = DateTime.UtcNow.AddMinutes(30);

        var resetToken = new PasswordResetToken(email, token, expires);

        await _tokenRepository.AddAsync(resetToken);
        await _tokenRepository.SaveChangesAsync();

        return resetToken;
    }

    /// <summary>
    /// Validates a token string and returns it if not expired or used.
    /// </summary>
    public async Task<PasswordResetToken?> ValidateTokenAsync(string token)
    {
        var result = await _tokenRepository.GetByTokenAsync(token);
        if (result == null || !result.IsValid())
            return null;

        return result;
    }

    /// <summary>
    /// Marks the token as used and saves changes.
    /// </summary>
    public async Task InvalidateTokenAsync(PasswordResetToken token)
    {
        token.MarkAsUsed();
        _tokenRepository.Update(token);
        await _tokenRepository.SaveChangesAsync();
    }
}
