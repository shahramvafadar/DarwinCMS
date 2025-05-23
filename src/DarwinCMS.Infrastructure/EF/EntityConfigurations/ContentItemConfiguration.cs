using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.EntityConfigurations;

/// <summary>
/// Fluent API configuration for the ContentItem entity.
/// </summary>
public class ContentItemConfiguration : IEntityTypeConfiguration<ContentItem>
{
    /// <summary>
    /// Configures the entity type for <see cref="ContentItem"/> in the database context.
    /// </summary>
    /// <remarks>This method defines the table name, primary key, property constraints, and relationships for
    /// the  <see cref="ContentItem"/> entity. It includes configurations for required fields, maximum lengths, 
    /// optional fields, value objects, and audit fields. Additionally, it specifies ignored and owned
    /// properties.</remarks>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="ContentItem"/> entity.</param>
    public void Configure(EntityTypeBuilder<ContentItem> builder)
    {
        builder.ToTable("ContentItems");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Title - required, max length
        builder.Property(c => c.Title)
               .IsRequired()
               .HasMaxLength(300);

        // Summary - optional
        builder.Property(c => c.Summary)
               .HasMaxLength(500);

        // Body - optional
        builder.Property(c => c.Body);

        // ContentType - required
        builder.Property(c => c.ContentType)
               .IsRequired()
               .HasMaxLength(100);

        // LanguageCode - required
        builder.Property(c => c.LanguageCode)
               .IsRequired()
               .HasMaxLength(2);

        // Metadata JSON
        builder.Property(c => c.MetadataJson)
               .HasMaxLength(2000);

        // Status - stored as string
        builder.Property(c => c.Status)
               .HasConversion<string>()
               .IsRequired()
               .HasMaxLength(50);

        // PublishedAt - optional
        builder.Property(c => c.PublishedAt);

        // Computed property - not mapped
        builder.Ignore(c => c.IsPublished);

        // Slug value object
        builder.OwnsOne(c => c.Slug, owned =>
        {
            owned.Property(s => s.Value)
                 .HasColumnName("Slug")
                 .IsRequired()
                 .HasMaxLength(200);
        });

        // Tags value object
        builder.OwnsOne(c => c.Tags, owned =>
        {
            owned.Property(t => t.Value)
                 .HasColumnName("Tags")
                 .HasMaxLength(500);
        });

        // Audit fields
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.ModifiedAt);
        builder.Property(c => c.CreatedByUserId).IsRequired();
        builder.Property(c => c.ModifiedByUserId);
    }
}
