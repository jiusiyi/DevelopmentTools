using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlEase.IoDrive.Modicon
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusConfigData
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private int mAddress = 1;
        /// <summary>
        /// 
        /// </summary>
        private CommFormat mCommFormat = CommFormat.RTU;
        /// <summary>
        /// 字节转换
        /// </summary>
        private Swap mSwap = Swap.SWAP3;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 地址
        /// </summary>
        public int Address
        {
            get
            {
                return mAddress;
            }
            set
            {
                if ( value != mAddress )
                {
                    if ( value < 1 || value > 255 )
                    {
                        return;
                    }
                    mAddress = value;
                }
            }
        }
        /// <summary>
        /// 使用的通信协议桢格式
        /// </summary>
        public CommFormat CommFormat
        {
            get
            {
                return mCommFormat;
            }
            set
            {
                if ( value != mCommFormat )
                {

                    mCommFormat = value;
                }
            }
        }
        /// <summary>
        /// 字节高低位转换寄存器
        /// </summary>
        public Swap Swap
        {
            get
            {
                return mSwap;
            }
            set
            {
                if ( value != mSwap )
                {

                    mSwap = value;
                }
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
            return string.Format ( "{0},{1}", Address.ToString ( ), CommFormat.ToString ( ) );
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


    }
}
