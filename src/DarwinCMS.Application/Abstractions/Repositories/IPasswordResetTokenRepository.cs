using DarwinCMS.Domain.Entities;

namespace DarwinCMS.Application.Abstractions.Repositories;

/// <summary>
/// Defines contract for managing password reset tokens.
/// </summary>
public interface IPasswordResetTokenRepository
{
    /// <summary>
    /// Gets a token entity by its string value.
    /// </summary>
    Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new password reset token.
    /// </summary>
    Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a token as used.
    /// </summary>
    void Update(PasswordResetToken token);

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
