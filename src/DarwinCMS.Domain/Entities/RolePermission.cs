using DarwinCMS.Domain.Interfaces;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Join entity that represents the many-to-many relationship between Roles and Permissions.
/// Each RolePermission defines access of a Role to a Permission, possibly within a specific module.
/// </summary>
public class RolePermission : BaseEntity, IAuditableEntity
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
    /// ID of the user who created this mapping.
    /// </summary>
    public Guid CreatedByUserId { get; private set; }

    /// <summary>
    /// ID of the user who last modified this mapping.
    /// </summary>
    public Guid? ModifiedByUserId { get; private set; }

    /// <summary>
    /// Navigation to the related Role.
    /// </summary>
    public Role? Role { get; private set; }

    /// <summary>
    /// Navigation to the related Permission.
    /// </summary>
    public Permission? Permission { get; private set; }

    /// <summary>
    /// Required by EF Core.
    /// </summary>
    protected RolePermission() { }

    /// <summary>
    /// Creates a new role-permission assignment.
    /// </summary>
    /// <param name="roleId">ID of the role</param>
    /// <param name="permissionId">ID of the permission</param>
    /// <param name="createdByUserId">ID of the creator</param>
    /// <param name="module">Optional module name</param>
    /// <param name="isSystemPermission">Whether the permission is built-in</param>
    public RolePermission(Guid roleId, Guid permissionId, Guid createdByUserId, string? module = null, bool isSystemPermission = false)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        CreatedByUserId = createdByUserId;
        Module = module?.Trim();
        IsSystemPermission = isSystemPermission;
    }

    /// <summary>
    /// Marks this permission as built-in (non-removable).
    /// </summary>
    public void MarkAsSystem() => IsSystemPermission = true;

    /// <summary>
    /// Marks this permission as user-defined and removable.
    /// </summary>
    public void MarkAsCustom() => IsSystemPermission = false;

    /// <summary>
    /// Updates module scope of this assignment.
    /// </summary>
    public void UpdateModule(string? module, Guid modifierUserId)
    {
        Module = module?.Trim();
        SetModifiedBy(modifierUserId);
    }

    /// <summary>
    /// Updates the ModifiedByUserId and ModifiedAt fields.
    /// </summary>
    private void SetModifiedBy(Guid userId)
    {
        ModifiedByUserId = userId;
        MarkAsModified();
    }

    /// <summary>
    /// Returns a string summary for debugging and logs.
    /// </summary>
    public override string ToString()
        => $"{RoleId} → {PermissionId}" + (string.IsNullOrWhiteSpace(Module) ? "" : $" @ {Module}");
}
