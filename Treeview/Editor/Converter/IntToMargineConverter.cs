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
using System.Windows;

namespace ControlEase.Inspec.TreeView
{
    public class IntToMargineConverter : IValueConverter
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
            int cint = ( int ) value;
            return new Thickness(cint,10,0,10);
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
