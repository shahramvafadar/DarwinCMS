namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Join entity that represents a many-to-many relationship between Users and Roles.
/// Each user can have multiple roles, and each role can be assigned to multiple users.
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
    /// Optional module context (e.g., "Store", "CRM").
    /// Allows role scoping to specific modules.
    /// </summary>
    public string? Module { get; private set; }

    /// <summary>
    /// Whether this role was assigned automatically or manually.
    /// Useful for user registration flows or defaults.
    /// </summary>
    public bool IsSystemAssigned { get; private set; }

    /// <summary>
    /// Navigation to the associated user.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Navigation to the associated role.
    /// </summary>
    public Role? Role { get; private set; }

    /// <summary>
    /// Required for EF Core.
    /// </summary>
    protected UserRole() { }

    /// <summary>
    /// Creates a new user-role link.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="roleId">Role ID</param>
    /// <param name="module">Optional module label</param>
    /// <param name="isSystemAssigned">Was this assignment done automatically?</param>
    public UserRole(Guid userId, Guid roleId, string? module = null, bool isSystemAssigned = false)
    {
        UserId = userId;
        RoleId = roleId;
        Module = module?.Trim();
        IsSystemAssigned = isSystemAssigned;
    }

    /// <summary>
    /// Marks this role as assigned by system.
    /// </summary>
    public void MarkAsSystemAssigned() => IsSystemAssigned = true;

    /// <summary>
    /// Marks this role as manually assigned by user or admin.
    /// </summary>
    public void MarkAsManuallyAssigned() => IsSystemAssigned = false;

    /// <summary>
    /// Updates the module context of the role assignment.
    /// </summary>
    /// <param name="module">New module name or null</param>
    public void UpdateModule(string? module)
    {
        Module = module?.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Compares UserRole by UserId and RoleId
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

}
