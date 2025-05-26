using DarwinCMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.Configuration;

/// <summary>
/// EF Core Fluent API configuration for the MenuItem entity.
/// Handles nested relationships and constraints.
/// </summary>
public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    /// <summary>
    /// Configures the EF Core mapping for the MenuItem entity.
    /// </summary>
    /// <param name="builder">The entity builder for MenuItem.</param>
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems");

        builder.HasKey(mi => mi.Id);

        builder.Property(mi => mi.Title)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(mi => mi.Icon)
            .HasMaxLength(64);

        builder.Property(mi => mi.Url)
            .HasMaxLength(512);

        builder.Property(mi => mi.VisibleForAuthenticatedOnly)
            .IsRequired();

        builder.Property(mi => mi.IsActive)
            .IsRequired();

        builder.Property(mi => mi.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(mi => mi.CreatedAt).IsRequired();
        builder.Property(mi => mi.ModifiedAt);

        // Optional self-referencing relationship for nested menus
        builder.HasOne(mi => mi.ParentItem)
               .WithMany(mi => mi.Children)
               .HasForeignKey(mi => mi.ParentItemId)
               .OnDelete(DeleteBehavior.Restrict);

        // Optional reference to Page (nullable foreign key)
        builder.HasOne(mi => mi.Page)
               .WithMany()
               .HasForeignKey(mi => mi.PageId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
