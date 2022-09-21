using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ControlEase.Nexus.Presentation;
using ControlEase.Inspec.Animates;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Xml.Linq;
using ControlEase.Inspec.Extension;
using System.Collections.ObjectModel;
using ControlEase.AI.View;
using ControlEase.Inspec.ViewCore;
using ControlEase.AI.Script;
using ControlEase.AI.Security;
using ControlEase.Nexus.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using ControlEase.Inspec.View;
using System.Diagnostics;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Class NodeTreeEditNodeViewModel
    /// </summary>
    [TypeConverter("ControlEase.Inspec.View.GlobalizedTypeConverter, CE.NX.VWD")]
    public class TreeNodeViewModel : ViewModel, IPropertyConflict
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        string mLeftMouseClickMenu;
        /// <summary>
        /// 
        /// </summary>
        string mRightMouseClickMenu;
        /// <summary>
        /// 
        /// </summary>
        PropertyLinkEvent<bool> mLeftMouseClick;
        /// <summary>
        /// 
        /// </summary>
        PropertyLinkEvent<bool> mLeftMouseDoubleClick;
        /// <summary>
        /// 勾选节点事件
        /// </summary>
        PropertyLinkEvent<bool> mCheckNodePrograme;
        /// <summary>
        /// 反勾选节点事件
        /// </summary>
        PropertyLinkEvent<bool> mUnCheckNodePrograme;
        /// <summary>
        /// 
        /// </summary>
        PropertyLinkEvent<bool> mRightMouseClick;
        /// <summary>
        /// 
        /// </summary>
        IPropertyAnimate mLeftMouseClickInput;
        /// <summary>
        /// 
        /// </summary>
        IPropertyAnimate mRightMouseClickInput;
        ///// <summary>
        ///// 
        ///// </summary>
        IPropertyAnimate mLeftMouseDoubleClickInput;
        /// <summary>
        /// 
        /// </summary>
        TreeNodeViewModel mParent;
        /// <summary>
        /// 
        /// </summary>
        TreeViewControlViewModel mRoot;
        /// <summary>
        /// 
        /// </summary>
        ObservableCollection<TreeNodeViewModel> mChildren;
        /// <summary>
        /// 
        /// </summary>
        bool mIsExpanded = true;
        /// <summary>
        /// 
        /// </summary>
        bool mIsSelected;
        /// <summary>
        /// 
        /// </summary>
        bool mIsChecked;
        /// <summary>
        /// 
        /// </summary>
        private string mName;
        /// <summary>
        /// 
        /// </summary>
        private string mNameIdentity;
        /// <summary>
        /// 
        /// </summary>
        private string mDescription;
        /// <summary>
        /// 
        /// </summary>
        private string mToolTipString;
        /// <summary>
        /// 
        /// </summary>
        protected int mImageSource;
        /// <summary>
        /// 
        /// </summary>
        protected int mFontSize = 16;
        /// <summary>
        /// 
        /// </summary>
        protected string mFontFamilyString = "Arial";
        /// <summary>
        /// 
        /// </summary>
        protected bool mCheckBoxVisibility;
        /// <summary>
        /// 默认为透明
        /// </summary>
        protected System.Drawing.Color mBackground = System.Drawing.Color.Transparent;
        /// <summary>
        /// 
        /// </summary>
        protected System.Drawing.Color mLostFocusBackground = System.Drawing.Color.Gray;
        /// <summary>
        /// 默认为黑色
        /// </summary>
        protected System.Drawing.Color mForeground = System.Drawing.Color.Black;
        /// <summary>
        /// 默认为白色
        /// </summary>
        protected System.Drawing.Color mSelectedForeground = System.Drawing.Color.White;
        /// <summary>
        /// 默认为蓝色
        /// </summary>
        protected System.Drawing.Color mSelectedBackground = System.Drawing.Color.Blue;
        /// <summary>
        /// 
        /// </summary>
        protected int mSpace = 0;
        /// <summary>
        /// 
        /// </summary>
        protected int mMargin = 0;
        /// <summary>
        /// 
        /// </summary>
        protected bool mLineVisibility;
        /// <summary>
        /// 
        /// </summary>
        protected bool mVerLineVisibility;
        /// <summary>
        /// 
        /// </summary>
        protected bool mExpandIconVisibility;
        /// <summary>
        /// 
        /// </summary>
        private int mNodeSpace = 0;
        /// <summary>
        /// The m is enabled
        /// </summary>
        private bool mIsEnabled = true;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        public TreeNodeViewModel()
        {
            Children = new ObservableCollection<TreeNodeViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeNodeViewModel(TreeViewControlViewModel treeViewControlViewModel, TreeNodeViewModel parent) :
            this()
        {
            Parent = parent;
            Root = treeViewControlViewModel;
            mNameIdentity = mRoot.NameRepository.GetNewNameIdenty();
            mRoot.NameRepository.Register(mNameIdentity);
            OnPropertyChanged("NameIdentity");
            Name = NameIdentity;
            if (Root != null)
            {
                LineVisibility = mRoot.LineVisibility;
                VerLineVisibility = mRoot.LineVisibility;
                ExpandIconVisibility = mRoot.ExpandIconVisibility;
            }
        }

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public new object DataContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public new bool IsDisposed { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is enabled.
        ///// </summary>
        ///// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        //[Browsable(false)]
        //public bool IsEnabled
        //{
        //    get
        //    {
        //        return mIsEnabled;
        //    }
        //    set
        //    {
        //        mIsEnabled = value;
        //        OnPropertyChanged("IsEnabled");
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [TypeConverter("ControlEase.Inspec.TreeView.BoolenToVisibilityConverter,CE.NX.TV")]
        public bool LineVisibility
        {
            get
            {
                return mLineVisibility;
            }
            set
            {
                mLineVisibility = value;
                OnPropertyChanged("LineVisibility");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [TypeConverter("ControlEase.Inspec.TreeView.BoolenToVisibilityConverter,CE.NX.TV")]
        public bool VerLineVisibility
        {
            get
            {
                return mVerLineVisibility;
            }
            set
            {
                mVerLineVisibility = value;
                OnPropertyChanged("VerLineVisibility");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [TypeConverter("ControlEase.Inspec.TreeView.BoolenToVisibilityConverter,CE.NX.TV")]
        public bool ExpandIconVisibility
        {
            get
            {
                return mExpandIconVisibility;
            }
            set
            {
                mExpandIconVisibility = value;
                OnPropertyChanged("ExpandIconVisibility");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Background")]
        [DefaultValue(typeof(System.Drawing.Color), "Transparent")]
        public System.Drawing.Color Background
        {
            get
            {
                return mBackground;
            }
            set
            {
                if (mBackground != value)
                {
                    mBackground = value;
                    OnPropertyChanged("Background");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Foreground")]
        [DefaultValue(typeof(System.Drawing.Color), "Black")]
        public System.Drawing.Color Foreground
        {
            get
            {
                return mForeground;
            }
            set
            {
                if (mForeground != value)
                {
                    mForeground = value;
                    OnPropertyChanged("Foreground");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("LostFocusBackground")]
        [DefaultValue(typeof(System.Drawing.Color), "Transparent")]
        public System.Drawing.Color LostFocusBackground
        {
            get
            {
                return mLostFocusBackground;
            }
            set
            {
                if (mLostFocusBackground != value)
                {
                    mLostFocusBackground = value;
                    OnPropertyChanged("LostFocusBackground");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SelectedBackground")]
        [DefaultValue(typeof(System.Drawing.Color), "Blue")]
        public System.Drawing.Color SelectedBackground
        {
            get
            {
                return mSelectedBackground;
            }
            set
            {
                if (mSelectedBackground != value)
                {
                    mSelectedBackground = value;
                    OnPropertyChanged("SelectedBackground");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SelectedForeground")]
        [DefaultValue(typeof(System.Drawing.Color), "White")]
        public System.Drawing.Color SelectedForeground
        {
            get
            {
                return mSelectedForeground;
            }
            set
            {
                if (mSelectedForeground != value)
                {
                    mSelectedForeground = value;
                    OnPropertyChanged("SelectedForeground");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FontSize")]
        [DefaultValue(16)]
        public int FontSize
        {
            get
            {
                return mFontSize;
            }
            set
            {
                if (mFontSize != value && (value > 0 && value <= 100))
                {
                    mFontSize = value;
                    OnPropertyChanged("FontSize");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Margin")]
        [TypeConverter("ControlEase.Inspec.TreeView.MarginSpaceConverter,CE.NX.TV")]
        [DefaultValue(0)]
        public int Margin
        {
            get
            {
                return mMargin;
            }
            set
            {
                if (mMargin != value && (value >= 0 && value <= 100))
                {
                    mMargin = value;
                    if (LineVisibility)
                    {
                        HorLineX1 = 0;
                    }
                    OnPropertyChanged("Margin");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [TypeConverter("ControlEase.Inspec.TreeView.MarginSpaceConverter,CE.NX.TV")]
        public int Space
        {
            get
            {
                if (Parent != null)
                    return Parent.NodeSpace;
                else
                    return Root.NodeSpace;
            }
            set
            {
                OnPropertyChanged("Space");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Displayable(true)]
        [DisplayName("NodeSpace")]
        [DefaultValue(0)]
        public int NodeSpace
        {
            get
            {
                return mNodeSpace;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    mNodeSpace = value;
                    foreach (TreeNodeViewModel item in Children.ToList<TreeNodeViewModel>())
                    {
                        item.Space = mNodeSpace;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Double VerLineY2
        {
            get
            {
                TreeNodeViewModel previous = GetPreviousNode();
                if (previous != null)
                {
                    if (previous.IsExpanded)
                    {
                        double total = 0;
                        foreach (var it in previous.Children.ToList<TreeNodeViewModel>())
                        {
                            total += (it.Space + (it.FontSize * 1.333));
                            if (it.IsExpanded)
                                LoopGetSpace(it, ref total);
                        }
                        total = (total + Space + (previous.FontSize * 1.33333333) / 2.0) * (-1);
                        return total;
                    }

                    else
                    {
                        return (-(Space + (previous.FontSize * 1.333) / 2));
                    }
                }
                //如果不存在前一个，则这个对象是节点中的第一个或者是子节点的第一个
                return (-Space / 2);

            }
            set
            {
                try
                {
                    OnPropertyChanged("VerLineY2");
                    TreeNodeViewModel next = null;
                    next = GetNextNode();
                    if (next != null)
                    {
                        next.VerLineY2 = 0;
                    }
                    if (Parent != null)
                    {
                        Parent.VerLineY2 = 0;
                    }
                }
                catch
                {
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Double VerLineY1
        {
            get
            {

                return (FontSize * 1.333) / 2;
            }
            set
            {
                OnPropertyChanged("VerLineY2");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private TreeNodeViewModel GetPreviousNode()
        {
            TreeNodeViewModel previous = null;
            if (Parent == null)
            {
                foreach (var item in Root.Items.ToList<TreeNodeViewModel>())
                {
                    if (item.NameIdentity != this.NameIdentity)
                    {
                        previous = item;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            else if (Parent != null)
            {
                foreach (var item in Parent.Children.ToList<TreeNodeViewModel>())
                {
                    if (item.NameIdentity != this.NameIdentity)
                    {
                        previous = item;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return previous;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="certain"></param>
        /// <returns></returns>
        private TreeNodeViewModel GetCertainPreviousNode(TreeNodeViewModel certain)
        {
            TreeNodeViewModel previous = null;
            if (certain.Parent == null)
            {
                foreach (var item in Root.Items.ToList<TreeNodeViewModel>())
                {
                    if (item.NameIdentity != certain.NameIdentity)
                    {
                        previous = item;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (certain.Parent != null)
            {
                foreach (var item in Parent.Children.ToList<TreeNodeViewModel>())
                {
                    if (item.NameIdentity != certain.NameIdentity)
                    {
                        previous = item;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return previous;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tnvm"></param>
        /// <param name="totalspace"></param>
        /// <returns></returns>
        public double LoopGetSpace(TreeNodeViewModel tnvm, ref double totalspace)
        {
            foreach (var it in tnvm.Children.ToList<TreeNodeViewModel>())
            {
                totalspace += (it.Space + (it.FontSize * 1.333));
                if (it.IsExpanded)
                    LoopGetSpace(it, ref totalspace);
            }
            return totalspace;
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public double HorLineX1
        {
            get
            {
                return (-(10 + Margin));
            }
            set
            {
                OnPropertyChanged("HorLineX1");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Name")]
        [DefaultValue(null)]
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                if (mName != value)
                {
                    mName = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("NameIdentity")]
        public string NameIdentity
        {
            get
            {
                return mNameIdentity;
            }
            set
            {
                if (mNameIdentity != value && CheckTheIdenty(value))
                {
                    Root.NameRepository.Rename(mNameIdentity, value);
                    mNameIdentity = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Description")]
        [DefaultValue(null)]
        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                if (mDescription != value)
                {
                    mDescription = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ToolTipString")]
        [DefaultValue(null)]
        public string ToolTipString
        {
            get
            {
                return mToolTipString;
            }
            set
            {
                if (mToolTipString != value)
                {
                    mToolTipString = value;
                    OnPropertyChanged("ToolTipString");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [TypeConverter("ControlEase.Inspec.TreeView.BoolToVisibilityConverter,CE.NX.TV")]
        public bool ToolTipVisibility
        {
            get
            {
                if (string.IsNullOrEmpty(ToolTipString))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("Arial")]
        [DisplayName("FontFamilyString")]
        [Editor("ControlEase.Inspec.TreeView.FontFamilyEditor,CE.NX.TV", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.TreeView.FontFamilyConverter,CE.NX.TV")]
        public string FontFamilyString
        {
            get
            {
                return mFontFamilyString;
            }
            set
            {
                if (mFontFamilyString != value)
                {
                    mFontFamilyString = value;
                    OnPropertyChanged("FontFamilyString");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [DisplayName("ImageSource")]
        [Editor("ControlEase.Inspec.TreeView.ResourceImageEditor,CE.NX.TV", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.TreeView.ResourceImageTypeConvertor,CE.NX.TV")]
        public int ImageSource
        {
            get
            {
                return mImageSource;
            }
            set
            {
                mImageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [TypeConverter("ControlEase.Inspec.TreeView.BoolToVisibilityConverter,CE.NX.TV")]
        public bool ImageVisibility
        {
            get
            {
                if (ImageSource == 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private SecurityEntity mPermissionProxy;
        /// <summary>
        /// 权限配置代理
        /// </summary>
        [DefaultValue(null)]
        [Category("Security")]
        [DisplayName("Permissions")]
        [Description("Permissions_Description")]
        [Editor("ControlEase.Inspec.ViewPresentation.PermissionEditor,CE.NX.VWP", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PermissionConverter,CE.NX.VWP")]
        public SecurityEntity PermissionProxy
        {
            get
            {
                return mPermissionProxy;
            }
            set
            {
                if (value != mPermissionProxy)
                {
                    mPermissionProxy = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawPermissions"></param>
        /// <returns></returns>
        List<string> TransformArrayList(Tuple<string, List<string>> rawPermissions)
        {
            List<string> mL = new List<string>();
            rawPermissions.Item2.ForEach(it =>
            {
                mL.Add(rawPermissions.Item1 + ":" + it.ToString());
            });
            return mL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawPermissions"></param>
        /// <returns></returns>
        string TransformString(Tuple<string, List<string>> rawPermissions)
        {
            List<string> mL = new List<string>();
            rawPermissions.Item2.ForEach(it =>
            {
                mL.Add(rawPermissions.Item1 + ":" + it.ToString());
            });
            StringBuilder mstrbulider = new StringBuilder();
            mL.ForEach(it =>
                {
                    mstrbulider.Append(it + ";");
                });
            return mstrbulider.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public new IDictionary<object, object> Properties
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool IsChecked
        {
            get
            {
                return mIsChecked;
            }
            set
            {
                if (mIsChecked != value)
                {
                    mIsChecked = value;
                    OnPropertyChanged("IsChecked");
                    //这个勾选只能在运行环境下执行，不用判断是否是运行环境
                    if (mIsChecked)
                    {//如果被选中，引发选中事件
                        if (CheckNodePrograme != null)
                        {
                            if (ValidateSignature())
                                CheckNodePrograme.LinkCaculate();
                        }
                    }
                    else
                    {
                        if (UnCheckNodePrograme != null)
                        {
                            if (ValidateSignature())
                               UnCheckNodePrograme.LinkCaculate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [TypeConverter("ControlEase.Inspec.TreeView.BoolToVisibilityConverter,CE.NX.TV")]
        public bool CheckBoxVisibility
        {
            get
            {
                if (Root != null)
                    return Root.CheckBoxVisibility;
                else
                    return false;
            }
            set
            {
                OnPropertyChanged("CheckBoxVisibility");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public TreeNodeViewModel Parent
        {
            get { return mParent; }
            set
            {
                mParent = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public TreeViewControlViewModel Root
        {
            get { return mRoot; }
            set
            {
                mRoot = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        [DefaultValue(null)]
        [DisplayName("Nodes")]
        [Editor("ControlEase.Inspec.TreeView.CustomCollectionEditor,CE.NX.TV", typeof(UITypeEditor))]
        public ObservableCollection<TreeNodeViewModel> Children
        {
            get
            {
                return mChildren;
            }
            set
            {
                mChildren = value;
                OnPropertyChanged("Children");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                if (mIsSelected != value)
                {
                    mIsSelected = value;
                    if (mIsSelected)
                    {
                        if (Root != null)
                            Root.SelectItem = this;
                    }
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool IsExpanded
        {
            get
            {
                return mIsExpanded;
            }
            set
            {
                if (Root != null)
                {
                    //目前不验证控件本身的操作，只验证工程操作权限
                    //if (!Root.IsDesignMode && Children.Count > 0)
                    if (!Root.IsDesignMode)
                    {
                        //if (ValidateSignature())
                        //{
                        mIsExpanded = value;
                        OnPropertyChanged("IsExpanded");
                        TreeNodeViewModel next = null;
                        next = GetNextNode();
                        if (next != null)
                        {
                            next.VerLineY2 = 0;
                        }
                        if (Parent != null)
                        {
                            Parent.VerLineY2 = 0;
                        }
                    }
                    else
                    {
                        mIsExpanded = value;
                        OnPropertyChanged("IsExpanded");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private TreeNodeViewModel GetNextNode()
        {
            TreeNodeViewModel next = null;
            bool flag = false;
            if (Parent == null)
            {
                foreach (var it in Root.Items.ToList<TreeNodeViewModel>())
                {
                    if (flag)
                    {
                        next = it;
                        break;
                    }
                    if (it.NameIdentity == this.NameIdentity)
                    {
                        flag = true;
                    }
                }

            }
            else if (Parent != null)
            {
                foreach (var it in Parent.Children.ToList<TreeNodeViewModel>())
                {
                    if (flag)
                    {
                        next = it;
                        break;
                    }
                    if (it.NameIdentity == this.NameIdentity)
                    {
                        flag = true;
                    }
                }


            }
            return next;
        }

        #region 左键
        /// <summary>
        /// 单击程序
        /// </summary>
        [Displayable(true)]
        [Category("KeyOperation")]
        [DisplayName("LeftSingleClick")]
        [Description("SingleClick_Description")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.ViewPresentation.PropertyLinkScriptEditor,CE.NX.VWP", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public PropertyLinkEvent<bool> LeftMouseClick
        {
            get
            {
                return mLeftMouseClick;
            }
            set
            {
                if (mLeftMouseClick != value)
                {
                    if (value != null)
                    {
                        value.IsCodeOnly = true;
                    }
                    mLeftMouseClick = value;
                    OnPropertyChanged("LeftMouseClick");
                }
            }
        }

        /// <summary>
        /// 双击程序
        /// </summary>
        [Displayable(true)]
        [DisplayName("DoubleClick")]
        [Description("DoubleClick_Description")]
        [Category("KeyOperation")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.ViewPresentation.PropertyLinkScriptEditor,CE.NX.VWP", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public PropertyLinkEvent<bool> LeftMouseDoubleClick
        {
            get
            {
                return mLeftMouseDoubleClick;
            }
            set
            {
                if (mLeftMouseDoubleClick != value)
                {
                    if (value != null)
                    {
                        value.IsCodeOnly = true;
                    }
                    mLeftMouseDoubleClick = value;
                    OnPropertyChanged("LeftMouseDoubleClick");
                }
            }
        }

        /// <summary>
        /// 单击输入工程变量
        /// </summary>
        [Displayable(true)]
        [DisplayName("LeftSingleClickInput")]
        [Description("SingleClickInput_Description")]
        [Category("KeyOperation")]
        [DefaultValue(null)]
        //[ScriptProperty]
        [Editor("ControlEase.Inspec.TreeView.AniTypeInputProxyEditor,CE.NX.TV", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public IPropertyAnimate LeftMouseClickInput
        {
            get
            {
                return mLeftMouseClickInput;
            }
            set
            {
                if (mLeftMouseClickInput != value)
                {
                    mLeftMouseClickInput = value;
                    OnPropertyChanged("LeftMouseClickInput");
                }
            }
        }

        /// <summary>
        /// 双击输入工程变量
        /// </summary>
        [Displayable(true)]
        [DisplayName("DoubleClickInput")]
        [Description("DoubleClickInput_Description")]
        [Category("KeyOperation")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.TreeView.AniTypeInputProxyEditor,CE.NX.TV", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public IPropertyAnimate LeftMouseDoubleClickInput
        {
            get
            {
                return mLeftMouseDoubleClickInput;
            }
            set
            {
                if (mLeftMouseDoubleClickInput != value)
                {
                    mLeftMouseDoubleClickInput = value;
                    OnPropertyChanged("LeftMouseDoubleClickInput");
                }
            }
        }

        /// <summary>
        /// 单击菜单
        /// </summary>
        [Displayable(true)]
        [Category("KeyOperation")]
        [DisplayName("LeftSingleClickMenu")]
        [Description("SingleClickMenu_Description")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.TreeView.ViewMenuItemEditor,CE.NX.TV", typeof(UITypeEditor))]
        public string LeftMouseClickMenu
        {
            get
            {
                return mLeftMouseClickMenu;
            }
            set
            {
                if (mLeftMouseClickMenu != value)
                {
                    mLeftMouseClickMenu = value;
                }
            }
        }

        #endregion

        #region 右键

        /// <summary>
        /// 单击程序
        /// </summary>
        [Browsable(true)]
        [Displayable(true)]
        [DisplayName("RightSingleClick")]
        [Description("SingleClick_Description")]
        [Category("KeyOperation")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.ViewPresentation.PropertyLinkScriptEditor,CE.NX.VWP", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public PropertyLinkEvent<bool> RightMouseClick
        {
            get
            {
                return mRightMouseClick;
            }
            set
            {
                if (mRightMouseClick != value)
                {
                    if (value != null)
                    {
                        value.IsCodeOnly = true;
                    }
                    else
                    {

                    }
                    mRightMouseClick = value;
                    OnPropertyChanged("RightMouseClick");
                }
            }
        }

        /// <summary>
        /// 单击输入工程变量
        /// </summary>
        [Browsable(true)]
        [Category("KeyOperation")]
        [Displayable(true)]
        [DisplayName("RightSingleClickInput")]
        [Description("SingleClickInput_Description")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.TreeView.AniTypeInputProxyEditor,CE.NX.TV", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public IPropertyAnimate RightMouseClickInput
        {
            get
            {
                return mRightMouseClickInput;
            }
            set
            {
                if (mRightMouseClickInput != value)
                {
                    mRightMouseClickInput = value;
                    OnPropertyChanged("RightMouseClickInput");
                }
            }
        }

        /// <summary>
        /// 单击菜单
        /// </summary>
        [Browsable(true)]
        [Displayable(true)]
        [Category("KeyOperation")]
        [DisplayName("RightSingleClickMenu")]
        [Description("SingleClickMenu_Description")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.TreeView.ViewMenuItemEditor,CE.NX.TV", typeof(UITypeEditor))]
        public string RightMouseClickMenu
        {
            get
            {
                return mRightMouseClickMenu;
            }
            set
            {
                if (mRightMouseClickMenu != value)
                {
                    mRightMouseClickMenu = value;
                    //OnPropertyChanged ( "RightMouseClickMenu" );
                }
            }
        }
        #endregion

        #region 勾选，反勾选节点事件
        //[Displayable ( true )]
        [DisplayName("CheckNodePrograme")]
        [Description("CheckNodePrograme_Description")]
        [Category("KeyOperation")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.ViewPresentation.PropertyLinkScriptEditor,CE.NX.VWP", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public PropertyLinkEvent<bool> CheckNodePrograme
        {
            get
            {
                return mCheckNodePrograme;
            }
            set
            {
                if (mCheckNodePrograme != value)
                {
                    if (value != null)
                    {
                        value.IsCodeOnly = true;
                    }
                    mCheckNodePrograme = value;
                    OnPropertyChanged("CheckNodePrograme");
                }
            }
        }

        [DisplayName("UnCheckNodePrograme")]
        [Description("UnCheckNodePrograme_Description")]
        [Category("KeyOperation")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.ViewPresentation.PropertyLinkScriptEditor,CE.NX.VWP", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public PropertyLinkEvent<bool> UnCheckNodePrograme
        {
            get
            {
                return mUnCheckNodePrograme;
            }
            set
            {
                if (mUnCheckNodePrograme != value)
                {
                    if (value != null)
                    {
                        value.IsCodeOnly = true;
                    }
                    mUnCheckNodePrograme = value;
                    OnPropertyChanged("UnCheckNodePrograme");
                }
            }
        }
        #endregion
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return SR.GetString("TreeNode");
        }

        /// <summary>
        /// 运行时需要验证在Set上
        /// </summary>
        /// <returns></returns>
        public bool ValidateSignature()
        {
            bool result = true;

            if (PermissionProxy != null)
            {
                result = ValidateSignature(PermissionProxy);
                //IsEnabled = result;
                return result;
            }
            else
                //IsEnabled = result;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="permissions"></param>
        /// <param name="pvType">校验模式</param>
        /// <returns></returns>
        public bool ValidateSignature(SecurityEntity se)
        {
            ISecurityService service = ServiceLocator.Current.Resolve<ISecurityService>();
            if (service != null)
            {
                if (!service.SignValidate(se))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ll"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> ToDictionary(List<string> ll)
        {
            string skey = "_InnerEmpty";
            Dictionary<string, List<string>> dd = new Dictionary<string, List<string>>();
            foreach (var vv in ll)
            {
                string[] stmp = vv.Split(new char[] { ':' });
                if (stmp.Length == 1)
                {
                    if (dd.ContainsKey(skey))
                    {
                        dd[skey].Add(vv);
                    }
                    else
                    {
                        dd.Add(skey, new List<string>() { vv });
                    }
                    //dd.Add ( skey, vv );
                }
                else
                {
                    if (!dd.ContainsKey(stmp[0]))
                    {
                        dd.Add(stmp[0], new List<string>() { stmp[1] });
                    }
                    else
                    {
                        dd[stmp[0]].Add(stmp[1]);
                    }
                }
            }
            return dd;
        }

        /// <summary>
        /// Executes the remove.
        /// </summary>
        public void Remove(TreeNodeViewModel tn)
        {
            if (mChildren.Contains(tn))
            {
                mChildren.Remove(tn);
                OnPropertyChanged("Children");
            }
        }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        public void Remove()
        {
          
        }

        /// <summary>
        /// Checks the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private bool CheckTheIdenty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            if (!ControlEase.Inspec.Extension.NameService.IsValidName(name))
            {
                return false;
            }
            return Root.NameRepository.Register(name);
        }

        /// <returns></returns>
        public System.Drawing.Image LoadFromResource(string resourceName, SerializeContext context, out string rName)
        {
            return ImageCachManager.Manager.GetBitmapWithoutCach(resourceName, true, out rName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToElement()
        {
            XElement element = new XElement("Node", new XAttribute("Name", string.IsNullOrEmpty(Name) ? string.Empty : Name)
                                                , new XAttribute("NameIdentity", string.IsNullOrEmpty(NameIdentity) ? string.Empty : NameIdentity)
                                                , new XAttribute("Description", string.IsNullOrEmpty(Description) ? string.Empty : Description)
                                                , new XAttribute("ToolTipString", string.IsNullOrEmpty(ToolTipString) ? string.Empty : ToolTipString)
                                                , new XAttribute("ImageSource", ImageSource)
                                                , new XAttribute("FontSize", FontSize)
                                                , new XAttribute("FontFamilyString", FontFamilyString)
                                                , new XAttribute("CheckBoxVisibility", CheckBoxVisibility)
                                                , new XAttribute("NodeSpace", NodeSpace)
                                                , new XAttribute("Margin", Margin)
                                                );

            XElement Bgelement = new XElement("Background", new XAttribute("A", mBackground.A)
                                                , new XAttribute("R", mBackground.R)
                                                , new XAttribute("G", mBackground.G)
                                                , new XAttribute("B", mBackground.B)
                                                );
            element.Add(Bgelement);
            XElement LostFocusBgelement = new XElement("LostFocusBackground", new XAttribute("A", mLostFocusBackground.A)
                                                , new XAttribute("R", mLostFocusBackground.R)
                                                , new XAttribute("G", mLostFocusBackground.G)
                                                , new XAttribute("B", mLostFocusBackground.B)
                                                );
            element.Add(LostFocusBgelement);
            XElement Fgelement = new XElement("Foreground", new XAttribute("A", mForeground.A)
                                                , new XAttribute("R", mForeground.R)
                                                , new XAttribute("G", mForeground.G)
                                                , new XAttribute("B", mForeground.B)
                                                );
            element.Add(Fgelement);


            XElement SelBgelement = new XElement("SelectedBackground", new XAttribute("A", mSelectedBackground.A)
                                                , new XAttribute("R", mSelectedBackground.R)
                                                , new XAttribute("G", mSelectedBackground.G)
                                                , new XAttribute("B", mSelectedBackground.B)
                                                );
            element.Add(SelBgelement);
            XElement SelFgelement = new XElement("SelectedForeground", new XAttribute("A", mSelectedForeground.A)
                                                , new XAttribute("R", mSelectedForeground.R)
                                                , new XAttribute("G", mSelectedForeground.G)
                                                , new XAttribute("B", mSelectedForeground.B)
                                                );
            element.Add(SelFgelement);
            #region PermissionProxy
            if (PermissionProxy != null)
            {
                XElement pms = PermissionProxy.ToElement();
                element.Add(pms);
            }
            #endregion

            #region ClickEvent
            //选中节点事件
            if (CheckNodePrograme != null)
            {
                XElement cnp = new XElement("CheckNodePrograme");
                this.CheckNodePrograme.SaveToXElement<Boolean>(cnp);
                element.Add(cnp);
            }
            if (UnCheckNodePrograme != null)
            {
                XElement ucnp = new XElement("UnCheckNodePrograme");
                this.UnCheckNodePrograme.SaveToXElement<Boolean>(ucnp);
                element.Add(ucnp);
            }

            if (LeftMouseClick != null)
            {
                XElement xe1 = new XElement("LeftMouseClick");
                this.LeftMouseClick.SaveToXElement<Boolean>(xe1);
                element.Add(xe1);
            }

            if (RightMouseClick != null)
            {
                XElement xe = new XElement("RightMouseClick");
                this.RightMouseClick.SaveToXElement<Boolean>(xe);
                element.Add(xe);
            }

            if (LeftMouseDoubleClick != null)
            {
                XElement xe = new XElement("LeftMouseDoubleClick");
                this.LeftMouseDoubleClick.SaveToXElement<Boolean>(xe);
                element.Add(xe);
            }

            if (LeftMouseClickInput != null)
            {
                XElement xe3 = new XElement("LeftMouseClickInput");
                if (this.LeftMouseClickInput is AniInputAnalog)
                    (this.LeftMouseClickInput as AniInputAnalog).SaveToXElement(xe3);
                else if (this.LeftMouseClickInput is AniInputString)
                    (this.LeftMouseClickInput as AniInputString).SaveToXElement(xe3);
                else if (this.LeftMouseClickInput is AniInputDateTime)
                    (this.LeftMouseClickInput as AniInputDateTime).SaveToXElement(xe3);
                else if (this.LeftMouseClickInput is AniInputDisc)
                    (this.LeftMouseClickInput as AniInputDisc).SaveToXElement(xe3);
                element.Add(xe3);
            }

            if (RightMouseClickInput != null)
            {
                XElement xe3 = new XElement("RightMouseClickInput");
                if (this.RightMouseClickInput is AniInputAnalog)
                    (this.RightMouseClickInput as AniInputAnalog).SaveToXElement(xe3);
                else if (this.RightMouseClickInput is AniInputString)
                    (this.RightMouseClickInput as AniInputString).SaveToXElement(xe3);
                else if (this.RightMouseClickInput is AniInputDateTime)
                    (this.RightMouseClickInput as AniInputDateTime).SaveToXElement(xe3);
                else if (this.RightMouseClickInput is AniInputDisc)
                    (this.RightMouseClickInput as AniInputDisc).SaveToXElement(xe3);
                element.Add(xe3);
            }

            if (LeftMouseDoubleClickInput != null)
            {
                XElement xe3 = new XElement("LeftMouseDoubleClickInput");
                if (this.LeftMouseDoubleClickInput is AniInputAnalog)
                    (this.LeftMouseDoubleClickInput as AniInputAnalog).SaveToXElement(xe3);
                else if (this.LeftMouseDoubleClickInput is AniInputString)
                    (this.LeftMouseDoubleClickInput as AniInputString).SaveToXElement(xe3);
                else if (this.LeftMouseDoubleClickInput is AniInputDateTime)
                    (this.LeftMouseDoubleClickInput as AniInputDateTime).SaveToXElement(xe3);
                else if (this.LeftMouseDoubleClickInput is AniInputDisc)
                    (this.LeftMouseDoubleClickInput as AniInputDisc).SaveToXElement(xe3);
                element.Add(xe3);
            }

            if (!string.IsNullOrEmpty(LeftMouseClickMenu))
            {
                XElement xe3 = new XElement("LeftMouseClickMenu",
                                new XAttribute("MenuName", string.IsNullOrEmpty(LeftMouseClickMenu) ? string.Empty : LeftMouseClickMenu));
                element.Add(xe3);
            }

            if (!string.IsNullOrEmpty(RightMouseClickMenu))
            {
                XElement xe3 = new XElement("RightMouseClickMenu",
                     new XAttribute("MenuName", string.IsNullOrEmpty(RightMouseClickMenu) ? string.Empty : RightMouseClickMenu));
                element.Add(xe3);
            }

            #endregion

            XElement xe2 = new XElement("Children");
            if (Children != null)
            {
                if (Children.Count > 0)
                {
                    foreach (var item in Children.ToList<TreeNodeViewModel>())
                    {
                        xe2.Add(item.ToElement());
                    }
                    element.Add(xe2);
                }
            }
            return element;
        }

        /// <summary>
        /// Loads from.
        /// </summary>
        /// <param name="element">The element.</param>
        public void LoadFrom(XElement element)
        {
            if (Root != null)
            {
                if (!Root.IsDesignMode)
                {
                    mIsExpanded = false;
                }
            }
            if (element.Element("Background") != null)
            {
                byte ba = Convert.ToByte(element.Element("Background").Attribute("A").Value);
                byte br = Convert.ToByte(element.Element("Background").Attribute("R").Value);
                byte bg = Convert.ToByte(element.Element("Background").Attribute("G").Value);
                byte bb = Convert.ToByte(element.Element("Background").Attribute("B").Value);
                mBackground = System.Drawing.Color.FromArgb(ba, br, bg, bb);
            }
            if (element.Element("LostFocusBackground") != null)
            {
                byte ba = Convert.ToByte(element.Element("LostFocusBackground").Attribute("A").Value);
                byte br = Convert.ToByte(element.Element("LostFocusBackground").Attribute("R").Value);
                byte bg = Convert.ToByte(element.Element("LostFocusBackground").Attribute("G").Value);
                byte bb = Convert.ToByte(element.Element("LostFocusBackground").Attribute("B").Value);
                mLostFocusBackground = System.Drawing.Color.FromArgb(ba, br, bg, bb);
            }
            if (element.Element("Foreground") != null)
            {
                byte ba = Convert.ToByte(element.Element("Foreground").Attribute("A").Value);
                byte br = Convert.ToByte(element.Element("Foreground").Attribute("R").Value);
                byte bg = Convert.ToByte(element.Element("Foreground").Attribute("G").Value);
                byte bb = Convert.ToByte(element.Element("Foreground").Attribute("B").Value);
                mForeground = System.Drawing.Color.FromArgb(ba, br, bg, bb);
            }

            if (element.Element("SelectedBackground") != null)
            {
                byte ba = Convert.ToByte(element.Element("SelectedBackground").Attribute("A").Value);
                byte br = Convert.ToByte(element.Element("SelectedBackground").Attribute("R").Value);
                byte bg = Convert.ToByte(element.Element("SelectedBackground").Attribute("G").Value);
                byte bb = Convert.ToByte(element.Element("SelectedBackground").Attribute("B").Value);
                mSelectedBackground = System.Drawing.Color.FromArgb(ba, br, bg, bb);
            }

            if (element.Element("SelectedForeground") != null)
            {
                byte ba = Convert.ToByte(element.Element("SelectedForeground").Attribute("A").Value);
                byte br = Convert.ToByte(element.Element("SelectedForeground").Attribute("R").Value);
                byte bg = Convert.ToByte(element.Element("SelectedForeground").Attribute("G").Value);
                byte bb = Convert.ToByte(element.Element("SelectedForeground").Attribute("B").Value);
                mSelectedForeground = System.Drawing.Color.FromArgb(ba, br, bg, bb);
            }

            NodeSpace = Convert.ToInt32(element.Attribute("NodeSpace").Value);
            Margin = Convert.ToInt32(element.Attribute("Margin").Value);
            FontFamilyString = element.Attribute("FontFamilyString").Value;
            Name = element.Attribute("Name").Value;
            NameIdentity = element.Attribute("NameIdentity").Value;
            Description = element.Attribute("Description").Value;
            ToolTipString = element.Attribute("ToolTipString").Value;
            ImageSource = Convert.ToInt32(element.Attribute("ImageSource").Value);

            //string stmp = string.Empty;
            //ForeImageResName = element.Attribute ( "ImageSource" ).Value;
            //SetImageSource ( LoadFromResource ( ForeImageResName, null, out stmp ) );
            //if ( stmp != ForeImageResName )
            //{
            //    ForeImageResName = stmp;
            //}
            CheckBoxVisibility = Convert.ToBoolean(element.Attribute("CheckBoxVisibility").Value);
            FontSize = Convert.ToInt32(element.Attribute("FontSize").Value);
            //V1版本
            if (element.Element("Permissions") != null)
            {
                string[] mstr = element.Element("Permissions").Attribute("permissionString").Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                PermissionProxy = new SecurityEntity();
                PermissionProxy.PermissionValidateType = (PermissionValidateType)Convert.ToInt32(element.Element("Permissions").Attribute("PermissionOperationMode").Value);
                List<string> temp = new List<string>();
                temp.AddRange(mstr);
                Dictionary<string, List<string>> mdic = ToDictionary(temp);
                if (mdic.Count > 0)
                {
                    foreach (var it in mdic)
                    {
                        //PermissionProxy.SecurityLevel = 1;
                        //PermissionProxy.OperateProcedureEntity = new OperateProcedureEntity ( ) { Name =c, Permissions = it.Value };
                        PermissionProxy.ValidateLevel = ValidateLevels.NormalValidate;
                        PermissionProxy.ValidateServerName = it.Key;
                        var result = PermissionProxy.NewOperateEntity();
                        result.Permissions = it.Value;
                        break;
                    }
                }
            }
            //V2版本
            if (element.Element("SecurityEntity") != null)
            {
                PermissionProxy = element.Element("SecurityEntity").ToSecurityEntity();
            }

            #region ClickEvent

            if (element.Element("CheckNodePrograme") != null)
                this.CheckNodePrograme = element.Element("CheckNodePrograme").LoadPropertyEventFromXElement<bool>();
            if (element.Element("UnCheckNodePrograme") != null)
                this.UnCheckNodePrograme = element.Element("UnCheckNodePrograme").LoadPropertyEventFromXElement<bool>();
            if (element.Element("LeftMouseClick") != null)
                this.LeftMouseClick = element.Element("LeftMouseClick").LoadPropertyEventFromXElement<bool>();
            if (element.Element("RightMouseClick") != null)
                this.RightMouseClick = element.Element("RightMouseClick").LoadPropertyEventFromXElement<bool>();
            if (element.Element("LeftMouseDoubleClick") != null)
                this.LeftMouseDoubleClick = element.Element("LeftMouseDoubleClick").LoadPropertyEventFromXElement<bool>();
            if (element.Element("LeftMouseClickInput") != null)
            {
                XElement mele = element.Element("LeftMouseClickInput").Element("AniInputAnalog");
                if (mele != null)
                {
                    AniInputAnalog mAniInputAnalog = new AniInputAnalog();
                    mAniInputAnalog.LoadFromXElement(mele);
                    this.LeftMouseClickInput = mAniInputAnalog;
                }

                XElement mele1 = element.Element("LeftMouseClickInput").Element("AniInputDisc");
                if (mele1 != null)
                {
                    AniInputDisc mAniInputDisc = new AniInputDisc();
                    mAniInputDisc.LoadFromXElement(mele1);
                    this.LeftMouseClickInput = mAniInputDisc;
                }

                XElement mele2 = element.Element("LeftMouseClickInput").Element("AniInputDateTime");
                if (mele2 != null)
                {
                    AniInputDateTime mAniInputDateTime = new AniInputDateTime();
                    mAniInputDateTime.LoadFromXElement(mele2);
                    this.LeftMouseClickInput = mAniInputDateTime;
                }

                XElement mele3 = element.Element("LeftMouseClickInput").Element("AniInputString");
                if (mele3 != null)
                {
                    AniInputString mAniInputString = new AniInputString();
                    mAniInputString.LoadFromXElement(mele3);
                    this.LeftMouseClickInput = mAniInputString;
                }
            }
            if (element.Element("LeftMouseDoubleClickInput") != null)
            {
                XElement mele = element.Element("LeftMouseDoubleClickInput").Element("AniInputAnalog");
                if (mele != null)
                {
                    AniInputAnalog mAniInputAnalog = new AniInputAnalog();
                    mAniInputAnalog.LoadFromXElement(mele);
                    this.LeftMouseDoubleClickInput = mAniInputAnalog;
                }

                XElement mele1 = element.Element("LeftMouseDoubleClickInput").Element("AniInputDisc");
                if (mele1 != null)
                {
                    AniInputDisc mAniInputDisc = new AniInputDisc();
                    mAniInputDisc.LoadFromXElement(mele1);
                    this.LeftMouseDoubleClickInput = mAniInputDisc;
                }

                XElement mele2 = element.Element("LeftMouseDoubleClickInput").Element("AniInputDateTime");
                if (mele2 != null)
                {
                    AniInputDateTime mAniInputDateTime = new AniInputDateTime();
                    mAniInputDateTime.LoadFromXElement(mele2);
                    this.LeftMouseDoubleClickInput = mAniInputDateTime;
                }

                XElement mele3 = element.Element("LeftMouseDoubleClickInput").Element("AniInputString");
                if (mele3 != null)
                {
                    AniInputString mAniInputString = new AniInputString();
                    mAniInputString.LoadFromXElement(mele3);
                    this.LeftMouseDoubleClickInput = mAniInputString;
                }
            }

            if (element.Element("RightMouseClickInput") != null)
            {
                XElement mele = element.Element("RightMouseClickInput").Element("AniInputAnalog");
                if (mele != null)
                {
                    AniInputAnalog mAniInputAnalog = new AniInputAnalog();
                    mAniInputAnalog.LoadFromXElement(mele);
                    this.RightMouseClickInput = mAniInputAnalog;
                }

                XElement mele1 = element.Element("RightMouseClickInput").Element("AniInputDisc");
                if (mele1 != null)
                {
                    AniInputDisc mAniInputDisc = new AniInputDisc();
                    mAniInputDisc.LoadFromXElement(mele1);
                    this.RightMouseClickInput = mAniInputDisc;
                }

                XElement mele2 = element.Element("RightMouseClickInput").Element("AniInputDateTime");
                if (mele2 != null)
                {
                    AniInputDateTime mAniInputDateTime = new AniInputDateTime();
                    mAniInputDateTime.LoadFromXElement(mele2);
                    this.RightMouseClickInput = mAniInputDateTime;
                }

                XElement mele3 = element.Element("RightMouseClickInput").Element("AniInputString");
                if (mele3 != null)
                {
                    AniInputString mAniInputString = new AniInputString();
                    mAniInputString.LoadFromXElement(mele3);
                    this.RightMouseClickInput = mAniInputString;
                }
            }
            if (element.Element("LeftMouseClickMenu") != null)
            {
                LeftMouseClickMenu = element.Element("LeftMouseClickMenu").Attribute("MenuName").Value;
            }
            if (element.Element("RightMouseClickMenu") != null)
            {
                RightMouseClickMenu = element.Element("RightMouseClickMenu").Attribute("MenuName").Value;
            }

            #endregion

            XElement ChildrenXElement = element.Element("Children");
            if (ChildrenXElement != null)
            {
                int number = ChildrenXElement.Elements("Node").Count();
                if (number > 0)
                {
                    foreach (var item in ChildrenXElement.Elements("Node"))
                    {
                        TreeNodeViewModel mTreeNodeViewModel = new TreeNodeViewModel(this.Root, this);
                        mTreeNodeViewModel.LoadFrom(item);
                        Children.Add(mTreeNodeViewModel);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the image resource.
        /// </summary>
        public void UpdateImageResource()
        {
            this.ImageSource = this.mImageSource;
        }
        #endregion ...Methods...

        #region ... Interfaces ...
        public bool IsPropertyReadOnly(PropertyDescriptor pd)
        {
            //左键三选一属性互斥
            if (pd.Name == "LeftMouseClickInput")
            {
                return (this.LeftMouseClick != null || (!string.IsNullOrEmpty(this.LeftMouseClickMenu)));
            }
            else if (pd.Name == "LeftMouseClick")
            {
                return (this.LeftMouseClickInput != null || (!string.IsNullOrEmpty(this.LeftMouseClickMenu)));
            }
            else if (pd.Name == "LeftMouseClickMenu")
            {
                return (this.LeftMouseClickInput != null || this.LeftMouseClick != null);
            }
            //右键三选一属性互斥
            else if (pd.Name == "RightMouseClickInput")
            {
                return (this.RightMouseClick != null || (!string.IsNullOrEmpty(this.RightMouseClickMenu)));
            }
            else if (pd.Name == "RightMouseClick")
            {
                return (this.RightMouseClickInput != null || (!string.IsNullOrEmpty(this.RightMouseClickMenu)));
            }
            else if (pd.Name == "RightMouseClickMenu")
            {
                return (this.RightMouseClickInput != null || this.RightMouseClick != null);
            }
            //双击属性互斥
            else if (pd.Name == "LeftMouseDoubleClick")
            {
                return (this.LeftMouseDoubleClickInput != null);
            }
            else if (pd.Name == "LeftMouseDoubleClickInput")
            {
                return (this.LeftMouseDoubleClick != null);
            }
            return false;
        }
        #endregion ...Interfaces...
    }
    internal class GlobalizedTreeNodeViewModelDescriptor : GlobalizedPropertyDescriptor
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        protected PropertyDescriptor basePropertyDescriptor;
        /// <summary>
        /// 
        /// </summary>
        private object _obj;
        /// <summary>
        /// 
        /// </summary>
        protected Func<object, bool> mIsReadOnlyHandler;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            return base.GetChildProperties(instance, filter);
        }
        /// <summary>
        /// 构造函数，根据原PropertyDescriptor构建
        /// </summary>
        /// <param name="basePropertyDescriptor">原PropertyDescriptor</param>
        public GlobalizedTreeNodeViewModelDescriptor(PropertyDescriptor basePropertyDescriptor)
            : base(basePropertyDescriptor)
        {
            this.basePropertyDescriptor = basePropertyDescriptor;
        }
        /// <summary>
        /// 根据原PropertyDescriptor和对象实例构建
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="basePropertyDescriptor">原PropertyDescriptor</param>
        public GlobalizedTreeNodeViewModelDescriptor(object obj, PropertyDescriptor basePropertyDescriptor)
            : this(basePropertyDescriptor)
        {
            this._obj = obj;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// Gets the description of the member, as specified in the <see cref="T:System.ComponentModel.DescriptionAttribute"/>.
        /// </summary>
        /// <returns>The description of the member. If there is no <see cref="T:System.ComponentModel.DescriptionAttribute"/>, the property value is set to the default, which is an empty string ("").</returns>

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        /// <returns>A <see cref="T:System.Type"/> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"/> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"/> methods are invoked, the object specified might be an instance of this type.</returns>
        public override Type ComponentType
        {
            get { return basePropertyDescriptor.ComponentType; }
        }
        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <returns>true if the property is read-only; otherwise, false.</returns>
        public override bool IsReadOnly
        {
            get
            {
                if (mIsReadOnlyHandler != null && _obj != null)
                {
                    return mIsReadOnlyHandler(_obj);
                }
                return this.basePropertyDescriptor.IsReadOnly;
            }
        }
        /// <summary>
        /// Gets or sets.
        /// </summary>
        /// <value>
        /// The is read only handler.
        /// </value>
        public override Func<object, bool> IsReadOnlyHandler
        {
            get { return mIsReadOnlyHandler; }
            set { mIsReadOnlyHandler = value; }
        }
        /// <summary>
        /// 当在派生类中被重写时，获取该属性的类型。
        /// </summary>
        public override Type PropertyType
        {
            get { return this.basePropertyDescriptor.PropertyType; }
        }
        /// <summary>
        /// 获取此成员的名称。
        /// </summary>
        public override string Name
        {
            get { return this.basePropertyDescriptor.Name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string Category
        {
            get
            {
                return SR.GetString(basePropertyDescriptor.Category);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return SR.GetString(basePropertyDescriptor.DisplayName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string Description
        {
            get
            {
                return basePropertyDescriptor.Description;
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 当在派生类中被重写时，返回重置对象时是否更改其值。
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override bool CanResetValue(object component)
        {
            return basePropertyDescriptor.CanResetValue(this._obj);
        }
        /// <summary>
        /// 当在派生类中被重写时，将组件的此属性的值重置为默认值。
        /// </summary>
        /// <param name="component"></param>
        public override void ResetValue(object component)
        {
            this.basePropertyDescriptor.ResetValue(this._obj);
            //TypeDescriptor.Refresh ( component );
        }
        /// <summary>
        /// 当在派生类中被重写时，获取组件上的属性的当前值。
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override object GetValue(object component)
        {
            return this.basePropertyDescriptor.GetValue(this._obj);
        }
        /// <summary>
        /// 当在派生类中被重写时，将组件的值设置为一个不同的值。
        /// </summary>
        /// <param name="component"></param>
        /// <param name="value"></param>
        public override void SetValue(object component, object value)
        {
            this.basePropertyDescriptor.SetValue(this._obj, value);
            TypeDescriptor.Refresh(component);
        }
        /// <summary>
        /// 当在派生类中被重写时，确定一个值，该值指示是否需要永久保存此属性的值。
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override bool ShouldSerializeValue(object component)
        {
            return this.basePropertyDescriptor.ShouldSerializeValue(this._obj);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class DelegateTreeNodeViewModel : ICustomTypeDescriptor
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private TreeNodeViewModel mSelectObject;
        /// <summary>
        /// 
        /// </summary>
        private PropertyDescriptorCollection mNewProps = new PropertyDescriptorCollection(null);
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentPropertyDescriptor"/> class.
        /// </summary>
        public DelegateTreeNodeViewModel()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public DelegateTreeNodeViewModel(TreeNodeViewModel obj)
        {
            mSelectObject = obj;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// Gets or sets the select object.
        /// </summary>
        /// <value>
        /// The select object.
        /// </value>
        public TreeNodeViewModel SelectObject
        {
            get { return mSelectObject; }
            set
            {
                mSelectObject = value;
            }
        }
        /// <summary>
        /// Gets the property descriptors.
        /// </summary>
        [Browsable(false)]
        public PropertyDescriptorCollection PropertyDescriptors
        {
            get { return mNewProps; }
        }
        #endregion ...Properties...

        #region ... Methods    ...
        public override string ToString()
        {
            return SR.GetString("TreeNode");
        }

        /// <summary>
        /// 返回类转换器
        /// </summary>
        /// <returns></returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <returns></returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        /// <summary>
        /// 返回组件名称
        /// </summary>
        /// <returns></returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// 返回属性拥有者
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public virtual object GetPropertyOwner(PropertyDescriptor pd)
        {
            return mSelectObject;
        }

        /// <summary>
        /// 返回特性
        /// </summary>
        /// <returns></returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// 返回属性，根据 ProvidePropertyAttribute 过滤属性
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the filtered properties for this component instance.
        /// </returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            //还没看明白
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(mSelectObject, null, true);
            mNewProps.Clear();
            Type tp = mSelectObject.GetType();
            foreach (PropertyDescriptor pd in props)
            {
                GlobalizedTreeNodeViewModelDescriptor newpd = new GlobalizedTreeNodeViewModelDescriptor(mSelectObject, pd);
                if (mSelectObject is IPropertyConflict)
                {
                    IPropertyConflict conflict = mSelectObject as IPropertyConflict;
                    newpd.IsReadOnlyHandler = new Func<object, bool>(
                        (obj) =>
                        {
                            return conflict.IsPropertyReadOnly(newpd);
                        });
                }
                mNewProps.Add(newpd);
            }
            return mNewProps;
        }

        /// <summary>
        /// 返回属性
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties for this component instance.
        /// </returns>
        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        /// <summary>
        /// 返回编辑控件
        /// </summary>
        /// <param name="editorBaseType"></param>
        /// <returns></returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyDescriptor GetDefaultProperty()
        {
            PropertyDescriptorCollection props = mNewProps;
            if (props.Count > 0)
                return (props[0]);
            else
                return (null);
        }

        /// <summary>
        /// 返回默认事件
        /// </summary>
        /// <returns></returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// 返回类名
        /// </summary>
        /// <returns></returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
