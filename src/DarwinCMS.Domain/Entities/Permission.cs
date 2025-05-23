using DarwinCMS.Domain.Interfaces;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents an access control unit that defines what actions can be performed in the system.
/// Permissions are typically grouped and assigned to roles.
/// </summary>
public class Permission : BaseEntity, IAuditableEntity
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
    /// ID of the user who created this permission.
    /// </summary>
    public Guid CreatedByUserId { get; private set; }

    /// <summary>
    /// ID of the last user who modified this permission.
    /// </summary>
    public Guid? ModifiedByUserId { get; private set; }

    /// <summary>
    /// Navigation to the roles that have this permission.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    /// <summary>
    /// EF Core constructor. Required for materialization.
    /// </summary>
    protected Permission() { }

    /// <summary>
    /// Initializes a new permission.
    /// </summary>
    /// <param name="name">System name (unique)</param>
    /// <param name="createdByUserId">User who creates this permission</param>
    /// <param name="displayName">Optional label for UI</param>
    /// <param name="module">Optional module association</param>
    /// <param name="description">Optional description</param>
    public Permission(
        string name,
        Guid createdByUserId,
        string? displayName = null,
        string? module = null,
        string? description = null)
    {
        SetName(name);
        CreatedByUserId = createdByUserId;
        DisplayName = displayName?.Trim();
        Module = module?.Trim();
        Description = description?.Trim();
    }

    /// <summary>
    /// Sets or updates the internal name. Should only be called internally or during seed.
    /// </summary>
    public void SetName(string name)
    {
        Name = string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Permission name is required.", nameof(name))
            : name.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Updates metadata such as label, module and description.
    /// </summary>
    public void UpdateInfo(string? displayName, string? description, string? module, Guid modifierUserId)
    {
        DisplayName = displayName?.Trim();
        Description = description?.Trim();
        Module = module?.Trim();
        SetModifiedBy(modifierUserId);
    }

    /// <summary>
    /// Sets the modifier ID and updates ModifiedAt timestamp.
    /// </summary>
    private void SetModifiedBy(Guid userId)
    {
        ModifiedByUserId = userId;
        MarkAsModified();
    }

    /// <summary>
    /// Returns the internal name of the permission.
    /// </summary>
    public override string ToString() => Name;
}
