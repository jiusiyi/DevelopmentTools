using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Collections;
using ControlEase.Inspec.View;
using System.ComponentModel;
using ControlEase.AI.View;
using ControlEase.Nexus.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using ControlEase.Inspec.ViewCore;
using System.Windows.Forms.Design;


namespace ControlEase.Inspec.TreeView
{
    public class ViewMenuItemEditor : UITypeEditor
    {
        new bool IsDropDownResizable
        { 
            get {return false;}
        }
        
        TreeViewControlViewModel mTreeViewControlViewModel;

        private IWindowsFormsEditorService _editorService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle ( System.ComponentModel.ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue ( System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            mTreeViewControlViewModel =GetTreeNodeControlViewModel ( context );
            if ( mTreeViewControlViewModel != null )
            {
                var ViewShapeService = mTreeViewControlViewModel.ViewShapeService;
                List<string> menus = ViewShapeService.GetShapeNames ( typeof ( InMenu ) );
                _editorService = ( System.Windows.Forms.Design.IWindowsFormsEditorService ) provider.GetService ( typeof ( System.Windows.Forms.Design.IWindowsFormsEditorService ) );
                if ( _editorService != null )
                {
                    ListBox mlb = new ListBox();
                    mlb.SelectionMode = SelectionMode.One;
                    mlb.SelectedValueChanged += OnListBoxSelectedValueChanged;
                    menus.ForEach ( it =>
                    {
                        int index = mlb.Items.Add ( it );
                        if ( it.Equals ( value ) )
                        {
                            mlb.SelectedIndex = index;
                        }
                    } );
                    // show this model stuff
                    _editorService.DropDownControl ( mlb );
                    if ( mlb.SelectedItem == null ) // no selection, return the passed-in value as is
                        return value;

                    return mlb.SelectedItem.ToString();
                }
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListBoxSelectedValueChanged ( object sender, EventArgs e )
        {
            // close the drop down as soon as something is clicked
            _editorService.CloseDropDown ( );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected TreeViewControlViewModel GetTreeNodeControlViewModel ( System.ComponentModel.ITypeDescriptorContext context )
        {
            TreeViewControlViewModel ControlVM = null;
            if ( ( context != null ) && ( context.Instance != null ) )
            {
                if ( context.Instance is NormalControlPropertyDescriptor )
                {
                    NormalControlPropertyDescriptor descrptor = context.Instance as NormalControlPropertyDescriptor;
                    if ( ( descrptor != null ) && ( descrptor.DelegateShape != null ) )
                    {
                        WpfSurrogate surrogate = descrptor.DelegateShape as WpfSurrogate;

                        if ( ( surrogate != null ) && ( surrogate.InnerElement != null ) )
                        {
                            if ( surrogate.InnerElement is TreeViewControlViewModel )
                                ControlVM = surrogate.InnerElement as TreeViewControlViewModel;
                        }
                    }
                }
                else if ( context.Instance is DelegateTreeNodeViewModel )
                {
                    ControlVM = ( context.Instance as DelegateTreeNodeViewModel ).SelectObject.Root;
                }
                return ControlVM;
            }
            return ControlVM;
        }

    }
}
