namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Join entity that represents the many-to-many relationship between users and roles.
/// This mapping defines which roles are assigned to a specific user.
/// </summary>
public class UserRole : BaseEntity
{
    /// <summary>
    /// ID of the user this role is assigned to.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// ID of the assigned role.
    /// </summary>
    public Guid RoleId { get; private set; }

    /// <summary>
    /// Optional module label to scope the role (e.g., "Store", "CRM").
    /// </summary>
    public string? Module { get; private set; }

    /// <summary>
    /// Indicates whether this role was assigned automatically by the system.
    /// </summary>
    public bool IsSystemAssigned { get; private set; }

    /// <summary>
    /// Navigation to the user.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Navigation to the role.
    /// </summary>
    public Role? Role { get; private set; }

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected UserRole() { }

    /// <summary>
    /// Creates a new UserRole mapping.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="roleId">The ID of the role.</param>
    /// <param name="module">Optional module label for scoping the role.</param>
    /// <param name="isSystemAssigned">Whether the assignment was automatic (true) or manual (false).</param>
    public UserRole(Guid userId, Guid roleId, string? module = null, bool isSystemAssigned = false)
    {
        UserId = userId;
        RoleId = roleId;
        Module = module?.Trim();
        IsSystemAssigned = isSystemAssigned;
        MarkAsCreated(null);
    }

    /// <summary>
    /// Marks this assignment as system-assigned (automatic).
    /// </summary>
    public void MarkAsSystemAssigned()
    {
        IsSystemAssigned = true;
        MarkAsModified(null);
    }

    /// <summary>
    /// Marks this assignment as manually assigned.
    /// </summary>
    public void MarkAsManuallyAssigned()
    {
        IsSystemAssigned = false;
        MarkAsModified(null);
    }

    /// <summary>
    /// Updates the module label of this assignment.
    /// </summary>
    /// <param name="module">New module label or null.</param>
    public void UpdateModule(string? module)
    {
        Module = module?.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Compares this mapping by UserId and RoleId only.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not UserRole other) return false;
        return UserId == other.UserId && RoleId == other.RoleId;
    }

    /// <summary>
    /// Returns hash code based on UserId and RoleId.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, RoleId);
    }

    /// <summary>
    /// Marks the mapping as logically deleted.
    /// </summary>
    public void MarkAsDeleted(Guid? modifierId = null)
    {
        IsDeleted = true;
        MarkAsModified(modifierId, isDeleted: true);
    }

    /// <summary>
    /// Restores the mapping from soft-deleted state.
    /// </summary>
    public void Restore(Guid? modifierId = null)
    {
        IsDeleted = false;
        MarkAsModified(modifierId, isDeleted: false);
    }
}
