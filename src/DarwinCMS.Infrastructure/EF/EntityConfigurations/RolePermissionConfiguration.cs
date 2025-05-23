using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.EntityConfigurations;

/// <summary>
/// Fluent API configuration for the RolePermission entity.
/// Defines indexes, constraints, and relationships to Role and Permission.
/// </summary>
public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    /// <summary>
    /// Configures the schema mapping and constraints for RolePermission.
    /// </summary>
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // Table name
        builder.ToTable("RolePermissions");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Unique index to avoid duplicate permission assignments within same module
        builder.HasIndex(x => new { x.RoleId, x.PermissionId, x.Module })
               .IsUnique();

        // Foreign key to Role
        builder.HasOne(x => x.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key to Permission
        builder.HasOne(x => x.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        // RoleId is required
        builder.Property(x => x.RoleId)
            .IsRequired();

        // PermissionId is required
        builder.Property(x => x.PermissionId)
            .IsRequired();

        // System-defined or custom permission assignment
        builder.Property(x => x.IsSystemPermission)
            .IsRequired();

        // Optional module scope
        builder.Property(x => x.Module)
            .HasMaxLength(100);

        // Auditing columns (from BaseEntity)
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ModifiedAt)
            .IsRequired(false);
    }
}
