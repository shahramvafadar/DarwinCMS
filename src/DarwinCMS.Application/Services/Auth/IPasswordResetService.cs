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
    /// <param name="email">The email for which to create the reset token.</param>
    /// <param name="createdByUserId">Optional: The ID of the user who initiated the request (for audit logs).</param>
    Task<PasswordResetToken> GenerateTokenAsync(string email, Guid? createdByUserId = null);

    /// <summary>
    /// Validates a given token and returns it if valid.
    /// </summary>
    /// <param name="token">The token string to validate.</param>
    Task<PasswordResetToken?> ValidateTokenAsync(string token);

    /// <summary>
    /// Marks the token as used to prevent reuse.
    /// </summary>
    /// <param name="token">The token entity to invalidate.</param>
    /// <param name="modifierId">Optional: The ID of the user who used the token (for audit logs).</param>
    Task InvalidateTokenAsync(PasswordResetToken token, Guid? modifierId = null);
}
