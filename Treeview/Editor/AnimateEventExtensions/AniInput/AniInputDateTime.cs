/**********************************************************************************
* =================================================================================
* Copyright © Beijing ControlEase Automation Software Co., Ltd. All rights reserved.
* =================================================================================
* Name: AniInputDateTime.cs
* Description: （Simple introduction to the class, module）
*               
* Revision history：	Date			Author			Version		Content	
* ---------------------------------------------------------------------------------
*               1.      2011-12-15		yafeya		0.1			Initial version
*               2.      
*               3.      
*               4.      
* ---------------------------------------------------------------------------------
**********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.AniInfos;
using ControlEase.Inspec.TreeView;
using ControlEase.AI.Tag;
using ControlEase.Nexus.ComponentModel;
using ControlEase.Inspec.Animates;
using System.Xml.Linq;
using System.Drawing;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Animate event datetime input.
    /// </summary>
    [AnimateConnectionValueType ( typeof ( DateTime ) )]
    [AniType ( typeof ( AniTypeInputDateTime ) )]
    public class AniInputDateTime : AniInputBase<DateTime> 
    {
        #region ... Variables  ...
        /// <summary>
        /// original value
        /// </summary>
        private static DateTime mOriginalValue;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        public AniInputDateTime ( )
        {
            IgnoreWhenRunning = true; 
        }
        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public Form GetRootParent(Control cc)
        {
            //if ( cc.Parent != null && cc.Parent is Form )
            //{
            //    return cc.Parent as Form;
            //}
            //else if ( cc.Parent != null )
            //{
            //    return GetRootParent ( cc.Parent );
            //}
            return null;
        }

        /// <summary>
        /// LinkCaculate
        /// </summary>
        /// <returns></returns>
        public override GraphicsPath LinkCaculate ( )
        {
            AniInputDateTime aniInfo = this  as AniInputDateTime;
            Debug.Assert ( aniInfo != null );
            DateTime value = mOriginalValue;
            DateTime tagValue;
            if ( GetTagValue (  out tagValue ) )
            {
                value = tagValue;
            }
            using ( InputDateTime form = new InputDateTime ( value, BaseShape, mGrpControl, this ) )
            {
                var pp = GetRootParent(mGrpControl);
                Point screenPoint = Control.MousePosition;//鼠标相对于屏幕左上角的坐标
                if (pp != null)
                {
                    form.Top = screenPoint.Y;
                    form.Left = screenPoint.X;
                    form.TopMost = pp.TopMost;
                }
                else
                {
                    form.Top = screenPoint.Y;
                    form.Left = screenPoint.X;
                    form.TopMost = true;
                }
                if ( form.ShowDialog ( ) == DialogResult.OK )
                {
                    mOriginalValue = form.Value;

                    if ( aniInfo.AnimateType == typeof ( AniInputDateTime ) )
                    {
                        // 设置变量的值
                        SetValue ( form.Value );
                    }
                    else
                    {
                        SetValue ( form.Value.ToString ( ) );
                    }
                }
            }
            return null;
        }

        /// <summary> Links the caculate. </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>GraphicsPath.</returns>
        public GraphicsPath LinkCaculate(int x,int y)
        {
            AniInputDateTime aniInfo = this as AniInputDateTime;
            Debug.Assert(aniInfo != null);
            DateTime value = mOriginalValue;
            DateTime tagValue;
            if (GetTagValue(out tagValue))
            {
                value = tagValue;
            }
            using (InputDateTime form = new InputDateTime(value, BaseShape, mGrpControl, this))
            {
                var pp = GetRootParent(mGrpControl);
                if (pp != null)
                {
                    form.Top = y;
                    form.Left = x;
                    form.TopMost = pp.TopMost;
                }
                else
                {
                    form.Top = y;
                    form.Left = x;
                    form.TopMost = true;
                }
                if (form.ShowDialog() == DialogResult.OK)
                {
                    mOriginalValue = form.Value;

                    if (aniInfo.AnimateType == typeof(AniInputDateTime))
                    {
                        // 设置变量的值
                        SetValue(form.Value);
                    }
                    else
                    {
                        SetValue(form.Value.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        /// <returns></returns>
        private bool GetTagValue ( out DateTime tagValue )
        {
            var tag = ServiceLocator.Current.Resolve<ITagProvider> ( ).GetTag ( this.Expression );
            if ( tag != null )
            {
                tagValue = tag.Value != null ? Convert.ToDateTime ( tag.Value.ToString ( ) ) : DateTime.MinValue;
                return true;
            }
            else
            {
                tagValue = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void SetValue ( object value )
        {
            ITagProvider tagProvider = ServiceLocator.Current.Resolve<ITagProvider> ( );
            tagProvider.WriteTag ( this.Expression, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void SaveToXElement ( XElement xe )
        {
            XElement element = new XElement ( "AniInputDateTime", new XAttribute ( "Expression", string.IsNullOrEmpty ( Expression ) ? string.Empty : Expression ) );
            xe.Add ( element );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXElement ( XElement xe )
        {
            //XElement ele = xe.Element ( "AniInputDateTime" );
            if ( xe != null )
            {
                Expression = xe.Attribute ( "Expression" ).Value;
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
