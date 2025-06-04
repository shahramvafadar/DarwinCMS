using System;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Join entity that represents the many-to-many relationship between Roles and Permissions.
/// Each RolePermission defines access of a Role to a Permission, possibly within a specific module.
/// </summary>
public class RolePermission : BaseEntity
{
    /// <summary>
    /// ID of the associated role.
    /// </summary>
    public Guid RoleId { get; private set; }

    /// <summary>
    /// ID of the associated permission.
    /// </summary>
    public Guid PermissionId { get; private set; }

    /// <summary>
    /// Optional module name to scope the permission.
    /// For example, the same permission may exist in different modules.
    /// </summary>
    public string? Module { get; private set; }

    /// <summary>
    /// Indicates whether this permission was assigned automatically by the system.
    /// Used to prevent removal of required system-level permissions.
    /// </summary>
    public bool IsSystemPermission { get; private set; }

    /// <summary>
    /// Navigation to the related Role.
    /// </summary>
    public Role? Role { get; private set; }

    /// <summary>
    /// Navigation to the related Permission.
    /// </summary>
    public Permission? Permission { get; private set; }

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected RolePermission() { }

    /// <summary>
    /// Creates a new role-permission assignment.
    /// </summary>
    /// <param name="roleId">ID of the role</param>
    /// <param name="permissionId">ID of the permission</param>
    /// <param name="createdByUserId">ID of the user who created this mapping</param>
    /// <param name="module">Optional module name</param>
    /// <param name="isSystemPermission">Indicates if this is a system-level permission</param>
    public RolePermission(Guid roleId, Guid permissionId, Guid createdByUserId, string? module = null, bool isSystemPermission = false)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        Module = module?.Trim();
        IsSystemPermission = isSystemPermission;
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Marks this permission as built-in (non-removable).
    /// </summary>
    public void MarkAsSystem(Guid? modifierUserId = null)
    {
        IsSystemPermission = true;
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Marks this permission as user-defined and removable.
    /// </summary>
    public void MarkAsCustom(Guid? modifierUserId = null)
    {
        IsSystemPermission = false;
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Updates module scope of this assignment.
    /// </summary>
    /// <param name="module">New module name</param>
    /// <param name="modifierUserId">ID of the user modifying</param>
    public void UpdateModule(string? module, Guid modifierUserId)
    {
        Module = module?.Trim();
        MarkAsModified(modifierUserId);
    }

    /// <summary>
    /// Marks this role-permission mapping as logically deleted.
    /// </summary>
    /// <param name="modifierUserId">ID of the user deleting</param>
    public void MarkAsDeleted(Guid? modifierUserId)
    {
        IsDeleted = true;
        MarkAsModified(modifierUserId, isDeleted: true);
    }

    /// <summary>
    /// Restores this mapping from a logically deleted state.
    /// </summary>
    /// <param name="modifierUserId">ID of the user restoring</param>
    public void Restore(Guid? modifierUserId)
    {
        IsDeleted = false;
        MarkAsModified(modifierUserId, isDeleted: false);
    }

    /// <summary>
    /// Returns a string summary for debugging and logs.
    /// </summary>
    public override string ToString()
        => $"{RoleId} → {PermissionId}" + (string.IsNullOrWhiteSpace(Module) ? "" : $" @ {Module}");
}
