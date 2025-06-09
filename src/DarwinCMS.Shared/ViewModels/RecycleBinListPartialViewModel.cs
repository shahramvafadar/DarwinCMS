using System;
using System.Collections.Generic;
using DarwinCMS.Shared.ViewModels.Interfaces; // For ILogicalDeletableViewModel

namespace DarwinCMS.Shared.ViewModels
{
    /// <summary>
    /// A generic ViewModel for the _RecycleBinList partial view.
    /// Contains the list of deleted items and configuration for displaying the RecycleBinListViewComponent.
    /// </summary>
    /// <typeparam name="TModel">
    /// The type of the ViewModel for each deleted item.
    /// Must implement ILogicalDeletableViewModel.
    /// </typeparam>
    public class RecycleBinListPartialViewModel<TModel>
        where TModel : ILogicalDeletableViewModel
    {
        /// <summary>
        /// The list of deleted (soft-deleted) items to display.
        /// </summary>
        public List<TModel> Items { get; set; } = new();

        /// <summary>
        /// The name of the controller to which the restore and hard delete actions should be posted.
        /// Example: "Pages", "Users".
        /// </summary>
        public string ControllerName { get; set; } = string.Empty;

        /// <summary>
        /// The column title for the first field to display in the table.
        /// Example: "Title" or "Name".
        /// </summary>
        public string FirstFieldName { get; set; } = string.Empty;

        /// <summary>
        /// A delegate function that extracts the first field value from the item for display.
        /// </summary>
        public Func<TModel, string>? FirstFieldSelector { get; set; }

        /// <summary>
        /// The column title for the second field to display in the table.
        /// Example: "Slug" or "Email".
        /// </summary>
        public string SecondFieldName { get; set; } = string.Empty;

        /// <summary>
        /// A delegate function that extracts the second field value from the item for display.
        /// </summary>
        public Func<TModel, string>? SecondFieldSelector { get; set; }
    }
}
