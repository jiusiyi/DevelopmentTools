using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using ControlEase.Nexus.Windows;
using ControlEase.Nexus.ComponentModel;
using ControlEase.Nexus.Presentation;
using System.Windows;
using System.Windows.Media;

namespace ControlEase.Inspec.TreeView
{
    public class ColorPickerEditor : UITypeEditor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle ( System.ComponentModel.ITypeDescriptorContext context )
        {
            //制定弹出Editor的样式，为弹出窗口或dropdownlist，或者为
            return UITypeEditorEditStyle.Modal;
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
            ColorPicker cp = new ColorPicker ( );
            cp.SelectedColor = ( Color ) value;
            
            var windowsVisualizer = ServiceLocator.Current.Resolve<IWindowVisualizer> ( );
            var result = windowsVisualizer.ShowDialogWithOKCancel ( cp, win =>
            {
                win.Title = SR.GetString ( "ColorPicker" );
                win.Width = 200;
                win.Height = 100;
                win.WindowStyle = WindowStyle.ToolWindow;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Topmost = true;

            } );
            if ( ( bool ) result )
            {
              return   cp.SelectedColor;
            }
            return value;
        }
    }
}
