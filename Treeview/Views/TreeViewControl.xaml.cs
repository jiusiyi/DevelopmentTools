using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Interaction logic for TreeViewControl.xaml
    /// </summary>
    public partial class TreeViewControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public TreeViewControl ( )
        {
            InitializeComponent ( );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded ( object sender, RoutedEventArgs e )
        {
            if ((sender as TreeViewControl).DataContext != null)
            {
                ((sender as TreeViewControl).DataContext as TreeViewControlViewModel).TreeViewControl = this;
                ((sender as TreeViewControl).DataContext as TreeViewControlViewModel).InitializeCommand();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_ContextMenuOpening ( object sender, ContextMenuEventArgs e )
        {
            if (( e .OriginalSource as FrameworkElement ).DataContext is TreeNodeViewModel )
                e.Handled = true;
        }
    }
}
