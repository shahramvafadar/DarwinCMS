using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarwinCMS.Shared.ViewModels.Interfaces
{
    /// <summary>
    /// Interface for ViewModels representing logically deleted items.
    /// </summary>
    public interface ILogicalDeletableViewModel
    {
        /// <summary>
        /// The unique identifier of the item.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The UTC date and time when the item was last modified (i.e., deleted).
        /// </summary>
        DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// The ID of the user who last modified (or deleted) the item.
        /// </summary>
        Guid? ModifiedByUserId { get; set; }
    }
}
