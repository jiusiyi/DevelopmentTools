using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Class ExtendedTreeView
    /// </summary>
    public class ExtendedTreeView : System.Windows.Controls.TreeView
    {
        /// <summary>
        /// Creates a <see cref="T:System.Windows.Controls.TreeViewItem" /> to use to display content.
        /// </summary>
        /// <returns>A new <see cref="T:System.Windows.Controls.TreeViewItem" /> to use as a container for content.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            var childItem = ExtendedTreeViewItem.CreateItemWithBinding();

            childItem.OnHierarchyMouseUp += OnChildItemMouseLeftButtonUp;

            return childItem;
        }

        /// <summary>
        /// Called when [child item mouse left button up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnChildItemMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (this.OnHierarchyMouseUp != null)
            {
                this.OnHierarchyMouseUp(this, e);
            }
        }

        /// <summary>
        /// Occurs when [on hierarchy mouse up].
        /// </summary>
        public event MouseEventHandler OnHierarchyMouseUp;
    }

    /// <summary>
    /// Class ExtendedTreeViewItem
    /// </summary>
    public class ExtendedTreeViewItem : TreeViewItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedTreeViewItem"/> class.
        /// </summary>
        public ExtendedTreeViewItem()
        {
            this.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        /// <summary>
        /// Called when [mouse left button up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.OnHierarchyMouseUp != null)
            {
                this.OnHierarchyMouseUp(this, e);
            }
        }

        /// <summary>
        /// Creates a new <see cref="T:System.Windows.Controls.TreeViewItem" /> to use to display the object.
        /// </summary>
        /// <returns>A new <see cref="T:System.Windows.Controls.TreeViewItem" />.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            var childItem = CreateItemWithBinding();

            childItem.MouseLeftButtonUp += OnMouseLeftButtonUp;

            return childItem;
        }

        /// <summary>
        /// Creates the item with binding.
        /// </summary>
        /// <returns>ExtendedTreeViewItem.</returns>
        public static ExtendedTreeViewItem CreateItemWithBinding()
        {
            var tvi = new ExtendedTreeViewItem();

            var expandedBinding = new Binding("IsExpanded");
            expandedBinding.Mode = BindingMode.TwoWay;
            tvi.SetBinding(ExtendedTreeViewItem.IsExpandedProperty, expandedBinding);

            var selectedBinding = new Binding("IsSelected");
            selectedBinding.Mode = BindingMode.TwoWay;
            tvi.SetBinding(ExtendedTreeViewItem.IsSelectedProperty, selectedBinding);

            return tvi;
        }

        /// <summary>
        /// Occurs when [on hierarchy mouse up].
        /// </summary>
        public event MouseEventHandler OnHierarchyMouseUp;
    }
}
