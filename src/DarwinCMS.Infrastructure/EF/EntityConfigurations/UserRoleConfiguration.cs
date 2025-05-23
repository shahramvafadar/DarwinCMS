using DarwinCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DarwinCMS.Infrastructure.EF.EntityConfigurations;

/// <summary>
/// Fluent API configuration for the UserRole entity.
/// Defines the join relationship between User and Role entities,
/// including composite uniqueness constraints and optional module scope.
/// </summary>
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <summary>
    /// Configures the UserRole entity mapping for EF Core.
    /// </summary>
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Table name
        builder.ToTable("UserRoles");

        // Primary key (inherited from BaseEntity.Id)
        builder.HasKey(ur => ur.Id);

        // Composite index to prevent duplicate assignments for same user-role-module
        builder.HasIndex(ur => new { ur.UserId, ur.RoleId, ur.Module })
            .IsUnique();

        // Foreign key to User
        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key to Role
        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Required: UserId
        builder.Property(ur => ur.UserId)
            .IsRequired();

        // Required: RoleId
        builder.Property(ur => ur.RoleId)
            .IsRequired();

        // Optional module (e.g., "Store", "CRM") — allows module-scoped role assignment
        builder.Property(ur => ur.Module)
            .HasMaxLength(100);

        // Required: Indicates if this was assigned automatically by the system
        builder.Property(ur => ur.IsSystemAssigned)
            .IsRequired();

        // Required: Created timestamp
        builder.Property(ur => ur.CreatedAt)
            .IsRequired();

        // Optional: Modified timestamp
        builder.Property(ur => ur.ModifiedAt)
            .IsRequired(false);
    }
}
