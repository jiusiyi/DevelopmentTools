using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Threading;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionReturnType ( typeof ( object ) )]
    [ContentProperty ( "Key" )]
    public class ResMarkerExtension : MarkupExtension
    {
        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public ResMarkerExtension ( )
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public ResMarkerExtension ( string key )
        {
            Key = key;
        }

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue ( IServiceProvider serviceProvider )
        {
            return GetValue ( );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetValue ( )
        {
            if ( string.IsNullOrEmpty ( Key ) )
            {
                return " ";
            }
            else
            {
                System.Globalization.CultureInfo cinfo = Thread.CurrentThread.CurrentUICulture;
                return Properties.Resources.ResourceManager.GetString ( Key, cinfo );
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


    }
}
