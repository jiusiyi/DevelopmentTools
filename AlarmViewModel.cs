using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.Nexus.Presentation;
using ControlEase.IoDrive.Hikvision.Properties;

namespace ControlEase.IoDrive.Hikvision
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmViewModel : ViewModel
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private AlarmConfigData mConfigData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configData"></param>
        public AlarmViewModel ( AlarmConfigData configData )
        {
            this.mConfigData = configData;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public string AlarmResetTimeStr
        {
            get
            {
                return Resources.AlarmResetTime;
            }
        }
        /// <summary>
        ///报警复位时间
        /// </summary>
        public int AlarmResetTime
        {
            get
            {
                return mConfigData.AlarmResetTime;
            }
            set
            {
                if ( value < 0 || value > 60 )
                    return;
                if ( value != mConfigData.AlarmResetTime )
                {
                    mConfigData.AlarmResetTime = value;
                    OnPropertyChanged ( "AlarmResetTime" );
                }
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
