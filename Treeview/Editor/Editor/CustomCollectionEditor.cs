using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Runtime;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using ControlEase.Inspec.View;
using ControlEase.Inspec.ViewCore;
using System.Collections.ObjectModel;
using ControlEase.Nexus;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace ControlEase.Inspec.TreeView
{
    public class CustomCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// 
        /// </summary>
        private  static TreeViewControlViewModel tvcroot;

        /// <summary>
        /// 
        /// </summary>
        private  TreeViewControlViewModel temptvc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public CustomCollectionEditor ( Type type )
            : base ( type )
        {
        }

        /// <summary> 
        /// 限制一次选一个实例 
        /// </summary> 
        /// <returns></returns> 
        protected override bool CanSelectMultipleInstances ( )
        {
            return false;
        }

        PropertyGrid grid;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override CollectionEditor.CollectionForm CreateCollectionForm()
        {
            CollectionEditor.CollectionForm oForm;
            Type oType  ;
            FieldInfo oFieldInfo ; 
            System.Windows.Forms.PropertyGrid oPropertyGrid  ;
            oForm = base.CreateCollectionForm();
            oForm.FormBorderStyle = FormBorderStyle.Sizable;
            oForm.WindowState = FormWindowState.Normal;
            //设置窗口的最大化和最小化按钮显示
            oForm.MinimizeBox = true;
            oForm.MaximizeBox = true;
       
            oForm.Text = SR.GetString ( "TreeNodeCollection" );
            oType = oForm.GetType();
            oFieldInfo = oType.GetField("propertyBrowser", BindingFlags.NonPublic|BindingFlags.Instance);
            if ( oFieldInfo != null )
            {
                //'取得属性窗口控件
                oPropertyGrid = oFieldInfo.GetValue ( oForm ) as System.Windows.Forms.PropertyGrid;
                //'设定属性窗口控件的[说明]区域为可视
                ContextMenuStrip mContextMenuStrip = new ContextMenuStrip ( );
                mContextMenuStrip.Items.Add ( SR.GetString ( "Reset" ) );
                mContextMenuStrip.Items[0].Click += new EventHandler ( CustomCollectionEditor_ResetClick );
                oPropertyGrid.ContextMenuStrip = mContextMenuStrip;
                grid = oPropertyGrid;
            }

            return oForm;
        }

        /// <summary> 
        /// 指定创建的对象类型 
        /// </summary> 
        /// <returns></returns> 
        protected override Type CreateCollectionItemType ( )
        {
            return typeof ( TreeNodeViewModel );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override string GetDisplayText ( object value )
        {
            if ( value is DelegateTreeNodeViewModel )
            {
                if ( !string.IsNullOrEmpty ( ( value as DelegateTreeNodeViewModel ).SelectObject.Name ) )
                    return ( value as DelegateTreeNodeViewModel ).SelectObject.Name;
                else
                    return string.Empty;
                   
            }
            else  
                return string.Empty; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        protected override object CreateInstance ( Type itemType )
        {
            //创建一个实例 
            TreeNodeViewModel mTreeNodeViewModel = GetTreeNodeViewModel ( Context );
            object[] parameters = new object[2];
            parameters[0] = tvcroot;
            parameters[1] = mTreeNodeViewModel;
            //IDesignerHost host = ( IDesignerHost ) this.GetService ( typeof ( IDesignerHost ) );
            //host.Container.Add ( o );//重要！自动生成组件的设计时代码！
            //TreeNodeViewModel o = ( TreeNodeViewModel ) itemType.Assembly.CreateInstance ( itemType.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null );
            TreeNodeViewModel o = new TreeNodeViewModel(tvcroot, mTreeNodeViewModel);

            DelegateTreeNodeViewModel delegateNode = new DelegateTreeNodeViewModel ( o );
            return delegateNode;
            //return o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected TreeNodeViewModel GetTreeNodeViewModel ( System.ComponentModel.ITypeDescriptorContext context )
        {
            TreeNodeViewModel nodeVM = null;
            if ( ( context != null ) && ( context.Instance != null ) )
            {
                if ( context.Instance is TreeNodeViewModel )
                {
                    nodeVM = ( context.Instance as TreeNodeViewModel );
                }
                else if ( context.Instance is DelegateTreeNodeViewModel )
                {
                    nodeVM = ( context.Instance as DelegateTreeNodeViewModel ).SelectObject;
                }
            }

            return nodeVM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected TreeViewControlViewModel GetTreeNodeControlViewModel ( System.ComponentModel.ITypeDescriptorContext context )
        {
            TreeViewControlViewModel ControlVM = null;
            if ( ( context != null ) && ( context.Instance != null ) )
            {
                //画面中定义的WPF属性描述类
                if ( context.Instance is NormalControlPropertyDescriptor )
                {
                    NormalControlPropertyDescriptor descrptor = context.Instance as NormalControlPropertyDescriptor;
                    if ( ( descrptor != null ) && ( descrptor.DelegateShape != null ) )
                    {
                        WpfSurrogate surrogate = descrptor.DelegateShape as WpfSurrogate;

                        if ( ( surrogate != null ) && ( surrogate.InnerElement != null ) )
                        {
                            if ( surrogate.InnerElement is TreeViewControlViewModel )
                                ControlVM = surrogate.InnerElement as TreeViewControlViewModel;
                        }
                    }
                }
                else if ( context.Instance is TreeNodeViewModel )
                {
                    ControlVM = ( context.Instance as TreeNodeViewModel ).Root;
                }
                else if ( context.Instance is DelegateTreeNodeViewModel )
                {
                    ControlVM = ( context.Instance as DelegateTreeNodeViewModel ).SelectObject.Root;
                }
                else if (context.Instance is TreeViewControlViewModel)
                {
                    ControlVM = context.Instance as TreeViewControlViewModel;
                }
                return ControlVM;
            }
            return ControlVM;
        }

        /// <summary>
        /// 将对象包装了一下，用Delegate 代理对象实现，通过实现ICustomTypeDescriptor使用propertygrid
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue ( ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            try
            {
                if ( tvcroot == null )
                    tvcroot = GetTreeNodeControlViewModel ( context );
                else
                {
                    temptvc = GetTreeNodeControlViewModel ( context );
                    if ( temptvc != null )
                        tvcroot = temptvc;
                }
                TreeNodeViewModel mTreeNodeViewModel = GetTreeNodeViewModel ( context );
                ObservableCollection<DelegateTreeNodeViewModel> editValue = new ObservableCollection<DelegateTreeNodeViewModel> ( );
                var collection = value as ObservableCollection<TreeNodeViewModel>;
                if ( collection != null )
                {
                    collection.ForEach ( x => editValue.Add ( new DelegateTreeNodeViewModel ( x ) ) );
                }
                ObservableCollection<DelegateTreeNodeViewModel> editResult = base.EditValue ( context, provider, editValue ) as ObservableCollection<DelegateTreeNodeViewModel>;
                if ( editResult != null )
                {
                    ObservableCollection<TreeNodeViewModel> result = new ObservableCollection<TreeNodeViewModel> ( );
                    editResult.ForEach ( x =>
                    {
                        x.SelectObject.Parent =  mTreeNodeViewModel;
                        result.Add ( x.SelectObject );
                    });
                    return result;
                }
                return null;

            }
            catch 
            {
                return null;
            }
            //return base.EditValue ( context, provider, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        protected override void DestroyInstance ( object instance )
        {
            if ( tvcroot != null )
                if ( instance is DelegateTreeNodeViewModel )
                {
                    //删除所有的名字
                    tvcroot.NameRepository.Remove ( ( instance as DelegateTreeNodeViewModel ).SelectObject.NameIdentity );
                    LoopRemoveNameIdentity ( ( instance as DelegateTreeNodeViewModel ).SelectObject );
                }
            base.DestroyInstance ( instance );//重要！自动删除组件的设计时代码！ 
        }

        private void LoopRemoveNameIdentity ( TreeNodeViewModel treeNodeViewModel )
        {
            if(tvcroot != null)
            {
                foreach(var item in  treeNodeViewModel.Children.ToList<TreeNodeViewModel>())
                {
                    tvcroot.NameRepository.Remove ( item.NameIdentity );
                    LoopRemoveNameIdentity ( item );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle ( ITypeDescriptorContext context )
        {
            if ( context != null && context.Instance != null )
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle ( context );
        }
   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CustomCollectionEditor_ResetClick ( object sender, EventArgs e )
        {
            if ( grid != null )
            {
                grid.ResetSelectedProperty ( );
                //为了解决属性重置后刷新问题，强制刷新grid选择的属性
                TypeDescriptor.Refresh ( grid.SelectedObject );
            }
        }
    }
}
