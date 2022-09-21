using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.Nexus.Presentation;

namespace ControlEase.IoDrive.Modicon
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusViewModel : ViewModel
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private ModbusConfigData mConfigData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configData"></param>
        public ModbusViewModel ( ModbusConfigData configData )
        {
            mConfigData = configData;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        #region ...Address...
        /// <summary>
        /// 设备地址
        /// </summary>
        public int Address
        {
            get
            {
                return mConfigData.Address;
            }
            set
            {
                if ( value < 0 || value > 255 )
                    return;
                if ( value != mConfigData.Address )
                {
                    mConfigData.Address = value;
                    OnPropertyChanged ( "Address" );
                }
            }
        }
        #endregion
        #region ...CommFormat...
        /// <summary>
        /// 协议格式为RTU
        /// </summary>
        public bool RTUChecked
        {
            get
            {
                return mConfigData.CommFormat == CommFormat.RTU;
            }
            set
            {
                if ( value )
                {
                    mConfigData.CommFormat = CommFormat.RTU;
                    OnPropertyChanged ( "RTUChecked" );
                }
            }
        }
        /// <summary>
        /// 协议格式为TCP
        /// </summary>
        public bool TCPChecked
        {
            get
            {
                return mConfigData.CommFormat == CommFormat.TCP;
            }
            set
            {
                if ( value )
                {
                    mConfigData.CommFormat = CommFormat.TCP;
                    OnPropertyChanged ( "TCPChecked" );
                }
            }
        }
        #endregion
        #region ...Swap...
        /// <summary>
        /// 4字节实型SWAP0
        /// </summary>
        public bool SWAP0Checked
        {
            get
            {
                return mConfigData.Swap == Swap.SWAP0;
            }
            set
            {
                if ( value )
                {
                    mConfigData.Swap = Swap.SWAP0;
                    OnPropertyChanged ( "SWAP0Checked" );
                }
            }
        }
        /// <summary>
        /// 4字节实型SWAP1
        /// </summary>
        public bool SWAP1Checked
        {
            get
            {
                return mConfigData.Swap == Swap.SWAP1;
            }
            set
            {
                if ( value )
                {
                    mConfigData.Swap = Swap.SWAP1;
                    OnPropertyChanged ( "SWAP1Checked" );
                }
            }
        }
        /// <summary>
        /// 4字节实型SWAP2
        /// </summary>
        public bool SWAP2Checked
        {
            get
            {
                return mConfigData.Swap == Swap.SWAP2;
            }
            set
            {
                if ( value )
                {
                    mConfigData.Swap = Swap.SWAP2;
                    OnPropertyChanged ( "SWAP2Checked" );
                }
            }
        }
        /// <summary>
        /// 4字节实型SWAP3
        /// </summary>
        public bool SWAP3Checked
        {
            get
            {
                return mConfigData.Swap == Swap.SWAP3;
            }
            set
            {
                if ( value )
                {
                    mConfigData.Swap = Swap.SWAP3;
                    OnPropertyChanged ( "SWAP3Checked" );
                }
            }
        }

        #endregion
        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
