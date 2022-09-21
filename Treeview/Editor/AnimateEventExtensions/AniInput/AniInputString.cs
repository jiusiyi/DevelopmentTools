/**********************************************************************************
* =================================================================================
* Copyright © Beijing ControlEase Automation Software Co., Ltd. All rights reserved.
* =================================================================================
* Name: AniInputString.cs
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
using ControlEase.Nexus.ComponentModel;
using ControlEase.AI.Tag;
using ControlEase.Inspec.Animates;
using System.Xml.Linq;
using System.Drawing;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Animate input string.
    /// </summary>
    [AnimateConnectionValueType ( typeof ( string ) )]
    [AniType ( typeof ( AniTypeInputString ) )]
    public class AniInputString : AniInputBase<string> 
    {
        #region ... Variables  ...
        /// <summary>
        /// oringinal value
        /// </summary>
        private static string mOriginalValue = string.Empty;

        /// <summary>
        /// 是否为密码输入
        /// </summary>
        private bool passwordInput = false;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public AniInputString ( )
        {
            IgnoreWhenRunning = true; 
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 是否为密码输入
        /// </summary>
        public bool PasswordInput
        {
            get { return passwordInput; }
            set { passwordInput = value; }
        }
        /// <summary>
        /// Gets default creator.
        /// </summary>
        public override Func<object> DefaultCreator
        {
            get { return ( ) => new AniInfoStringInput ( ); }
        }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public Form GetRootParent(Control cc)
        {
            //if (cc.Parent != null && cc.Parent is Form)
            //{
            //    return cc.Parent as Form;
            //}
            //else if (cc.Parent != null)
            //{
            //    return GetRootParent(cc.Parent);
            //}
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  GraphicsPath LinkCaculate ( int  x ,int y)
        {
            //Point screenPoint = Control.MousePosition;//鼠标相对于屏幕左上角的坐标
            AniInputString aniInfo = this as AniInputString;
            Debug.Assert ( aniInfo != null );
            string value = mOriginalValue;
            string tagValue;
            if ( GetTagValue ( out tagValue ) )
            {
                value = tagValue;
            }
            using ( InputStringWithVirkeyForm form = new InputStringWithVirkeyForm ( value, BaseShape, mGrpControl, aniInfo ) )
            {
                var pp = GetRootParent ( mGrpControl );

                if ( pp != null )
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
                if ( form.ShowDialog ( ) == DialogResult.OK )
                {
                    mOriginalValue = form.Value;
                    // 设置变量的值
                    SetValue ( form.Value );
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        /// <returns></returns>
        private bool GetTagValue ( out string tagValue )
        {
            var tag =   ServiceLocator.Current.Resolve<ITagProvider>().GetTag(this.Expression);
            if ( tag != null )
            {
                tagValue = tag.Value != null ? tag.Value.ToString ( ) : string.Empty;
                return true;
            }
            else
            {
                tagValue = string.Empty;
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
            XElement element = new XElement ( "AniInputString", new XAttribute ( "Expression", string.IsNullOrEmpty ( Expression ) ? string.Empty : Expression )
             , new XAttribute ( "PasswordInput", PasswordInput ) );
            xe.Add ( element );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXElement ( XElement xe )
        {
            //XElement ele = xe.Element ( "AniInputString" );
            if ( xe != null )
            {
                Expression = xe.Attribute ( "Expression" ).Value;
                PasswordInput = Convert.ToBoolean ( xe.Attribute ( "PasswordInput" ).Value );
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
