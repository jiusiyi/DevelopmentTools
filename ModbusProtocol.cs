using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.AI.IO;
using ControlEase.IoDrive.CheckOut;
using ControlEase.IoDrive.Modicon.Properties;
using System.Globalization;

namespace ControlEase.IoDrive.Modicon
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusProtocol
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private ModbusConfigData mConfig;
        /// <summary>
        /// 最大打包长度
        /// </summary>
        private int maxCombineLength = 60;//可以是127
        /// <summary>
        /// CRC校验
        /// </summary>
        private Crc16CheckOut mCrcCheck = new Crc16CheckOut ( );
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mConfig"></param>
        public ModbusProtocol ( ModbusConfigData mConfig )
        {
            this.mConfig = mConfig;
        }
        /// <summary>
        /// 
        /// </summary>
        public ModbusProtocol ( )
        {
            // TODO: Complete member initialization
        }
        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...
        #region ...打包...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        internal bool CanAddToReadSampleGroup ( SampleGroup group, Register register )
        {
            //不同寄存器不打包
            if ( group.Memory != register.Memory ) return false;
            //  超过打包长度，不允许将IoTag添加到该包中
            if ( GetCombineLength ( group, register ) >= maxCombineLength )
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 计算打包后的数据长度
        /// </summary>
        /// <param name="group">包</param>
        /// <param name="register">将要添加的变量</param>
        /// <returns>数据长度</returns>
        private int GetCombineLength ( SampleGroup group, Register register )
        {
            int startAddr = Math.Min ( int.Parse ( group.StartAddress ), int.Parse ( register.Index ) );
            int endAddr = Math.Max ( int.Parse ( group.StartAddress ) + group.Length, int.Parse ( register.Index ) + register.Length );
            return endAddr - startAddr;
        }
        #endregion

        #region ...读命令...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        internal TaskInfo BuildReadTaskInfo ( SampleGroup group )
        {
            try
            {
                byte[] sendBuffer = FormReadFarmat ( group );
                var taskInfo = new TaskInfo ( );
                if ( mConfig.CommFormat == CommFormat.RTU )
                {
                    var responseInfo = new ResponseInfo ( )
                    {
                        ResponseType = ResponseType.LL,
                        Length = 3,
                        ComputeLengthFromData = new ResponseInfo.ComputeLength ( myComputeReadLengthRTU )
                    };
                    taskInfo.Add ( new CommandInfo ( ) { Content = sendBuffer }, responseInfo );

                }
                else if ( mConfig.CommFormat == CommFormat.TCP )
                {
                    var responseInfo = new ResponseInfo ( )
                    {
                        ResponseType = ResponseType.LL,
                        Length = 6,
                        ComputeLengthFromData = new ResponseInfo.ComputeLength ( myComputeLengthTCP )
                    };
                    taskInfo.Add ( new CommandInfo ( ) { Content = sendBuffer }, responseInfo );
                }
                else
                    return null;
                return taskInfo;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private byte[] FormReadFarmat ( SampleGroup group )
        {
            try
            {
                var regType = ( RegTypeList ) Enum.Parse ( typeof ( RegTypeList ), group.Memory );
                byte[] mSendBuffer = null;
                int regLen = group.Length;
                int starAdd = int.Parse ( group.StartAddress );
                #region ...RTU...
                if ( mConfig.CommFormat == CommFormat.RTU )
                {
                    mSendBuffer = new byte[8];
                    //  设备地址
                    mSendBuffer[0] = ( Byte ) mConfig.Address;
                    //  功能码
                    mSendBuffer[1] = GetReadFunctionCode ( regType );
                    //起始寄存器地址高8位，低8位
                    mSendBuffer[2] = BitConverter.GetBytes ( starAdd )[1];
                    mSendBuffer[3] = BitConverter.GetBytes ( starAdd )[0];
                    //寄存器数高8位、低8位
                    mSendBuffer[4] = BitConverter.GetBytes ( regLen )[1];
                    mSendBuffer[5] = BitConverter.GetBytes ( regLen )[0];
                    //  CRC校验
                    int crc = mCrcCheck.Check ( mSendBuffer, 6 );
                    mSendBuffer[6] = Marshal.GetByte ( crc, 0 );
                    mSendBuffer[7] = Marshal.GetByte ( crc, 1 );

                }
                #endregion
                #region ...TCP...
                else if ( mConfig.CommFormat == CommFormat.TCP )
                {
                    mSendBuffer = new byte[12];
                    //长度
                    mSendBuffer[5] = 0x06;
                    //  设备地址
                    mSendBuffer[6] = ( Byte ) mConfig.Address;
                    //  功能码
                    mSendBuffer[7] = GetReadFunctionCode ( regType );
                    //起始寄存器地址高8位，低8位
                    mSendBuffer[8] = BitConverter.GetBytes ( starAdd )[1];
                    mSendBuffer[9] = BitConverter.GetBytes ( starAdd )[0];
                    //寄存器数高8位、低8位
                    mSendBuffer[10] = BitConverter.GetBytes ( regLen )[1];
                    mSendBuffer[11] = BitConverter.GetBytes ( regLen )[0];
                }
                #endregion
                return mSendBuffer;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取读功能码
        /// </summary>
        /// <param name="regType"></param>
        /// <returns></returns>
        private byte GetReadFunctionCode ( RegTypeList regType )
        {
            byte funcode = 0x03;
            switch ( regType )
            {
                case RegTypeList.Holding_register:
                    funcode = 03;
                    break;
                default: break;
            }
            return funcode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int myComputeReadLengthRTU ( byte[] data )
        {
            if ( data[1] > 0x80 )
                return 2;
            else
                return data[2] + 2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int myComputeLengthTCP ( byte[] data )
        {
            return data[5];
        }

        /// <summary>
        /// 校验数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="group"></param>
        /// <param name="errcode"></param>
        /// <param name="errorType"></param>
        /// <param name="isRead"></param>
        /// <returns></returns>
        internal bool CheckReceiveData ( byte[] data, SampleGroup group, ref FailureCodes errcode, bool isRead )
        {
            if ( mConfig.CommFormat == CommFormat.RTU )
                return CheckReceiveDataRTU ( data, group, ref errcode, isRead );
            else if ( mConfig.CommFormat == CommFormat.TCP )
                return CheckReceiveDataTCP ( data, group, ref errcode, isRead );
            else
                return false;
        }
        /// <summary>
        /// TCP校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="group"></param>
        /// <param name="errcode"></param>
        /// <param name="errorType"></param>
        /// <param name="isRead"></param>
        /// <returns></returns>
        private bool CheckReceiveDataTCP ( byte[] data, SampleGroup group, ref FailureCodes errcode, bool isRead )
        {

            //异常码
            if ( data[7] > 0x80 )
            {
                if ( data[8] == 0x06 )
                    errcode = FailureCodes.Response_UnableDetermine;
                else if ( data[8] == 0x02 )
                    errcode = FailureCodes.Response_IllegalAddress;
                else
                    errcode = FailureCodes.Response_NoConformProtocol;
                Logging.Error ( string.Format ( "ModbusProtocol:{0};{1}:{2} {3}", mConfig.Address.ToString ( ), Resources.Error_Code, data[7].ToString ( "X2" ), data[8].ToString ( "X2" ) ) );

                return false;
            }
            //地址校验
            if ( data[6] != mConfig.Address )
            {
                errcode = FailureCodes.Response_NoConformProtocol;
                return false;
            }
            //长度
            int len = 0;
            if ( isRead )
                len = GetReadReceiveSize ( group );
            else
                len = 12;
            if ( len != data.Length )
            {
                errcode = FailureCodes.Response_NoConformProtocol;
                return false;

            }
            return true;
        }

        /// <summary>
        /// rtu检查
        /// </summary>
        /// <param name="data"></param>
        /// <param name="group"></param>
        /// <param name="errcode"></param>
        /// <param name="errorType"></param>
        /// <param name="isRead"></param>
        /// <returns></returns>
        private bool CheckReceiveDataRTU ( byte[] data, SampleGroup group, ref FailureCodes errcode, bool isRead )
        {
            //异常码
            if ( data[1] > 0x80 )
            {
                if ( data[2] == 0x06 )
                    errcode = FailureCodes.Response_UnableDetermine;
                else if ( data[2] == 0x02 )
                    errcode = FailureCodes.Response_IllegalAddress;
                else
                    errcode = FailureCodes.Response_NoConformProtocol;
                Logging.Error ( string.Format ( "ModbusProtocol:{0};{1}:{2} {3}", mConfig.Address.ToString ( ), Resources.Error_Code, data[1].ToString ( "X2" ), data[2].ToString ( "X2" ) ) );
                return false;
            }
            //地址校验
            if ( data[0] != mConfig.Address )
            {
                errcode = FailureCodes.Response_NoConformProtocol;
                return false;
            }
            //  3.检查CRC
            int crc = mCrcCheck.Check ( data, 0, data.Length );
            if ( crc != 0 )
            {
                errcode = FailureCodes.Response_NoConformProtocol;
                return false;
            }
            //长度
            int len = 0;
            if ( isRead )
                len = GetReadReceiveSize ( group );
            else
                len = 8;
            if ( len != data.Length )
            {
                errcode = FailureCodes.Response_NoConformProtocol;
                return false;

            }
            return true;
        }
        /// <summary>
        /// 返回要读取的长度
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public int GetReadReceiveSize ( SampleGroup group )
        {
            int redundancySize = 0;
            if ( mConfig.CommFormat == CommFormat.RTU )
                redundancySize = 5;
            else if ( mConfig.CommFormat == CommFormat.TCP )
                redundancySize = 9;
            return redundancySize + 2 * group.Length;
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="group"></param>
        /// <param name="data"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        internal bool ExplainIO ( SampleGroup group, byte[] data, out ValueInfo[] values )
        {

            try
            {
                List<ValueInfo> mValueInfos = new List<ValueInfo> ( );
                int nStartIndex = 0;
                switch ( mConfig.CommFormat )
                {

                    case CommFormat.RTU:
                        nStartIndex = 3;
                        break;
                    case CommFormat.TCP:
                        nStartIndex = 9;
                        break;
                    default:
                        break;
                }
                int site = 0;
                foreach ( Register register in group.Registers )
                {
                    site = nStartIndex + ( int.Parse ( register.Index ) - int.Parse ( group.StartAddress ) ) * 2;
                    switch ( register.Length )
                    {

                        #region ...1...
                        case 1:
                            {
                                if ( register.DataType == DataTypes.String )
                                    goto default;

                                byte[] tempData = new byte[2];
                                tempData[0] = data[site + 1];
                                tempData[1] = data[site];
                                if ( register.DataType == DataTypes.Int64 )
                                {
                                    Int16 dataValue = BitConverter.ToInt16 ( tempData, 0 );
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );
                                }
                                else if ( register.DataType == DataTypes.BCD )
                                {
                                    UInt16 dataValue = UInt16.Parse ( tempData[0].ToString ( "X2" ) + tempData[1].ToString ( "X2" ) );
                                    mValueInfos.Add ( new ValueInfo ( register, dataValue ) );

                                }
                                else
                                {
                                    UInt16 dataValue = BitConverter.ToUInt16 ( tempData, 0 );
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );
                                }
                            } break;
                        #endregion
                        #region ...2..
                        case 2:
                            {
                                if ( register.DataType == DataTypes.String )
                                    goto default;
                                byte[] tempData = new byte[4];
                                SwapFormat ( data, ( uint ) site, mConfig.Swap, ref tempData );
                                if ( register.DataType == DataTypes.Int64 )
                                {
                                    int dataValue = BitConverter.ToInt32 ( tempData, 0 );
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );
                                }
                                else if ( register.DataType == DataTypes.Double )
                                {
                                    float dataValue = BitConverter.ToSingle ( tempData, 0 );
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );

                                }
                                else if ( register.DataType == DataTypes.BCD )
                                {
                                    string temp = string.Format ( "{0}{1}{2}{3}", tempData[0].ToString ( "X2" ), tempData[1].ToString ( "X2" ), tempData[2].ToString ( "X2" ), tempData[3].ToString ( "X2" ) );

                                    uint dataValue = UInt32.Parse ( temp );
                                    mValueInfos.Add ( new ValueInfo ( register, dataValue ) );

                                }
                                else
                                {
                                    uint dataValue = BitConverter.ToUInt32 ( tempData, 0 );
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );
                                }
                            } break;
                        #endregion
                        #region ...4...
                        case 4:
                            {
                                if ( register.DataType == DataTypes.String )
                                    goto default;

                                byte[] tempData = new byte[8];
                                SwapExFormat ( data, ( uint ) site, mConfig.Swap, ref tempData );
                                if ( register.DataType == DataTypes.Int64 )
                                {
                                    Int64 dataValue = BitConverter.ToInt64 ( tempData, 0 );
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );
                                }
                                else if ( register.DataType == DataTypes.Double )
                                {
                                    double dataValue = BitConverter.ToDouble ( tempData, 0 );
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );

                                }
                                else if ( register.DataType == DataTypes.BCD )
                                {
                                    string temp = string.Format ( "{0}{1}{2}{3}{4}{5}{6}{7}", tempData[0].ToString ( "X2" ), tempData[1].ToString ( "X2" ),
                                        tempData[2].ToString ( "X2" ), tempData[3].ToString ( "X2" ),
                                     tempData[4].ToString ( "X2" ), tempData[5].ToString ( "X2" ),
                                     tempData[6].ToString ( "X2" ), tempData[7].ToString ( "X2" ) );

                                    UInt64 dataValue = UInt64.Parse ( temp );
                                    mValueInfos.Add ( new ValueInfo ( register, dataValue ) );

                                }
                                else
                                {
                                    UInt64 dataValue = BitConverter.ToUInt64 ( tempData, 0 );
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );
                                }

                            } break;
                        #endregion
                        //string
                        #region ...string...
                        default:
                            {
                                
                                byte[] buffer = new byte[register.Length * 2];
                                Array.Copy ( data, site, buffer, 0, register.Length * 2 );
                                try
                                {
                                    string dataValue = Encoding.ASCII.GetString ( buffer, 0, buffer.Length ).TrimEnd ( '\0' ); ;
                                    mValueInfos.Add ( new ValueInfo ( register, register.ConvertToUserTypeValue ( dataValue ) ) );
                                }
                                catch
                                { }
                            } break;
                        #endregion
                    }
                }
                values = mValueInfos.ToArray ( );
                return true;
            }
            catch
            {
                values = new ValueInfo[] { };
                return false;
            }
        }
        /// <summary>
        /// 4字节转换
        /// </summary>
        /// <param name="data"></param>
        /// <param name="nByteSite"></param>
        /// <param name="swap"></param>
        /// <param name="tempData"></param>
        private void SwapFormat ( byte[] data, uint site, Swap swap, ref byte[] tempData )
        {
            //1==0
            if ( swap == Swap.SWAP0 )
            {
                tempData[0] = data[site + 3];
                tempData[1] = data[site + 2];
                tempData[2] = data[site + 1];
                tempData[3] = data[site + 0];
            }
            else if ( swap == Swap.SWAP1 )
            {
                tempData[0] = data[site + 2];
                tempData[1] = data[site + 3];
                tempData[2] = data[site];
                tempData[3] = data[site + 1];
            }
            else if ( swap == Swap.SWAP2 )
            {
                tempData[0] = data[site];
                tempData[1] = data[site + 1];
                tempData[2] = data[site + 2];
                tempData[3] = data[site + 3];
            }
            else if ( swap == Swap.SWAP3 ) //对
            {
                tempData[0] = data[site + 1];
                tempData[1] = data[site];
                tempData[2] = data[site + 3];
                tempData[3] = data[site + 2];
            }
        }
        /// <summary>
        /// 8字节转化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="site"></param>
        /// <param name="swap"></param>
        /// <param name="tempData"></param>
        private void SwapExFormat ( byte[] data, uint site, Swap swap, ref byte[] tempData )
        {
            //1==0
            if ( swap == Swap.SWAP0 )
            {
                tempData[0] = data[site + 7];
                tempData[1] = data[site + 6];
                tempData[2] = data[site + 5];
                tempData[3] = data[site + 4];
                tempData[4] = data[site + 3];
                tempData[5] = data[site + 2];
                tempData[6] = data[site + 1];
                tempData[7] = data[site];
            }
            else if ( swap == Swap.SWAP1 )
            {
                tempData[0] = data[site + 6];
                tempData[1] = data[site + 7];
                tempData[2] = data[site + 4];
                tempData[3] = data[site + 5];
                tempData[4] = data[site + 2];
                tempData[5] = data[site + 3];
                tempData[6] = data[site];
                tempData[7] = data[site + 1];
            }
            else if ( swap == Swap.SWAP2 )
            {
                tempData[0] = data[site];
                tempData[1] = data[site + 1];
                tempData[2] = data[site + 2];
                tempData[3] = data[site + 3];
                tempData[4] = data[site + 4];
                tempData[5] = data[site + 5];
                tempData[6] = data[site + 6];
                tempData[7] = data[site + 7];
            }
            else if ( swap == Swap.SWAP3 )
            {
                tempData[0] = data[site + 1];
                tempData[1] = data[site];
                tempData[2] = data[site + 3];
                tempData[3] = data[site + 2];
                tempData[4] = data[site + 5];
                tempData[5] = data[site + 4];
                tempData[6] = data[site + 7];
                tempData[7] = data[site + 6];
            }
        }
        #endregion

        #region  ...写命令...
        /// <summary>
        /// 用户写入数据合法性校验
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        internal bool ValidateWriteValue ( Register register )
        {
            if ( register.DataType == DataTypes.String )
            {
                string writeValue = "";
                if ( !register.GetWriteValue ( out writeValue ) )
                    return false;
                try
                {
                    Encoding.ASCII.GetBytes ( writeValue );
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        internal TaskInfo BuildWriteTaskInfo ( SampleGroup group )
        {
            try
            {

                byte[] sendBuffer = FormWriteFarmat ( group.Registers[0] );
                var taskInfo = new TaskInfo ( );
                if ( mConfig.CommFormat == CommFormat.RTU )
                {
                    var responseInfo = new ResponseInfo ( )
                    {
                        ResponseType = ResponseType.LL,
                        Length = 3,
                        ComputeLengthFromData = new ResponseInfo.ComputeLength ( myComputeWriteLengthRTU )
                    };
                    taskInfo.Add ( new CommandInfo ( ) { Content = sendBuffer }, responseInfo );

                }
                else if ( mConfig.CommFormat == CommFormat.TCP )
                {
                    var responseInfo = new ResponseInfo ( )
                    {
                        ResponseType = ResponseType.LL,
                        Length = 6,
                        ComputeLengthFromData = new ResponseInfo.ComputeLength ( myComputeLengthTCP )
                    };
                    taskInfo.Add ( new CommandInfo ( ) { Content = sendBuffer }, responseInfo );
                }
                else
                    return null;
                return taskInfo;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int myComputeWriteLengthRTU ( byte[] data )
        {
            if ( data[1] > 0x80 )
                return 2;
            else
                return 5;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        private byte[] FormWriteFarmat ( Register register )
        {
            try
            {
                var regType = ( RegTypeList ) Enum.Parse ( typeof ( RegTypeList ), register.Memory );

                byte[] mSendBuffer = new byte[270];
                byte[] WriteData = GetWriteData ( register );
                int starAddr = int.Parse ( register.Index );
                int index = 0;
                #region ...RTU...
                if ( mConfig.CommFormat == CommFormat.RTU )
                {
                    //  设备地址
                    mSendBuffer[index++] = ( Byte ) mConfig.Address;
                    //  功能码
                    mSendBuffer[index++] = GetWriteFunctionCode ( register );
                    //起始寄存器地址高8位，低8位
                    mSendBuffer[index++] = BitConverter.GetBytes ( starAddr )[1];
                    mSendBuffer[index++] = BitConverter.GetBytes ( starAddr )[0];
                    if ( register.Length > 1 )
                    {

                        mSendBuffer[index++] = BitConverter.GetBytes ( register.Length )[1];
                        mSendBuffer[index++] = BitConverter.GetBytes ( register.Length )[0];
                        mSendBuffer[index++] = Convert.ToByte ( WriteData.Length );
                    }
                    for ( var i = 0; i < WriteData.Length; i++ )
                    {
                        mSendBuffer[index++] = WriteData[i];
                    }
                    //  CRC校验
                    int crc = mCrcCheck.Check ( mSendBuffer, index );
                    mSendBuffer[index++] = Marshal.GetByte ( crc, 0 );
                    mSendBuffer[index++] = Marshal.GetByte ( crc, 1 );

                }
                #endregion
                #region ...TCP...
                else if ( mConfig.CommFormat == CommFormat.TCP )
                {
                    index = 6;
                    //  设备地址
                    mSendBuffer[index++] = ( Byte ) mConfig.Address;
                    //  功能码
                    mSendBuffer[index++] = GetWriteFunctionCode ( register );
                    //起始寄存器地址高8位，低8位
                    mSendBuffer[index++] = BitConverter.GetBytes ( starAddr )[1];
                    mSendBuffer[index++] = BitConverter.GetBytes ( starAddr )[0];
                    if ( register.Length > 1 )
                    {
                        mSendBuffer[index++] = BitConverter.GetBytes ( register.Length )[1];
                        mSendBuffer[index++] = BitConverter.GetBytes ( register.Length )[0];
                        mSendBuffer[index++] = Convert.ToByte ( WriteData.Length );
                    }
                    for ( var i = 0; i < WriteData.Length; i++ )
                    {
                        mSendBuffer[index++] = WriteData[i];
                    }
                    //长度
                    mSendBuffer[5] = ( byte ) ( index - 6 );
                }
                #endregion
                byte[] mSend = new byte[index];
                Array.Copy ( mSendBuffer, 0, mSend, 0, index );
                return mSend;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        private byte GetWriteFunctionCode ( Register register )
        {
            if ( register.Memory == RegTypeList.Holding_register.ToString ( ) )
            {
                if ( register.Length > 1 )
                    return 0x10;
                else
                    return 0x06;
            }
            return 0x06;
        }
        /// <summary>
        /// 获取写数据
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        private byte[] GetWriteData ( Register register )
        {
            byte[] WriteData = new byte[register.Length * 2];
            switch ( register.Length )
            {
                #region ...1...
                case 1:
                    {
                        if ( register.DataType == DataTypes.String )
                            return WriteStringFormat ( register );
                        else if ( register.DataType == DataTypes.BCD )
                        {
                            UInt16 writeData = 0;
                            register.GetWriteValue ( out writeData );
                            string str = writeData.ToString ( "X4" );
                            WriteData[0] = byte.Parse ( str.Substring ( 2, 2 ), NumberStyles.HexNumber );
                            WriteData[1] = byte.Parse ( str.Substring ( 0, 2 ), NumberStyles.HexNumber );
                            return WriteData;
                        }
                        else if ( register.DataType == DataTypes.Int64 )
                        {
                            Int16 writeData = 0;
                            register.GetWriteValue ( out writeData );

                            WriteData[0] = BitConverter.GetBytes ( writeData )[1];
                            WriteData[1] = BitConverter.GetBytes ( writeData )[0];
                        }
                        else
                        {
                            UInt16 writeData = 0;
                            register.GetWriteValue ( out writeData );

                            WriteData[0] = BitConverter.GetBytes ( writeData )[1];
                            WriteData[1] = BitConverter.GetBytes ( writeData )[0];
                        }
                    }
                    break;
                #endregion
                #region ...2...
                case 2:
                    {
                        if ( register.DataType == DataTypes.String )
                            return WriteStringFormat ( register );
                        else if ( register.DataType == DataTypes.BCD )
                        {
                            UInt32 writeData = 0;
                            register.GetWriteValue ( out writeData );
                            string str = writeData.ToString ( "X8" );
                            byte[] tempBuff = new byte[4];
                            tempBuff[0] = byte.Parse ( str.Substring ( 6, 2 ), NumberStyles.HexNumber );
                            tempBuff[1] = byte.Parse ( str.Substring ( 4, 2 ), NumberStyles.HexNumber );
                            tempBuff[2] = byte.Parse ( str.Substring ( 2, 2 ), NumberStyles.HexNumber );
                            tempBuff[3] = byte.Parse ( str.Substring ( 0, 2 ), NumberStyles.HexNumber );
                            WriteData = SwapFormatWrite ( tempBuff, mConfig.Swap );
                        }

                        else if ( register.DataType == DataTypes.Int64 )
                        {
                            Int32 writeData = 0;
                            register.GetWriteValue ( out writeData );
                            WriteData = SwapFormatWrite ( BitConverter.GetBytes ( writeData ), mConfig.Swap );
                        }
                        else if ( register.DataType == DataTypes.Double )
                        {
                            float writeData = 0;
                            register.GetWriteValue ( out writeData );
                            WriteData = SwapFormatWrite ( BitConverter.GetBytes ( writeData ), mConfig.Swap );
                        }
                        else
                        {
                            UInt32 writeData = 0;
                            register.GetWriteValue ( out writeData );
                            WriteData = SwapFormatWrite ( BitConverter.GetBytes ( writeData ), mConfig.Swap );
                        }
                    } break;
                #endregion
                #region ...4...
                case 4:
                    {
                        if ( register.DataType == DataTypes.String )
                            return WriteStringFormat ( register );
                        else if ( register.DataType == DataTypes.BCD )
                        {
                            UInt64 writeData = 0;
                            register.GetWriteValue ( out writeData );
                            string str = writeData.ToString ( "X16" );
                            byte[] tempBuff = new byte[8];
                            tempBuff[0] = byte.Parse ( str.Substring ( 14, 2 ), NumberStyles.HexNumber );
                            tempBuff[1] = byte.Parse ( str.Substring ( 12, 2 ), NumberStyles.HexNumber );
                            tempBuff[2] = byte.Parse ( str.Substring ( 10, 2 ), NumberStyles.HexNumber );
                            tempBuff[3] = byte.Parse ( str.Substring ( 8, 2 ), NumberStyles.HexNumber );
                            tempBuff[4] = byte.Parse ( str.Substring ( 6, 2 ), NumberStyles.HexNumber );
                            tempBuff[5] = byte.Parse ( str.Substring ( 4, 2 ), NumberStyles.HexNumber );
                            tempBuff[6] = byte.Parse ( str.Substring ( 2, 2 ), NumberStyles.HexNumber );
                            tempBuff[7] = byte.Parse ( str.Substring ( 0, 2 ), NumberStyles.HexNumber );
                            WriteData = SwapExFormatWrite ( tempBuff, mConfig.Swap );
                        }

                        else if ( register.DataType == DataTypes.Int64 )
                        {
                            Int64 writeData = 0;
                            register.GetWriteValue ( out writeData );
                            WriteData = SwapExFormatWrite ( BitConverter.GetBytes ( writeData ), mConfig.Swap );
                        }
                        else if ( register.DataType == DataTypes.Double )
                        {
                            double writeData = 0;
                            register.GetWriteValue ( out writeData );
                            WriteData = SwapExFormatWrite ( BitConverter.GetBytes ( writeData ), mConfig.Swap );
                        }
                        else
                        {
                            UInt64 writeData = 0;
                            register.GetWriteValue ( out writeData );
                            WriteData = SwapExFormatWrite ( BitConverter.GetBytes ( writeData ), mConfig.Swap );
                        }
                    } break;
                #endregion
                #region ...字符串...
                default:
                    return WriteStringFormat ( register );

                #endregion
            }
            return WriteData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="swap"></param>
        /// <returns></returns>
        private byte[] SwapFormatWrite ( byte[] data, Swap swap )
        {
            //1==0
            byte[] tempData = new byte[4];
            // tempBuff是低位在前；
            if ( swap == Swap.SWAP0 )
            {
                tempData[0] = data[3];
                tempData[1] = data[2];
                tempData[2] = data[1];
                tempData[3] = data[0];
            }
            else if ( swap == Swap.SWAP1 )
            {
                tempData[0] = data[2];
                tempData[1] = data[3];
                tempData[2] = data[0];
                tempData[3] = data[1];
            }
            else if ( swap == Swap.SWAP2 )
            {
                tempData[0] = data[0];
                tempData[1] = data[1];
                tempData[2] = data[2];
                tempData[3] = data[3];
            }
            else if ( swap == Swap.SWAP3 )
            {
                tempData[0] = data[1];
                tempData[1] = data[0];
                tempData[2] = data[3];
                tempData[3] = data[2];
            }
            return tempData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="swap"></param>
        /// <returns></returns>
        private byte[] SwapExFormatWrite ( byte[] data, Swap swap )
        {
            //1==0
            byte[] tempData = new byte[8];
            if ( swap == Swap.SWAP0 )
            {
                tempData[0] = data[7];
                tempData[1] = data[6];
                tempData[2] = data[5];
                tempData[3] = data[4];
                tempData[4] = data[3];
                tempData[5] = data[2];
                tempData[6] = data[1];
                tempData[7] = data[0];
            }
            else if ( swap == Swap.SWAP1 )
            {
                tempData[0] = data[6];
                tempData[1] = data[7];
                tempData[2] = data[4];
                tempData[3] = data[5];
                tempData[4] = data[2];
                tempData[5] = data[3];
                tempData[6] = data[0];
                tempData[7] = data[1];
            }
            else if ( swap == Swap.SWAP2 )
            {
                tempData[0] = data[0];
                tempData[1] = data[1];
                tempData[2] = data[2];
                tempData[3] = data[3];
                tempData[4] = data[4];
                tempData[5] = data[5];
                tempData[6] = data[6];
                tempData[7] = data[7];
            }
            else if ( swap == Swap.SWAP3 )
            {
                tempData[0] = data[1];
                tempData[1] = data[0];
                tempData[2] = data[3];
                tempData[3] = data[2];
                tempData[4] = data[5];
                tempData[5] = data[4];
                tempData[6] = data[7];
                tempData[7] = data[6];
            }
            return tempData;
        }
        /// <summary>
        /// 字符串写
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        private byte[] WriteStringFormat ( Register register )
        {
            byte[] WriteData = new byte[register.Length * 2];
            string writeData = string.Empty;
            register.GetWriteValue ( out writeData );
            byte[] dValue = Encoding.ASCII.GetBytes ( writeData );
            if ( dValue.Length < register.Length * 2 )
                Array.Copy ( dValue, 0, WriteData, 0, dValue.Length );
            else
                Array.Copy ( dValue, 0, WriteData, 0, register.Length * 2 );
            return WriteData;
        }
        #endregion
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


    }
}
