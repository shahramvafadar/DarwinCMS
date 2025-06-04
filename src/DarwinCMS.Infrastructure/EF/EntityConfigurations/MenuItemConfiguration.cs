using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.Configuration;

/// <summary>
/// EF Core Fluent API configuration for MenuItem entity.
/// Handles relationships, value conversions, and field constraints.
/// </summary>
public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    /// <summary>
    /// Configures the MenuItem entity schema and constraints.
    /// </summary>
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems");

        builder.HasKey(mi => mi.Id);

        builder.Property(mi => mi.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(mi => mi.Icon)
            .HasMaxLength(100);

        builder.Property(mi => mi.DisplayOrder)
            .IsRequired();

        builder.Property(mi => mi.Url)
            .HasMaxLength(500);

        builder.Property(mi => mi.DisplayCondition)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(mi => mi.IsActive)
            .IsRequired();

        // 🔁 ValueObject: LinkType conversion to string
        builder.Property(mi => mi.LinkType)
            .HasConversion(
                v => v.Value,
                v => LinkType.From(v))
            .HasMaxLength(20)
            .IsRequired();

        // Relationships
        builder.HasOne(mi => mi.Menu)
            .WithMany(m => m.Items)
            .HasForeignKey(mi => mi.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mi => mi.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(mi => mi.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(mi => new { mi.MenuId, mi.DisplayOrder });
    }
}
