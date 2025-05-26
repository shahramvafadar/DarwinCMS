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
    private readonly DarwinDbContext _db;
    private readonly DbSet<PasswordResetToken> _set;

    /// <summary>
    /// Initializes the repository with the database context.
    /// </summary>
    public PasswordResetTokenRepository(DarwinDbContext db)
    {
        _db = db;
        _set = _db.PasswordResetTokens;
    }

    /// <inheritdoc />
    public async Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _set
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(token, cancellationToken);
    }

    /// <inheritdoc />
    public void Update(PasswordResetToken token)
    {
        _set.Update(token);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
