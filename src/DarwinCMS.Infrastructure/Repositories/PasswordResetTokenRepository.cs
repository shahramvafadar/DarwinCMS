using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Repositories;

/// <summary>
/// Provides Entity Framework Core implementation for managing password reset tokens.
/// </summary>
public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly DarwinDbContext _dbContext;

    /// <summary>
    /// Initializes the repository with the database context.
    /// </summary>
    public PasswordResetTokenRepository(DarwinDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves a token entity by its unique string value.
    /// Returns null if not found or expired.
    /// </summary>
    public async Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PasswordResetTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);
    }

    /// <summary>
    /// Adds a new password reset token to the database.
    /// </summary>
    public async Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
    {
        await _dbContext.PasswordResetTokens.AddAsync(token, cancellationToken);
    }

    /// <summary>
    /// Updates the token state (e.g., to mark as used).
    /// </summary>
    public void Update(PasswordResetToken token)
    {
        _dbContext.PasswordResetTokens.Update(token);
    }

    /// <summary>
    /// Commits all changes to the underlying database.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
