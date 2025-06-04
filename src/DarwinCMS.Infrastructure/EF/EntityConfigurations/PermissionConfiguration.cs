using DarwinCMS.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.EntityConfigurations;

/// <summary>
/// Fluent API configuration for the Permission entity.
/// Maps schema, constraints, and relationships for access control.
/// </summary>
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <summary>
    /// Configures the Permission entity's schema and relationships.
    /// </summary>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Table name
        builder.ToTable("Permissions");

        // Primary key
        builder.HasKey(p => p.Id);

        // Unique index on Name to prevent duplicates
        builder.HasIndex(p => p.Name)
            .IsUnique(); // Unique technical identifier

        // Properties
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.DisplayName)
            .HasMaxLength(150);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Module)
            .HasMaxLength(100);

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.IsSystem)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.ModifiedAt)
            .IsRequired(false);

        // Relationships
        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent accidental cascade deletes
    }
}
