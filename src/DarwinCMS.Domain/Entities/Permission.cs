using System;
using System.Collections.Generic;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents an access control unit that defines what actions can be performed in the system.
/// Permissions are typically grouped and assigned to roles.
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// Unique technical identifier of the permission (e.g., "user.manage").
    /// Used in authorization checks and policy evaluation.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Friendly label shown in the UI (e.g., "Manage Users").
    /// Optional, useful for localization and readability.
    /// </summary>
    public string? DisplayName { get; private set; }

    /// <summary>
    /// Name of the module this permission belongs to (e.g., "CMS", "CRM").
    /// Allows grouping and isolation per module.
    /// </summary>
    public string? Module { get; private set; }

    /// <summary>
    /// Detailed explanation or tooltip shown in the admin panel.
    /// Optional.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Indicates whether this permission is a system-critical permission that cannot be deleted or renamed.
    /// </summary>
    public bool IsSystem { get; private set; }

    /// <summary>
    /// Navigation to the roles that have this permission.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    /// <summary>
    /// EF Core constructor for entity materialization.
    /// </summary>
    protected Permission() { }

    /// <summary>
    /// Initializes a new permission entity.
    /// </summary>
    /// <param name="name">Unique system-level identifier.</param>
    /// <param name="createdByUserId">User who created this permission.</param>
    /// <param name="displayName">Optional friendly label.</param>
    /// <param name="module">Optional module name.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="isSystem">Indicates if this is a system-protected permission.</param>
    public Permission(
        string name,
        Guid createdByUserId,
        string? displayName = null,
        string? module = null,
        string? description = null,
        bool isSystem = false)
    {
        SetName(name);
        DisplayName = displayName?.Trim();
        Module = module?.Trim();
        Description = description?.Trim();
        IsSystem = isSystem;
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Sets or updates the internal name. Should only be used internally or during seeding.
    /// </summary>
    /// <param name="name">Unique system-level identifier.</param>
    public void SetName(string name)
    {
        Name = string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Permission name is required.", nameof(name))
            : name.Trim();
    }

    /// <summary>
    /// Updates metadata such as label, module and description.
    /// </summary>
    /// <param name="displayName">Friendly label (optional).</param>
    /// <param name="description">Description for admins (optional).</param>
    /// <param name="module">Module association (optional).</param>
    /// <param name="modifierUserId">ID of the user making this update.</param>
    public void UpdateInfo(string? displayName, string? description, string? module, Guid modifierUserId)
    {
        DisplayName = displayName?.Trim();
        Description = description?.Trim();
        Module = module?.Trim();
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Marks the permission as system-protected (cannot be deleted).
    /// Should only be used by internal logic.
    /// </summary>
    /// <param name="modifierUserId">ID of the user marking this as system.</param>
    public void MarkAsSystem(Guid? modifierUserId)
    {
        IsSystem = true;
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Marks the permission as logically deleted (soft delete).
    /// </summary>
    /// <param name="modifierUserId">ID of the user performing the deletion.</param>
    public void MarkAsDeleted(Guid? modifierUserId)
    {
        IsDeleted = true;
        MarkAsModified(modifierUserId, isDeleted: true);
    }

    /// <summary>
    /// Restores a previously soft-deleted permission.
    /// </summary>
    /// <param name="modifierUserId">ID of the user performing the restore.</param>
    public void Restore(Guid? modifierUserId)
    {
        IsDeleted = false;
        MarkAsModified(modifierUserId, isDeleted: false);
    }

    /// <summary>
    /// Returns the internal name of the permission for debugging.
    /// </summary>
    public override string ToString() => Name;
}
