using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlEase.IoDrive.Hikvision
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmConfigData
    {
        #region ... Variables  ...
        ///// <summary>
        ///// DVR的设备地址
        ///// </summary>
        //private string mIPAddress = "192.168.0.77";
        ///// <summary>
        ///// DVR的端口号
        ///// </summary>
        //private int mPortNumber = 8000;
        ///// <summary>
        ///// 用户名
        ///// </summary>
        //private string mUserName = "admin";
        ///// <summary>
        ///// 密码
        ///// </summary>
        //private string mPassword = "abcd1234";
        /// <summary>
        /// 报警复位
        /// </summary>
        private int mAlarmResetTime = 2;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public AlarmConfigData ( )
        { }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 输入报警复位时间
        /// </summary>
        public int AlarmResetTime
        {
            get
            {
                return mAlarmResetTime;
            }
            set
            {
                if ( value != mAlarmResetTime )
                    mAlarmResetTime = value;
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return string.Format ( "{0}", AlarmResetTime );
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
