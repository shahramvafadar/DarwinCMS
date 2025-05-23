using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Services.Auth;

/// <summary>
/// Defines operations for generating, storing, and verifying password reset tokens.
/// </summary>
public interface IPasswordResetService
{
    /// <summary>
    /// Generates and stores a secure reset token for a given email.
    /// </summary>
    Task<PasswordResetToken> GenerateTokenAsync(string email);

    /// <summary>
    /// Validates a given token and returns it if valid.
    /// </summary>
    Task<PasswordResetToken?> ValidateTokenAsync(string token);

    /// <summary>
    /// Marks the token as used to prevent reuse.
    /// </summary>
    Task InvalidateTokenAsync(PasswordResetToken token);
}
