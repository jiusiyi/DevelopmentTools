using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.AI.IO;
using ControlEase.IoDrive.Modicon.Properties;

namespace ControlEase.IoDrive.Modicon
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusEthernetTestDevice : EthernetDevice
    {
        #region ... Variables  ...
        /// <summary>
        /// 设备的配置信息
        /// </summary>
        private ModbusEthernetConfigData mConfig = new ModbusEthernetConfigData ( );
        /// <summary>
        /// 
        /// </summary>
        private ConnectionOption mConnectionOption = new ConnectionOption ( ) { IsActiveConnect = false };
        /// <summary>
        /// 实现IO架构规范的辅助类
        /// </summary>
        private ModbusProtocol mProtocol = new ModbusProtocol ( );
        /// <summary>
        /// 序列码
        /// </summary>
        private int mSerialNum = 1;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 构造
        /// </summary>
        public ModbusEthernetTestDevice ( )
        {
            //注：此时mConfig还没有加载，初始化中重新初始化
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 通讯类型
        /// </summary>
        public override CommunicationType CommType
        {
            get { return CommunicationType.Poll; }
        }
        /// <summary>
        /// 用户配置
        /// </summary>
        public override object ConfigData
        {
            get { return mConfig; }
            set
            {
                if ( value == null )
                    return;
                mConfig = value as ModbusEthernetConfigData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override ConnectionOption ConnectOption
        {
            get
            {
                if ( CommonCfgData.Role == EthernetRole.Client )
                    mConnectionOption.IsActiveConnect = true;
                else
                    mConnectionOption.IsActiveConnect = false;
                return mConnectionOption;
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...
        #region ...正反初始化...
        /// <summary>
        /// 
        /// </summary>
        protected override void Initialize ( )
        {
            mProtocol = new ModbusProtocol ( mConfig );
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void UnInitialize ( )
        {
            base.UnInitialize ( );
        }
        #endregion
        #region ...连接...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ExecutionResult Connect ( )
        {
            return base.Connect ( );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ExecutionResult Disconnect ( )
        {
            return base.Disconnect ( );
        }
        #endregion
        #region ...读写分组...
        /// <summary>
        /// 创建一个包
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        protected override SampleGroup CreateSampleGroup ( Register register )
        {
            return base.CreateSampleGroup ( register );
            //base默认采用是按照寄存器名称建立包名称。
            //SampleGroup group = new SampleGroup ( register );
            //return group;
        }
        /// <summary>
        /// 是否可以把寄存器打到此包中
        /// </summary>
        /// <param name="group"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        protected override bool CanAddToReadSampleGroup ( SampleGroup group, Register register )
        {
            return mProtocol.CanAddToReadSampleGroup ( group, register );
        }
        /// <summary>
        /// 加进读包中
        /// </summary>
        /// <param name="group"></param>
        /// <param name="register"></param>
        protected override void AddToReadSampleGroup ( SampleGroup group, Register register )
        {
            //这里默认会更新group的信息，计算规则是：group.StartAddress是最小地址
            // group.Length= group.Length+register.Length 
            //如果不满足驱动重写。
            base.AddToReadSampleGroup ( group, register );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        protected override bool CanAddToWriteSampleGroup ( SampleGroup group, Register register )
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="register"></param>
        protected override void AddToWriteSampleGroup ( SampleGroup group, Register register )
        {
            base.AddToWriteSampleGroup ( group, register );
        }
        #endregion
        #region ...读命令...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        protected override TaskInfo BuildReadTaskInfo ( SampleGroup group )
        {
            return mProtocol.BuildReadTaskInfo ( group );
        }

        /// <summary>
        /// 执行读命令
        /// </summary>
        /// <param name="cmdInfo"></param>
        /// <returns></returns>
        protected override ExecutionResult ExecuteReadCommand ( CommandInfo cmdInfo )
        {
            if ( mConfig.CommFormat == CommFormat.TCP )
            {
                byte[] byteProtocol = ( byte[] ) cmdInfo.Content;
                //重新填写序列号
                byte[] accessNo = GetAccessSerialNumber ( );
                byteProtocol[0] = accessNo[1];
                byteProtocol[1] = accessNo[0];
            }
            return base.ExecuteReadCommand ( cmdInfo );
        }
        /// <summary>
        /// 获取访问序列
        /// </summary>
        /// <returns></returns>
        public byte[] GetAccessSerialNumber ( )
        {
            byte[] number = BitConverter.GetBytes ( mSerialNum );
            if ( mSerialNum >= 0xFFFE )
            {
                mSerialNum = 0x01;
            }
            else
            {
                mSerialNum = ( ushort ) ( mSerialNum + 1 );
            }
            return number;
        }
        /// <summary>
        /// 	处理读寄存器的设备反馈数据 接收数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override ExecutionResult ProcessReadResponse ( ProcessInfo info, byte[] data )
        {
            var result = new ExecutionResult ( );
            try
            {
                SampleGroup group = info.SampleGroup;
                FailureCodes errcode = FailureCodes.Response_NoConformProtocol;
                if ( mConfig.CommFormat == CommFormat.TCP )
                {
                    byte[] sendbuffer = ( byte[] ) ( ( CommandInfo ) info.CurrentCmdInfo ).Content;
                    if ( data[0] != sendbuffer[0] || data[1] != sendbuffer[1] )
                    {
                        result.IsSucceed = false;
                        result.Message = string.Format ( Resources.ErrCheck );
                        result.FailureCode = errcode;
                        return result;
                    }
                }
                if ( !mProtocol.CheckReceiveData ( data, group, ref errcode, true ) )
                {
                    result.IsSucceed = false;
                    result.Message = Resources.ErrCheck;
                    result.FailureCode = errcode;
                    return result;
                }
                else
                {
                    ValueInfo[] values;
                    if ( mProtocol.ExplainIO ( group, data, out values ) )
                    {
                        if ( values != null && values.Count ( ) > 0 )
                            SetRegistersValue ( values );
                        result.IsSucceed = true;
                    }
                    else
                    {
                        result.IsSucceed = false;
                        result.Message = string.Format ( Resources.ErrParse );
                        result.FailureCode = FailureCodes.Response_NoConformProtocol;
                    }
                    return result;
                }
            }
            catch
            {
                result.IsSucceed = false;
                result.FailureCode = FailureCodes.Response_NoConformProtocol;
                return result;
            }
        }

        #endregion
        #region ...写...
        /// <summary>
        /// 校验用户输入得值是否正确
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        protected override bool ValidateWriteValue ( Register register )
        {
            return mProtocol.ValidateWriteValue ( register );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        protected override TaskInfo BuildWriteTaskInfo ( SampleGroup group )
        {
            return mProtocol.BuildWriteTaskInfo ( group );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdInfo"></param>
        /// <returns></returns>
        protected override ExecutionResult ExecuteWriteCommand ( CommandInfo cmdInfo )
        {
            if ( mConfig.CommFormat == CommFormat.TCP )
            {
                byte[] byteProtocol = ( byte[] ) cmdInfo.Content;
                //重新填写序列号
                byte[] accessNo = GetAccessSerialNumber ( );
                byteProtocol[0] = accessNo[1];
                byteProtocol[1] = accessNo[0];
            }
            return base.ExecuteWriteCommand ( cmdInfo );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override ExecutionResult ProcessWriteResponse ( ProcessInfo info, byte[] data )
        {
            ExecutionResult result = new ExecutionResult ( );
            try
            {
                FailureCodes errcode = FailureCodes.Response_NoConformProtocol;
                if ( mConfig.CommFormat == CommFormat.TCP )
                {
                    byte[] sendbuffer = ( byte[] ) ( ( CommandInfo ) info.CurrentCmdInfo ).Content;
                    if ( data[0] != sendbuffer[0] || data[1] != sendbuffer[1] )
                    {
                        result.IsSucceed = false;
                        result.Message = string.Format ( Resources.ErrCheck );
                        result.FailureCode = errcode;
                        return result;
                    }
                }
                if ( !mProtocol.CheckReceiveData ( data, info.SampleGroup, ref errcode, false ) )
                {
                    result.IsSucceed = false;
                    result.FailureCode = errcode;
                }
                return result;
            }
            catch
            {
                result.IsSucceed = false;
                return result;
            }
        }
        #endregion
        #region ...服务...
        /// <summary>
        /// 执行服务
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="cmdInfo"></param>
        /// <returns></returns>
        protected override ExecutionResult ExecuteServiceCommand ( string serviceId, CommandInfo cmdInfo )
        {
            if ( mConfig.CommFormat == CommFormat.TCP )
            {
                byte[] byteProtocol = ( byte[] ) cmdInfo.Content;
                //重新填写序列号
                byte[] accessNo = GetAccessSerialNumber ( );
                byteProtocol[0] = accessNo[1];
                byteProtocol[1] = accessNo[0];
            }
            return base.ExecuteServiceCommand ( serviceId, cmdInfo );
        }
        /// <summary>
        /// 服务解析
        /// </summary>
        /// <param name="info"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override ExecutionResult ProcessServiceResponse ( ServiceProcessInfo info, byte[] data )
        {
            var result = new ExecutionResult ( );
            try
            {
                if ( mConfig.CommFormat == CommFormat.TCP )
                {
                    byte[] sendbuffer = ( byte[] ) ( ( CommandInfo ) info.CurrentCmdInfo ).Content;
                    if ( data[0] != sendbuffer[0] || data[1] != sendbuffer[1] )
                    {
                        result.IsSucceed = false;
                        result.Message = string.Format ( Resources.ErrCheck );
                        result.FailureCode = FailureCodes.Response_NoConformProtocol;
                        SetServiceResult ( new ServiceResult ( ) { ServiceId = info.ServiceId, Result = null } );
                        return result;
                    }
                }
                if ( info.ServiceId == "Read" )
                {
                    var regValues = ExplainReadServiceIO ( info.SampleGroup, data, ref result );
                    SetServiceResult ( new ServiceResult ( ) { ServiceId = info.ServiceId, Result = regValues } );
                }
                else if ( info.ServiceId == "Reads" )
                {
                    var regValues = ExplainReadsServiceIO ( info.SampleGroup, data, ref result );
                    SetServiceResult ( new ServiceResult ( ) { ServiceId = info.ServiceId, Result = regValues, SampleGroup = info.SampleGroup } );
                }
                else if ( info.ServiceId == "Write" )
                {
                    FailureCodes errcode = FailureCodes.Response_NoConformProtocol;
                    if ( !mProtocol.CheckReceiveData ( data, info.SampleGroup, ref errcode, false ) )
                    {
                        result.IsSucceed = false;
                        result.FailureCode = errcode;
                    }
                    SetServiceResult ( new ServiceResult ( ) { ServiceId = info.ServiceId, Result = result, SampleGroup = info.SampleGroup } );
                }
                else
                {
                    SetServiceResult ( new ServiceResult ( ) { ServiceId = info.ServiceId, Result = result, SampleGroup = info.SampleGroup } );
                }
                return result;
            }
            catch
            {
                result.IsSucceed = false;
                SetServiceResult ( new ServiceResult ( ) { ServiceId = info.ServiceId, Result = result.IsSucceed } );
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="data"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private Tuple<object, QualityDefinition, DateTime> ExplainReadServiceIO ( SampleGroup group, byte[] data, ref ExecutionResult result )
        {
            try
            {

                FailureCodes errcode = FailureCodes.Response_NoConformProtocol;
                if ( !mProtocol.CheckReceiveData ( data, group, ref errcode, true ) )
                {
                    result.IsSucceed = false;
                    result.Message = Resources.ErrCheck;
                    result.FailureCode = errcode;
                    return null;
                }

                ValueInfo[] values;
                if ( mProtocol.ExplainIO ( group, data, out values ) )
                {
                    if ( values != null && values.Count ( ) > 0 )
                        return new Tuple<object, QualityDefinition, DateTime> ( values[0].Value, QualityDefinition.Good_NoReason, DateTime.Now );
                }
                return null;
            }
            catch
            {
                result.IsSucceed = false;
                result.FailureCode = FailureCodes.Response_NoConformProtocol;
                return null;
            }
        }
        /// <summary>
        ///  批量读解析
        /// </summary>
        /// <param name="group"></param>
        /// <param name="data"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private ValueInfo[] ExplainReadsServiceIO ( SampleGroup group, byte[] data, ref ExecutionResult result )
        {
            try
            {
                FailureCodes errcode = FailureCodes.Response_NoConformProtocol;
                if ( !mProtocol.CheckReceiveData ( data, group, ref errcode, true ) )
                {
                    result.IsSucceed = false;
                    result.Message = Resources.ErrCheck;
                    result.FailureCode = errcode;
                    return null;
                }

                ValueInfo[] values;
                mProtocol.ExplainIO ( group, data, out values );
                return values;
            }
            catch
            {
                result.IsSucceed = false;
                result.FailureCode = FailureCodes.Response_NoConformProtocol;
                return null;
            }
        }

        #endregion
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
