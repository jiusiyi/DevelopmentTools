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
    public class ModbusEthernetTestDeviceDev : DeviceDev
    {
        #region ... Variables  ...
        /// <summary>
        /// 设备的配置信息
        /// </summary>
        private ModbusEthernetConfigData mConfig = new ModbusEthernetConfigData ( );
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override Type CofigCtrlType
        {
            get { return typeof ( ModbusView ); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override object ConfigData
        {
            get
            {
                return mConfig;
            }
            set
            {
                mConfig = value as ModbusEthernetConfigData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override DeviceCommonCfgData CommCfgData
        {
            get
            {
                return base.CommCfgData;
            }
            set
            {
                base.CommCfgData = value;
                var data = base.CommCfgData as EthernetDeviceCfgData;   //端口号默认为502
                data.Port = 502;
                mConfig.CommCfgData = data;
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...
        /// 获取支持的寄存器类型
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public override IList<string> GetSupportedDatas ( ColumnType column )
        {

            if ( column == ColumnType.Memory ) //获取支持的寄存器类型
                return ModbusDevCommon.GetSupportRegionTypes ( );
            return base.GetSupportedDatas ( column );
        }
        /// <summary>
        /// 寄存器列支持的属性
        /// </summary>
        /// <returns></returns>
        protected override ColumnInfo[] GetColumnInfos ( )
        {
            return ModbusDevCommon.GetColumnInfos ( );
        }
        /// <summary>
        /// 获取支持的读写模式
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public override ReadWriteMode GetSupportRWMode ( string region )
        {
            return ReadWriteMode.ReadAndWrite;
        }
        /// <summary>
        /// 重建新的寄存器
        /// </summary>
        /// <param name="lastRegister"></param>
        /// <returns></returns>
        public override Register CreateNewRegister ( Register lastRegister )
        {
            Register register = new Register ( );
            if ( lastRegister == null )
            {
                register.Memory = RegTypeList.Holding_register.ToString ( );
                register.RWMode = GetSupportRWMode ( register.Memory );
                register.Index = "0";
                register.CanonicalDataType = typeof ( long );
            }
            else
            {
                register.Memory = lastRegister.Memory;
                register.DataType = lastRegister.DataType;
                register.RWMode = lastRegister.RWMode;
                register.Index = ( int.Parse ( lastRegister.Index ) + lastRegister.Length ).ToString ( );
                register.Length = lastRegister.Length;
            }
            return register;

        }
        /// <summary>
        /// 错误检查
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        protected override ErrorMessage[] CheckRegister ( Register register )
        {
            return ModbusDevCommon.CheckRegister ( register );
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
