using DarwinCMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for SiteSetting entity.
/// Defines table structure, constraints, and relationships.
/// </summary>
public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
{
    /// <summary>
    /// Configures the SiteSetting entity in the EF Core model.
    /// </summary>
    /// <param name="builder">Entity builder for SiteSetting.</param>
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        // Table name
        builder.ToTable("SiteSettings");

        // Primary key
        builder.HasKey(s => s.Id);

        // Unique constraint on Key and optional LanguageCode
        builder.HasIndex(s => new { s.Key, s.LanguageCode }).IsUnique();

        // Properties
        builder.Property(s => s.Key)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Value)
            .IsRequired();

        builder.Property(s => s.ValueType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Category)
            .HasMaxLength(100);

        builder.Property(s => s.LanguageCode)
            .HasMaxLength(10);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.IsSystem)
            .IsRequired();

        builder.Property(s => s.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.ModifiedAt)
            .IsRequired(false);

        // No navigation properties to configure for SiteSetting currently.
    }
}
