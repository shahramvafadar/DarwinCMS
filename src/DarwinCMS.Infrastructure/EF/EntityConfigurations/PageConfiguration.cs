using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DarwinCMS.Infrastructure.EF.Configuration;

/// <summary>
/// EF Core Fluent API configuration for the Page entity.
/// Maps properties, constraints, indexes, and soft deletion flag.
/// </summary>
public class PageConfiguration : IEntityTypeConfiguration<Page>
{
    /// <summary>
    /// Configures the EF Core mapping for the Page entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the Page entity.</param>
    public void Configure(EntityTypeBuilder<Page> builder)
    {
        // Table mapping
        builder.ToTable("Pages");

        // Primary key
        builder.HasKey(p => p.Id);

        // Basic properties
        builder.Property(p => p.LanguageCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Summary)
            .HasMaxLength(512);

        // Slug value mapping only!
        builder.Property(p => p.SlugValue)
            .HasColumnName("Slug")
            .HasMaxLength(200)
            .IsRequired();

        // Ignore the ValueObject itself (not stored directly)
        builder.Ignore(p => p.Slug);

        builder.HasIndex(p => new { p.SlugValue, p.LanguageCode })
            .IsUnique();

        builder.Property(p => p.ContentHtml)
            .IsRequired();

        builder.Property(p => p.IsPublished)
            .IsRequired();

        builder.Property(p => p.PublishDateUtc);
        builder.Property(p => p.ExpireDateUtc);

        // SEO fields
        builder.Property(p => p.MetaTitle).HasMaxLength(256);
        builder.Property(p => p.MetaDescription).HasMaxLength(512);
        builder.Property(p => p.CanonicalUrl).HasMaxLength(300);

        builder.Property(p => p.OgTitle).HasMaxLength(256);
        builder.Property(p => p.OgImage).HasMaxLength(512);
        builder.Property(p => p.OgDescription).HasMaxLength(512);

        builder.Property(p => p.StructuredDataJsonLd);

        builder.Property(p => p.CoverImageUrl).HasMaxLength(512);
        builder.Property(p => p.Category).HasMaxLength(128);

        // System page flag
        builder.Property(p => p.IsSystem)
            .IsRequired();

        // Soft delete flag
        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Audit fields
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.ModifiedAt)
            .IsRequired(false);

        // Display order
        builder.Property(p => p.DisplayOrder)
            .HasDefaultValue(0);

        // Index for display order (optional)
        builder.HasIndex(p => p.DisplayOrder);
    }
}
