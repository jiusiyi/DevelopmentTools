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
    public class ModbusSerialConfigData : ModbusConfigData
    {
        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ModbusSerialConfigData ( )
        {
            CommFormat = CommFormat.RTU;
        }
        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 重载ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
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
