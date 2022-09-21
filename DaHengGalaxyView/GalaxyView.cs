using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using System.IO;
using ControlEase.Nexus.ComponentModel;
using ControlEase.Inspec.Extension;
using ControlEase.AI.Tag;
using System.Drawing.Design;
using ControlEase.Inspec.Animates;
using ControlEase.AI.View;
using System.Xml.Linq;
using ControlEase.Inspec.ViewCore;
using ControlEase.AI.Script;
using GxIAPINET;
using ControlEase.Nexus.Logging;

namespace ControlEase.Inspec.DaHeng
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GalaxyView : UserControl, ICustomSerializable, IHideProperty
    {
        #region ... Variables  ...
        #region ...配置属性...
        /// <summary>
        /// 保存图片
        /// </summary>
        private bool mSaveBMP = true;
        /// <summary>
        /// 
        /// </summary>
        private string mBMPName = "Image";
        /// <summary>
        /// 保存路径
        /// </summary>
        private string mSavePath = "C:\\InspecImageFile\\GalaxyViewImages";
        /// <summary>
        /// 采集模式
        /// </summary>
        private AcquisitionModes mAcquisitionMode = AcquisitionModes.Continuous;
        #endregion
        #region ...变量属性...
        /// <summary>
        /// 触发模式
        /// </summary>
        private PropertyLinkAnimate<string> mTriggerMode;
        /// <summary>
        /// 触发源
        /// </summary>
        private PropertyLinkAnimate<string> mTriggerSource;
        /// <summary>
        /// 触发极性
        /// </summary>
        private PropertyLinkAnimate<string> mTriggerActivation;
        /// <summary>
        /// 曝光时间
        /// </summary>
        private PropertyLinkAnimate<string> mExposureTime;
        /// <summary>
        /// 增益
        /// </summary>
        private PropertyLinkAnimate<string> mGain;
        /// <summary>
        /// 自动白平衡
        /// </summary>
        private PropertyLinkAnimate<string> mAutoWhite;
        /// <summary>
        /// 白平衡通道选择
        /// </summary>
        private PropertyLinkAnimate<string> mRatioSelector;
        /// <summary>
        /// 白平衡系数
        /// </summary>
        private PropertyLinkAnimate<string> mBalanceRatio;
        #endregion
        #region ...存储...
        /// <summary>
        /// 
        /// </summary>
        private List<string> mTriggerModeList = new List<string> ( );
        /// <summary>
        /// 
        /// </summary>
        private List<string> mTriggerSourceList = new List<string> ( );
        /// <summary>
        /// 
        /// </summary>
        private List<string> mTriggerActivationList = new List<string> ( );
        /// <summary>
        /// 
        /// </summary>
        private List<string> mAutoWhiteList = new List<string> ( );
        /// <summary>
        /// 
        /// </summary>
        private List<string> mRatioSelectorList = new List<string> ( );
        /// <summary>
        /// 曝光值最小值
        /// </summary>
        private double mExposureTimeMin = 0.0;
        /// <summary>
        /// 曝光值最大值
        /// </summary>
        private double mExposureTimeMax = 0.0;
        /// <summary>
        /// 增益值最小值
        /// </summary>
        private double mGainMin = 0.0;
        /// <summary>
        /// 增益值最大值
        /// </summary>
        private double mGainMax = 0.0;
        /// <summary>
        /// 白平衡值最小值
        /// </summary>
        private double mBalanceRatioMin = 0.0;
        /// <summary>
        /// 白平衡最大值
        /// </summary>
        private double mBalanceRatioMax = 0.0;
        #endregion
        #region ...控件...
        /// <summary>
        /// 
        /// </summary>
        private IGXFeatureControl mIGXFeatureControl = null;
        /// <summary>
        /// 
        /// </summary>
        private IGXFactory mIGXFactory = null;
        /// <summary>
        /// 
        /// </summary>
        private IGXDevice mIGXDevice = null;
        /// <summary>
        /// 
        /// </summary>
        private IGXStream mIGXStream = null;
        /// <summary>
        /// 
        /// </summary>
        private IGXFeatureControl mIGXStreamFeatureControl = null;
        /// <summary>
        /// 
        /// </summary>
        private GxBitmap mGxBitmap = null;
        #endregion
        #region ...程序应用..
        /// <summary>
        /// 是否开始采集
        /// </summary>
        private bool mIsSnap = false;
        /// <summary>
        /// 设备是否已经打开
        /// </summary>
        private bool mIsOpen = false;
        #endregion
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public GalaxyView ( )
        {
            InitializeComponent ( );
            if ( !string.IsNullOrEmpty ( mSavePath ) )
            {
                //查看是否存在此文件
                if ( !Directory.Exists ( mSavePath ) )
                {
                    Directory.CreateDirectory ( mSavePath );
                }
            }

        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose ( bool disposing )
        {
            try
            {
                CloseDevice_Internal ( );
            }
            catch
            { }
            if ( disposing && ( components != null ) )
            {
                components.Dispose ( );
            }
            base.Dispose ( disposing );
        }
        #endregion ...Constructor...

        #region ... Properties ...
        #region ...tag...
        /// <summary>
        /// 
        /// </summary>
        [Browsable ( false )]
        private ITagProvider TagProvider
        {
            get
            {
                return ServiceLocator.Current.Resolve<ITagProvider> ( );
            }

        }
        #endregion
        #region ...配置...
        /// <summary>
        /// 是否保存图片
        /// </summary>
        [DisplayName ( "SaveBMP" )]
        [Description ( "SaveBMP_Description" )]
        [Category ( "Configuration" )]
        public bool SaveBMP
        {
            get { return mSaveBMP; }
            set { mSaveBMP = value; }
        }
        /// <summary>
        /// 图片名称
        /// </summary>
        [DisplayName ( "BMPName" )]
        [Description ( "BMPName_Description" )]
        [Category ( "Configuration" )]
        public string BMPName
        {
            get { return mBMPName; }
            set
            {
                if ( string.IsNullOrEmpty ( value ) || string.IsNullOrWhiteSpace ( value ) )
                    return;
                mBMPName = value;
            }
        }
        /// <summary>
        /// 是否保存图片
        /// </summary>
        [DisplayName ( "SavePath" )]
        [Description ( "SavePath_Description" )]
        [Category ( "Configuration" )]
        public string SavePath
        {
            get { return mSavePath; }
            set
            {
                if ( string.IsNullOrEmpty ( value ) || string.IsNullOrWhiteSpace ( value ) )
                    return;
                if ( mSavePath != value )
                    mSavePath = value;
                //查看是否存在此文件
                if ( !Directory.Exists ( mSavePath ) )
                {
                    Directory.CreateDirectory ( mSavePath );
                }
            }
        }
        /// <summary>
        /// 采集模式
        /// </summary>
        [DisplayName ( "AcquisitionMode" )]
        [Description ( "AcquisitionMode_Description" )]
        [Category ( "Configuration" )]
        public AcquisitionModes AcquisitionMode
        {
            get { return mAcquisitionMode; }
            set { mAcquisitionMode = value; }
        }
        #endregion
        #region ...关联变量...
        /// <summary>
        /// 触发模式
        /// </summary>
        [DisplayName ( "TriggerMode" )]
        [Description ( "TriggerMode_Description" )]
        [Category ( "Attribute" )]
        //[DefaultValue ( null )]
        public PropertyLinkAnimate<string> TriggerMode
        {
            get { return mTriggerMode; }
            set
            {
                if ( mTriggerMode != value )
                {
                    if ( mTriggerMode != null )
                        mTriggerMode.ValueChanged -= TriggerMode_ValueChanged;
                    mTriggerMode = value;
                    mTriggerMode.PropertyName = "TriggerMode";
                    mTriggerMode.ValueChanged += TriggerMode_ValueChanged;
                }
            }
        }
        /// <summary>
        /// 触发源
        /// </summary>
        [DisplayName ( "TriggerSource" )]
        [Description ( "TriggerSource_Description" )]
        [Category ( "Attribute" )]
        //[DefaultValue ( null )]
        public PropertyLinkAnimate<string> TriggerSource
        {
            get { return mTriggerSource; }
            set
            {
                if ( mTriggerSource != value )
                {
                    if ( mTriggerSource != null )
                        mTriggerSource.ValueChanged -= TriggerSource_ValueChanged;
                    mTriggerSource = value;
                    mTriggerSource.PropertyName = "TriggerSource";
                    mTriggerSource.ValueChanged += TriggerSource_ValueChanged;
                }
            }
        }
        /// <summary>
        /// 触发极性
        /// </summary>
        [DisplayName ( "TriggerActivation" )]
        [Description ( "TriggerActivation_Description" )]
        [Category ( "Attribute" )]
        //[DefaultValue ( null )]
        public PropertyLinkAnimate<string> TriggerActivation
        {
            get { return mTriggerActivation; }
            set
            {
                if ( mTriggerActivation != value )
                {
                    if ( mTriggerActivation != null )
                        mTriggerActivation.ValueChanged -= TriggerActivation_ValueChanged;
                    mTriggerActivation = value;
                    mTriggerActivation.PropertyName = "TriggerActivation";
                    mTriggerActivation.ValueChanged += TriggerActivation_ValueChanged;
                }
            }
        }

        /// <summary>
        /// 曝光时间
        /// </summary>
        [DisplayName ( "ExposureTime" )]
        [Description ( "ExposureTime_Description" )]
        [Category ( "Attribute" )]
        //[DefaultValue ( null )]
        public PropertyLinkAnimate<string> ExposureTime
        {
            get { return mExposureTime; }
            set
            {
                if ( mExposureTime != value )
                {
                    if ( mExposureTime != null )
                        mExposureTime.ValueChanged -= ExposureTime_ValueChanged;
                    mExposureTime = value;
                    mExposureTime.PropertyName = "ExposureTime";
                    mExposureTime.ValueChanged += ExposureTime_ValueChanged;
                }
            }
        }

        /// <summary>
        /// 曝光增益
        /// </summary>
        [DisplayName ( "Gain" )]
        [Description ( "Gain_Description" )]
        [Category ( "Attribute" )]
        //[DefaultValue ( null )]
        public PropertyLinkAnimate<string> Gain
        {
            get { return mGain; }
            set
            {
                if ( mGain != value )
                {
                    if ( mGain != null )
                        mGain.ValueChanged -= Gain_ValueChanged;
                    mGain = value;
                    mGain.PropertyName = "Gain";
                    mGain.ValueChanged += Gain_ValueChanged;
                }
            }
        }
        /// <summary>
        /// 自动白平衡
        /// </summary>
        [DisplayName ( "AutoWhite" )]
        [Description ( "AutoWhite_Description" )]
        [Category ( "Attribute" )]
        //[DefaultValue ( null )]
        public PropertyLinkAnimate<string> AutoWhite
        {
            get { return mAutoWhite; }
            set
            {
                if ( mAutoWhite != value )
                {
                    if ( mAutoWhite != null )
                        mAutoWhite.ValueChanged -= AutoWhite_ValueChanged;
                    mAutoWhite = value;
                    mAutoWhite.PropertyName = "AutoWhite";
                    mAutoWhite.ValueChanged += AutoWhite_ValueChanged;
                }
            }
        }
        /// <summary>
        /// 白平衡通道选择
        /// </summary>
        [DisplayName ( "RatioSelector" )]
        [Description ( "RatioSelector_Description" )]
        [Category ( "Attribute" )]
        //[DefaultValue ( null )]
        public PropertyLinkAnimate<string> RatioSelector
        {
            get { return mRatioSelector; }
            set
            {
                if ( mRatioSelector != value )
                {
                    if ( mRatioSelector != null )
                        mRatioSelector.ValueChanged -= RatioSelector_ValueChanged;
                    mRatioSelector = value;
                    mRatioSelector.PropertyName = "RatioSelector";
                    mRatioSelector.ValueChanged += RatioSelector_ValueChanged;
                }
            }
        }
        /// <summary>
        /// 白平衡系数
        /// </summary>
        [DisplayName ( "BalanceRatio" )]
        [Description ( "BalanceRatio_Description" )]
        [Category ( "Attribute" )]
        //[DefaultValue ( null )]
        public PropertyLinkAnimate<string> BalanceRatio
        {
            get { return mBalanceRatio; }
            set
            {
                if ( mBalanceRatio != value )
                {
                    if ( mBalanceRatio != null )
                        mBalanceRatio.ValueChanged -= BalanceRatio_ValueChanged;
                    mBalanceRatio = value;
                    mBalanceRatio.PropertyName = "BalanceRatio";
                    mBalanceRatio.ValueChanged += BalanceRatio_ValueChanged;
                }
            }
        }
        #endregion
        #endregion ...Properties...

        #region ... Methods    ...
        #region ...变量改变事件...
        /// <summary>
        /// 触发模式值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        void TriggerMode_ValueChanged ( object sender, IConvertible value )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return;
                string strValue = ( string ) TagProvider.GetTag ( TriggerMode.Expression ).Value.ToString ( );
                if ( !mTriggerModeList.Contains ( strValue ) )
                    return;
                //设置
                SetEnumValue ( "TriggerMode", strValue, mIGXFeatureControl );
                GetEnumCurrentValue ( "TriggerMode", TriggerMode, false );
            }
            catch
            {
                //把变量设置成正确值
                GetEnumCurrentValue ( "TriggerMode", TriggerMode, false );
            }
        }
        /// <summary>
        /// 触发源值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        void TriggerSource_ValueChanged ( object sender, IConvertible value )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return;
                string strValue = ( string ) TagProvider.GetTag ( TriggerSource.Expression ).Value.ToString ( );
                if ( !mTriggerSourceList.Contains ( strValue ) )
                    return;
                SetEnumValue ( "TriggerSource", strValue, mIGXFeatureControl );
                GetEnumCurrentValue ( "TriggerSource", TriggerSource, false );
            }
            catch
            {
                GetEnumCurrentValue ( "TriggerSource", TriggerSource, false );
            }
        }
        /// <summary>
        /// 触发极性改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        void TriggerActivation_ValueChanged ( object sender, IConvertible value )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return;
                string strValue = ( string ) TagProvider.GetTag ( TriggerActivation.Expression ).Value.ToString ( );
                if ( !mTriggerActivationList.Contains ( strValue ) )
                    return;
                SetEnumValue ( "TriggerActivation", strValue, mIGXFeatureControl );
                GetEnumCurrentValue ( "TriggerActivation", TriggerActivation, false );
            }
            catch
            {
                GetEnumCurrentValue ( "TriggerActivation", TriggerActivation, false );
            }
        }
        /// <summary>
        /// 曝光时间值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        void ExposureTime_ValueChanged ( object sender, IConvertible value )
        {
            try
            {
                double dShutterValue = 0.0;              //曝光值

                if ( mIGXFeatureControl == null )
                    return;
                if ( mIsOpen )
                    return;
                string strValue = ( string ) TagProvider.GetTag ( ExposureTime.Expression ).Value.ToString ( );
                dShutterValue = Convert.ToDouble ( strValue );
                //判断输入值是否在曝光时间的范围内
                //若大于最大值则将曝光值设为最大值
                if ( dShutterValue > mExposureTimeMax )
                    dShutterValue = mExposureTimeMax;
                //若小于最小值将曝光值设为最小值
                if ( dShutterValue < mExposureTimeMin )
                    dShutterValue = mExposureTimeMin;

                mIGXFeatureControl.GetFloatFeature ( "ExposureTime" ).SetValue ( dShutterValue );
                InitExposureTime ( ExposureTime, false );
            }
            catch
            {
                InitExposureTime ( ExposureTime, false );
            }
        }
        /// <summary>
        /// 增益值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        void Gain_ValueChanged ( object sender, IConvertible value )
        {

            try
            {
                double dGain = 0; //增益值
                if ( mIGXFeatureControl == null )
                    return;
                if ( mIsOpen )
                    return;
                string strValue = ( string ) TagProvider.GetTag ( Gain.Expression ).Value.ToString ( );
                dGain = Convert.ToDouble ( strValue );

                //判断输入值是否在增益值的范围内
                //若输入的值大于最大值则将增益值设置成最大值
                if ( dGain > mGainMax )
                    dGain = mGainMax;

                //若输入的值小于最小值则将增益的值设置成最小值
                if ( dGain < mGainMin )
                    dGain = mGainMin;
                mIGXFeatureControl.GetFloatFeature ( "Gain" ).SetValue ( dGain );
                InitGain ( Gain, false );
            }
            catch
            {
                InitGain ( Gain, false );
            }
        }
        /// <summary>
        /// 白平衡系数值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        void BalanceRatio_ValueChanged ( object sender, IConvertible value )
        {

            try
            {
                double dBalanceRatio = 0;    //白平衡系数值
                if ( mIGXFeatureControl == null )
                    return;
                if ( mIsOpen )
                    return;
                bool bIsBalanceRatio = mIGXFeatureControl.IsImplemented ( "BalanceRatio" );
                if ( !bIsBalanceRatio )
                    return;
                string strValue = ( string ) TagProvider.GetTag ( BalanceRatio.Expression ).Value.ToString ( );
                dBalanceRatio = Convert.ToDouble ( strValue );
                //判断输入值是否在白平衡系数的范围内
                //若大于最大值则将白平衡系数设为最大值
                if ( dBalanceRatio > mBalanceRatioMax )
                    dBalanceRatio = mBalanceRatioMax;
                //若小于最小值将白平衡系数设为最小值
                if ( dBalanceRatio < mBalanceRatioMin )
                    dBalanceRatio = mBalanceRatioMin;
                mIGXFeatureControl.GetFloatFeature ( "BalanceRatio" ).SetValue ( dBalanceRatio );
                InitWhiteRatio ( BalanceRatio, false );
            }
            catch
            {
                InitWhiteRatio ( BalanceRatio, false );
            }
        }
        /// <summary>
        /// 自动白平衡值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        void AutoWhite_ValueChanged ( object sender, IConvertible value )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return;
                string strValue = ( string ) TagProvider.GetTag ( AutoWhite.Expression ).Value.ToString ( );
                if ( !mAutoWhiteList.Contains ( strValue ) )
                    return;
                SetEnumValue ( "BalanceWhiteAuto", strValue, mIGXFeatureControl );
                GetEnumCurrentValue ( "BalanceWhiteAuto", AutoWhite, false );
            }
            catch
            {
                GetEnumCurrentValue ( "BalanceWhiteAuto", AutoWhite, false );
            }
        }
        /// <summary>
        /// 白平衡值通道选择改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        void RatioSelector_ValueChanged ( object sender, IConvertible value )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return;
                string strValue = ( string ) TagProvider.GetTag ( RatioSelector.Expression ).Value.ToString ( );
                if ( !mRatioSelectorList.Contains ( strValue ) )
                    return;
                SetEnumValue ( "BalanceRatioSelector", strValue, mIGXFeatureControl );
                GetEnumCurrentValue ( "BalanceRatioSelector", RatioSelector, false );
            }
            catch
            {
                GetEnumCurrentValue ( "BalanceRatioSelector", RatioSelector, false );
            }
        }

        #endregion
        #region ...参数...
        /// <summary>
        /// 对枚举型变量按照功能名称设置值
        /// </summary>
        /// <param name="strFeatureName">枚举功能名称</param>
        /// <param name="strValue">功能的值</param>
        /// <param name="objIGXFeatureControl">属性控制器对像</param>
        private void SetEnumValue ( string strFeatureName, string strValue, IGXFeatureControl objIGXFeatureControl )
        {
            //设置当前功能值
            objIGXFeatureControl.GetEnumFeature ( strFeatureName ).SetValue ( strValue );
        }

        /// <summary>
        /// 获取相机参数,初始化界面属性变量
        /// </summary>
        private void InitUI ( )
        {
            GetEnumCurrentValue ( "TriggerMode", TriggerMode, true );
            GetEnumCurrentValue ( "TriggerSource", TriggerSource, true );
            GetEnumCurrentValue ( "TriggerActivation", TriggerActivation, true );
            InitExposureTime ( ExposureTime, true );
            InitGain ( Gain, true );
            InitWhiteRatio ( BalanceRatio, true );
            GetEnumCurrentValue ( "BalanceWhiteAuto", AutoWhite, true );
            GetEnumCurrentValue ( "BalanceRatioSelector", RatioSelector, true );

        }
        /// <summary>
        /// 白平衡系数
        /// </summary>
        /// <param name="tagProperty"></param>
        /// <param name="init">是否更新缓存</param>
        private void InitWhiteRatio ( PropertyLinkAnimate<string> tagProperty, bool init )
        {
            try
            {
                double dWhiteRatio = 0.0;                       //当前曝光值
                double dMin = 0.0;                       //最小值
                double dMax = 0.0;                       //最大值
                //string strUnit = "";                        //单位
                //string strText = "";                        //显示内容
                bool bIsBalanceRatio = false;                   //是否白平衡是否支持
                //获取当前相机的白平衡系数、最小值、最大值和单位
                if ( null != mIGXFeatureControl )
                {
                    bIsBalanceRatio = mIGXFeatureControl.IsImplemented ( "BalanceRatio" );
                    if ( !bIsBalanceRatio )
                    {
                        if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                            TagProvider.WriteTag ( tagProperty.Expression, "" );
                        return;
                    }
                    dWhiteRatio = mIGXFeatureControl.GetFloatFeature ( "BalanceRatio" ).GetValue ( );
                    if ( init )
                    {
                        dMin = mIGXFeatureControl.GetFloatFeature ( "BalanceRatio" ).GetMin ( );
                        dMax = mIGXFeatureControl.GetFloatFeature ( "BalanceRatio" ).GetMax ( );
                        //strUnit = mIGXFeatureControl.GetFloatFeature ( "BalanceRatio" ).GetUnit ( );
                        mBalanceRatioMin = dMin;
                        mBalanceRatioMax = dMax;
                    }
                }

                ////刷新获取白平衡系数范围及单位到界面上
                //strText = string.Format ( "白平衡系数({0}~{1}){2}", dMin.ToString ( "0.00" ), dMax.ToString ( "0.00" ), strUnit );


                //当前的白平衡系数
                if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                    TagProvider.WriteTag ( tagProperty.Expression, dWhiteRatio.ToString ( "0.00" ) );
            }
            catch
            {
                if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                    TagProvider.WriteTag ( tagProperty.Expression, "" );
            }
        }
        /// <summary>
        /// 增益
        /// </summary>
        /// <param name="tagProperty"></param>
        /// <param name="init"></param>
        private void InitGain ( PropertyLinkAnimate<string> tagProperty, bool init )
        {
            try
            {
                double dCurGain = 0;             //当前增益值
                double dMin = 0.0;           //最小值
                double dMax = 0.0;           //最大值
                //string strUnit = "";            //单位
                //string strText = "";            //显示内容

                //获取当前相机的增益值、最小值、最大值和单位
                if ( null != mIGXFeatureControl )
                {
                    dCurGain = mIGXFeatureControl.GetFloatFeature ( "Gain" ).GetValue ( );
                    if ( init )
                    {
                        dMin = mIGXFeatureControl.GetFloatFeature ( "Gain" ).GetMin ( );
                        dMax = mIGXFeatureControl.GetFloatFeature ( "Gain" ).GetMax ( );
                        //strUnit = mIGXFeatureControl.GetFloatFeature ( "Gain" ).GetUnit ( );
                        mGainMin = dMin;
                        mGainMax = dMax;
                    }
                }

                ////更新增益值范围到界面
                //strText = string.Format ( "增益({0}~{1}){2}", dMin.ToString ( "0.00" ), dMax.ToString ( "0.00" ), strUnit );

                //当前的增益值
                string strCurGain = dCurGain.ToString ( "0.00" );
                if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                    TagProvider.WriteTag ( tagProperty.Expression, dCurGain.ToString ( "0.00" ) );

            }
            catch
            {
                if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                    TagProvider.WriteTag ( tagProperty.Expression, "" );
            }
        }
        /// <summary>
        /// 曝光控制
        /// </summary>
        /// <param name="tagProperty"></param>
        /// <param name="init">是否更新缓存</param>
        private void InitExposureTime ( PropertyLinkAnimate<string> tagProperty, bool init )
        {
            try
            {
                double dCurShuter = 0.0;                       //当前曝光值
                double dMin = 0.0;                       //最小值
                double dMax = 0.0;                       //最大值
                //string strUnit = "";                        //单位
                //string strText = "";                        //显示内容

                //获取当前相机的曝光值、最小值、最大值和单位
                if ( null != mIGXFeatureControl )
                {
                    dCurShuter = mIGXFeatureControl.GetFloatFeature ( "ExposureTime" ).GetValue ( );
                    if ( init )
                    {
                        dMin = mIGXFeatureControl.GetFloatFeature ( "ExposureTime" ).GetMin ( );
                        dMax = mIGXFeatureControl.GetFloatFeature ( "ExposureTime" ).GetMax ( );
                        //strUnit = mIGXFeatureControl.GetFloatFeature ( "ExposureTime" ).GetUnit ( );
                        mExposureTimeMax = dMax;
                        mExposureTimeMin = dMin;
                    }
                }
                //刷新曝光范围及单位到界面上
                //strText = string.Format ( "曝光时间({0}~{1}){2}", dMin.ToString ( "0.00" ), dMax.ToString ( "0.00" ), strUnit );
                //当前的曝光值
                if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                    TagProvider.WriteTag ( tagProperty.Expression, dCurShuter.ToString ( "0.00" ) );

            }
            catch
            {
                if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                    TagProvider.WriteTag ( tagProperty.Expression, "" );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFeatureName">枚举功能名称</param>
        /// <param name="tagProperty">对应的变量实例</param>
        /// <param name="init">是否更新缓存</param>
        private void GetEnumCurrentValue ( string strFeatureName, PropertyLinkAnimate<string> tagProperty, bool init )
        {
            try
            {
                bool bIsImplemented = mIGXFeatureControl.IsImplemented ( strFeatureName );
                // 如果不支持则直接返回
                if ( !bIsImplemented )
                    return;
                //是否可以读
                bool bIsReadable = mIGXFeatureControl.IsReadable ( strFeatureName );
                if ( !bIsReadable )
                    return;
                //获取当前功能值
                string strCurrentValue = mIGXFeatureControl.GetEnumFeature ( strFeatureName ).GetValue ( );

                if ( init )
                    InitList ( strFeatureName );

                if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                    TagProvider.WriteTag ( tagProperty.Expression, strCurrentValue );

            }
            catch
            {
                if ( ( string ) TagProvider.GetTag ( tagProperty.Expression ).Value != null )
                    TagProvider.WriteTag ( tagProperty.Expression, "" );
            }
        }
        /// <summary>
        /// 初始化各个枚举值到内存中
        /// </summary>
        /// <param name="strFeatureName"></param>
        private void InitList ( string strFeatureName )
        {
            List<string> temp = mIGXFeatureControl.GetEnumFeature ( strFeatureName ).GetEnumEntryList ( );
            if ( strFeatureName == "TriggerMode" )
            {
                mTriggerModeList.Clear ( );
                mTriggerModeList.AddRange ( temp );
            }
            else if ( strFeatureName == "TriggerSource" )
            {
                mTriggerSourceList.Clear ( );
                mTriggerSourceList.AddRange ( temp );
            }
            else if ( strFeatureName == "TriggerActivation" )
            {
                mTriggerActivationList.Clear ( );
                mTriggerActivationList.AddRange ( temp );
            }
            else if ( strFeatureName == "BalanceWhiteAuto" )
            {
                mAutoWhiteList.Clear ( );
                mAutoWhiteList.AddRange ( temp );
            }
            else if ( strFeatureName == "BalanceRatioSelector" )
            {
                mRatioSelectorList.Clear ( );
                mRatioSelectorList.AddRange ( temp );
            }
        }
        #endregion
        #region ...服务方法...
        /// <summary>
        /// 枚举设备SN
        /// </summary>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public List<string> EnumDeviceSN ( )
        {
            try
            {
                if ( mIGXFactory == null )
                {
                    mIGXFactory = IGXFactory.GetInstance ( );
                    mIGXFactory.Init ( );
                }
                List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo> ( );
                mIGXFactory.UpdateDeviceList ( 200, listGXDeviceInfo );
                if ( listGXDeviceInfo.Count <= 0 )
                    return null;

                List<string> SNlist = new List<string> ( );
                foreach ( IGXDeviceInfo DeviceInfo in listGXDeviceInfo )
                {
                    SNlist.Add ( DeviceInfo.GetSN ( ) );
                }
                return SNlist;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:EnumDeviceSN:{0}", ex.Message ) );
                return null;
            }
        }
        /// <summary>
        /// 打开设备
        /// </summary>
        /// <param name="SN">EnumDeviceSN方法枚举回来得设备SN</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool OpenDeviceBySN ( string SN )
        {
            try
            {

                //关闭流
                if ( null != mIGXStream )
                {
                    mIGXStream.Close ( );
                    mIGXStream = null;
                    mIGXStreamFeatureControl = null;
                }
                // 如果设备已经打开则关闭，保证相机在初始化出错情况下能再次打开
                if ( null != mIGXDevice )
                {
                    mIGXDevice.Close ( );
                    mIGXDevice = null;
                }
                if ( mIGXFactory == null )
                {
                    mIGXFactory = IGXFactory.GetInstance ( );
                    mIGXFactory.Init ( );
                }
                List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo> ( );
                mIGXFactory.UpdateDeviceList ( 200, listGXDeviceInfo );
                if ( listGXDeviceInfo.Count <= 0 )
                    return false;
                mIGXDevice = mIGXFactory.OpenDeviceBySN ( SN, GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE );
                mIGXFeatureControl = mIGXDevice.GetRemoteFeatureControl ( );

                //打开流
                if ( null != mIGXDevice )
                {
                    mIGXStream = mIGXDevice.OpenStream ( 0 );
                    mIGXStreamFeatureControl = mIGXStream.GetFeatureControl ( );
                    mGxBitmap = new GxBitmap ( mIGXDevice, m_pic_ShowImage );
                }
                mIsOpen = true;
                //设置采集模式
                if ( null != mIGXFeatureControl )
                {
                    mIGXFeatureControl.GetEnumFeature ( "AcquisitionMode" ).SetValue ( AcquisitionMode.ToString ( ) );
                }
                // 获取相机参数,初始化界面属性变量
                InitUI ( );

                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:OpenDeviceBySN:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool CloseDevice ( )
        {

            return CloseDevice_Internal ( );
        }
        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public bool CloseDevice_Internal ( )
        {
            try
            {

                // 如果未停采则先停止采集
                if ( mIsSnap && null != mIGXFeatureControl )
                {
                    mIGXFeatureControl.GetCommandFeature ( "AcquisitionStop" ).Execute ( );
                    mIGXFeatureControl = null;
                }
                mIsSnap = false;
                //停止流通道、注销采集回调和关闭流
                if ( null != mIGXStream )
                {
                    mIGXStream.StopGrab ( );
                    //注销采集回调函数
                    mIGXStream.UnregisterCaptureCallback ( );
                    mIGXStream.Close ( );
                    mIGXStream = null;
                    mIGXStreamFeatureControl = null;
                }
                //关闭设备
                if ( null != mIGXDevice )
                {
                    mIGXDevice.Close ( );
                    mIGXDevice = null;
                }
                mIsOpen = false;
                if ( mIGXFactory != null )
                {
                    mIGXFactory.Uninit ( );
                    mIGXFactory = null;
                }
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:CloseDevice:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 开始采集
        /// </summary>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool StartDevice ( )
        {
            try
            {
                if ( null != mIGXStreamFeatureControl )
                {
                    //设置流层Buffer处理模式为OldestFirst
                    mIGXStreamFeatureControl.GetEnumFeature ( "StreamBufferHandlingMode" ).SetValue ( "OldestFirst" );
                }
                //开启采集流通道
                if ( null != mIGXStream )
                {
                    //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
                    //类型)，若用户想用这个参数可以在委托函数中进行使用
                    mIGXStream.RegisterCaptureCallback ( this, CaptureCallbackPro );
                    mIGXStream.StartGrab ( );
                }
                //发送开采命令
                if ( null != mIGXFeatureControl )
                {
                    mIGXFeatureControl.GetCommandFeature ( "AcquisitionStart" ).Execute ( );
                }
                mIsSnap = true;
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:StartDevice:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 停止采集
        /// </summary>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool StopDevice ( )
        {
            try
            {
                //发送停采命令
                if ( null != mIGXFeatureControl )
                {
                    mIGXFeatureControl.GetCommandFeature ( "AcquisitionStop" ).Execute ( );
                }
                //关闭采集流通道
                if ( null != mIGXStream )
                {
                    mIGXStream.StopGrab ( );
                    //注销采集回调函数
                    mIGXStream.UnregisterCaptureCallback ( );
                }
                mIsSnap = false;
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:StopDevice:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 发送软触发命令
        /// </summary>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SoftTriggerCommand ( )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return false;
                mIGXFeatureControl.GetCommandFeature ( "TriggerSoftware" ).Execute ( );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SoftTriggerCommand:{0}", ex.Message ) );
                return false;
            }
        }

        /// <summary>
        /// 获取枚举功能是否支持及是否可读
        /// </summary>
        /// <param name="strFeatureName">枚举功能名称</param>
        /// <returns>0：方法执行错误;1：不支持枚举;2：不支持读,3:即支持枚举也支持读</returns>
        [ScriptMethod]
        public int GetFeatureIsImplemented ( string strFeatureName )
        {
            try
            {
                bool bIsImplemented = mIGXFeatureControl.IsImplemented ( strFeatureName );
                // 如果不支持则直接返回
                if ( !bIsImplemented )
                    return 1;
                //是否可读
                bool bIsReadable = mIGXFeatureControl.IsReadable ( strFeatureName );
                if ( !bIsReadable )
                {
                    return 2;
                }
                return 3;

            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:GetFeatureIsImplemented:{0}", ex.Message ) );
                return 0;
            }
        }
        /// <summary>
        /// 枚举功能支持的值（调用此方法前先调用GetFeatureIsImplemented方法查看是否支持）
        /// </summary>
        /// <param name="strFeatureName">枚举功能名称</param>
        /// <returns>枚举功能值列表</returns>
        [ScriptMethod]
        public List<string> GetEnumFeature ( string strFeatureName )
        {
            try
            {
                return mIGXFeatureControl.GetEnumFeature ( strFeatureName ).GetEnumEntryList ( );
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:GetEnumFeature:{0}", ex.Message ) );
                return null;
            }
        }
        /// <summary>
        /// 设置触发模式
        /// </summary>
        /// <param name="mode">触发模式枚举值</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SetTriggerMode ( string mode )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return false;
                if ( !mTriggerModeList.Contains ( mode ) )
                    return false;
                SetEnumValue ( "TriggerMode", mode, mIGXFeatureControl );
                GetEnumCurrentValue ( "TriggerMode", TriggerMode, false );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SetTriggerMode:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 设置触发源
        /// </summary>
        /// <param name="source">触发源枚举值</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SetTriggerSource ( string source )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return false;
                if ( !mTriggerSourceList.Contains ( source ) )
                    return false;
                SetEnumValue ( "TriggerSource", source, mIGXFeatureControl );
                GetEnumCurrentValue ( "TriggerSource", TriggerSource, false );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SetTriggerSource:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 设置触发极性
        /// </summary>
        /// <param name="activation">触发极性枚举值</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SetTriggerActivation ( string activation )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return false;
                if ( !mTriggerActivationList.Contains ( activation ) )
                    return false;
                SetEnumValue ( "TriggerActivation", activation, mIGXFeatureControl );
                GetEnumCurrentValue ( "TriggerActivation", TriggerActivation, false );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SetTriggerActivation:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        ///设置曝光时间
        /// </summary>
        /// <param name="exposureTime">曝光时间</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SetExposureTime ( double exposureTime )
        {
            try
            {
                if ( exposureTime < mExposureTimeMin || exposureTime > mExposureTimeMax )
                    return false;
                if ( mIGXFeatureControl == null )
                    return false;
                if ( !mIsOpen )
                    return false;
                mIGXFeatureControl.GetFloatFeature ( "ExposureTime" ).SetValue ( exposureTime );
                InitExposureTime ( ExposureTime, false );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SetExposureTime:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="gain">增益</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SetGain ( double gain )
        {
            try
            {
                if ( gain < mGainMin || gain > mGainMax )
                    return false;
                if ( mIGXFeatureControl == null )
                    return false;
                if ( !mIsOpen )
                    return false;
                mIGXFeatureControl.GetFloatFeature ( "Gain" ).SetValue ( gain );
                InitGain ( Gain, false );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SetGain:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 设置白平衡系数
        /// </summary>
        /// <param name="balanceRatio">白平衡系数</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SetBalanceRatio ( double balanceRatio )
        {
            try
            {
                if ( balanceRatio < mBalanceRatioMin || balanceRatio > mBalanceRatioMax )
                    return false;
                if ( mIGXFeatureControl == null )
                    return false;
                if ( !mIsOpen )
                    return false;
                mIGXFeatureControl.GetFloatFeature ( "BalanceRatio" ).SetValue ( balanceRatio );
                InitWhiteRatio ( BalanceRatio, false );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SetBalanceRatio:{0}", ex.Message ) );
                return false;
            }

        }
        /// <summary>
        /// 设置自动白平衡
        /// </summary>
        /// <param name="autoWhite">自动白平衡枚举值</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SetAutoWhite ( string autoWhite )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return false;
                if ( !mAutoWhiteList.Contains ( autoWhite ) )
                    return false;
                if ( !mIsOpen )
                    return false;
                SetEnumValue ( "BalanceWhiteAuto", autoWhite, mIGXFeatureControl );
                GetEnumCurrentValue ( "BalanceWhiteAuto", TriggerActivation, false );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SetAutoWhite:{0}", ex.Message ) );
                return false;
            }
        }
        /// <summary>
        /// 设置白平衡通道
        /// </summary>
        /// <param name="ratioSelector">白平衡通道枚举值</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool SetRatioSelector ( string ratioSelector )
        {
            try
            {
                if ( mIGXFeatureControl == null )
                    return false;
                if ( !mRatioSelectorList.Contains ( ratioSelector ) )
                    return false;
                SetEnumValue ( "BalanceRatioSelector", ratioSelector, mIGXFeatureControl );
                GetEnumCurrentValue ( "BalanceRatioSelector", RatioSelector, false );
                return true;
            }
            catch ( Exception ex )
            {
                Logging.Logger.Error ( string.Format ( "GalaxyView:SetRatioSelector:{0}", ex.Message ) );
                return false;
            }
        }
        #endregion
        #region ...回调...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUserParam"></param>
        /// <param name="objIFrameData"></param>
        private void CaptureCallbackPro ( object objUserParam, IFrameData objIFrameData )
        {
            try
            {
                GalaxyView objGalaxyView = objUserParam as GalaxyView;
                objGalaxyView.ImageShowAndSave ( objIFrameData );
            }
            catch ( Exception )
            {
            }
        }
        /// <summary>
        /// 图像的显示和存储
        /// </summary>
        /// <param name="objIFrameData">图像信息对象</param>
        void ImageShowAndSave ( IFrameData objIFrameData )
        {
            try
            {
                mGxBitmap.Show ( objIFrameData );
            }
            catch ( Exception )
            {

            }
            if ( mSaveBMP )
            {
                //DateTime dtNow = System.DateTime.Now;  // 获取系统当前时间
                //string strDateTime = dtNow.Year.ToString ( ) + "_"
                //                   + dtNow.Month.ToString ( ) + "_"
                //                   + dtNow.Day.ToString ( ) + "_"
                //                   + dtNow.Hour.ToString ( ) + "_"
                //                   + dtNow.Minute.ToString ( ) + "_"
                //                   + dtNow.Second.ToString ( ) + "_"
                //                   + dtNow.Millisecond.ToString ( );
                if ( string.IsNullOrEmpty ( mBMPName ) )
                    mBMPName = "Image";
                string stfFileName = string.Format ( "{0}\\{1}.bmp", mSavePath, mBMPName );  // 默认的图像保存名称
                if ( File.Exists ( stfFileName ) )
                    File.Delete ( stfFileName );
                mGxBitmap.SaveBmp ( objIFrameData, stfFileName );
            }

        }
        #endregion

        #region ICustomSerializable 成员
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="element"></param>
        public void Decode ( System.Xml.Linq.XElement element )
        {
            XElement xx = element.Element ( "DaHengGalaxyView" );
            if ( xx == null )
                return;
            if ( xx.Element ( "TriggerMode" ) != null )
            {
                mTriggerMode = xx.Element ( "TriggerMode" ).LoadPropertyAnimateFromXElement<string> ( );
            }
            if ( xx.Element ( "TriggerSource" ) != null )
            {
                mTriggerSource = xx.Element ( "TriggerSource" ).LoadPropertyAnimateFromXElement<string> ( );
            }
            if ( xx.Element ( "TriggerActivation" ) != null )
            {
                mTriggerActivation = xx.Element ( "TriggerActivation" ).LoadPropertyAnimateFromXElement<string> ( );
            }
            if ( xx.Element ( "ExposureTime" ) != null )
            {
                mExposureTime = xx.Element ( "ExposureTime" ).LoadPropertyAnimateFromXElement<string> ( );
            }
            if ( xx.Element ( "Gain" ) != null )
            {
                mGain = xx.Element ( "Gain" ).LoadPropertyAnimateFromXElement<string> ( );
            }
            if ( xx.Element ( "AutoWhite" ) != null )
            {
                mAutoWhite = xx.Element ( "AutoWhite" ).LoadPropertyAnimateFromXElement<string> ( );
            }
            if ( xx.Element ( "RatioSelector" ) != null )
            {
                mRatioSelector = xx.Element ( "RatioSelector" ).LoadPropertyAnimateFromXElement<string> ( );
            }
            if ( xx.Element ( "BalanceRatio" ) != null )
            {
                mBalanceRatio = xx.Element ( "BalanceRatio" ).LoadPropertyAnimateFromXElement<string> ( );
            }

            if ( xx.Attribute ( "SaveBMP" ) != null )
                mSaveBMP = Convert.ToBoolean ( xx.Attribute ( "SaveBMP" ).Value );
            if ( xx.Attribute ( "BMPName" ) != null )
                mBMPName = Convert.ToString ( xx.Attribute ( "BMPName" ).Value );
            if ( xx.Attribute ( "SavePath" ) != null )
                mSavePath = Convert.ToString ( xx.Attribute ( "SavePath" ).Value );
            if ( xx.Attribute ( "AcquisitionMode" ) != null )
                mAcquisitionMode = ( AcquisitionModes ) Enum.Parse ( typeof ( AcquisitionModes ), xx.Attribute ( "AcquisitionMode" ).Value );

        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="element"></param>
        public void Encode ( System.Xml.Linq.XElement element )
        {
            XElement xx = new XElement ( "DaHengGalaxyView" );

            XElement mtriggerMode = new XElement ( "TriggerMode" );
            mTriggerMode.SaveToXElement<string> ( mtriggerMode );
            xx.Add ( mtriggerMode );

            XElement mtriggerSource = new XElement ( "TriggerSource" );
            mTriggerSource.SaveToXElement<string> ( mtriggerSource );
            xx.Add ( mtriggerSource );

            XElement mtriggerActivation = new XElement ( "TriggerActivation" );
            mTriggerActivation.SaveToXElement<string> ( mtriggerActivation );
            xx.Add ( mtriggerActivation );

            XElement mexposureTime = new XElement ( "ExposureTime" );
            mExposureTime.SaveToXElement<string> ( mexposureTime );
            xx.Add ( mexposureTime );

            XElement mgain = new XElement ( "Gain" );
            mGain.SaveToXElement<string> ( mgain );
            xx.Add ( mgain );

            XElement mautoWhite = new XElement ( "AutoWhite" );
            mAutoWhite.SaveToXElement<string> ( mautoWhite );
            xx.Add ( mautoWhite );

            XElement mratioSelector = new XElement ( "RatioSelector" );
            mRatioSelector.SaveToXElement<string> ( mratioSelector );
            xx.Add ( mratioSelector );

            XElement mbalanceRatio = new XElement ( "BalanceRatio" );
            mBalanceRatio.SaveToXElement<string> ( mbalanceRatio );
            xx.Add ( mbalanceRatio );
            xx.SetAttributeValue ( "SaveBMP", mSaveBMP );
            xx.SetAttributeValue ( "BMPName", mBMPName );
            xx.SetAttributeValue ( "SavePath", mSavePath );
            xx.SetAttributeValue ( "AcquisitionMode", mAcquisitionMode.ToString ( ) );

            element.Add ( xx );
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable ( false )]
        public bool IsFullCustomSerialize
        {
            get { return true; }
        }
        #endregion
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


      
        #region IHideProperty 成员

        #region ...静态变量...
        /// <summary>
        /// 要隐藏的属性
        /// </summary>
        private static string[] HidePropertyList = new string[] {"AllowDrop","AutoScroll", "AutoScrollMargin", "AutoScrollMinSize","AutoSize","AutoSizeMode","AutoValidate",
                                                                   "BackgroundImage","BackgroundImageLayout","Locked",
																   "CausesValidation", "ContextMenuStrip", "Cursor",
                                                                    "DockPadding", "DataBindings", "ImeMode", "MaximumSize","MinimumSize","Margin","RightToLeft", "Padding",
																   "TabIndex","TabStop","UseWaitCursor","ForeColor","Font","BorderStyle","BackColor"};

        private static string[] HideEventList = new string[] {
            "AutoSizeChanged","AutoValidateChanged",
            "BackColorChanged", "BackgroundImageChanged","BackgroundImageLayoutChanged","BindingContextChanged",
            "Click","CausesValidationChanged","ChangeUICues","ClientSizeChanged","ContextMenuStripChanged","ControlAdded","ControlRemoved","CursorChanged",
            "DockChanged","DoubleClick","DragDrop","DragEnter","DragLeave","DragOver",
            "EnabledChanged","Enter",
            "FontChanged","ForeColorChanged",
            "GiveFeedback","HelpRequested","ImeModeChanged",
            "KeyDown","KeyPress","KeyUp",
            "Layout","Leave",   "Load","LocationChanged",
            "MarginChanged","MouseCaptureChanged",
            "MouseClick","MouseDoubleClick","MouseDown","MouseEnter","MouseHover","MouseLeave","MouseMove","MouseUp","Move",
            "PaddingChanged","Paint","PropertyChanged","PreviewKeyDown","ParentChanged",
            "QueryAccessibilityHelp","QueryContinueDrag",
            "RegionChanged","Resize","RightToLeftChanged",
            "Scroll","SizeChanged","StyleChanged","SystemColorsChanged",
            "TabIndexChanged","TabStopChanged",
            "Validated","Validating","VisibleChanged","BackColorChanged"};
        #endregion ...静态变量...

        string[] IHideProperty.GetHideProperties ( PropertyPanelType pannelType )
        {
            if ( pannelType == PropertyPanelType.Normal )
            {
                return HidePropertyList;
            }
            if ( pannelType == PropertyPanelType.Operation )
            {
                return HideEventList;
            }
            return null;
        }

        #endregion
    }
}
