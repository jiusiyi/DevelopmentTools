using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Interface ITreeViewItemModel
    /// </summary>
    public interface ITreeViewItemModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the selected value path.
        /// </summary>
        /// <value>The selected value path.</value>
        string SelectedValuePath { get; }

        /// <summary>
        /// Gets the display value path.
        /// </summary>
        /// <value>The display value path.</value>
        string DisplayValuePath { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value><c>true</c> if this instance is expanded; otherwise, <c>false</c>.</value>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets the hierarchy.
        /// </summary>
        /// <returns>IEnumerable{ITreeViewItemModel}.</returns>
        IEnumerable<ITreeViewItemModel> GetHierarchy();

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns>IEnumerable{ITreeViewItemModel}.</returns>
        IEnumerable<ITreeViewItemModel> GetChildren();
    }
}
