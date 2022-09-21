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
    public class FontFamilyConverter : IValueConverter
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
            string font = (string)value ;
            if(!string.IsNullOrEmpty(font))
            {
                System.Windows.Media.FontFamily mFontFamily = new System.Windows.Media.FontFamily ( font );
                return mFontFamily;
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
