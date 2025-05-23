using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.EntityConfigurations;

/// <summary>
/// Fluent API configuration for the Permission entity.
/// Maps schema and constraints for access control units.
/// </summary>
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <summary>
    /// Configures table, fields, and constraints for the Permission entity.
    /// </summary>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Table name
        builder.ToTable("Permissions");

        // Primary key
        builder.HasKey(p => p.Id);

        // Unique constraint on Name (technical identifier)
        builder.HasIndex(p => p.Name)
            .IsUnique();

        // Property configurations

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.DisplayName)
            .HasMaxLength(150);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Module)
            .HasMaxLength(100);

        // Auditing fields from BaseEntity
        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.ModifiedAt)
            .IsRequired(false);

        // Relationships (RolePermission join entity handles linkage)
        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
