using DarwinCMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.Configuration;

/// <summary>
/// EF Core Fluent API configuration for the SiteSetting entity.
/// Controls mapping and unique constraints for settings.
/// </summary>
public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
{
    /// <summary>
    /// Configures the EF Core mapping for the SiteSetting entity.
    /// </summary>
    /// <param name="builder">The entity builder for SiteSetting.</param>
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        builder.ToTable("SiteSettings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Value)
            .IsRequired();

        builder.Property(s => s.Category)
            .HasMaxLength(64);

        builder.Property(s => s.LanguageCode)
            .HasMaxLength(10);

        builder.Property(s => s.ValueType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Description)
            .HasMaxLength(300);

        builder.Property(s => s.IsSystem)
            .IsRequired();

        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.ModifiedAt);

        // Unique setting key + language to allow per-language overrides
        builder.HasIndex(s => new { s.Key, s.LanguageCode })
               .IsUnique();
    }
}
