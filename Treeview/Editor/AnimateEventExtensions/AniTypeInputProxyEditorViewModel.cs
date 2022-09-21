using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ControlEase.Nexus.Presentation;
using ControlEase.AI.Tag;
using ControlEase.Nexus.ComponentModel;
using ControlEase.Inspec.AniInfos;
using ControlEase.Inspec.ViewPluginShapes;
using ControlEase.Inspec.Animates;
using ControlEase.Inspec.Resources;
using ControlEase.Nexus;
using System.Windows;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.ViewPresentation;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// AniTypeInputProxyEditorViewModel
    /// </summary>
    public class AniTypeInputProxyEditorViewModel : WindowViewModelBase
    {
        #region ... Variables  ...

        private ICommand mBrowserCommand;

        private string mExpression = "";

        private ICommand mClearCommand;

       

        /// <summary>
        /// 
        /// </summary>
        public enum InputType { Boolean, String, Number, DateTime }

        private bool mIsEnableInput = false;

        private InputType mType;

        private string mSetTrueExpression = SR.GetString("TrueString");

        private string mSetFalseExpression = SR.GetString("FalseString");

        private bool mIsPassword;

        private double mMinBounds;

        private double mMaxBounds;

        private int mInputPointNumber=14;
        private Decimal mMin = -9999;
        private Decimal mMax = 9999;

        private ICommand mSetTrueBrowserCommand;

        private ICommand mSetFalseBrowserCommand;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public AniTypeInputProxyEditorViewModel()
        {
            DefaultWidth = 400;
            DefaultHeight = 150;
            Type = InputType.String;
            Title = SR.GetString("AniTypeInputProxyEditorViewModel_Title");
            IsEnableMax = false;
            MessageMediatorHelper.Mediator.Register(this); 
        }

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 最大值范围
        /// </summary>
        public double MaxBounds
        {
            get
            {
                return mMaxBounds;
            }
            set
            {
                if (mMaxBounds != value)
                {
                    mMaxBounds = value;
                    OnPropertyChanged("MaxBounds");
                }
            }
        }


        /// <summary>
        /// 最小值范围
        /// </summary>
        public double MinBounds
        {
            get
            {
                return mMinBounds;
            }
            set
            {
                if (mMinBounds != value)
                {
                    mMinBounds = value;
                    OnPropertyChanged("MinBounds");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        IResourceService ResSvr
        {
            get
            {
                return ServiceLocator.Current.Resolve<IResourceService>();
            }
        }

        #region numberInput
        /// <summary>
        /// 
        /// </summary>
        public int InputPointNumber
        {
            get
            {
                return mInputPointNumber;
            }
            set
            {
                if (mInputPointNumber != value)
                {
                    
                    mInputPointNumber = value;
                    OnPropertyChanged("InputPointNumber");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Decimal Min
        {
            get
            {
                return mMin;
            }
            set
            {
                if (mMin != value)
                {
                    mMin = value;
                    OnPropertyChanged("Min");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Decimal Max
        {
            get
            {
                return mMax;
            }
            set
            {
                if (mMax != value)
                {
                    mMax = value;
                    OnPropertyChanged("Max");
                }
            }
        }
        #endregion

        #region stringInput
        /// <summary>
        /// 
        /// </summary>
        public bool IsPassword
        {
            get
            {
                return mIsPassword;
            }
            set
            {
                if (mIsPassword != value)
                {
                    mIsPassword = value;
                    OnPropertyChanged("IsPassword");
                }
            }
        }
        #endregion

        #region booleanInput
        /// <summary>
        /// 
        /// </summary>
        public string SetFalseExpression
        {
            get
            {
                return mSetFalseExpression;
            }
            set
            {
                if (mSetFalseExpression != value)
                {
                    mSetFalseExpression = value;
                    OnPropertyChanged("SetFalseExpression");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SetTrueExpression
        {
            get
            {
                return mSetTrueExpression;
            }
            set
            {
                if (mSetTrueExpression != value)
                {
                    mSetTrueExpression = value;
                    OnPropertyChanged("SetTrueExpression");
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public InputType Type
        {
            get
            {
                return mType;
            }
            set
            {
                if (mType != value)
                {
                    mType = value;
                    OnPropertyChanged("IsStringInput");
                    OnPropertyChanged("IsBooleanInput");
                    OnPropertyChanged("IsNumberInput");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsStringInput
        {
            get
            {
                return mType == InputType.String;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsBooleanInput
        {
            get
            {
                return mType == InputType.Boolean;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNumberInput
        {
            get
            {
                return mType == InputType.Number;
            }
        }


        /// <summary>
        /// 变量浏览服务
        /// </summary>
        public ITagBrowser TagBrowser
        {
            get
            {
                return ServiceLocator.Current.Resolve<ITagBrowser> ( );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ITagProvider TagProvider
        {
            get
            {
                return ServiceLocator.Current.Resolve<ITagProvider> ( );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand BrowserCommand
        {
            get
            {
                if ( mBrowserCommand == null )
                {
                    mBrowserCommand = new RelayCommand ( ( ) => {
                        var vv = TagBrowser.ShowBrowser ( true, true,null);
                        if ( vv != null && vv.Count ( ) > 0 )
                        {
                            Expression = vv.First().FullName;
                        }
                    } );
                }
                return mBrowserCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                if (mClearCommand == null)
                {
                    mClearCommand = new RelayCommand(() => {
                        Expression = "";
                    });
                }
                return mClearCommand;
            }
        }

        /// <summary>
        /// 定义的表达式
        /// </summary>
        public string Expression
        {
            get
            {
                return mExpression;
            }
            set
            {
                if ( mExpression != value )
                {
                    mExpression = value;
                    CheckType(value);
                    OnPropertyChanged ( "Expression" );
                }
            }
        }

        ///// <summary>
        ///// 删除快捷键
        ///// </summary>
        //public ICommand ClearKeyCommand
        //{
        //    get
        //    {
        //        if (mClearKeyCommand == null)
        //        {
        //            mClearKeyCommand = new RelayCommand(() => {
        //                Keys = System.Windows.Forms.Keys.None;
        //            });
        //        }
        //        return mClearKeyCommand;
        //    }
        //}

        ///// <summary>
        ///// 提示信息资源浏览
        ///// </summary>
        //public ICommand InfoBrowserCommand
        //{
        //    get
        //    {
        //        if (mInfoBrowserCommand == null)
        //        {
        //            mInfoBrowserCommand = new RelayCommand(() => {
        //                ResourceTextEditorViewModel mm = new ResourceTextEditorViewModel(this.Info);
        //                if (mm.ShowDialog().Value)
        //                {
        //                    Info = mm.Text;
        //                }        
        //            });
        //        }
        //        return mInfoBrowserCommand;
        //    }
        //}


        /// <summary>
        /// 设置成True文本提示信息资源浏览
        /// </summary>
        public ICommand SetTrueBrowserCommand
        {
            get
            {
                if (mSetTrueBrowserCommand == null)
                {
                    mSetTrueBrowserCommand = new RelayCommand(() =>
                    {
                        ResourceTextEditorViewModel mm = new ResourceTextEditorViewModel(this.SetTrueExpression);
                        if (mm.ShowDialog().Value)
                        {
                            SetTrueExpression = mm.Text;
                        }
                    });
                }
                return mSetTrueBrowserCommand;
            }
        }

        /// <summary>
        /// 设置成False文本提示信息资源浏览
        /// </summary>
        public ICommand SetFalseBrowserCommand
        {
            get
            {
                if (mSetFalseBrowserCommand == null)
                {
                    mSetFalseBrowserCommand = new RelayCommand(() =>
                    {
                        ResourceTextEditorViewModel mm = new ResourceTextEditorViewModel(this.SetFalseExpression);
                        if (mm.ShowDialog().Value)
                        {
                            SetFalseExpression = mm.Text;
                        }
                    });
                }
                return mSetFalseBrowserCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IPropertyAnimate BackType { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vv"></param>
        private void InitBoolInput ( AniInputDisc vv )
        {
            SetTrueExpression = vv.TrueText;
            SetFalseExpression = vv.FalseText;
            if ( !string.IsNullOrEmpty ( vv.TrueTextResName ) )
            {
                var itmp = ResSvr.GetResourceItem ( ResourceTypes.String, vv.TrueTextResName );
                if ( itmp != null )
                {
                    SetTrueExpression = ( itmp.ResourceTypeInfo as TextStringResourceType ).CurrentValue;
                }
            }
            if ( !string.IsNullOrEmpty ( vv.FalseTextResName ) )
            {
                var itmp = ResSvr.GetResourceItem ( ResourceTypes.String, vv.FalseTextResName );
                if ( itmp != null )
                {
                    SetFalseExpression = ( itmp.ResourceTypeInfo as TextStringResourceType ).CurrentValue;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private AniInputDisc GetBoolInput ( )
        {
            var mInput = new AniInputDisc ( ) { FalseText = mSetFalseExpression, TrueText = mSetTrueExpression, Key = new NamedIdentity ( "AniInputDisc" ), AnimateType = typeof ( AniInputDisc ) };
            IEnumerable<ResourceItem> vv;
            if (!string.IsNullOrEmpty(mSetTrueExpression))
            {
                vv = ResSvr.GetResourceItems(ResourceTypes.String).Where(e => (e.ResourceTypeInfo as TextStringResourceType).CurrentValue == mSetTrueExpression);
                if (vv.Count() > 0)
                {
                    mInput.TrueTextResName = vv.First().Name;
                }
                else
                {
                    string result = "";
                    var item = new ResourceItem() { Name = ResSvr.GetValidResourceName(ResourceTypes.String), ResourceTypeInfo = new TextStringResourceType() { CurrentValue = mSetTrueExpression } };
                    ResSvr.AddResourceItem(item, out result);
                    mInput.TrueTextResName = item.Name;
                }
            }
           if (!string.IsNullOrEmpty(mSetFalseExpression))
           {
               vv = ResSvr.GetResourceItems(ResourceTypes.String).Where(e => (e.ResourceTypeInfo as TextStringResourceType).CurrentValue == mSetFalseExpression);
               if (vv.Count() > 0)
               {
                   mInput.FalseTextResName = vv.First().Name;
               }
               else
               {
                   string result = "";
                   var item = new ResourceItem() { Name = ResSvr.GetValidResourceName(ResourceTypes.String), ResourceTypeInfo = new TextStringResourceType() { CurrentValue = mSetFalseExpression } };
                   ResSvr.AddResourceItem(item, out result);
                   mInput.FalseTextResName = item.Name;
               }
            }
            return mInput;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private AniInputDateTime GetDateTimeInput ( )
        {
            AniInputDateTime mAniInfoTimeIn = new AniInputDateTime ( ) { Key = new NamedIdentity ( "AniInputDateTime" ), AnimateType = typeof ( AniInputDateTime ) };
            return mAniInfoTimeIn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private AniInputString GetStringInput ( )
        {
            AniInputString re = new AniInputString ( ) { Key = new NamedIdentity ( "AniInputString" ), AnimateType = typeof ( AniInputString ) };
            re.PasswordInput = IsPassword;
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vv"></param>
        private void InitStringInput ( AniInputString vv )
        {
            IsPassword = vv.PasswordInput;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private AniInputAnalog GetNumberInput ( )
        {
            var mAniInfoNumericIn = new AniInputAnalog ( ) { ValueMax = Max, ValueMin = Min, DecimalNum = InputPointNumber, Key = new NamedIdentity ( "AniInputAnalog" ), AnimateType = typeof ( AniInputAnalog ) };
            return mAniInfoNumericIn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vv"></param>
        private void InitNumberInput ( AniInputAnalog vv )
        {
            Max = vv.ValueMax;
            Min = vv.ValueMin;
            InputPointNumber = vv.DecimalNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget ( object target )
        {
            if (target!=null && target is  IPropertyAnimate)
            {
                Expression = (target as IPropertyAnimate).Expression;
                if ( target is AniInputDisc )
                {
                    InitBoolInput ( target as AniInputDisc );
                }
                else if ( target is AniInputAnalog )
                {
                    InitNumberInput ( target as AniInputAnalog );
                }
                else if ( target is AniInputString )
                {
                    InitStringInput ( target as AniInputString );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget ( IPropertyAnimate target )
        {
            if ( target != null  )
            {
                if ( target.Expression != null )
                {
                    Expression = target.Expression;
                }
                if ( target is AniInputDisc )
                {
                    InitBoolInput ( target as AniInputDisc );
                }
                else if ( target is AniInputAnalog )
                {
                    InitNumberInput ( target as AniInputAnalog );
                }
                else if ( target is AniInputString )
                {
                    InitStringInput ( target as AniInputString );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private void CheckType ( string expression )
        {
            if (string.IsNullOrEmpty(expression))
            {
                Type = InputType.String;
                mIsEnableInput = false;
            }

            var vv = TagProvider.GetTag(expression.Trim());
            if (vv != null && !vv.IsReadOnly)
            {
                if (vv.ValueType == typeof(Int64) || vv.ValueType == typeof(double)||vv.ValueType==typeof(UInt64)||vv.ValueType==typeof(UInt32))
                {
                    Type = InputType.Number;
                    if (vv.ValueType == typeof(Int64))
                    {
                        MinBounds = Int64.MinValue;
                        MaxBounds = Int64.MaxValue;
                        Min = -9999;
                    }
                    else if (vv.ValueType == typeof(double))
                    {
                        MinBounds = double.MinValue;
                        MaxBounds = double.MaxValue;
                        Min = -9999;
                    }
                    else if (vv.ValueType == typeof(UInt64))
                    {
                        MinBounds = UInt64.MinValue;
                        MaxBounds = UInt64.MaxValue;
                        Min = 0;
                    }

                }
                else if (vv.ValueType == typeof(bool))
                {
                    Type = InputType.Boolean;
                }
                else if (vv.ValueType == typeof(DateTime))
                {
                    Type = InputType.DateTime;
                }
                else if (vv.ValueType == typeof(string))
                {
                    Type = InputType.String;
                }
                else
                {
                    Type = InputType.String;
                    mIsEnableInput = false;
                    return;
                }
                mIsEnableInput = true;
            }
            else
            {
                Type = InputType.String;
                mIsEnableInput = false;
                ReceiveMessage ( SR.GetString ( "TagIsReadonly" ) );
            }
        }

        protected override bool CanOKCommandProcess()
        {
            return mIsEnableInput;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool OKCommandProcess()
        {
            string erros = string.Empty;
            if (ExpressionService.Service.Check(Expression, out erros))
            {
                switch (Type)
                {
                    case InputType.String:
                        BackType = GetStringInput();
                        break;
                    case InputType.Boolean:
                        BackType = GetBoolInput() ;
                        break;
                    case InputType.Number:
                        BackType = GetNumberInput();
                        break;
                    case InputType.DateTime:
                        BackType = GetDateTimeInput ( );
                        break;
                }
                if (BackType != null)
                {
                    BackType.Expression = Expression;
                }
                return base.OKCommandProcess();
            }
            else
            {
                if (!string.IsNullOrEmpty(erros))
                {
                    ControlEase.Nexus.Windows.MessageBox.Show(string.Format(SR.GetString("ExpressionErro"), erros), SR.GetString("Warning"), System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                else
                {
                    ControlEase.Nexus.Windows.MessageBox.Show(SR.GetString("ExpressionIsNull"), SR.GetString("Warning"), System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            MessageMediatorHelper.Mediator.Unregister(this);
        }

        [MessageMediatorTarget ( "AniTypeInputProxyEditorViewModel" )]
        private void ReceiveMessage(string msg)
        {
            Message = msg;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
