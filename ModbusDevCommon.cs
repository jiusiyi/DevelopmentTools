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
    public class ModbusDevCommon
    {
        /// <summary>
        /// 获取寄存器类型
        /// </summary>
        /// <returns></returns>
        internal static IList<string> GetSupportRegionTypes ( )
        {
            return Enum.GetNames ( typeof ( RegTypeList ) );
        }
        /// <summary>
        /// 获取寄存器列的风格
        /// </summary>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos ( )
        {
            return new ColumnInfo[]
           {
                new ColumnInfo(){ColumnStyle = ColumnStyle.Combox, ColumnType = ColumnType.Memory},
                new ColumnInfo(){ColumnStyle = ColumnStyle.TextBox, ColumnType = ColumnType.Index},
                new ColumnInfo(){ColumnStyle = ColumnStyle.TextBox, ColumnType = ColumnType.Length}
           };
        }
        /// <summary>
        /// 校验寄存器 建立是否符合协议规范
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        internal static ErrorMessage[] CheckRegister ( Register register )
        {
            var errorList = new List<ErrorMessage> ( 4 );
            try
            {
                if ( int.Parse ( register.Index ) < 0 )
                {
                    errorList.Add ( new ErrorMessage ( )
                    {
                        Message = string.Format ( Resources.ValidateIndexError ),
                        Level = ErrorLevel.Error
                    } );
                }
                if ( register.DataType == DataTypes.String )
                {
                    if ( register.Length < 1 || register.Length > 127 )
                    {
                        errorList.Add ( new ErrorMessage ( )
                        {
                            Message = string.Format ( Resources.ValidateStringError ),
                            Level = ErrorLevel.Error
                        } );
                    }
                }
                else if ( register.DataType == DataTypes.Double )
                {
                    if ( register.Length != 2 && register.Length != 4 )
                    {
                        errorList.Add ( new ErrorMessage ( )
                        {
                            Message = string.Format ( Resources.DoubleLengthError ),
                            Level = ErrorLevel.Error
                        } );
                    }
                }
                else
                {
                    if ( register.Length != 1 && register.Length != 2 && register.Length != 4 )
                    {
                        errorList.Add ( new ErrorMessage ( )
                        {
                            Message = string.Format ( Resources.ValidateLengthError ),
                            Level = ErrorLevel.Error
                        } );
                    }
                }
            }
            catch ( Exception ex )
            {
                errorList.Add ( new ErrorMessage ( )
                {
                    Message = string.Format ( "{0},{1}", register.Memory,Resources.RegisterError),
                    Level = ErrorLevel.Error
                } );
            }
            return errorList.ToArray ( );
        }
    }
}
