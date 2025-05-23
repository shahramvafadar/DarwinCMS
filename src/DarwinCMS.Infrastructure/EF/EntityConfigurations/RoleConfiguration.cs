using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DarwinCMS.Infrastructure.EF.EntityConfigurations;

/// <summary>
/// Fluent API configuration for the Role entity.
/// Maps schema, constraints, indexes, and relationships for access management.
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// Configures the Role entity.
    /// </summary>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Table name
        builder.ToTable("Roles");

        // Primary key
        builder.HasKey(r => r.Id);

        // Name: required and unique system identifier
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        // DisplayName: optional label shown in UI
        builder.Property(r => r.DisplayName)
            .HasMaxLength(150);

        // Description: optional field
        builder.Property(r => r.Description)
            .HasMaxLength(500);

        // Module: optional, for module-specific roles
        builder.Property(r => r.Module)
            .HasMaxLength(100);

        // DisplayOrder: nullable
        builder.Property(r => r.DisplayOrder);

        // IsActive: required soft delete marker
        builder.Property(r => r.IsActive)
            .IsRequired();

        // Audit Fields from IAuditableEntity
        builder.Property(r => r.CreatedByUserId)
            .IsRequired();

        builder.Property(r => r.ModifiedByUserId)
            .IsRequired(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.ModifiedAt)
            .IsRequired(false);

        // Navigation: Role → UserRoles (many-to-many)
        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Navigation: Role → RolePermissions (many-to-many)
        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
