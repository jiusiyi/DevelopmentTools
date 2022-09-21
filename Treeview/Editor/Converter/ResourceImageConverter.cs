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
using ControlEase.Inspec.ViewCore;

namespace ControlEase.Inspec.TreeView
{
    public class ResourceImageConverter : IValueConverter
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
            //if ( value is ImageResourceItem )
            //{
            //    ImageResourceItem pp = ( value as ImageResourceItem );
            //    if ( pp != null )
            //    {
                    //int cint = System.Convert.ToInt32 ( ( value as ImageResourceItem ).ImageId );

                    int cint = System.Convert.ToInt32 ( value );
                    var resSvr = ServiceLocator.Current.Resolve<IResourceService> ( );
                    ResourceItem ss = resSvr.GetResourceItem ( cint );
                    if ( ss != null )
                    {
                        PictureResourceType pres = ss.ResourceTypeInfo as PictureResourceType;
                        if ( pres != null )
                        {
                            Uri uri = new Uri ( pres.RelativeFullFileName, UriKind.Absolute );
                            return uri;
                        }
                        return null;
                    }
            //    }
            //}
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
