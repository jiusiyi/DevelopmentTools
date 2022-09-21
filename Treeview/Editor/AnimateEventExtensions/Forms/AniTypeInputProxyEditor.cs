using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using ControlEase.Inspec.Animates;
using ControlEase.Nexus;
using System.Collections;
using ControlEase.Inspec.View;
using System.ComponentModel;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 
    /// </summary>
    public class AniTypeInputProxyEditor : UITypeEditor
    {
        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null && context.PropertyDescriptor != null)
            {
                if (context.Instance is ICollection)
                {
                    bool isReadOnly = false;
                    ICollection list = context.Instance as ICollection;
                    foreach (object obj in list)
                    {
                        if (obj is BaseShapePropertyDescriptorProxy)
                        {
                            BaseShapePropertyDescriptorProxy descriptor = obj.As<BaseShapePropertyDescriptorProxy>();
                            PropertyDescriptor prop = descriptor.PropertyDescriptors.Find(context.PropertyDescriptor.Name, false);
                            if (prop != null && prop.IsReadOnly)
                            {
                                isReadOnly = true;
                                break;
                            }
                        }
                    }
                    return isReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.Modal;
                }
                else
                {
                    return context.PropertyDescriptor.IsReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            AniTypeInputProxyEditorViewModel mm = new AniTypeInputProxyEditorViewModel();
            if (value != null)
            {
                if ( value is IPropertyAnimate )
                {
                    IPropertyAnimate proxy = value as IPropertyAnimate;
                    if ( proxy != null )
                    {
                        mm.SetTarget ( value );
                    }
                }
            }
            if (mm.ShowDialog().Value)
            {
                return mm.BackType;
            }
            return base.EditValue(context, provider, value);
        }

        
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
