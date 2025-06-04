using System;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a one-time use token for resetting user passwords.
/// Includes expiration and usage status.
/// </summary>
public class PasswordResetToken : BaseEntity
{
    /// <summary>
    /// Email associated with the reset request.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Secure token used to verify the reset link.
    /// </summary>
    public string Token { get; private set; } = string.Empty;

    /// <summary>
    /// Expiration timestamp for this token (UTC).
    /// </summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>
    /// Indicates whether this token has already been used.
    /// </summary>
    public bool IsUsed { get; private set; }

    /// <summary>
    /// EF Core parameterless constructor.
    /// </summary>
    protected PasswordResetToken() { }

    /// <summary>
    /// Initializes a new password reset token with required fields.
    /// </summary>
    /// <param name="email">Email associated with the reset request.</param>
    /// <param name="token">Unique reset token.</param>
    /// <param name="expiresAt">Expiration date/time (UTC).</param>
    /// <param name="createdByUserId">ID of the user who initiated the reset (optional).</param>
    public PasswordResetToken(string email, string token, DateTime expiresAt, Guid? createdByUserId)
    {
        Email = email.Trim().ToLowerInvariant();
        Token = token;
        ExpiresAt = expiresAt;
        IsUsed = false;
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Marks the token as used to prevent further usage.
    /// </summary>
    /// <param name="modifierId">ID of the user who used the token (optional).</param>
    public void MarkAsUsed(Guid? modifierId)
    {
        IsUsed = true;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Checks if the token is still valid (not expired and not used).
    /// </summary>
    /// <returns>True if valid, false otherwise.</returns>
    public bool IsValid() => !IsUsed && DateTime.UtcNow <= ExpiresAt;
}
