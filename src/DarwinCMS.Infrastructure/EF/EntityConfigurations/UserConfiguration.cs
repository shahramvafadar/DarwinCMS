using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.EntityConfigurations;

/// <summary>
/// Fluent API configuration for the User entity.
/// Maps table structure, property constraints, and value objects.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the User entity schema.
    /// </summary>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name
        builder.ToTable("Users");

        // Primary key
        builder.HasKey(u => u.Id);

        // First name (required, max 100)
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        // Last name (required, max 100)
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        // Gender (required, e.g., "Male", "Female")
        builder.Property(u => u.Gender)
            .IsRequired()
            .HasMaxLength(20);

        // BirthDate (required)
        builder.Property(u => u.BirthDate)
            .IsRequired();

        // Username (required, unique, max 100)
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.Username).IsUnique();

        // PasswordHash (required, max 512)
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(512);

        // Email as owned ValueObject
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(256);
        });

        // LanguageCode as owned ValueObject
        builder.OwnsOne(u => u.LanguageCode, lang =>
        {
            lang.Property(l => l.Value)
                .HasColumnName("LanguageCode")
                .IsRequired()
                .HasMaxLength(2);
        });

        // MobilePhone (optional string)
        builder.Property(u => u.MobilePhone)
            .HasMaxLength(30);

        // Email confirmation status
        builder.Property(u => u.IsEmailConfirmed)
            .IsRequired();

        // Mobile confirmation status
        builder.Property(u => u.IsMobileConfirmed)
            .IsRequired();

        // ProfilePictureUrl (optional, max 512)
        builder.Property(u => u.ProfilePictureUrl)
            .HasMaxLength(512);

        // Activity flag
        builder.Property(u => u.IsActive)
            .IsRequired();

        // Last login timestamp (nullable)
        builder.Property(u => u.LastLoginAt);

        // CreatedByUserId (required)
        builder.Property(u => u.CreatedByUserId)
            .IsRequired();

        // ModifiedByUserId (nullable)
        builder.Property(u => u.ModifiedByUserId);

        // CreatedAt (required)
        builder.Property(u => u.CreatedAt)
            .IsRequired();

        // ModifiedAt (nullable)
        builder.Property(u => u.ModifiedAt);
    }
}
