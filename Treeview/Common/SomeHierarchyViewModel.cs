using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ControlEase.Inspec.TreeView
{
/// <summary>
/// Class SomeHierarchyViewModel
/// </summary>
  public class SomeHierarchyViewModel : ITreeViewItemModel
    {
      private List<SomeHierarchyViewModel> mChildren;

      /// <summary>
      /// Initializes a new instance of the <see cref="SomeHierarchyViewModel"/> class.
      /// </summary>
      /// <param name="title">The title.</param>
      /// <param name="children">The children.</param>
      /// <param name="Parent">The Parent.</param>
        public SomeHierarchyViewModel(ComboboxItemViewModel title, List<SomeHierarchyViewModel> children, SomeHierarchyViewModel Parent)
        {
            this.Title = title;
            this.Parent = Parent;
            mChildren = children;


        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SomeHierarchyViewModel"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="children">The children.</param>
        public SomeHierarchyViewModel(ComboboxItemViewModel title, List<SomeHierarchyViewModel> children)
            : this(title, children, null)
        {
            mChildren = new List<SomeHierarchyViewModel>();
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public ComboboxItemViewModel Title { get; set; }

        /// <summary>
        /// Gets or sets the Parent.
        /// </summary>
        /// <value>The Parent.</value>
        public SomeHierarchyViewModel Parent { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<SomeHierarchyViewModel> Children 
        {
            get { return mChildren; } 
            set
            {
                if (value != null)
                {
                    mChildren = value;
                    mChildren.ForEach(ch => ch.Parent = this);
                }
            }
        }

        #region ITreeViewItemModel
        /// <summary>
        /// Gets the selected value path.
        /// </summary>
        /// <value>The selected value path.</value>
        public string SelectedValuePath
        {
            get { return Title.ToString(); }
        }

        /// <summary>
        /// Gets the display value path.
        /// </summary>
        /// <value>The display value path.</value>
        public string DisplayValuePath
        {
            get { return Title.ToString(); }
        }

        private bool isExpanded;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value><c>true</c> if this instance is expanded; otherwise, <c>false</c>.</value>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        private bool isSelected;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// Gets the hierarchy.
        /// </summary>
        /// <returns>IEnumerable{ITreeViewItemModel}.</returns>
        public IEnumerable<ITreeViewItemModel> GetHierarchy()
        {
            var list = GetAscendingHierarchy().Reverse();
            List<ITreeViewItemModel> resultList = new List<ITreeViewItemModel>();
            foreach (var item in list)
            {
                ITreeViewItemModel treeNode = item as ITreeViewItemModel;
                if (treeNode != null)
                    resultList.Add(treeNode);
            }
            return resultList;
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns>IEnumerable{ITreeViewItemModel}.</returns>
        public IEnumerable<ITreeViewItemModel> GetChildren()
        {
            if (Children == null) return null;
            List<ITreeViewItemModel> resultList = new List<ITreeViewItemModel>();
            foreach (var item in this.Children)
            {
                ITreeViewItemModel treeNode = item as ITreeViewItemModel;
                if (treeNode != null)
                    resultList.Add(treeNode);
            }
            return resultList;
        }

        #endregion

        private IEnumerable<SomeHierarchyViewModel> GetAscendingHierarchy()
        {
            var vm = this;

            yield return vm;
            while (vm.Parent != null)
            {
                yield return vm.Parent;
                vm = vm.Parent;
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
