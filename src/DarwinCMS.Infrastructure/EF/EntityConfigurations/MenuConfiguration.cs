using DarwinCMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.Configuration;

/// <summary>
/// EF Core Fluent API configuration for the Menu entity.
/// Defines table mapping, constraints, and property rules.
/// </summary>
public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    /// <summary>
    /// Configures the EF Core mapping for the Menu entity.
    /// </summary>
    /// <param name="builder">The entity builder for Menu.</param>
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(m => m.Position)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.LanguageCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(m => m.IsActive)
            .IsRequired();

        builder.Property(m => m.IsSystem)
            .IsRequired();

        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.ModifiedAt);

        // Each menu has many items
        builder.HasMany(m => m.Items)
               .WithOne(mi => mi.Menu)
               .HasForeignKey(mi => mi.MenuId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
