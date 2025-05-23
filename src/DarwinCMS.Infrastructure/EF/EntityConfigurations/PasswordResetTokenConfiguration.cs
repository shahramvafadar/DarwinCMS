using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.Configurations;

/// <summary>
/// EF Core Fluent API configuration for PasswordResetToken entity.
/// </summary>
public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    /// <summary>
    /// Configures the entity type <see cref="PasswordResetToken"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This method sets up the table name, primary key, property constraints, and indexes for the 
    /// <see cref="PasswordResetToken"/> entity. It ensures that required fields are enforced,  defines maximum lengths
    /// for string properties, and configures unique and composite indexes.</remarks>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="PasswordResetToken"/> entity.</param>
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("PasswordResetTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.IsUsed)
            .IsRequired();

        builder.HasIndex(x => x.Token).IsUnique();
        builder.HasIndex(x => new { x.Email, x.IsUsed });
    }
}
