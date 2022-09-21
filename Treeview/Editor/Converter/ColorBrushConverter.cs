using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ControlEase.Nexus.ComponentModel;
using ControlEase.Inspec.Resources;
using System.Windows.Media;

using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace ControlEase.Inspec.TreeView
{
    public class ColorBrushConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            System.Drawing.Color clr = ( System.Drawing.Color ) value;
            if ( clr  != null)
            {
                SolidColorBrush mSolidColorBrush = new SolidColorBrush (  Color.FromArgb(clr.A,clr.R,clr.G,clr.B));
                return mSolidColorBrush;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return null;
        }

        #endregion
    }
}
