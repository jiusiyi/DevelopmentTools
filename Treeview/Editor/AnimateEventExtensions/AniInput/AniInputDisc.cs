/**********************************************************************************
* =================================================================================
* Copyright © Beijing ControlEase Automation Software Co., Ltd. All rights reserved.
* =================================================================================
* Name: AniInputDisc.cs
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
    /// Animate input disc.
    /// </summary>
    [AnimateConnectionValueType ( typeof ( bool ) )]
    [AniType ( typeof ( AniTypeInputDisc ) )]
    public class AniInputDisc : AniInputBase<bool> 
    {
        #region ... Variables  ...
        /// <summary>
        /// original value
        /// </summary>
        private static bool mOriginalValue = false;

        /// <summary>
        /// False text.
        /// </summary>
        private string falseText = string.Empty;
        /// <summary>
        /// True text.
        /// </summary>
        private string trueText = string.Empty;
        /// <summary>
        /// 保存false文本的资源名称
        /// </summary>
        private string mFalseTextResName = string.Empty;
        /// <summary>
        /// 保存true文本的资源名称
        /// </summary>
        private string mTrueTextResName = string.Empty;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        public AniInputDisc ( )
        {
            IgnoreWhenRunning = true; 
        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 获取或设置离散值“假”文本
        /// </summary>
        public string FalseText
        {
            get { return falseText; }
            set { falseText = value; }
        }
        /// <summary>
        /// 获取或设置离散值“真”文本
        /// </summary>
        public string TrueText
        {
            get { return trueText; }
            set { trueText = value; }
        }
        /// <summary>
        /// 获取或设置false在资源中的文本
        /// </summary>
        public string FalseTextResName
        {
            get { return mFalseTextResName; }
            set { mFalseTextResName = value; }
        }
        /// <summary>
        /// 获取或设置true在资源中的文本
        /// </summary>
        public string TrueTextResName
        {
            get { return mTrueTextResName; }
            set { mTrueTextResName = value; }
        }
        /// <summary>
        /// Gets default creator
        /// </summary>
        public override Func<object> DefaultCreator
        {
            get { return ( ) => new AniInfoDiscInput ( ); }
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
        public override GraphicsPath LinkCaculate ( )
        {
            AniInputDisc aniInfo = this  as AniInputDisc;
            Debug.Assert ( aniInfo != null );                
            bool value = mOriginalValue;
            bool tagValue;
            if ( GetTagValue (  out tagValue ) )
            {
                value = tagValue;
            }
            //IMainForm mainForm = ServiceManager.Services.GetService<IMainForm> ( );
            //Debug.Assert ( mainForm != null );
            using ( InputDiscForm form = new InputDiscForm ( value, BaseShape, mGrpControl, this ) )
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
                if ( form.ShowDialog (  ) == DialogResult.OK )
                {
                    mOriginalValue = form.Value;
                    SetValue ( form.Value );
                }
            }
            return null;
        }

        /// <summary> Links the caculate. </summary>
        /// <returns>GraphicsPath.</returns>
        public  GraphicsPath LinkCaculate (int x,int y )
        {
            AniInputDisc aniInfo = this as AniInputDisc;
            Debug.Assert ( aniInfo != null );
            bool value = mOriginalValue;
            bool tagValue;
            if ( GetTagValue ( out tagValue ) )
            {
                value = tagValue;
            }
            //IMainForm mainForm = ServiceManager.Services.GetService<IMainForm> ( );
            //Debug.Assert ( mainForm != null );
            using ( InputDiscForm form = new InputDiscForm ( value, BaseShape, mGrpControl, this ) )
            {
                var pp = GetRootParent ( mGrpControl );
                Point screenPoint = Control.MousePosition;//鼠标相对于屏幕左上角的坐标
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
        private bool GetTagValue ( out bool tagValue )
        {
            var tag = ServiceLocator.Current.Resolve<ITagProvider> ( ).GetTag ( this.Expression );
            if ( tag != null )
            {
                tagValue = tag.Value != null ?Convert.ToBoolean( tag.Value.ToString ( )) :false;
                return true;
            }
            else
            {
                tagValue = false;
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
            XElement element = new XElement ( "AniInputDisc", new XAttribute ( "Expression", string.IsNullOrEmpty ( Expression ) ? string.Empty : Expression )
                                    , new XAttribute ( "TrueText", string.IsNullOrEmpty ( TrueText ) ? string.Empty : TrueText )
                                    , new XAttribute ( "FalseText", string.IsNullOrEmpty ( FalseText ) ? string.Empty : FalseText ) );
            xe.Add ( element );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXElement ( XElement xe )
        {
            //XElement ele = xe.Element ( "AniInputDisc" );
            if ( xe != null )
            {
                Expression = xe.Attribute ( "Expression" ).Value;
                TrueText = xe.Attribute ( "TrueText" ).Value;
                FalseText = xe.Attribute ( "FalseText" ).Value;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
