using DarwinCMS.Domain.Interfaces;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a role in the system. Roles define access levels and user grouping.
/// A role can be global or module-specific and contains a set of permissions.
/// </summary>
public class Role : BaseEntity, IAuditableEntity
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
    /// ID of the user who created the role.
    /// </summary>
    public Guid CreatedByUserId { get; private set; }

    /// <summary>
    /// ID of the user who last modified the role.
    /// </summary>
    public Guid? ModifiedByUserId { get; private set; }

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
        CreatedByUserId = createdByUserId;
        DisplayName = displayName?.Trim();
        Description = description?.Trim();
        Module = module?.Trim();
        DisplayOrder = displayOrder;
    }

    /// <summary>
    /// Updates the technical name of the role. Rarely used.
    /// </summary>
    /// <param name="name">New system name</param>
    public void SetName(string name)
    {
        Name = string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Role name is required.", nameof(name))
            : name.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Updates optional UI name and description.
    /// </summary>
    public void UpdateInfo(string? displayName, string? description, Guid modifierUserId)
    {
        DisplayName = displayName?.Trim();
        Description = description?.Trim();
        SetModifiedBy(modifierUserId);
    }

    /// <summary>
    /// Marks this role as a system-critical role.
    /// </summary>
    public void MarkAsSystem()
    {
        IsSystem = true;
        MarkAsModified();
    }


    /// <summary>
    /// Changes module scope of this role.
    /// </summary>
    public void SetModule(string? module, Guid modifierUserId)
    {
        Module = module?.Trim();
        SetModifiedBy(modifierUserId);
    }

    /// <summary>
    /// Updates the display order (for UI or logic).
    /// </summary>
    public void SetDisplayOrder(int? order, Guid modifierUserId)
    {
        DisplayOrder = order;
        SetModifiedBy(modifierUserId);
    }

    /// <summary>
    /// Disables this role (soft-delete).
    /// </summary>
    public void Deactivate(Guid modifierUserId)
    {
        IsActive = false;
        SetModifiedBy(modifierUserId);
    }

    /// <summary>
    /// Re-activates this role.
    /// </summary>
    public void Activate(Guid modifierUserId)
    {
        IsActive = true;
        SetModifiedBy(modifierUserId);
    }

    /// <summary>
    /// Marks this role as modified by a user.
    /// </summary>
    private void SetModifiedBy(Guid userId)
    {
        ModifiedByUserId = userId;
        MarkAsModified();
    }

    /// <summary>
    /// Returns system name as string.
    /// </summary>
    public override string ToString() => Name;
}
