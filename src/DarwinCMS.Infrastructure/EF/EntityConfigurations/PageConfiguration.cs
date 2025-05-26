using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.Configuration;

/// <summary>
/// EF Core Fluent API configuration for the Page entity.
/// Maps properties, constraints, and indexes.
/// </summary>
public class PageConfiguration : IEntityTypeConfiguration<Page>
{
    /// <summary>
    /// Configures the EF Core mapping for the Page entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the Page entity.</param>
    public void Configure(EntityTypeBuilder<Page> builder)
    {
        builder.ToTable("Pages");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.LanguageCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Summary)
            .HasMaxLength(512);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => new { p.Slug, p.LanguageCode })
            .IsUnique();

        builder.Property(p => p.ContentHtml)
            .IsRequired();

        builder.Property(p => p.IsPublished)
            .IsRequired();

        builder.Property(p => p.PublishDateUtc);
        builder.Property(p => p.ExpireDateUtc);

        builder.Property(p => p.MetaTitle).HasMaxLength(256);
        builder.Property(p => p.MetaDescription).HasMaxLength(512);
        builder.Property(p => p.CanonicalUrl).HasMaxLength(300);

        builder.Property(p => p.OgTitle).HasMaxLength(256);
        builder.Property(p => p.OgImage).HasMaxLength(512);
        builder.Property(p => p.OgDescription).HasMaxLength(512);
        builder.Property(p => p.StructuredDataJsonLd);

        builder.Property(p => p.CoverImageUrl).HasMaxLength(512);
        builder.Property(p => p.Category).HasMaxLength(128);

        builder.Property(p => p.IsSystem).IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.ModifiedAt);

        builder.Property(p => p.DisplayOrder).HasDefaultValue(0);

        builder.HasIndex(p => p.DisplayOrder);
    }
}
