using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlEase.IoDrive.Hikvision
{
    /// <summary>
    /// 
    /// </summary>
    internal enum RegTypeList
    {
        /// <summary>
        /// 报警输入
        /// </summary>
        AlarmInput,
        #region ...刷卡人员信息...

        /// <summary>
        /// 事件类型；1：报警；2：异常；3：操作；5：事件
        /// 对应门禁主机报警信息结构体中报警主类型
        /// </summary>
        EventType,
        /// <summary>
        /// 事件次类型：
        /// 详见：门禁主机报警信息结构体中报警次类型
        /// </summary>
        EventSubType,
        /// <summary>
        /// 门编号
        /// </summary>
        DoorNo,
        /// <summary>
        /// 读卡器编号
        /// </summary>
        CardReaderNo,
        /// <summary>
        /// 卡号
        /// </summary>
        Card_No,
        /// <summary>
        /// 门禁主机报警时间
        /// </summary>
        AccessControlTime,
        /// <summary>
        /// 门禁主机报警全数据
        /// </summary>
        AccessControlFallInfo,
        /// <summary>
        /// 门禁主机报警到达
        /// </summary>
        AccessControlArrive,

        #endregion
    }
}
