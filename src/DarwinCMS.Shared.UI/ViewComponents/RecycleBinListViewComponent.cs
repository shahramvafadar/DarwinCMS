using DarwinCMS.Shared.ViewModels.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace DarwinCMS.Shared.UI.ViewComponents
{
    /// <summary>
    /// A flexible ViewComponent for displaying a list of soft-deleted items in a table.
    /// It renders action buttons (Restore/HardDelete) that post to the appropriate controller actions.
    /// </summary>
    public class RecycleBinListViewComponent : ViewComponent
    {
        /// <summary>
        /// Renders the recycle bin list table for soft-deleted items.
        /// </summary>
        /// <typeparam name="TModel">The type of items to display (must implement ILogicalDeletableViewModel).</typeparam>
        /// <param name="model">The list of soft-deleted items to display.</param>
        /// <param name="controllerName">
        /// The name of the controller to which the restore and hard delete actions will be posted.
        /// Example: "Pages", "Users". Required.
        /// </param>
        /// <param name="restoreAction">
        /// The name of the action method to restore an item.
        /// Defaults to "Restore" if not specified.
        /// </param>
        /// <param name="hardDeleteAction">
        /// The name of the action method to permanently delete an item.
        /// Defaults to "HardDelete" if not specified.
        /// </param>
        /// <param name="firstFieldName">
        /// The column title of the first field to display in the table. Example: "Title" or "Name".
        /// </param>
        /// <param name="firstFieldSelector">
        /// A delegate function that extracts the first field value from the item for display.
        /// </param>
        /// <param name="secondFieldName">
        /// The column title of the second field to display in the table. Example: "Slug" or "Email".
        /// </param>
        /// <param name="secondFieldSelector">
        /// A delegate function that extracts the second field value from the item for display.
        /// </param>
        /// <returns>The rendered HTML table of soft-deleted items.</returns>
        public IViewComponentResult Invoke<TModel>(
            List<TModel> model,
            string controllerName,
            string? restoreAction,
            string? hardDeleteAction,
            string firstFieldName,
            Func<TModel, string> firstFieldSelector,
            string secondFieldName,
            Func<TModel, string> secondFieldSelector)
            where TModel : ILogicalDeletableViewModel
        {
            // Use default action names if not provided.
            restoreAction ??= "Restore";
            hardDeleteAction ??= "HardDelete";

            var viewModel = new RecycleBinListViewModel<TModel>
            {
                Items = model,
                ControllerName = controllerName,
                RestoreAction = restoreAction,
                HardDeleteAction = hardDeleteAction,
                FirstFieldName = firstFieldName,
                FirstFieldSelector = firstFieldSelector,
                SecondFieldName = secondFieldName,
                SecondFieldSelector = secondFieldSelector
            };

            return View(viewModel);
        }
    }

    /// <summary>
    /// ViewModel for the RecycleBinListViewComponent, containing configuration and data for rendering the table.
    /// </summary>
    /// <typeparam name="TModel">The type of items to display.</typeparam>
    public class RecycleBinListViewModel<TModel>
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
        /// The name of the action method to restore an item.
        /// Defaults to "Restore" if not specified.
        /// </summary>
        public string RestoreAction { get; set; } = "Restore";

        /// <summary>
        /// The name of the action method to permanently delete an item.
        /// Defaults to "HardDelete" if not specified.
        /// </summary>
        public string HardDeleteAction { get; set; } = "HardDelete";

        /// <summary>
        /// The column title for the first field to display in the table. Example: "Title" or "Name".
        /// </summary>
        public string FirstFieldName { get; set; } = string.Empty;

        /// <summary>
        /// A delegate function that extracts the first field value from the item for display.
        /// </summary>
        public Func<TModel, string>? FirstFieldSelector { get; set; }

        /// <summary>
        /// The column title for the second field to display in the table. Example: "Slug" or "Email".
        /// </summary>
        public string SecondFieldName { get; set; } = string.Empty;

        /// <summary>
        /// A delegate function that extracts the second field value from the item for display.
        /// </summary>
        public Func<TModel, string>? SecondFieldSelector { get; set; }
    }
}
