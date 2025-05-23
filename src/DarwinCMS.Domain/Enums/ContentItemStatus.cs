namespace DarwinCMS.Domain.Enums;

/// <summary>
/// Represents the publication lifecycle status of a content item.
/// Helps with scheduling, moderation, versioning, etc.
/// </summary>
public enum ContentItemStatus
{
  /// <summary>
  /// Content is in draft mode and not visible to the public.
  /// </summary>
  Draft = 0,

  /// <summary>
  /// Content is scheduled to be published in the future.
  /// </summary>
  Scheduled = 1,

  /// <summary>
  /// Content is published and visible to users.
  /// </summary>
  Published = 2,

  /// <summary>
  /// Content is unpublished or archived.
  /// </summary>
  Archived = 3
}
