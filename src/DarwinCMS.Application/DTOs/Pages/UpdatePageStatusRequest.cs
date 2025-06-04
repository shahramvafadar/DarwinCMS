/// <summary>
/// Request to soft delete or restore a page.
/// </summary>
public class UpdatePageStatusRequest
{
    /// <summary>
    /// ID of the page to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// If true, mark as deleted; if false, restore.
    /// </summary>
    public bool IsDeleted { get; set; }
}
