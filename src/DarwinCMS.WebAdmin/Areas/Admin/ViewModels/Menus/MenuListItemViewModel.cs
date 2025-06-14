﻿using System;

using DarwinCMS.Shared.ViewModels.Interfaces;

namespace DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;

/// <summary>
/// View model used for listing menus in the admin UI.
/// Includes deletion metadata for Recycle Bin display.
/// </summary>
public class MenuListItemViewModel : ILogicalDeletableViewModel
{
    /// <summary>
    /// Unique identifier of the menu.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name of the menu (e.g. Main Menu, Footer Links).
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Logical position of the menu (e.g. Header, Footer, Sidebar).
    /// </summary>
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the menu is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Total number of items inside this menu.
    /// This is useful for display purposes in the index table.
    /// </summary>
    public int ItemCount { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public DateTime? ModifiedAt { get; set; }

    /// <inheritdoc />
    public Guid? ModifiedByUserId { get; set; }
}
