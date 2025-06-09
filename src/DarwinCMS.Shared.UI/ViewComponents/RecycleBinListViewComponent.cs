using Microsoft.AspNetCore.Mvc;

using System.Reflection;

namespace DarwinCMS.Shared.UI.ViewComponents
{
    /// <summary>
    /// A reusable ViewComponent that renders a table of soft-deleted items for any entity type.
    /// It uses reflection to extract and display property values dynamically.
    /// </summary>
    public class RecycleBinListViewComponent : ViewComponent
    {
        /// <summary>
        /// Renders a list of soft-deleted items using reflection to extract two main fields, DeletedAt, and DeletedByUserId.
        /// </summary>
        /// <param name="items">
        /// A list of soft-deleted items (can be any entity or ViewModel that has the needed properties).
        /// </param>
        /// <param name="controllerName">
        /// The name of the controller to which the restore and hard delete actions will be posted.
        /// Example: "Pages", "Users".
        /// </param>
        /// <param name="firstFieldName">
        /// The column title of the first field to display (example: "Title").
        /// </param>
        /// <param name="secondFieldName">
        /// The column title of the second field to display (example: "Slug").
        /// </param>
        /// <param name="firstPropertyName">
        /// The name of the property in the items that will be shown in the first column.
        /// </param>
        /// <param name="secondPropertyName">
        /// The name of the property in the items that will be shown in the second column.
        /// </param>
        /// <returns>The rendered HTML table of soft-deleted items.</returns>
        public IViewComponentResult Invoke(
            IEnumerable<object> items,
            string controllerName,
            string firstFieldName,
            string secondFieldName,
            string firstPropertyName,
            string secondPropertyName)
        {
            // Use reflection to extract the needed fields from each item
            var displayItems = items.Select(item =>
            {
                var type = item.GetType();
                return new RecycleBinDisplayItem
                {
                    Id = (Guid?)type.GetProperty("Id")?.GetValue(item),
                    FirstField = type.GetProperty(firstPropertyName)?.GetValue(item)?.ToString() ?? string.Empty,
                    SecondField = type.GetProperty(secondPropertyName)?.GetValue(item)?.ToString() ?? string.Empty,
                    DeletedAt = (DateTime?)type.GetProperty("ModifiedAt")?.GetValue(item),
                    DeletedByUserId = (Guid?)type.GetProperty("ModifiedByUserId")?.GetValue(item)
                };
            }).ToList();

            var viewModel = new RecycleBinListDisplayViewModel
            {
                ControllerName = controllerName,
                FirstFieldName = firstFieldName,
                SecondFieldName = secondFieldName,
                Items = displayItems
            };
            // TODO: BUG: Fix the issue where <format-date> TagHelper does not render correctly in RecycleBinListViewComponent. in Defult.cshtml

            return View(viewModel);
        }
    }

    /// <summary>
    /// Represents a single row in the recycle bin table.
    /// Contains the dynamic data extracted from an entity for display.
    /// </summary>
    public class RecycleBinDisplayItem
    {
        /// <summary>
        /// The unique identifier of the deleted item.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The display value for the first field in the table row.
        /// This could be a title, name, etc.
        /// </summary>
        public string FirstField { get; set; } = string.Empty;

        /// <summary>
        /// The display value for the second field in the table row.
        /// This could be a slug, email, etc.
        /// </summary>
        public string SecondField { get; set; } = string.Empty;

        /// <summary>
        /// The UTC date and time when the item was last modified (i.e., soft-deleted).
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// The ID of the user who last modified (deleted) the item.
        /// </summary>
        public Guid? DeletedByUserId { get; set; }
    }

    /// <summary>
    /// ViewModel used to render the recycle bin list table in the Razor view.
    /// Contains configuration and the list of extracted display items.
    /// </summary>
    public class RecycleBinListDisplayViewModel
    {
        /// <summary>
        /// The name of the controller to which the restore and hard delete actions should be posted.
        /// Example: "Pages", "Users".
        /// </summary>
        public string ControllerName { get; set; } = string.Empty;

        /// <summary>
        /// The column title for the first field.
        /// Example: "Title" or "Name".
        /// </summary>
        public string FirstFieldName { get; set; } = string.Empty;

        /// <summary>
        /// The column title for the second field.
        /// Example: "Slug" or "Email".
        /// </summary>
        public string SecondFieldName { get; set; } = string.Empty;

        /// <summary>
        /// The list of display items to render in the table.
        /// </summary>
        public List<RecycleBinDisplayItem> Items { get; set; } = new();
    }
}
