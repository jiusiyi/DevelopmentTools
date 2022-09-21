using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.Nexus.Presentation;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections;
using ControlEase.AI.View;
using System.ComponentModel;
using System.Xml.Linq;
using ControlEase.Inspec.Extension;
using ControlEase.Nexus.ComponentModel;
using System.Drawing.Design;
using ControlEase.Nexus;
using ControlEase.Inspec.ViewCore;
using ControlEase.AI.Script;
using ControlEase.Inspec.View;
using System.Drawing;
using ControlEase.Inspec.Animates;
using System.Diagnostics;
using System.IO;
using ControlEase.Nexus.Windows;
using ControlEase.AI.Security;
using ControlEase.Inspec.Resources;


namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// This is 
    /// </summary>
    [Icon("pack://application:,,,/CE.NX.TV;component/Resources/{0}_TreeView.ico")]
    public class TreeViewControlViewModel : ViewModel, ICustomSerializable, IPropertyAnimateCompatible, IViewShape, IPropertyConflict, IPluginPermission, IResourceLinks, IRuntimeResourceChange
    //, IToolbarMenuCommand
    {
        #region ... Variables  ...
        /// <summary>
        /// The items
        /// </summary>
        private ObservableCollection<TreeNodeViewModel> mItems;
        /// <summary>
        /// The select item
        /// </summary>
        public TreeNodeViewModel mSelectItem;
        /// <summary>
        /// 
        /// </summary>
        private bool mLineVisibility;
        /// <summary>
        /// 
        /// </summary>
        private bool mExpandIconVisibility = true;
        /// <summary>
        /// 
        /// </summary>
        private bool mCheckBoxVisibility = false;
        /// <summary>
        /// 
        /// </summary>
        private bool mLeftToRightLayout = true;
        /// <summary>
        /// 
        /// </summary>
        private int mNodeSpace = 0;
        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<CommandViewModel> mCommands;
        /// <summary>
        /// 
        /// </summary>
        private ICommand mExpandAllCommand;
        /// <summary>
        /// 
        /// </summary>
        private ICommand mCollapseAllCommand;
        /// <summary>
        /// 
        /// </summary>
        private ICommand mCheckAllCommand;
        /// <summary>
        /// 
        /// </summary>
        private ICommand mUnCheckAllCommand;
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
        /// The m node selected event
        /// </summary>
        PropertyLinkEvent<bool> mNodeSelectedEvent;
        /// <summary>
        /// 
        /// </summary>
        PropertyLinkEvent<bool> mLeftMouseDoubleClick;
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
        /// 菜单是否显示
        /// </summary>
        bool mMenuVisible = true;
        /// <summary>
        /// 默认为透明
        /// </summary>
        protected System.Drawing.Color mBackground = System.Drawing.Color.White;
        /// <summary>
        /// The file name
        /// </summary>
        private string dataChangeFileSuffix = string.Empty;
        /// <summary>权限 </summary>
        private SecurityEntity mPermissionProxy;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeManagerViewModel"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <summary>
        /// 
        /// </summary>
        public TreeViewControlViewModel()
        {
            IResourceProviderRegistry registry = ServiceLocator.Current.Resolve<IResourceProviderRegistry>();
            if (registry != null)
            {
                registry.RegisterProvider(SR.Provider);
            }
            mItems = new ObservableCollection<TreeNodeViewModel>();
            NameRepository = new TreeNodeNameRepository();
            //InitializeCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitializeCommand()
        {
            mCommands = new ObservableCollection<CommandViewModel>();
            Commands.Add(new CommandViewModel(SR.GetString("ExpandAll"), ExpandAllCommand));
            Commands.Add(new CommandViewModel(SR.GetString("CollapseAll"), CollapseAllCommand));
            if (CheckBoxVisibility)
            {
                Commands.Add(new CommandViewModel(SR.GetString("CheckAll"), CheckAllCommand));
                Commands.Add(new CommandViewModel(SR.GetString("UnCheckAll"), UnCheckAllCommand));
            }
        }

        #endregion ...Constructor...

        #region ... Properties ...
        [Displayable(true)]
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
        [Displayable(true)]
        [DefaultValue(true)]
        [DisplayName("MenuVisibility")]
        [Description("MenuVisibility_Description")]
        [TypeConverter("ControlEase.Inspec.TreeView.BoolenToVisibilityConverter,CE.NX.TV")]
        public bool MenuVisibility
        {
            get { return mMenuVisible; }
            set
            {
                if (mMenuVisible != value)
                {
                    mMenuVisible = value;
                    OnPropertyChanged("MenuVisibility");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ExpandAllCommand
        {
            get
            {
                if (mExpandAllCommand == null)
                {
                    mExpandAllCommand = new RelayCommand(() =>
                    {
                        ExecuteExpandAll();
                    });
                }
                return mExpandAllCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CollapseAllCommand
        {
            get
            {
                if (mCollapseAllCommand == null)
                {
                    mCollapseAllCommand = new RelayCommand(() =>
                    {
                        ExecuteCollapseAll();
                    });
                }
                return mCollapseAllCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CheckAllCommand
        {
            get
            {
                if (mCheckAllCommand == null)
                {
                    mCheckAllCommand = new RelayCommand(() =>
                    {
                        ExecuteCheckAll();
                    });
                }
                return mCheckAllCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand UnCheckAllCommand
        {
            get
            {
                if (mUnCheckAllCommand == null)
                {
                    mUnCheckAllCommand = new RelayCommand(() =>
                    {
                        ExecuteUnCheckAll();
                    });
                }
                return mUnCheckAllCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteCheckAll()
        {
            if (!ValidateSignature()) return;
            CheckedAll(true);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteUnCheckAll()
        {
            if (!ValidateSignature()) return;
            CheckedAll(false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteExpandAll()
        {
            if (!ValidateSignature()) return;
            ExpandAll(true);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteCollapseAll()
        {
            if (!ValidateSignature()) return;
            ExpandAll(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<CommandViewModel> Commands
        {
            get { return mCommands; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [Displayable(false)]
        public new object DataContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Displayable(false)]
        public TreeViewControl TreeViewControl
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeNodeNameRepository NameRepository
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        [Category("")]
        [DisplayName("ExpandIconVisibility")]
        [Description("ExpandIconVisibility_Description")]
        [Displayable(true)]
        public bool ExpandIconVisibility
        {
            get
            {
                return mExpandIconVisibility;
            }
            set
            {
                mExpandIconVisibility = value;
                SetIconAll(mExpandIconVisibility);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Displayable(true)]
        [DefaultValue(false)]
        [DisplayName("CheckBoxVisibility")]
        [Description("CheckBoxVisibility_Description")]
        public bool CheckBoxVisibility
        {
            get
            {
                return mCheckBoxVisibility;
            }
            set
            {
                if (mCheckBoxVisibility != value)
                {
                    mCheckBoxVisibility = value;
                    SetCheckBoxAll(mCheckBoxVisibility);
                    mCheckBoxVisibility = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDesignMode
        {
            get
            {
                IProjectInformation prjInfo = ServiceLocator.Current.Resolve<IProjectInformation>();
                if (prjInfo != null)
                    return !prjInfo.IsRunMode;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCtrlEditMode
        {
            get
            {
                return !IsDesignMode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("Config")]
        [DisplayName("Nodes")]
        [Displayable(true)]
        [Editor("ControlEase.Inspec.TreeView.CustomCollectionEditor,CE.NX.TV", typeof(UITypeEditor))]
        public ObservableCollection<TreeNodeViewModel> Items
        {
            get
            {
                if (mItems != null && mItems.Count > 0)
                {

                    if (mItems.ToList<TreeNodeViewModel>().Count > 0)
                    {
                        foreach (var item in mItems.ToList<TreeNodeViewModel>())
                        {
                            if (item.VerLineVisibility)
                                item.VerLineVisibility = false;
                            break;
                        }
                    }
                }
                return mItems;
            }
            set
            {
                mItems = value;
                OnPropertyChanged("Items");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Displayable(true)]
        [DisplayName("NodeSpace")]
        [DefaultValue(0)]
        [Description("NodeSpace_Description")]
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
                    foreach (TreeNodeViewModel item in mItems.ToList<TreeNodeViewModel>())
                    {
                        item.Space = mNodeSpace;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        [Displayable(true)]
        [DisplayName("LineVisibility")]
        [Description("LineVisibility_Description")]
        public bool LineVisibility
        {
            get { return mLineVisibility; }
            set
            {
                mLineVisibility = value;
                SetLineAll(mLineVisibility);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Displayable(true)]
        [DefaultValue(true)]
        [DisplayName("LeftToRightLayout")]
        [Description("LeftToRightLayout_Description")]
        [TypeConverter("ControlEase.Inspec.TreeView.BoolenToLayoutConverter,CE.NX.TV")]
        public bool LeftToRightLayout
        {
            get
            {
                return mLeftToRightLayout;
            }
            set
            {
                mLeftToRightLayout = value;
                OnPropertyChanged("LeftToRightLayout");
            }
        }

        /// <summary>
        /// Gets or sets the select item.
        /// </summary>
        /// <value>The select item.</value>
        public TreeNodeViewModel SelectItem
        {
            get { return mSelectItem; }
            set
            {
                if (mSelectItem != value)
                {
                    mSelectItem = value;
                    //NodeSelectedEvent.LinkCaculate();
                }
            }
        }

        #region 权限配置
        /// <summary>
        /// 权限配置代理
        /// </summary>
        [Displayable(true)]
        [DefaultValue(null)]
        [Category("Security")]
        [DisplayName("Permissionss")]
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
        #endregion

        //节点选中事件。当某一个节点被选中时，触发此事件
        /// <summary>
        /// Gets or sets the left mouse click.
        /// </summary>
        /// <value>The left mouse click.</value>
        [Displayable(true)]
        [Category("KeyOperation")]
        [DisplayName("NodeSelected")]
        [Description("NodeSelected_Description")]
        [DefaultValue(null)]
        [Editor("ControlEase.Inspec.ViewPresentation.PropertyLinkScriptEditor,CE.NX.VWP", typeof(UITypeEditor))]
        [TypeConverter("ControlEase.Inspec.ViewPresentation.PropertyAnimateTypeConverter,CE.NX.VWP")]
        public PropertyLinkEvent<bool> NodeSelectedEvent
        {
            get
            {
                return mNodeSelectedEvent;
            }
            set
            {
                if (mNodeSelectedEvent != value)
                {
                    if (value != null)
                    {
                        value.IsCodeOnly = true;
                    }
                    mNodeSelectedEvent = value;
                    OnPropertyChanged("NodeSelectedEvent");
                }
            }
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
                        MenuVisibility = false;
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
                    if (value != null)
                        MenuVisibility = false;
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
                    if (value != null)
                        MenuVisibility = false;
                    //OnPropertyChanged ( "RightMouseClickMenu" );
                }
            }
        }
        #endregion
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// Validates the signature.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ValidateSignature()
        {
            bool result = true;

            if (PermissionProxy != null)
            {
                result = ValidateSignature(PermissionProxy);
                return result;
            }
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

        #region 用户方法
        ///// <summary> 获得所有的节点信息 </summary>
        ///// <returns>所有的节点信息</returns>
        /// <summary>
        /// Gets all nodes.
        /// </summary>
        /// <returns>List;TreeNodeViewModel.</returns>
        [ScriptMethod]
        public List<TreeNodeViewModel> GetAllNodes()
        {
            List<TreeNodeViewModel> result = new List<TreeNodeViewModel>();
            if (mItems.Count > 0)
            {
                foreach (var item in mItems)
                {
                    result.Add(item);
                    LoopGetNodes(ref result, item.Children);
                }
            }
            return result;
        }

        /// <summary>
        /// Loops the get nodes.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="children">The children.</param>
        private void LoopGetNodes(ref List<TreeNodeViewModel> result, ObservableCollection<TreeNodeViewModel> children)
        {
            if (children != null && children.Count > 0)
            {
                foreach (var item in children)
                {
                    result.Add(item);
                    LoopGetNodes(ref  result, item.Children);
                }
            }
        }

        ///// <summary>
        ///// 展开全部或折叠全部
        ///// </summary>
        ///// <param name="ExpandFlag">if set to <c>true</c> [expand flag].</param>
        /// <summary>
        /// Expands all.
        /// </summary>
        /// <param name="ExpandFlag">The expand flag.</param>
        [ScriptMethod]
        public void ExpandAll(bool ExpandFlag)
        {
            LoopSetExpand(Items, ref ExpandFlag);
        }

        ///// <summary>
        ///// 反馈选中的节点列表
        ///// </summary>
        ///// <returns></returns>
        /// <summary>
        /// Gets the checked items.
        /// </summary>
        /// <returns>List;TreeNodeViewModel.</returns>
        [ScriptMethod]
        public List<TreeNodeViewModel> GetCheckedItems()
        {
            List<TreeNodeViewModel> mtemp = new List<TreeNodeViewModel>();
            LoopGetCheckedItems(ref mtemp, Items);
            return mtemp;
        }

        /// <summary>
        /// 获取用户选中的节点名称集合.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        [ScriptMethod]
        public string[] GetCheckedItemsName()
        {
            var source = GetCheckedItems();
            if (source != null && source.Count > 0)
            {
                string[] result = new string[source.Count];
                int i = 0;
                source.ForEach(it =>
                    {
                        result[i] = it.Name;
                        i++;
                    });
                return result;
            }
            return null;
        }

        /// <summary>
        /// 返回当前选中项
        /// </summary>
        /// <returns></returns>
        [ScriptMethod]
        public TreeNodeViewModel GetSelectedItem()
        {
            return SelectItem;
        }

        /// <summary>
        /// 设置选中节点
        /// </summary>
        /// <param name="nodeIdentity">选中节点唯一ID</param>
        [ScriptMethod]
        public void SetSelectedItem(string nodeIdentity)
        {
            TreeNodeViewModel mSelectNode = null;
            GetCertainNode(Items, nodeIdentity, ref mSelectNode);
            //仅仅是对象在选中焦点离开的状态
            if (mSelectNode != null)
            {
                mSelectNode.IsSelected = true;
                SelectItem = mSelectNode;
                LoopExpandNood(SelectItem);
            }
        }

        /// <summary>
        /// Loops the expand nood.
        /// </summary>
        /// <param name="node">The node.</param>
        public void LoopExpandNood(TreeNodeViewModel node)
        {
            if (node.Parent != null)
            {
                node.Parent.IsExpanded = true;
                LoopExpandNood(node.Parent);
            }
        }

        /// <summary> 返回当前选中项的名称 </summary>
        /// <returns>System.String.</returns>
        [ScriptMethod]
        public string GetSelectedItemName()
        {
            return SelectItem.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodename"></param>
        /// <param name="ExpandFlag"></param>
        [ScriptMethod]
        public void ExpandOrCollapseCertainTreeNode(string nodeNameIdentity, bool ExpandFlag)
        {
            TreeNodeViewModel mtemp = null;
            if (!string.IsNullOrEmpty(nodeNameIdentity))
            {
                GetCertainNode(Items, nodeNameIdentity, ref mtemp);
            }
            if (mtemp != null)
            {
                mtemp.IsExpanded = ExpandFlag;
            }
        }

        /// <summary> 添加节点 </summary>
        /// <param name="addNodeName">添加节点的名字</param>
        /// <param name="addNodeIdenty">添加节点唯一ID</param>
        /// <param name="parentIdenty">父节点ID</param>
        /// <returns>新增节点唯一ID</returns>
        [ScriptMethod]
        public string AddTreeNode(string addNodeName, string addNodeIdenty, string parentIdenty)
        {
            if (!CheckTheIdenty(addNodeIdenty))
            {
                addNodeIdenty = NameRepository.GetNewNameIdenty();
            }
            TreeNodeViewModel mtemp = null;
            if (!string.IsNullOrEmpty(parentIdenty))
            {
                GetCertainNode(Items, parentIdenty, ref mtemp);
                if (mtemp != null)
                {
                    TreeNodeViewModel mTreeNodeViewModel = new TreeNodeViewModel(this, mtemp);
                    mTreeNodeViewModel.Name = addNodeName;
                    mTreeNodeViewModel.NameIdentity = addNodeIdenty;
                    mTreeNodeViewModel.PermissionProxy = mtemp.PermissionProxy;
                    mtemp.Children.Add(mTreeNodeViewModel);
                }
                else
                    return null;
            }
            else//根节点
            {
                TreeNodeViewModel mTreeNodeViewModel = new TreeNodeViewModel(this, null);
                mTreeNodeViewModel.Name = addNodeName;
                mTreeNodeViewModel.NameIdentity = addNodeIdenty;
                mTreeNodeViewModel.PermissionProxy = PermissionProxy;
                Items.Add(mTreeNodeViewModel);
            }
            IProjectInformation prjInfo = ServiceLocator.Current.Resolve<IProjectInformation>();
            //只能生成运行环境的数据文件，不能生成开发环境的数据文件，开发环境的数据，开发环境的数据开发加载
            string FullFileName = System.IO.Path.Combine(prjInfo.RuntimeFileDirectory, CommConst.ViewSubPath, CommConst.TreeViewData + dataChangeFileSuffix + CommConst.FileNameExtension);
            CustomEncode(FullFileName);
            return addNodeIdenty;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="removeNodeIdenty">删除节点的唯一ID</param>
        /// <param name="parentIdenty">父节点唯一ID</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool RemoveTreeNode(string removeNodeIdenty, string parentIdenty)
        {
            bool result = false;
            TreeNodeViewModel mparentnode = null;
            TreeNodeViewModel mremovenode = null;
            GetCertainNode(Items, removeNodeIdenty, ref mremovenode);
            if (!string.IsNullOrEmpty(parentIdenty))
            {
                GetCertainNode(Items, parentIdenty, ref mparentnode);
                if (mparentnode != null && mremovenode != null)
                {
                    if (mparentnode.Children.Contains(mremovenode))
                    {
                        mparentnode.Children.Remove(mremovenode);
                        NameRepository.Remove(removeNodeIdenty);
                        result = true;
                    }
                    result = false;
                }
                else
                    result = false;
            }
            else//根节点de parentIdenty为空
            {
                if (mremovenode != null && Items.Contains(mremovenode))
                {
                    Items.Remove(mremovenode);
                    NameRepository.Remove(removeNodeIdenty);
                    result = true;
                }
                else
                    result = false;
            }
            if (result)
            {
                IProjectInformation prjInfo = ServiceLocator.Current.Resolve<IProjectInformation>();
                //只能生成运行环境的数据文件，不能生成开发环境的数据文件，开发环境的数据，开发环境的数据开发加载
                string FullFileName = System.IO.Path.Combine(prjInfo.RuntimeFileDirectory, CommConst.ViewSubPath, CommConst.TreeViewData + dataChangeFileSuffix + CommConst.FileNameExtension);
                CustomEncode(FullFileName);
            }
            return result;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="removeNodeIdenty">删除节点唯一ID</param>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool RemoveTreeNode(string removeNodeIdenty)
        {
            bool result = false;
            TreeNodeViewModel mremovenode = null;
            GetCertainNode(Items, removeNodeIdenty, ref mremovenode);
            if (mremovenode != null && mremovenode.Parent != null)
            {
                if (mremovenode.Parent.Children.Contains(mremovenode))
                {
                    mremovenode.Parent.Children.Remove(mremovenode);
                    NameRepository.Remove(removeNodeIdenty);
                    result = true;
                }
                else
                    result = false;
            }

            else//根节点de parentIdenty为空
            {
                if (mremovenode != null && Items.Contains(mremovenode))
                {
                    Items.Remove(mremovenode);
                    NameRepository.Remove(removeNodeIdenty);
                    result = true;
                }
                else
                    result = false;
            }
            if (result)
            {
                IProjectInformation prjInfo = ServiceLocator.Current.Resolve<IProjectInformation>();
                //只能生成运行环境的数据文件，不能生成开发环境的数据文件，开发环境的数据，开发环境的数据开发加载
                string FullFileName = System.IO.Path.Combine(prjInfo.RuntimeFileDirectory, CommConst.ViewSubPath, CommConst.TreeViewData + dataChangeFileSuffix + CommConst.FileNameExtension);
                CustomEncode(FullFileName);
            }
            return result;
        }

        /// <summary>
        /// 删除所有节点
        /// </summary>
        /// <returns>是否成功</returns>
        [ScriptMethod]
        public bool RemoveAllNodes()
        {
            Items.Clear();
            NameRepository.RemoveAll();
            IProjectInformation prjInfo = ServiceLocator.Current.Resolve<IProjectInformation>();
            //判断运行的新增数据是否存在,如果存在,编译时将运行的数据文件删掉
            string FullFileName = System.IO.Path.Combine(prjInfo.RuntimeFileDirectory, CommConst.ViewSubPath, CommConst.TreeViewData + dataChangeFileSuffix + CommConst.FileNameExtension);
            DeleteFile(FullFileName);
            OnPropertyChanged("Items");
            return true;
        }

        /// <summary>
        /// 获得特定节点.
        /// </summary>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="parentIdenty">The parentnameidenty.</param>
        /// <param name="treeNodeVM">The tree node vm.</param>
        /// <returns>TreeNodeViewModel.</returns>
        private TreeNodeViewModel GetCertainNode(ObservableCollection<TreeNodeViewModel> treeNodes, string parentIdenty, ref TreeNodeViewModel treeNodeVM)
        {
            if (treeNodes != null && treeNodes.Count > 0)
            {
                foreach (var it in treeNodes)
                {
                    if (it.NameIdentity == parentIdenty)
                    {
                        treeNodeVM = it;
                        return treeNodeVM;
                    }
                    else
                    {
                        if (it.Children != null && it.Children.Count > 0)
                        {
                            GetCertainNode(it.Children, parentIdenty, ref treeNodeVM);
                            if (treeNodeVM != null)
                                return treeNodeVM;
                        }
                    }
                    if (treeNodeVM != null)
                        return treeNodeVM;
                }
            }
            return treeNodeVM;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExpIconVisible"></param>
        private void SetCheckBoxAll(bool ChkBoxVisible)
        {
            foreach (var it in Items.ToList<TreeNodeViewModel>())
            {
                it.CheckBoxVisibility = ChkBoxVisible;
                LoopSetCheckBoxVisible(it, ChkBoxVisible);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="ExpIconVisible"></param>
        private void LoopSetCheckBoxVisible(TreeNodeViewModel tn, bool ChkBoxVisible)
        {
            foreach (var it in tn.Children.ToList<TreeNodeViewModel>())
            {
                it.CheckBoxVisibility = ChkBoxVisible;
                LoopSetCheckBoxVisible(it, ChkBoxVisible);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExpIconVisible"></param>
        private void SetIconAll(bool ExpIconVisible)
        {
            foreach (var it in Items.ToList<TreeNodeViewModel>())
            {
                it.ExpandIconVisibility = ExpIconVisible;
                LoopSetExpIconVisible(it, ExpIconVisible);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="ExpIconVisible"></param>
        private void LoopSetExpIconVisible(TreeNodeViewModel tn, bool ExpIconVisible)
        {
            foreach (var it in tn.Children.ToList<TreeNodeViewModel>())
            {
                it.ExpandIconVisibility = ExpIconVisible;
                LoopSetExpIconVisible(it, ExpIconVisible);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LineVisible"></param>
        private void SetLineAll(bool LineVisible)
        {
            bool flag = false;
            foreach (var it in Items.ToList<TreeNodeViewModel>())
            {
                if (!flag)
                {
                    it.VerLineVisibility = false;
                    flag = true;
                }
                else
                    it.VerLineVisibility = LineVisible;
                it.LineVisibility = LineVisible;
                LoopSetLineVisible(it, LineVisible);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="LineVisible"></param>
        private void LoopSetLineVisible(TreeNodeViewModel tn, bool LineVisible)
        {
            foreach (var it in tn.Children.ToList<TreeNodeViewModel>())
            {
                it.LineVisibility = LineVisible;
                it.VerLineVisibility = LineVisible;
                LoopSetLineVisible(it, LineVisible);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExpandFlag"></param>
        private void CheckedAll(bool CheckedFlag)
        {
            if (CheckBoxVisibility)
            {
                foreach (var it in Items.ToList<TreeNodeViewModel>())
                {
                    it.IsChecked = CheckedFlag;
                    LoopSetChecked(it, CheckedFlag);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tvm"></param>
        /// <param name="flag"></param>
        private void LoopSetChecked(TreeNodeViewModel tvm, bool flag)
        {
            foreach (var it in tvm.Children.ToList<TreeNodeViewModel>())
            {
                it.IsChecked = flag;
                LoopSetChecked(it, flag);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tvm"></param>
        /// <param name="flag"></param>
        private void LoopSetExpand(TreeNodeViewModel tvm, bool flag)
        {
            foreach (var it in tvm.Children.ToList<TreeNodeViewModel>())
            {
                it.IsExpanded = flag;
                LoopSetExpand(it, flag);
            }
        }

        /// <summary>
        /// Loops the set expand.
        /// </summary>
        /// <param name="tvms">The TVMS.</param>
        /// <param name="flag">if set to <c>true</c> [flag].</param>
        private void LoopSetExpand(ObservableCollection<TreeNodeViewModel> tvms, ref bool flag)
        {
            if (tvms != null && tvms.Count > 0)
            {
                foreach (var it in tvms)
                {
                    it.IsExpanded = flag;
                    LoopSetExpand(it.Children, ref flag);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private TreeNodeViewModel LoopGetCheckedItem(TreeNodeViewModel current)
        {
            foreach (var it in current.Children.ToList<TreeNodeViewModel>())
            {
                if (it.IsChecked)
                    return current;
                else
                    return LoopGetCheckedItem(it);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="curentitem"></param>
        /// <returns></returns>
        private void LoopGetCheckedItems(ref  List<TreeNodeViewModel> tn, ObservableCollection<TreeNodeViewModel> items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (var it in items)
                {
                    if (it.IsChecked)
                        tn.Add(it);
                    LoopGetCheckedItems(ref tn, it.Children);
                }
            }
        }

        /// <summary>检查唯一标示</summary>
        /// <param name="name">The name.</param>
        /// <returns>如果 XXXX 则 <c>true</c> ,否则 <c>false</c> .</returns>
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
            if (NameRepository.Nodes != null)
                return !NameRepository.Nodes.Contains(name);
            return true;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

        #region ICustomSerializable Members
        /// <summary>
        /// 加载数据，全新编译也会调用此方法，但是编译的是运行环境下的数据,load
        /// </summary>
        /// <param name="element"></param>
        public void Decode(System.Xml.Linq.XElement element)
        {
            IProjectInformation prjInfo = ServiceLocator.Current.Resolve<IProjectInformation>();
            string FullFileName = string.Empty;
            GetChangeFileSuffix(element);
            //只有运行环境才加载运行环境的数据
            if (prjInfo.IsRunMode)
            {
                FullFileName = System.IO.Path.Combine(prjInfo.RuntimeFileDirectory, CommConst.ViewSubPath, CommConst.TreeViewData + dataChangeFileSuffix + CommConst.FileNameExtension);
                if (File.Exists(FullFileName))
                {
                    XDocument rundocument = XDocument.Load(FullFileName);
                    GetTreeViewCtrlData(rundocument.Root);
                    GetTreeNodesData(rundocument.Root);
                    SetLineAll(LineVisibility);
                    SetCheckBoxAll(mCheckBoxVisibility);
                    SetIconAll(ExpandIconVisibility);
                }
                else//不存在运行时改变的数据则正常加载数据
                    GetData(element);
            }
            else
            {
                GetData(element);
            }
        }

        /// <summary>得到数据</summary>
        /// <param name="element">The element.</param>
        private void GetData(System.Xml.Linq.XElement element)
        {
            GetTreeViewCtrlData(element);
            GetTreeNodesData(element);
            SetLineAll(LineVisibility);
            SetCheckBoxAll(mCheckBoxVisibility);
            SetIconAll(ExpandIconVisibility);
        }

        /// <summary> 获取运行时变化的数据文件名称</summary>
        /// <param name="element">The element.</param>
        private void GetChangeFileSuffix(System.Xml.Linq.XElement element)
        {
            if (element.Element("TreeViewControlData") != null)
            {
                if (element.Element("TreeViewControlData").Attribute("dataChangeFileSuffix") != null)
                    dataChangeFileSuffix = element.Element("TreeViewControlData").Attribute("dataChangeFileSuffix").Value;
            }
        }

        /// <summary>保存数据</summary>
        /// <param name="element">The element.</param>
        public void Encode(System.Xml.Linq.XElement element)
        {
            //不能自己实现序列化，因为导出的时候需要数据文件，导入的时候根据数据文件加载控件的数据,
            //编译序列化文件的时候需要将上次变更的数据文件删除
            IProjectInformation prjInfo = ServiceLocator.Current.Resolve<IProjectInformation>();
            //判断运行的新增数据是否存在,如果存在,编译时将运行的数据文件删掉
            string FullFileName = System.IO.Path.Combine(prjInfo.RuntimeFileDirectory, CommConst.ViewSubPath, CommConst.TreeViewData + dataChangeFileSuffix + CommConst.FileNameExtension);
            DeleteFile(FullFileName);
            //开发时，画面的保存会触发Encode,工程的编译又会调用Encode方法
            if (string.IsNullOrEmpty(dataChangeFileSuffix))
                dataChangeFileSuffix = Guid.NewGuid().ToString();
            CtrlEncode(element);
        }

        /// <summary> 删除文件</summary>
        /// <param name="FullFileName">文件全名</param>
        private static void DeleteFile(string FullFileName)
        {
            if (File.Exists(FullFileName))
            {
                FileInfo fi = new FileInfo(FullFileName);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(FullFileName);
            }
        }

        /// <summary>
        /// Customs the encode.
        /// </summary>
        /// <param name="fullFilePath">The full file path.</param>
        private void CustomEncode(string fullFilePath)
        {
            var dir = System.IO.Path.GetDirectoryName(fullFilePath);
            if (Directory.Exists(dir))
            {
                if (File.Exists(fullFilePath))
                {
                    File.Delete(fullFilePath);
                }
                XDocument rundocument = new XDocument();
                rundocument.Add(new XElement("Root"));
                GenerateTreeViewCtrlData(rundocument.Root);
                GenerateTreeNodesData(rundocument.Root);
                rundocument.Save(fullFilePath);
            }
        }

        /// <summary> Controls the encode. </summary>
        /// <param name="element">The element.</param>
        private void CtrlEncode(System.Xml.Linq.XElement element)
        {
            GenerateTreeViewCtrlData(element);
            GenerateTreeNodesData(element);
        }

        ///// <summary>
        ///// Compiles the specified erro MSG.
        ///// </summary>
        ///// <param name="erroMsg">The erro MSG.</param>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //public bool Compile ( out string erroMsg )
        //{
        //    ////拷贝文件
        //    //IProjectInformation prjInfo = ServiceLocator.Current.Resolve<IProjectInformation> ( );
        //    ////encode时，将dataChangeFileName保存在XML中
        //    //string sourceFileName = System.IO.Path.Combine ( prjInfo.SourceFileDirectory, CommConst.ViewSubPath, CommConst.TreeViewData );
        //    //string targetPath = System.IO.Path.Combine ( prjInfo.RuntimeFileDirectory, CommConst.ViewSubPath, CommConst.TreeViewData );
        //    //File.Copy ( sourceFileName, targetPath, true );
        //    erroMsg = null;
        //    return true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TVCtrlElement"></param>
        private void GetTreeViewCtrlData(XElement element)
        {
            if (element.Element("TreeViewControlData").Attribute("dataChangeFileSuffix") != null)
                dataChangeFileSuffix = element.Element("TreeViewControlData").Attribute("dataChangeFileSuffix").Value;
            LineVisibility = Convert.ToBoolean(element.Element("TreeViewControlData").Attribute("LineVisibility").Value);
            if (element.Element("TreeViewControlData").Attribute("MenuVisibility") != null)
                MenuVisibility = Convert.ToBoolean(element.Element("TreeViewControlData").Attribute("MenuVisibility").Value);
            if (element.Element("TreeViewControlData").Attribute("LeftToRightLayout") != null)
                LeftToRightLayout = Convert.ToBoolean(element.Element("TreeViewControlData").Attribute("LeftToRightLayout").Value);
            mNodeSpace = Convert.ToInt32(element.Element("TreeViewControlData").Attribute("NodeSpace").Value);
            mCheckBoxVisibility = Convert.ToBoolean(element.Element("TreeViewControlData").Attribute("CheckBoxVisibility").Value);
            ExpandIconVisibility = Convert.ToBoolean(element.Element("TreeViewControlData").Attribute("ExpandIconVisibility").Value);

            if (element.Element("Background") != null)
            {
                byte ba = Convert.ToByte(element.Element("Background").Attribute("A").Value);
                byte br = Convert.ToByte(element.Element("Background").Attribute("R").Value);
                byte bg = Convert.ToByte(element.Element("Background").Attribute("G").Value);
                byte bb = Convert.ToByte(element.Element("Background").Attribute("B").Value);
                mBackground = System.Drawing.Color.FromArgb(ba, br, bg, bb);
            }
            #region ClickEvent
            if (element.Element("NodeSelectedEvent") != null)
                this.NodeSelectedEvent = element.Element("NodeSelectedEvent").LoadPropertyEventFromXElement<bool>();
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

            if (element.Element("SecurityEntity") != null)
            {
                PermissionProxy = element.Element("SecurityEntity").ToSecurityEntity();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        private void GetTreeNodesData(XElement element)
        {
            XElement Itemsele = element.Element("Items");
            if (Itemsele == null) return;
            int number = Itemsele.Elements("Node").Count();
            if (number > 0)
            {
                foreach (var item in Itemsele.Elements("Node"))
                {
                    TreeNodeViewModel mtemp = new TreeNodeViewModel(this, null);
                    mtemp.LoadFrom((item as XElement));
                    Items.Add(mtemp);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        private void GenerateTreeNodesData(XElement element)
        {
            XElement Itemsele = new XElement("Items");
            if (Items != null && Items.Count > 0)
            {
                foreach (var item in Items.ToList<TreeNodeViewModel>())
                {
                    Itemsele.Add(item.ToElement());
                }
                element.Add(Itemsele);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        private void GenerateTreeViewCtrlData(XElement element)
        {
            XElement Itemsele = new XElement("TreeViewControlData", new XAttribute("LineVisibility", LineVisibility)
                                                                        , new XAttribute("ExpandIconVisibility", ExpandIconVisibility)
                                                                     , new XAttribute("NodeSpace", NodeSpace)
                                                                     , new XAttribute("CheckBoxVisibility", CheckBoxVisibility)
                                                                      , new XAttribute("MenuVisibility", MenuVisibility)
                                                                      , new XAttribute("LeftToRightLayout", LeftToRightLayout)
                                                                      , new XAttribute("dataChangeFileSuffix", string.IsNullOrEmpty(dataChangeFileSuffix) ? "" : dataChangeFileSuffix)
                                                                     );


            XElement Bgelement = new XElement("Background", new XAttribute("A", mBackground.A)
                                , new XAttribute("R", mBackground.R)
                                , new XAttribute("G", mBackground.G)
                                , new XAttribute("B", mBackground.B)
                                );
            element.Add(Bgelement);
            element.Add(Itemsele);

            #region ClickEvent

            if (LeftMouseClick != null)
            {
                XElement xe1 = new XElement("LeftMouseClick");
                this.LeftMouseClick.SaveToXElement<Boolean>(xe1);
                element.Add(xe1);
            }

            if (NodeSelectedEvent != null)
            {
                XElement xe1 = new XElement("NodeSelectedEvent");
                this.NodeSelectedEvent.SaveToXElement<Boolean>(xe1);
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

            #region 安全
            if (PermissionProxy != null)
            {
                XElement pms = PermissionProxy.ToElement();
                element.Add(pms);
            }
            #endregion
        }

        /// <summary>
        /// Gets a value indicating whether this instance is full custom serialize.
        /// </summary>
        /// <value><c>true</c> if this instance is full custom serialize; otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public bool IsFullCustomSerialize
        {
            get { return true; }
        }
        #endregion

        #region IPropertyAnimateCompatible Members
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Animates.IPropertyAnimate> ListAnimate()
        {
            List<Animates.IPropertyAnimate> list = new List<Animates.IPropertyAnimate>();

            #region AddTVCClickEvent
            if (this.LeftMouseDoubleClick != null && !string.IsNullOrEmpty(this.LeftMouseDoubleClick.Expression))
            {
                //this.LeftMouseDoubleClick.PropertyName = "LeftMouseDoubleClick";
                this.LeftMouseDoubleClick.PropertyName = SR.GetString("DoubleClick");
                list.Add(this.LeftMouseDoubleClick);
            }
            if (this.NodeSelectedEvent != null && !string.IsNullOrEmpty(this.NodeSelectedEvent.Expression))
            {
                //this.LeftMouseDoubleClick.PropertyName = "LeftMouseDoubleClick";
                this.NodeSelectedEvent.PropertyName = SR.GetString("NodeSelectedEvent");
                list.Add(this.NodeSelectedEvent);
            }
            if (this.LeftMouseClick != null && !string.IsNullOrEmpty(this.LeftMouseClick.Expression))
            {
                //this.LeftMouseClick.PropertyName = "LeftMouseClick";
                this.LeftMouseClick.PropertyName = SR.GetString("LeftSingleClick");
                list.Add(this.LeftMouseClick);
            }
            if (this.LeftMouseClickInput != null && !string.IsNullOrEmpty(this.LeftMouseClickInput.Expression))
            {
                list.Add(this.LeftMouseClickInput);
            }
            if (this.LeftMouseDoubleClickInput != null && !string.IsNullOrEmpty(this.LeftMouseDoubleClickInput.Expression))
            {
                list.Add(this.LeftMouseDoubleClickInput);
            }

            if (this.RightMouseClick != null && !string.IsNullOrEmpty(this.RightMouseClick.Expression))
            {
                //this.RightMouseClick.PropertyName = "RightMouseClick";
                this.RightMouseClick.PropertyName = SR.GetString("RightSingleClick");

                list.Add(this.RightMouseClick);
            }
            if (this.RightMouseClickInput != null && !string.IsNullOrEmpty(this.RightMouseClickInput.Expression))
            {
                list.Add(this.RightMouseClickInput);
            }

            #endregion

            #region AddNodesClickEvevt
            foreach (var item in Items.ToList<TreeNodeViewModel>())
            {
                if (item.LeftMouseDoubleClick != null && !string.IsNullOrEmpty(item.LeftMouseDoubleClick.Expression))
                {
                    //item.LeftMouseDoubleClick.PropertyName = item.Name + ".LeftMouseDoubleClick";
                    item.LeftMouseDoubleClick.PropertyName = SR.GetString("DoubleClick"); ;
                    list.Add(item.LeftMouseDoubleClick);
                }
                if (item.LeftMouseClick != null && !string.IsNullOrEmpty(item.LeftMouseClick.Expression))
                {
                    //item.LeftMouseClick.PropertyName = item.Name + ".LeftMouseClick";
                    item.LeftMouseClick.PropertyName = SR.GetString("LeftSingleClick"); ;

                    list.Add(item.LeftMouseClick);
                }
                if (item.LeftMouseClickInput != null && !string.IsNullOrEmpty(item.LeftMouseClickInput.Expression))
                {
                    list.Add(item.LeftMouseClickInput);
                }
                if (item.LeftMouseDoubleClickInput != null && !string.IsNullOrEmpty(item.LeftMouseDoubleClickInput.Expression))
                {
                    list.Add(item.LeftMouseDoubleClickInput);
                }
                if (item.CheckNodePrograme != null && !string.IsNullOrEmpty(item.CheckNodePrograme.Expression))
                {
                    //item.CheckNodePrograme.PropertyName = item.Name + ".CheckNodePrograme";
                    item.CheckNodePrograme.PropertyName = SR.GetString("CheckNodePrograme");

                    list.Add(item.CheckNodePrograme);
                }
                if (item.UnCheckNodePrograme != null && !string.IsNullOrEmpty(item.UnCheckNodePrograme.Expression))
                {
                    //item.UnCheckNodePrograme.PropertyName = item.Name + ".UnCheckNodePrograme";
                    item.UnCheckNodePrograme.PropertyName = SR.GetString("UnCheckNodePrograme");

                    list.Add(item.UnCheckNodePrograme);
                }
                if (item.RightMouseClick != null && !string.IsNullOrEmpty(item.RightMouseClick.Expression))
                {
                    //item.RightMouseClick.PropertyName = item.Name + ".RightMouseClick";
                    item.RightMouseClick.PropertyName = SR.GetString("RightSingleClick");

                    list.Add(item.RightMouseClick);
                }
                if (item.RightMouseClickInput != null && !string.IsNullOrEmpty(item.RightMouseClickInput.Expression))
                {
                    list.Add(item.RightMouseClickInput);
                }

                LoopPropertyAnimate(list, item);
            }
            #endregion

            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        List<Animates.IPropertyAnimate> LoopPropertyAnimate(List<Animates.IPropertyAnimate> list, TreeNodeViewModel item)
        {
            if (item.Children != null)
            {
                foreach (var subitem in item.Children.ToList<TreeNodeViewModel>())
                {
                    if (subitem.LeftMouseDoubleClick != null && !string.IsNullOrEmpty(subitem.LeftMouseDoubleClick.Expression))
                    {
                        //subitem.LeftMouseDoubleClick.PropertyName = item.Name + ".LeftMouseDoubleClick";
                        subitem.LeftMouseDoubleClick.PropertyName = SR.GetString("DoubleClick");

                        list.Add(subitem.LeftMouseDoubleClick);
                    }
                    if (subitem.LeftMouseClick != null && !string.IsNullOrEmpty(subitem.LeftMouseClick.Expression))
                    {
                        //subitem.LeftMouseClick.PropertyName = item.Name + ".LeftMouseClick";
                        subitem.LeftMouseClick.PropertyName = SR.GetString("LeftSingleClick"); ;

                        list.Add(subitem.LeftMouseClick);
                    }
                    if (subitem.LeftMouseClickInput != null && !string.IsNullOrEmpty(subitem.LeftMouseClickInput.Expression))
                    {
                        list.Add(subitem.LeftMouseClickInput);
                    }
                    if (subitem.LeftMouseDoubleClickInput != null && !string.IsNullOrEmpty(subitem.LeftMouseDoubleClickInput.Expression))
                    {
                        list.Add(subitem.LeftMouseDoubleClickInput);
                    }
                    if (subitem.RightMouseClick != null && !string.IsNullOrEmpty(subitem.RightMouseClick.Expression))
                    {
                        //subitem.RightMouseClick.PropertyName = item.Name + ".RightMouseClick";
                        subitem.RightMouseClick.PropertyName = SR.GetString("RightSingleClick"); ;

                        list.Add(subitem.RightMouseClick);
                    }
                    if (subitem.RightMouseClickInput != null && !string.IsNullOrEmpty(subitem.RightMouseClickInput.Expression))
                    {
                        list.Add(subitem.RightMouseClickInput);
                    }
                    if (subitem.CheckNodePrograme != null && !string.IsNullOrEmpty(subitem.CheckNodePrograme.Expression))
                    {
                        //subitem.CheckNodePrograme.PropertyName = subitem.Name + ".CheckNodePrograme";
                        subitem.CheckNodePrograme.PropertyName = SR.GetString(")CheckNodePrograme");

                        list.Add(subitem.CheckNodePrograme);
                    }
                    if (subitem.UnCheckNodePrograme != null && !string.IsNullOrEmpty(subitem.UnCheckNodePrograme.Expression))
                    {
                        //subitem.UnCheckNodePrograme.PropertyName = subitem.Name + ".UnCheckNodePrograme";
                        subitem.UnCheckNodePrograme.PropertyName = SR.GetString("UnCheckNodePrograme");

                        list.Add(subitem.UnCheckNodePrograme);
                    }
                    LoopPropertyAnimate(list, subitem);
                }
                return list;
            }
            return list;
        }
        #endregion

        #region IViewShape Members
        /// <summary>
        /// 
        /// </summary>
        public IViewShapeService ViewShapeService
        {
            get;
            set;
        }

        /// <summary>
        /// 执行菜单动画
        /// <param name="menuname"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        public void ExeMenuItem(string menuname, double X, double Y)
        {
            var ishape = ViewShapeService.GetShapeByName(menuname);
            if (ishape is InMenu)
            {
                //System.Windows.Point zeropoint = new System.Windows.Point ( ) { X = 0, Y = 0 };
                if (TreeViewControl != null)
                {
                    //System.Windows.Point screenpoint = TreeViewControl.PointToScreen ( zeropoint );
                    //System.Drawing.Point ipoint = new Point ( ( int ) screenpoint.X + (int)X, ( int ) screenpoint.Y + ( int ) Y );
                    System.Drawing.Point ipoint = new Point((int)X, (int)Y);

                    (ishape as InMenu).Show(ipoint);
                }
            }
        }
        #endregion

        #region IPropertyConflict Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
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

        #endregion

        #region IPluginPermission Members

        /// <summary> 使用自定义权限 </summary>
        public bool IsUsingCustomPermission
        {
            get { return true; }
        }

        #endregion

        public List<ResourceRefernceItem> GetResourceId()
        {
            List<ResourceRefernceItem> vv = new List<ResourceRefernceItem>();
            LoopSetNodeResourceRefernce(vv, Items);
            return vv;
        }

        /// <summary>
        ///  循环访问节点并设置资源引用
        /// </summary>
        /// <param name="vv">The vv.</param>
        /// <param name="children">The children.</param>
        void LoopSetNodeResourceRefernce(List<ResourceRefernceItem> vv, ObservableCollection<TreeNodeViewModel> children)
        {
            if (children != null && children.Count > 0)
            {
                foreach (var item in children)
                {
                    NodeResourceRefernce(vv, item);
                    LoopSetNodeResourceRefernce(vv, item.Children);
                }
            }
        }

        /// <summary>
        /// 设置每一项的资源引用
        /// </summary>
        /// <param name="vv">The vv.</param>
        /// <param name="node">The node.</param>
        void NodeResourceRefernce(List<ResourceRefernceItem> vv, TreeNodeViewModel node)
        {
            vv.Add(new ResourceRefernceItem()
            {
                ResId = node.ImageSource.ToString(),
                Type = ResourceTypes.Picture,
                Value = node.ImageSource.ToString(),
                UpdateResourceDelegate = new Func<string, bool>((name) =>
                {
                    node.ImageSource = Convert.ToInt32(name);
                    return true;
                })
            });
        }

        public List<ToolBarMenuCommand> GetToolbarMenu()
        {
            List<ToolBarMenuCommand> re = new List<ToolBarMenuCommand>();
            re.Add(new ToolBarMenuCommand() { Image = (" pack://application:,,,/CE.NX.TV;component/Resources/{0}_TreeView.ico"), Description = SR.GetString("Nodes"), ExecuteCommand = new Action(() => { SetUrlProperty(); }) });
            return re;
        }

        /// <summary>
        /// Sets the URL property.
        /// </summary>
        private void SetUrlProperty()
        {
            ServiceLocator.Current.Resolve<IViewEditorHelper>().EditValue(this, Items, "Items");
        }

        /// <summary>
        /// 资源更改
        /// </summary>
        /// <param name="Service">The service.</param>
        public void ResourceUpdate(IViewResourceService Service)
        {
            LoopUpDateResourceImage(Items);
        }

        /// <summary>
        /// Loops up date resource image.
        /// </summary>
        /// <param name="children">The children.</param>
        void LoopUpDateResourceImage(ObservableCollection<TreeNodeViewModel> children)
        {
            foreach (var item in children)
            {
                item.UpdateImageResource();
                if (item.Children.Count > 0)
                {
                    LoopUpDateResourceImage(item.Children);
                }
            }
        }
    }
}
