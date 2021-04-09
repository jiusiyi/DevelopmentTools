using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.AI.IO;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ControlEase.IoDrive.Hikvision
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmDevice : NonTransparentDevice
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public AlarmConfigData mConfig = new AlarmConfigData ( );
        /// <summary>
        /// 
        /// </summary>
        public ConnectionOption mConnectionOption = new ConnectionOption ( );
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Register> mAlarmRegistersDic;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> mAccessControlArriveList;
        /// <summary>
        /// 不包含AlarmInput，原因是因为已有对应字典存
        /// </summary>
        private List<Register> mRegistersList;
        /// <summary>
        /// 登录字典
        /// </summary>
        private Dictionary<string, int[]> mLoginDic;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Register, DateTime> mAlarmTimeDic;
        /// <summary>
        /// 回调函数
        /// </summary>
        private readonly HCNetSDK.MSGCallBack msgCallBack;
        /// <summary>
        /// 报警复位线程
        /// </summary>
        private Thread mAlarmResetThread;
        /// <summary>
        /// 
        /// </summary>
        private object mLockAlarm = new object ( );
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public AlarmDevice ( )
        {
            msgCallBack = MsgCallBackInstance;
            mAlarmRegistersDic = new Dictionary<string, Register> ( );
            mLoginDic = new Dictionary<string, int[]> ( );
            mAccessControlArriveList = new Dictionary<string, int> ( );
            mAlarmTimeDic = new Dictionary<Register, DateTime> ( );
            mRegistersList = new List<Register> ( );
        }
        static AlarmDevice ( )
        {
            var temp = AppDomain.CurrentDomain.BaseDirectory;
            var temp1 = Path.Combine ( temp, "Drivers\\Device\\HikVision" );
            var probe = Path.Combine ( temp1, Environment.Is64BitProcess ? "x64" : "x86" );
            var ptr = SetDllDirectory ( probe );
        }
        /// <summary>
        /// 加载dll文件
        /// </summary>
        /// <param name="lpFileName">dll文件路径</param>
        /// <returns></returns>
        [DllImport ( "kernel32.dll", SetLastError = true )]
        public static extern bool SetDllDirectory ( string lpFileName );

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override CommunicationType CommType
        {
            get { return CommunicationType.Poll; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override object ConfigData
        {
            get { return mConfig; }
            set
            {
                if ( value == null || !( value is AlarmConfigData ) )
                {
                    return;
                }
                mConfig = value as AlarmConfigData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override ConnectionOption ConnectOption
        {
            get
            {
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
            try
            {
                InitRegister ( );
            }
            catch
            { }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitRegister ( )
        {
            foreach ( Register register in Registers.Values )
            {
                if ( register is ComplexRegister )
                    InitComplexRegister ( register );
                else
                    InitSimpleRegister ( register );
            }
        }
        /// <summary>
        /// 复杂类型
        /// </summary>
        /// <param name="register"></param>
        private void InitComplexRegister ( Register register )
        {
            ComplexRegister complexRegister = register as ComplexRegister;
            foreach ( Register iRegister in complexRegister.MemberRegsiters.Values )
            {
                if ( iRegister is ComplexRegister )
                    InitComplexRegister ( iRegister );
                else
                    InitSimpleRegister ( iRegister );
            }
        }
        /// <summary>
        /// 简单类型
        /// </summary>
        /// <param name="register"></param>
        private void InitSimpleRegister ( Register register )
        {
            if ( register.Memory == RegTypeList.AlarmInput.ToString ( ) )
            {
                string key = string.Format ( "{0}_{1}", register.DeviceNo, register.Index );
                if ( mAlarmRegistersDic.ContainsKey ( key ) )
                    mAlarmRegistersDic[key] = register;
                else
                    mAlarmRegistersDic.Add ( key, register );
            }
            else if ( register.Memory == RegTypeList.AccessControlArrive.ToString ( ) )
            {
                mRegistersList.Add ( register );
                if ( !mAccessControlArriveList.ContainsKey ( register.DeviceNo ) )
                    mAccessControlArriveList.Add ( register.DeviceNo, 1 );

            }
            else
            {
                mRegistersList.Add ( register );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void UnInitialize ( )
        {
            try
            {
                mAlarmRegistersDic.Clear ( );
                mAccessControlArriveList.Clear ( );
                mRegistersList.Clear ( );
            }
            catch
            { }
        }
        #endregion
        #region ...打包...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        protected override SampleGroup CreateSampleGroup ( Register register )
        {
            return base.CreateSampleGroup ( register );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        protected override bool CanAddToReadSampleGroup ( SampleGroup group, Register register )
        {
            return true;
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
        #endregion
        #region ...连接...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ExecutionResult Connect ( )
        {
            ExecutionResult result = new ExecutionResult ( );

            try
            {
                if ( HCNetSDK.NET_DVR_Init ( ) )
                {
                    var userdata = new IntPtr ( );
                    HCNetSDK.NET_DVR_SetConnectTime ( 5000, 1 );

                    //HCNetSDK.NET_DVR_SetDVRMessageCallBack_V30(msgCallBack, userdata);
                    HCNetSDK.NET_DVR_SetDVRMessageCallBack_V50 ( 0, msgCallBack, userdata );
                    if ( mAlarmRegistersDic.Count ( ) > 0 )
                    {
                        mAlarmResetThread = new Thread ( AlarmResetThreadProc );
                        mAlarmResetThread.Name = "AlarmResetThread";
                        mAlarmResetThread.IsBackground = true;
                        mAlarmResetThread.Start ( );
                    }
                }
                else
                    result.IsSucceed = false;
                return result;
            }
            catch
            {
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ExecutionResult Disconnect ( )
        {
            ExecutionResult result = new ExecutionResult ( );
            try
            {
                //没有撤防的进行撤防，没有登出额进行登出
                foreach ( var item in mLoginDic )
                {
                    int[] values = item.Value;
                    if ( values[1] != -1 )
                    {
                        HCNetSDK.NET_DVR_CloseAlarmChan_V30 ( values[1] );
                    }
                    if ( values[0] >= 0 )
                    {
                        HCNetSDK.NET_DVR_Logout_V30 ( values[0] );
                    }
                }
                HCNetSDK.NET_DVR_Cleanup ( );
                mAlarmTimeDic.Clear ( );
                mLoginDic.Clear ( );
                if ( mAlarmResetThread != null )
                {
                    mAlarmResetThread.Abort ( );
                    mAlarmResetThread = null;
                }

                return result;
            }
            catch ( Exception e )
            {
                return result;
            }
        }
        #endregion
        #region ...读写...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        protected override TaskInfo BuildReadTaskInfo ( SampleGroup group )
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        protected override TaskInfo BuildWriteTaskInfo ( SampleGroup group )
        {
            return null;
        }
        #endregion
        #region ...报警复位线程...
        /// <summary>
        /// 开始线程
        /// </summary>
        public void AlarmResetThreadProc ( )
        {
            do
            {
                try
                {
                    lock ( mLockAlarm )
                    {
                        foreach ( var item in mAlarmTimeDic )
                        {
                            Register register = item.Key;
                            TimeSpan uSpan = DateTime.Now - item.Value;
                            if ( uSpan.TotalMilliseconds >= mConfig.AlarmResetTime * 1000 )
                                SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( 0 ) ) );

                        }
                    }
                    Thread.Sleep ( 100 );

                }
                catch ( Exception ex )
                {
                    Thread.Sleep ( 100 );
                }
            }
            while ( true );
        }
        #endregion
        #region ...报警回调...
        /// <summary>
        /// 报警处理事件方法
        /// </summary>
        /// <param name="lCommand"></param>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        private void MsgCallBackInstance ( int lCommand, ref HCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser )
        {
            try
            {
                //参数lCommand代表报警类型，pAlarmer存放报警设备信息，pAlarmInfo存放具体的报警信息，根据报警类型的变化而变化
                switch ( lCommand )  //根据报警类型不同，分别处理不同的报警信息
                {
                    case HCNetSDK.COMM_ALARM:
                    case HCNetSDK.COMM_ALARM_V30:
                        {
                            HCNetSDK.NET_DVR_ALARMINFO_V30 mc = new HCNetSDK.NET_DVR_ALARMINFO_V30 ( );
                            mc = ( HCNetSDK.NET_DVR_ALARMINFO_V30 ) System.Runtime.InteropServices.Marshal.PtrToStructure ( pAlarmInfo, typeof ( HCNetSDK.NET_DVR_ALARMINFO_V30 ) );
                            if ( mc.dwAlarmType != 0 )
                                return;
                            string mAlarmDevicIP = pAlarmer.sDeviceIP;
                            string key = string.Format ( "{0}_{1}", pAlarmer.sDeviceIP, mc.dwAlarmInputNumber );
                            //Logging.Info ( string.Format ( "报警输入：{0}", key ) );
                            if ( mAlarmRegistersDic.ContainsKey ( key ) )
                            {
                                Register register = mAlarmRegistersDic[key];
                                SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( 1 ) ) );

                                lock ( mLockAlarm )
                                {
                                    if ( mAlarmTimeDic.ContainsKey ( register ) )
                                        mAlarmTimeDic[register] = DateTime.Now;
                                    else
                                        mAlarmTimeDic.Add ( register, DateTime.Now );
                                }
                            }
                        } break;
                    case HCNetSDK.COMM_ALARM_ACS://门禁主机报警上传
                        ProcessCommAlarm_AcsAlarm ( ref pAlarmer, pAlarmInfo, dwBufLen, pUser );
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }
        /// <summary>
        /// //门禁主机报警上传
        /// </summary>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        private void ProcessCommAlarm_AcsAlarm ( ref HCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser )
        {
            HCNetSDK.NET_DVR_ACS_ALARM_INFO struAcsAlarm = new HCNetSDK.NET_DVR_ACS_ALARM_INFO ( );
            uint dwSize = ( uint ) System.Runtime.InteropServices.Marshal.SizeOf ( struAcsAlarm );
            struAcsAlarm = ( HCNetSDK.NET_DVR_ACS_ALARM_INFO ) System.Runtime.InteropServices.Marshal.PtrToStructure ( pAlarmInfo, typeof ( HCNetSDK.NET_DVR_ACS_ALARM_INFO ) );

            //上海仙泉客户所有信息都需要
            PraseIotag ( pAlarmer, struAcsAlarm );

            //if ( struAcsAlarm.dwMajor != 5 )//客户只要刷卡信息
            //    return;

            //if ( ( struAcsAlarm.dwMinor >= 1 && struAcsAlarm.dwMinor <= 0x0a ) ||
            //    ( struAcsAlarm.dwMinor >= 0x26 && struAcsAlarm.dwMinor <= 0x31 ) ||
            //     ( struAcsAlarm.dwMinor >= 0x36 && struAcsAlarm.dwMinor <= 0x50 ) ||
            //     ( struAcsAlarm.dwMinor >= 0x65 && struAcsAlarm.dwMinor <= 0x67 ) )
            //{
            //    PraseIotag ( pAlarmer, struAcsAlarm );
            //}



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAlarmer"></param>
        /// <param name="struAcsAlarm"></param>
        private void PraseIotag ( HCNetSDK.NET_DVR_ALARMER pAlarmer, HCNetSDK.NET_DVR_ACS_ALARM_INFO struAcsAlarm )
        {
            //报警设备IP地址
            string strIP = pAlarmer.sDeviceIP;
            string strTimeYear = ( struAcsAlarm.struTime.dwYear ).ToString ( );
            string strTimeMonth = ( struAcsAlarm.struTime.dwMonth ).ToString ( "d2" );
            string strTimeDay = ( struAcsAlarm.struTime.dwDay ).ToString ( "d2" );
            string strTimeHour = ( struAcsAlarm.struTime.dwHour ).ToString ( "d2" );
            string strTimeMinute = ( struAcsAlarm.struTime.dwMinute ).ToString ( "d2" );
            string strTimeSecond = ( struAcsAlarm.struTime.dwSecond ).ToString ( "d2" );
            string strTime = strTimeYear + "-" + strTimeMonth + "-" + strTimeDay + " " + strTimeHour + ":" + strTimeMinute + ":" + strTimeSecond;
            string temp = System.Text.Encoding.UTF8.GetString ( struAcsAlarm.struAcsEventInfo.byCardNo ).TrimEnd ( '\0' );
            string stringAlarm = string.Format ( string.Format ( "{0};{1};{2};{3};{4};{5}", struAcsAlarm.dwMajor, struAcsAlarm.dwMinor, struAcsAlarm.struAcsEventInfo.dwDoorNo, struAcsAlarm.struAcsEventInfo.dwCardReaderNo, temp, strTime ) );

            //string stringAlarm = "门禁主机报警信息：dwMajor：" + struAcsAlarm.dwMajor + "，dwMinor：" + struAcsAlarm.dwMinor + "，卡号："
            //     + System.Text.Encoding.UTF8.GetString ( struAcsAlarm.struAcsEventInfo.byCardNo ).TrimEnd ( '\0' ) + "，读卡器编号：" +
            //     struAcsAlarm.struAcsEventInfo.dwCardReaderNo + "，门编号：" + struAcsAlarm.struAcsEventInfo.dwDoorNo + "，报警触发时间：" + strTime;

            foreach ( var register in mRegistersList )
            {
                if ( register.DeviceNo != strIP )
                    continue;

                if ( register.Memory == RegTypeList.EventType.ToString ( ) )
                    SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( struAcsAlarm.dwMajor ) ) );

                else if ( register.Memory == RegTypeList.EventSubType.ToString ( ) )
                    SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( struAcsAlarm.dwMinor ) ) );

                else if ( register.Memory == RegTypeList.DoorNo.ToString ( ) )
                    SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( struAcsAlarm.struAcsEventInfo.dwDoorNo ) ) );

                else if ( register.Memory == RegTypeList.CardReaderNo.ToString ( ) )
                    SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( struAcsAlarm.struAcsEventInfo.dwCardReaderNo ) ) );

                else if ( register.Memory == RegTypeList.Card_No.ToString ( ) )
                {
                    SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( temp ) ) );
                }
                else if ( register.Memory == RegTypeList.AccessControlTime.ToString ( ) )
                { //报警时间：年月日时分秒
                    SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( strTime ) ) );
                }
                else if ( register.Memory == RegTypeList.AccessControlFallInfo.ToString ( ) )
                {
                    SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( stringAlarm ) ) );
                }

            }
            foreach ( var register in mRegistersList )
            {
                if ( register.Memory == RegTypeList.AccessControlArrive.ToString ( ) )
                {
                    if ( register.DeviceNo == strIP )
                    {
                        SetRegistersValue ( new ValueInfo ( register, register.ConvertToUserTypeValue ( mAccessControlArriveList[register.DeviceNo] ) ) );
                        mAccessControlArriveList[register.DeviceNo] += 1;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="dwResult"></param>
        /// <param name="lpDeviceInfo"></param>
        /// <param name="pUser"></param>
        public void AsynLoginMsgCallback ( int lUserID, int dwResult, IntPtr lpDeviceInfo, IntPtr pUser )
        {

        }

        #endregion
        #region ...服务器方法...
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="IPAddress">ip地址</param>
        /// <param name="UserName">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="PortNum">端口号</param>
        /// <returns>是否成功</returns>
        [ServiceMethod]
        public bool Login ( string IPAddress, string UserName, string PassWord, ushort PortNum )
        {
            return Internal_Login ( IPAddress, UserName, PassWord, PortNum );
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="IPAddress">IP地址</param>
        /// <returns>是否成功</returns>
        [ServiceMethod]
        public bool LogOut ( string IPAddress )
        {
            return Internal_LogOut ( IPAddress );
        }
        /// <summary>
        /// 设置布防 
        /// </summary>
        /// <param name="IPAddress">IP地址</param>
        /// <returns>是否成功</returns>
        [ServiceMethod]
        public bool SetDeviceProtection ( string IPAddress )
        {
            return Internal_SetDeviceProtection ( IPAddress );
        }
        /// <summary>
        /// 取消布防
        /// </summary>
        /// <param name="IPAddress">IP地址</param>
        /// <returns>是否成功</returns>
        [ServiceMethod]
        public bool CloseDeviceProtection ( string IPAddress )
        {
            return Internal_CloseDeviceProtection ( IPAddress );
        }
        /// <summary>
        /// 远程操作门禁
        /// </summary>
        /// <param name="IPAddress">IP地址</param>
        /// <param name="DoorIndex">门禁序号(从1开始，-1代表所有门)</param>
        /// <param name="dwStaic">操作(0- 关闭（对于梯控，表示受控），
        /// 1- 打开（对于梯控，表示开门），2- 常开（对于梯控，表示自由、通道状态），
        /// 3- 常关（对于梯控，表示禁用），4- 恢复（梯控，普通状态），
        /// 5- 访客呼梯（梯控），6- 住户呼梯（梯控）)</param>
        /// <returns>是否成功</returns>
        [ServiceMethod]
        public bool RemoteAccess ( string IPAddress, int DoorIndex, int dwStaic )
        {
            return Internal_RemoteAccess ( IPAddress, DoorIndex, dwStaic );
        }


        #endregion
        #region ...内部方法...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <param name="PortNum"></param>
        /// <returns></returns>
        private bool Internal_Login ( string IPAddress, string UserName, string PassWord, ushort PortNum )
        {
            try
            {
                if ( mLoginDic.ContainsKey ( IPAddress ) && mLoginDic[IPAddress][0] >= 0 )
                {
                    //Logging.Info(string.Format ( "HikvisionAlarm:网络摄像机{0}:{1}已登录，", IPAddress, PortNum ) )
                    return true;
                }
                else
                {
                    HCNetSDK.NET_DVR_DEVICEINFO_V30 m_struDeviceInfo = new HCNetSDK.NET_DVR_DEVICEINFO_V30 ( );

                    HCNetSDK.NET_DVR_DEVICEINFO_V30 struDeviceInfo = new HCNetSDK.NET_DVR_DEVICEINFO_V30 ( );
                    struDeviceInfo.sSerialNumber = new byte[HCNetSDK.SERIALNO_LEN];

                    HCNetSDK.NET_DVR_NETCFG_V50 struNetCfg = new HCNetSDK.NET_DVR_NETCFG_V50 ( );
                    struNetCfg.Init ( );
                    HCNetSDK.NET_DVR_DEVICECFG_V40 struDevCfg = new HCNetSDK.NET_DVR_DEVICECFG_V40 ( );
                    struDevCfg.sDVRName = new byte[HCNetSDK.NAME_LEN];
                    struDevCfg.sSerialNumber = new byte[HCNetSDK.SERIALNO_LEN];
                    struDevCfg.byDevTypeName = new byte[HCNetSDK.DEV_TYPE_NAME_LEN];
                    HCNetSDK.NET_DVR_USER_LOGIN_INFO struLoginInfo = new HCNetSDK.NET_DVR_USER_LOGIN_INFO ( );
                    HCNetSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new HCNetSDK.NET_DVR_DEVICEINFO_V40 ( );
                    struDeviceInfoV40.struDeviceV30.sSerialNumber = new byte[HCNetSDK.SERIALNO_LEN];
                    uint dwReturned = 0;
                    int lUserID = -1;
                    struLoginInfo.bUseAsynLogin = false;
                    struLoginInfo.cbLoginResult = new HCNetSDK.LoginResultCallBack ( AsynLoginMsgCallback );
                    struLoginInfo.byLoginMode = 2;

                    struLoginInfo.sDeviceAddress = IPAddress;
                    struLoginInfo.sUserName = UserName;
                    struLoginInfo.sPassword = PassWord;
                    struLoginInfo.wPort = PortNum;

                    int UserID = HCNetSDK.NET_DVR_Login_V40 ( ref struLoginInfo, ref struDeviceInfoV40 );
                    if ( UserID == -1 )
                    {
                        Logging.Error ( string.Format ( "HikvisionAlarm:登录网络摄像机{0}:{1}失败！", IPAddress, PortNum ) );
                        return false;
                    }
                    else
                    {
                        int[] values = new int[2] { UserID, -1 };
                        if ( mLoginDic.ContainsKey ( IPAddress ) )
                        {
                            mLoginDic[IPAddress][0] = UserID;
                        }
                        else
                        {
                            mLoginDic.Add ( IPAddress, values );
                        }
                        Logging.Info ( string.Format ( "HikvisionAlarm:登录网络摄像机{0}:{1}成功！", IPAddress, PortNum ) );
                        return true;
                    }
                }
            }
            catch ( Exception ex )
            {
                Logging.Error ( string.Format ( "HikvisionAlarm:登录网络摄像机:{0}异常{1}！", IPAddress, ex.Message ) );
                return false;
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <returns></returns>
        private bool Internal_LogOut ( string IPAddress )
        {
            try
            {
                if ( mLoginDic.ContainsKey ( IPAddress ) && mLoginDic[IPAddress][0] >= 0 )
                {
                    if ( mLoginDic[IPAddress][1] != -1 )
                    {
                        CloseDeviceProtection ( IPAddress );
                        mLoginDic[IPAddress][1] = -1;
                    }
                    if ( HCNetSDK.NET_DVR_Logout_V30 ( mLoginDic[IPAddress][0] ) )
                    {
                        //Logging.Info( string.Format ( "HikvisionAlarm:退出网络摄像机！", IPAddress ) );
                    }
                    mLoginDic[IPAddress][0] = -1;
                }
                else
                {
                    Logging.Error ( string.Format ( "HikvisionAlarm:设备还没有登录:{0}！", IPAddress ) );
                }
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Error ( string.Format ( "HikvisionAlarm:登出网络摄像机:{0}异常{1}！", IPAddress, ex.Message ) );
                return false;
            }
        }

        /// <summary>
        /// 布防
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private bool Internal_SetDeviceProtection ( string ipAddress )
        {
            try
            {
                if ( mLoginDic.ContainsKey ( ipAddress ) && mLoginDic[ipAddress][0] >= 0 )
                {
                    if ( mLoginDic[ipAddress][1] != -1 )
                    {
                        Logging.Info ( string.Format ( "HikvisionAlarm:设备{0}已设防！", ipAddress ) );
                        return true;
                    }
                    else
                    {
                        //int AlarmHandle = HCNetSDK.NET_DVR_SetupAlarmChan_V30(LoginDic[ipAddress][0]);
                        HCNetSDK.NET_DVR_SETUPALARM_PARAM struSetupAlarmParam = new HCNetSDK.NET_DVR_SETUPALARM_PARAM ( );
                        struSetupAlarmParam.dwSize = ( uint ) System.Runtime.InteropServices.Marshal.SizeOf ( struSetupAlarmParam );
                        struSetupAlarmParam.byLevel = 1;
                        struSetupAlarmParam.byAlarmInfoType = 1;
                        int AlarmHandle = HCNetSDK.NET_DVR_SetupAlarmChan_V41 ( mLoginDic[ipAddress][0], ref struSetupAlarmParam );
                        if ( AlarmHandle != -1 )
                        {
                            // Logging.Info( string.Format ( "HikvisionAlarm:设备{0}布防成功！", ipAddress ) );
                            mLoginDic[ipAddress][1] = AlarmHandle;
                            return true;
                        }
                        else
                        {
                            Logging.Error ( string.Format ( "HikvisionAlarm:设备{0}布防失败！", ipAddress ) );
                            return false;
                        }
                    }
                }
                else
                {
                    Logging.Error ( string.Format ( "HikvisionAlarm:设备还没有登录:{0}！", ipAddress ) );
                    return false;
                }
            }
            catch ( Exception ex )
            {
                Logging.Error ( string.Format ( "HikvisionAlarm:布防网络摄像机:{0}异常{1}！", ipAddress, ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 撤防
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private bool Internal_CloseDeviceProtection ( string ipAddress )
        {
            try
            {
                if ( mLoginDic.ContainsKey ( ipAddress ) && mLoginDic[ipAddress][0] >= 0 )
                {
                    if ( mLoginDic[ipAddress][1] != -1 )
                    {
                        bool result = HCNetSDK.NET_DVR_CloseAlarmChan_V30 ( mLoginDic[ipAddress][1] );
                        if ( result )
                        {
                            //Logging.Info ( string.Format ( "HikvisionAlarm:设备{0}撤防成功！", ipAddress ) );
                            mLoginDic[ipAddress][1] = -1;
                            return true;
                        }
                        else
                        {
                            Logging.Error ( string.Format ( "HikvisionAlarm:设备{0}撤防失败！", ipAddress ) );
                            return false;
                        }
                    }
                    else
                    {
                        Logging.Error ( string.Format ( "HikvisionAlarm:设备{0}并没有布防！", ipAddress ) );
                        return true;
                    }
                }
                else
                {
                    Logging.Error ( string.Format ( "HikvisionAlarm:设备还没有登录:{0}！", ipAddress ) );
                    return false;
                }
            }
            catch ( Exception ex )
            {
                Logging.Error ( string.Format ( "HikvisionAlarm:撤防网络摄像机：{0}异常{1}！", ipAddress, ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="doorIndex"></param>
        /// <param name="dwStaic"></param>
        /// <returns></returns>
        private bool Internal_RemoteAccess ( string ipAddress, int doorIndex, int dwStaic )
        {
            try
            {
                if ( mLoginDic.ContainsKey ( ipAddress ) && mLoginDic[ipAddress][0] >= 0 )
                {
                    if ( dwStaic < 0 || dwStaic > 6 )
                    {
                        Logging.Error ( string.Format ( "HikvisionAlarm:设备{0}远程操作命令值范围应该是0--6！", ipAddress ) );
                        return false;
                    }
                    bool result = HCNetSDK.NET_DVR_ControlGateway ( mLoginDic[ipAddress][0], doorIndex, ( uint ) dwStaic );
                    return result;
                }
                else
                {
                    Logging.Error ( string.Format ( "HikvisionAlarm:设备还没有登录:{0}！", ipAddress ) );
                    return false;
                }
            }
            catch ( Exception ex )
            {
                Logging.Error ( string.Format ( "HikvisionAlarm:远程操作门{0}异常{1}！", ipAddress, ex.Message ) );
                return false;
            }
        }
        #endregion
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
