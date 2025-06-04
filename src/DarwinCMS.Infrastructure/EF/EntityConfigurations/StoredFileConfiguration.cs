using DarwinCMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.Configuration;

/// <summary>
/// EF Core Fluent API configuration for the StoredFile entity.
/// Handles media-specific properties and constraints.
/// </summary>
public class StoredFileConfiguration : IEntityTypeConfiguration<StoredFile>
{
    /// <summary>
    /// Configures the EF Core mapping for the StoredFile entity.
    /// </summary>
    /// <param name="builder">The entity builder for StoredFile.</param>
    public void Configure(EntityTypeBuilder<StoredFile> builder)
    {
        builder.ToTable("StoredFiles");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(f => f.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(f => f.MimeType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.FileSize)
            .IsRequired();

        builder.Property(f => f.Width);
        builder.Property(f => f.Height);

        builder.Property(f => f.AltText).HasMaxLength(300);
        builder.Property(f => f.Caption).HasMaxLength(500);
        builder.Property(f => f.Folder).HasMaxLength(100);
        builder.Property(f => f.LanguageCode).HasMaxLength(10);

        builder.Property(f => f.IsSystem).IsRequired();

        builder.Property(f => f.CreatedAt).IsRequired();
        builder.Property(f => f.ModifiedAt);

        // Ensure combination of path + file name is unique
        builder.HasIndex(f => new { f.FilePath, f.FileName }).IsUnique();
    }
}
