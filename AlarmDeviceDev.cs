using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.AI.IO;
using ControlEase.IoDrive.Hikvision.Properties;
using System.Net;

namespace ControlEase.IoDrive.Hikvision
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmDeviceDev : DeviceDev
    {
        #region ... Variables  ...
        /// <summary>
        /// 设备的配置信息
        /// </summary>
        private AlarmConfigData mConfig = new AlarmConfigData ( );
        /// <summary>
        /// 
        /// </summary>
        private string[] mRegList = null;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 构造
        /// </summary>
        public AlarmDeviceDev ( )
        {

        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 使用底层配置控件
        /// </summary>
        public override bool UseCommCfgDataCtrl
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 属性控件
        /// </summary>
        public override Type CofigCtrlType
        {
            get { return typeof ( AlarmView ); }
        }
        /// <summary>
        /// 设备属性
        /// </summary>
        public override object ConfigData
        {
            get
            {
                return mConfig;
            }
            set
            {
                if ( mConfig != value )
                {
                    mConfig = value as AlarmConfigData;
                }
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 获取支持的寄存器列表
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public override IList<string> GetSupportedDatas ( ColumnType column )
        {
            if ( column == ColumnType.Memory )
                return GetSupportRegionTypes ( );
            return base.GetSupportedDatas ( column );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IList<string> GetSupportRegionTypes ( )
        {
            return GetRegTypeList ( ).ToList ( );
        }
        /// <summary>
        /// 获取本设备支持的寄存器列表
        /// </summary>
        /// <returns>列表</returns>
        public string[] GetRegTypeList ( )
        {
            return Enum.GetNames ( typeof ( RegTypeList ) );
        }
        /// <summary>
        /// 获取支持的读写模式
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        public override ReadWriteMode GetSupportRWMode ( string memory )
        {
            return ReadWriteMode.ReadOnly;
        }
        /// <summary>
        /// 获取列的风格
        /// </summary>
        /// <returns></returns>
        protected override ColumnInfo[] GetColumnInfos ( )
        {
            return ColumnInfo;
        }
        /// <summary>
        /// 寄存器支持的列风格
        /// </summary>
        private static readonly ColumnInfo[] ColumnInfo = new[]
            {
                new ColumnInfo(){ColumnStyle = ColumnStyle.Combox, ColumnType = ColumnType.Memory},
                new ColumnInfo(){ColumnStyle = ColumnStyle.TextBox, ColumnType = ColumnType.DeviceNo},//设备地址
                new ColumnInfo(){ColumnStyle = ColumnStyle.TextBox, ColumnType = ColumnType.Index},//索引 用于输入报警
            };
        /// <summary>
        /// 建立寄存器
        /// </summary>
        /// <param name="lastRegister">上一个寄存器</param>
        /// <returns>返回寄存器</returns>
        public override Register CreateNewRegister ( Register lastRegister )
        {
            Register register = new Register ( );
            if ( lastRegister == null )
            {
                register.Memory = RegTypeList.AlarmInput.ToString ( );
                register.RWMode = GetSupportRWMode ( register.Memory );
                register.Index = "0";
                register.CanonicalDataType = typeof ( bool );
                register.DeviceNo = "192.168.0.77";
            }
            else
            {
                if ( lastRegister.Memory == RegTypeList.AlarmInput.ToString ( ) )
                {
                    register.Memory = RegTypeList.AlarmInput.ToString ( );
                    register.RWMode = GetSupportRWMode ( register.Memory );
                    register.Index = ( int.Parse ( lastRegister.Index ) + 1 ).ToString ( );
                    register.DeviceNo = lastRegister.DeviceNo;
                }
                else
                {
                    var index = ( int ) Enum.Parse ( typeof ( RegTypeList ), lastRegister.Memory );
                    if ( mRegList == null )
                    {
                        mRegList = GetRegTypeList ( );
                    }
                    register.Memory = ( index == mRegList.Length - 1 ) ? ( lastRegister.Memory ) : ( mRegList[index + 1] );
                    register.Index = lastRegister.Index;
                    register.DeviceNo = lastRegister.DeviceNo;
                    register.RWMode = GetSupportRWMode ( register.Memory );
                }
            }
            return register;
        }
        /// <summary>
        /// 校验数据
        /// </summary>
        /// <param name="register">要校验的寄存器</param>
        /// <returns>错误列表</returns>
        protected override ErrorMessage[] CheckRegister ( Register register )
        {
            var errorList = new List<ErrorMessage> ( 3 );
            try
            {
                int index = 0;
                if ( !int.TryParse ( register.Index, out index ) )
                {
                    errorList.Add ( new ErrorMessage ( )
                    {
                        Message = string.Format ( "{0}:{1}", register.Memory, Resources.IndexError ),
                        Level = ErrorLevel.Error
                    } );
                }
                //据了解有更多路报警，这里不限制
                if ( index < 0 )
                {
                    errorList.Add ( new ErrorMessage ( )
                    {
                        Message = string.Format ( "{0}:{1}:{2}", register.Memory, Resources.IndexError, ">=0" ),
                        Level = ErrorLevel.Error
                    } );
                }
                IPAddress address = null;
                if ( !IPAddress.TryParse ( register.DeviceNo, out address ) )
                {
                    errorList.Add ( new ErrorMessage ( )
                    {
                        Message = string.Format ( "{0}:{1}", register.Memory, Resources.DeviceNoError ),
                        Level = ErrorLevel.Error
                    } );
                }

            }
            catch
            {
                errorList.Add ( new ErrorMessage ( )
                {
                    Message = string.Format ( "{0}:{1}", register.Memory, Resources.IndexError ),
                    Level = ErrorLevel.Error
                } );
            }
            return errorList.ToArray ( );
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
