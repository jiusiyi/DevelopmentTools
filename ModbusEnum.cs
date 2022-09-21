using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlEase.IoDrive.Modicon
{
    /// <summary>
    /// 设备支持寄存器列表
    /// 
    /// 枚举出设备驱动所要支持所有寄存器类型
    /// </summary>
    public enum RegTypeList
    {
        /// <summary>
        /// 寄存器
        /// </summary>
        Holding_register = 0x03,
    }
    /// <summary>
    /// Modbus通信协议高低位转换方式
    /// </summary>
    public enum Swap
    {
        /// <summary>
        /// 方式0，4321
        /// </summary>
        SWAP0,
        /// <summary>
        /// 方式1，3412
        /// </summary>
        SWAP1,
        /// <summary>
        /// 方式2，1234
        /// </summary>
        SWAP2,
        /// <summary>
        /// 方式3，2143
        /// </summary>
        SWAP3
    }
    /// <summary>
    /// 通信协议桢格式
    /// </summary>
    public enum CommFormat
    {
        /// <summary>
        /// RTU通信方式
        /// </summary>
        RTU,
        /// <summary>
        /// TCP通信方式
        /// </summary>
        TCP
    }
    /// <summary>
    /// 编码格式
    /// </summary>
    public enum CodeFormats
    {
        /// <summary>
        ///GB2312
        /// </summary>
        GB2312,
        /// <summary>
        ///Unicode
        /// </summary>
        Unicode,
        /// <summary>
        /// Asccii
        /// </summary>
        ASCII,
    }
}
