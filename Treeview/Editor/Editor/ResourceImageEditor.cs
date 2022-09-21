using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Drawing.Design;
using ControlEase.Inspec.ViewPresentation;
using ControlEase.Nexus.ComponentModel;
using ControlEase.Inspec.Resources;
using System.Drawing;
using ControlEase.Inspec.ViewCore;

namespace ControlEase.Inspec.TreeView
{
    public class ResourceImageEditor : UITypeEditor
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
            ResourceImageEditorViewModel mm = new ResourceImageEditorViewModel ( );
            int cint =  (int)value ;
            var resSvr = ServiceLocator.Current.Resolve<IResourceService> ( );
            var ss = resSvr.GetResourceItem ( cint );
            if ( ss != null )
            {
                mm.SetTargetResId ( ss.Name );
            }
            if (mm.ShowDialog().Value)
            {
                if (mm.CurrentItem != null)
                {
                    return mm.CurrentItem.Identity;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetPaintValueSupported ( System.ComponentModel.ITypeDescriptorContext context )
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void PaintValue ( System.Drawing.Design.PaintValueEventArgs e )
        {
            int  item = (int)e.Value;
            try
            {
                if ( item == 0 )
                {
                    e.Graphics.DrawRectangle ( Pens.Black, e.Bounds );
                }
                else
                {
                    var resSvr = ServiceLocator.Current.Resolve<IResourceService> ( );
                    string stmp;
                    System.Drawing.Image mImage = ImageCachManager.Manager.GetBitmapWithoutCach ( resSvr.GetResourceItem ( item ).Name, true, out stmp );
                    e.Graphics.DrawImage ( mImage, e.Bounds );
                }
            }
            catch ( Exception )
            {
            }

        }
    }
}
