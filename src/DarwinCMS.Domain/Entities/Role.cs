using System;
using System.Collections.Generic;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a role in the system. Roles define access levels and user grouping.
/// A role can be global or module-specific and contains a set of permissions.
/// </summary>
public class Role : BaseEntity
{
    /// <summary>
    /// Technical identifier of the role (e.g., "Admin"). Must be unique.
    /// Used for programmatic access. Cannot be changed after creation.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Friendly display name shown in UI (e.g., "Administrator").
    /// </summary>
    public string? DisplayName { get; private set; }

    /// <summary>
    /// Optional description of the role's purpose.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Indicates if the role is active and usable. Supports soft-deletion.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Indicates if this role is a system-critical role that cannot be deleted or renamed.
    /// </summary>
    public bool IsSystem { get; private set; }

    /// <summary>
    /// Optional module name if the role is scoped to a specific module (e.g., "CRM", "Blog").
    /// If null, role is system-wide.
    /// </summary>
    public string? Module { get; private set; }

    /// <summary>
    /// Optional order for UI display or evaluation precedence.
    /// </summary>
    public int? DisplayOrder { get; private set; }

    /// <summary>
    /// Navigation to users assigned to this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    /// <summary>
    /// Navigation to permissions associated with this role.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    /// <summary>
    /// EF Core requires a parameterless constructor.
    /// </summary>
    protected Role() { }

    /// <summary>
    /// Creates a new Role with required and optional parameters.
    /// </summary>
    public Role(
        string name,
        Guid createdByUserId,
        string? displayName = null,
        string? description = null,
        string? module = null,
        int? displayOrder = null)
    {
        SetName(name);
        DisplayName = displayName?.Trim();
        Description = description?.Trim();
        Module = module?.Trim();
        DisplayOrder = displayOrder;
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Updates the technical name of the role. Should only be used during seed or internal logic.
    /// </summary>
    /// <param name="name">New system name</param>
    public void SetName(string name)
    {
        Name = string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Role name is required.", nameof(name))
            : name.Trim();
    }

    /// <summary>
    /// Updates optional UI name and description.
    /// </summary>
    public void UpdateInfo(string? displayName, string? description, Guid modifierUserId)
    {
        DisplayName = displayName?.Trim();
        Description = description?.Trim();
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Marks this role as a system-critical role.
    /// </summary>
    /// <param name="modifierUserId">ID of the user performing this action.</param>
    public void MarkAsSystem(Guid? modifierUserId)
    {
        IsSystem = true;
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Changes module scope of this role.
    /// </summary>
    /// <param name="module">New module name</param>
    /// <param name="modifierUserId">ID of the user modifying</param>
    public void SetModule(string? module, Guid modifierUserId)
    {
        Module = module?.Trim();
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Updates the display order (for UI or logic).
    /// </summary>
    /// <param name="order">New display order</param>
    /// <param name="modifierUserId">ID of the user modifying</param>
    public void SetDisplayOrder(int? order, Guid modifierUserId)
    {
        DisplayOrder = order;
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Disables this role (soft-delete).
    /// </summary>
    /// <param name="modifierUserId">ID of the user modifying</param>
    public void Deactivate(Guid modifierUserId)
    {
        IsActive = false;
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Re-activates this role.
    /// </summary>
    /// <param name="modifierUserId">ID of the user modifying</param>
    public void Activate(Guid modifierUserId)
    {
        IsActive = true;
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Marks this role as logically deleted (soft delete).
    /// </summary>
    /// <param name="modifierUserId">ID of the user performing the deletion</param>
    public void MarkAsDeleted(Guid? modifierUserId)
    {
        IsDeleted = true;
        MarkAsModified(modifierUserId, isDeleted: true);
    }

    /// <summary>
    /// Restores a previously soft-deleted role.
    /// </summary>
    /// <param name="modifierUserId">ID of the user performing the restore</param>
    public void Restore(Guid? modifierUserId)
    {
        IsDeleted = false;
        MarkAsModified(modifierUserId, isDeleted: false);
    }

    /// <summary>
    /// Returns system name as string.
    /// </summary>
    public override string ToString() => Name;
}
