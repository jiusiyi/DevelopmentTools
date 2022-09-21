using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.Nexus.Logging;

namespace ControlEase.Inspec.DaHeng
{
    internal class Logging
    {
        /// <summary>
        /// 
        /// </summary>
        static readonly ILogger mLogger = Log.GetLogger ( "View" );
        /// <summary>
        /// Internal logger for trends.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public static ILogger Logger
        {
            get { return mLogger; }
        }
    } 
}
