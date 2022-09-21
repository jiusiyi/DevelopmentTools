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
using System.Globalization;

namespace ControlEase.Inspec.TreeView
{
    public class MarginSpaceConverter : IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int Margin = ( int ) values[0];
                int Space = ( int ) values[1];
                return new Thickness ( Margin, ( ( double ) Space / 2.0 ), 0, ( ( double ) Space / 2.0 ) );
            }
            catch
            {
                return new Thickness (0,0,0,0 );
            }
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
     }
}
