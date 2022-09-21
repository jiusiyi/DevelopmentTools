using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.AI.IO;

namespace ControlEase.IoDrive.Modicon
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusEthernetConfigData : ModbusConfigData
    {
        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
         /// <summary>
        /// 
        /// </summary>
        public ModbusEthernetConfigData ( )
        {
            CommFormat = CommFormat.TCP;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        internal DeviceCommonCfgData CommCfgData
        {
            get;
            set;
        }
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 重载ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            var eConfigData = CommCfgData as EthernetDeviceCfgData;
            if ( eConfigData != null )
                return string.Format ( "{0}:{1},{2},{3}",
                    eConfigData.IPAddress, eConfigData.Port,
                    Address.ToString ( ),
                    CommFormat.ToString ( ),
                    Swap.ToString ( ) );
            else
                return string.Format ( "{0},{1},{2}",
                    Address.ToString ( ),
                    CommFormat.ToString ( ),
                    Swap.ToString ( ) );
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
