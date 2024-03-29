﻿/**********************************************************************************
* =================================================================================
* Copyright © Beijing ControlEase Automation Software Co., Ltd. All rights reserved.
* =================================================================================
* Name: AniInputAnalog.cs
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
using ControlEase.AI.Security;
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
    /// Animate input analog.
    /// </summary>
    [AnimateConnectionValueType ( typeof ( decimal ) )]
    [AniType ( typeof ( AniTypeInputAnalog ) )]
    public class AniInputAnalog : AniInputBase<Double> 
    {
        #region ... Variables  ...
        private static Decimal mCurrentValue = 0.0m;
        /// <summary>
        /// Max value.
        /// </summary>
        private Decimal valueMax = 9999m;
        /// <summary>
        /// Min value.
        /// </summary>
        private Decimal valueMin = -9999m;
        /// <summary>
        /// decimal number.
        /// </summary>
        private int decimalNum = 15;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        public AniInputAnalog ( )
        {
            IgnoreWhenRunning = true; 
        }

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// Gets or sets max value.
        /// </summary>
        public Decimal ValueMax
        {
            get { return valueMax; }
            set { valueMax = value; }
        }
        /// <summary>
        /// Gets or sets value min.
        /// </summary>
        public Decimal ValueMin
        {
            get { return valueMin; }
            set { valueMin = value; }
        }
        /// <summary>
        /// Gets allow decimal number.
        /// </summary>
        public bool AllowDecimalNum
        {
            get { return decimalNum != 0; }
        }
        /// <summary>
        /// Gets or sets decimal number.
        /// </summary>
        public int DecimalNum
        {
            get { return decimalNum; }
            set
            {
                if ( value >= 0 && value <= 15 )
                {
                    decimalNum = value;
                }
            }
        }
        /// <summary>
        /// Gets default creator.
        /// </summary>
        public override Func<object> DefaultCreator
        {
            get { return ( ) => new AniInfoAnalogInput ( ); }
        }
        /// <summary>
        /// 变量类型
        /// </summary>
        public Type TagType { get; set; }

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
        /// LinkCaculate
        /// </summary>
        /// <returns></returns>
        public override GraphicsPath LinkCaculate ( )
        {            
          
                AniInputAnalog aniInfo = this as AniInputAnalog;
                Debug.Assert ( aniInfo != null );

                Decimal value = 0;

                Decimal tagValue;
                if ( GetTagValue (out tagValue ) )
                {
                    value = tagValue;
                }

                using ( InputAnalogWithVirkeyForm form = new InputAnalogWithVirkeyForm ( value, BaseShape, mGrpControl, this, TagType ) )
                {
                    var pp = GetRootParent ( mGrpControl );
                    Point screenPoint = Control.MousePosition;//鼠标相对于屏幕左上角的坐标
                    if ( pp != null )
                    {
                        form.TopMost = pp.TopMost;
                        form.Top = screenPoint.Y;
                        form.Left = screenPoint.X;
                    }
                    else
                    {
                        form.TopMost = true;
                        form.Top = screenPoint.Y;
                        form.Left = screenPoint.X;
                    }
                    if ( form.ShowDialog ( ) == DialogResult.OK )
                    {
                        mCurrentValue = form.Value;
                        SetValue ( form.Value );
                    }
                }         
            return null;
        }
        /// <summary>
        /// Links the caculate.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>GraphicsPath.</returns>
        public GraphicsPath LinkCaculate(int x,int y)
        {

            AniInputAnalog aniInfo = this as AniInputAnalog;
            Debug.Assert(aniInfo != null);

            Decimal value = 0;

            Decimal tagValue;
            if (GetTagValue(out tagValue))
            {
                value = tagValue;
            }

            using (InputAnalogWithVirkeyForm form = new InputAnalogWithVirkeyForm(value, BaseShape, mGrpControl, this, TagType))
            {
                var pp = GetRootParent(mGrpControl);
                if (pp != null)
                {
                    form.TopMost = pp.TopMost;
                    form.Top = y;
                    form.Left = x;
                }
                else
                {
                    form.TopMost = true;
                    form.Top = y;
                    form.Left = x;
                }
                if (form.ShowDialog() == DialogResult.OK)
                {
                    mCurrentValue = form.Value;
                    SetValue(form.Value);
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        /// <returns></returns>
        private bool GetTagValue ( out Decimal tagValue )
        {
            var tag = ServiceLocator.Current.Resolve<ITagProvider> ( ).GetTag ( this.Expression );
            if ( tag != null )
            {
                tagValue = tag.Value != null ?Convert.ToDecimal( tag.Value.ToString ( )) : Decimal.MinValue;
                return true;
            }
            else
            {
                tagValue = Decimal.MinValue;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void  SetValue(object value)
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
            XElement element = new XElement ( "AniInputAnalog", new XAttribute ( "Expression", string.IsNullOrEmpty ( Expression ) ? string.Empty : Expression )
                        , new XAttribute ( "ValueMax", ValueMax )
                        , new XAttribute ( "ValueMin", ValueMin )
                        , new XAttribute ( "DecimalNum", DecimalNum )
                        );
            xe.Add ( element );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXElement ( XElement xe )
        {
            //XElement ele = xe.Element ( "AniInputAnalog" );
            if ( xe != null )
            {
                Expression = xe.Attribute ( "Expression" ).Value;
                ValueMax = Convert.ToDecimal ( xe.Attribute ( "ValueMax" ).Value );
                ValueMin = Convert.ToDecimal ( xe.Attribute ( "ValueMin" ).Value );
                DecimalNum = Convert.ToInt32 ( xe.Attribute ( "DecimalNum" ).Value );
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
