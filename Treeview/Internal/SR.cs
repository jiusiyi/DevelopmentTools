using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.Nexus;
using System.Threading;
using System.Globalization;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// String resources
    /// </summary>
    public class SR
    {
        /// <summary>
        /// 
        /// </summary>
        static readonly IResourceProvider mProvider = new ResourceProvider ( Properties.Resources.ResourceManager );

        /// <summary>
        /// Get string resource value.
        /// </summary>
        /// <param name="name">Resource key.</param>
        /// <returns>String value.</returns>
        public static string GetString ( string name )
        {
            return mProvider.GetString ( name, Thread.CurrentThread.CurrentUICulture );
        }

        /// <summary>
        /// GetString
        /// </summary>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string GetString ( string name, CultureInfo culture )
        {
            return mProvider.GetString ( name, culture );
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        public static IResourceProvider Provider
        {
            get { return mProvider; }
        }

    }
}
