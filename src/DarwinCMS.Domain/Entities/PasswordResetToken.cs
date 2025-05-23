using DarwinCMS.Domain.Interfaces;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a one-time use token for resetting user passwords.
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
    /// Expiration timestamp for this token.
    /// </summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>
    /// Indicates whether this token has already been used.
    /// </summary>
    public bool IsUsed { get; private set; }

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected PasswordResetToken() { }

    /// <summary>
    /// Creates a new password reset token.
    /// </summary>
    public PasswordResetToken(string email, string token, DateTime expiresAt)
    {
        Email = email.Trim().ToLowerInvariant();
        Token = token;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        IsUsed = false;
    }

    /// <summary>
    /// Marks the token as used to prevent reuse.
    /// </summary>
    public void MarkAsUsed()
    {
        IsUsed = true;
    }

    /// <summary>
    /// Checks if the token is still valid.
    /// </summary>
    public bool IsValid() => !IsUsed && DateTime.UtcNow <= ExpiresAt;
}
