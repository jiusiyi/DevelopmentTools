using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.Inspec.Resources;
using ControlEase.Nexus.ComponentModel;
using ControlEase.AI.Tag;
using Microsoft.Win32;
using System.Windows;
using System.ComponentModel;
using ControlEase.Nexus;
using ControlEase.Nexus.Presentation;
using ControlEase.Inspec.ViewPresentation;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 对话框显示服务
    /// </summary>
    public static class DialogHelper
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public delegate object HelperDelagate ( );
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 是否进入测试模式
        /// </summary>
        public static bool IsTest { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Window GetActiveWindow()
        {
            if (Application.Current != null)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.IsActive)
                    {
                        return w;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool? ShowDialog ( this WindowViewModelBase model )
        {
            CustomWindowBase cwb = new CustomWindowBase ( );
            cwb.Owner = GetActiveWindow();

            cwb.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            cwb.DataContext = model;
            if ( model.MinHeight != null )
            {
                cwb.MinHeight = model.MinHeight.Value;
            }
            if ( model.MinWidth != null )
            {
                cwb.MinWidth = model.MinWidth.Value;
            }

            if ( model.DefaultHeight != null )
            {
                if ( model.IsOkCancel )
                {
                    cwb.Height = model.DefaultHeight.Value + 70;
                }
                else
                {
                    cwb.Height = model.DefaultHeight.Value + 10;
                }
                cwb.SizeToContent = SizeToContent.Manual;
            }

            if ( model.DefaultWidth != null )
            {
                cwb.Width = model.DefaultWidth.Value + 16;
                cwb.SizeToContent = SizeToContent.Manual;
            }

            return cwb.ShowDialog ( ).Value;
        }

        public static void ShowErrorMessageBox(string msg)
        {
            if (!IsTest)
            {
                ControlEase.Nexus.Windows.MessageBox.Show(msg, SR.GetString("TitleInfo"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
       

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        public static void ShowMessageBox ( string msg )
        {
            if ( !IsTest )
            {
                //if (!string.IsNullOrEmpty(title))
                //{
                //    ControlEase.Nexus.Windows.MessageBox.Show(msg,title);
                //}
                //else
                {
                    ControlEase.Nexus.Windows.MessageBox.Show(msg,SR.GetString("TitleInfo"),MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// 显示一个MessageBox对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static bool ShowMessageBox ( string msg, string header )
        {
            if ( !IsTest )
            {
                return ControlEase.Nexus.Windows.MessageBox.Show(msg, header, MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static MessageBoxResult ShowMessageBoxWithCancel ( string msg, string header,MessageBoxImage image= MessageBoxImage.None )
        {
            if ( !IsTest )
            {
                return ControlEase.Nexus.Windows.MessageBox.Show(msg, header, MessageBoxButton.YesNoCancel, image);
            }
            else
            {
                return MessageBoxResult.No;
            }
        }

        /// <summary>
        /// 显示一个Warnning的MessageBox对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static bool ShowWarnningDialog ( string msg, string header )
        {
            if ( !IsTest )
            {
                return ControlEase.Nexus.Windows.MessageBox.Show(msg, header, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 显示路径浏览对话框
        /// </summary>
        /// <param name="defaultFolder">初始路径</param>
        /// <returns></returns>
        public static string ShowFolderDialog ( string defaultFolder = "" )
        {
            if ( !IsTest )
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog ( );
                if ( !string.IsNullOrEmpty ( defaultFolder ) )
                {
                    fbd.SelectedPath = defaultFolder;
                }
                fbd.ShowNewFolderButton = false;
                if ( fbd.ShowDialog ( ) == System.Windows.Forms.DialogResult.OK )
                {
                    return fbd.SelectedPath;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 显示OpenFileDialog对话框
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="filter"></param>
        /// <param name="isEnableMutiSelect">是否允许多选</param>
        /// <returns></returns>
        public static bool ShowOpenFileDialog ( out OpenFileDialog dialog, string filter = "All File|*.*",bool isEnableMutiSelect=false )
        {
            if ( !IsTest )
            {
                OpenFileDialog ofd = new OpenFileDialog ( );
                ofd.Multiselect = isEnableMutiSelect;
                if ( !string.IsNullOrEmpty ( filter ) )
                    ofd.Filter = filter;
                else
                    ofd.Filter = "All File|*.*";
                dialog = ofd;
                return ofd.ShowDialog ( ).Value;
            }
            else
            {
                dialog = null;
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static bool ShowSaveFileDialog ( out SaveFileDialog dialog, string filter = "All File|*.*" )
        {
            if ( !IsTest )
            {
                SaveFileDialog ofd = new SaveFileDialog ( );
                if ( !string.IsNullOrEmpty ( filter ) )
                    ofd.Filter = filter;
                else
                    ofd.Filter = "All File|*.*";
                dialog = ofd;
                ofd.Title = SR.GetString("SaveFileDialogTitle");
                return ofd.ShowDialog ( ).Value;
            }
            else
            {
                dialog = null;
                return false;
            }
        }

        /// <summary>
        /// 显示变量列表对话框
        /// </summary>
        /// <returns></returns>
        public static List<string> ShowTagListWindow ( )
        {
            List<string> re = new List<string> ( );
            if ( !IsTest )
            {
                if ( ServiceLocator.Current != null )
                {
                    var vv = ServiceLocator.Current.Resolve<ITagBrowser> ( );
                    if ( vv != null )
                    {
                        var tags = vv.ShowBrowser ( false, true, null );
                        if ( tags != null )
                        {
                            re.AddRange ( tags.Select ( e => e.FullName ) );
                        }
                    }
                }
            }
            return re;
        }



        ///// <summary>
        ///// 显示消息列表对话框
        ///// </summary>
        ///// <returns></returns>
        //public static List<string> ShowMessageCollectionListWindow ( )
        //{
        //    List<string> re = new List<string> ( );
        //    if ( !IsTest )
        //    {
        //        if ( ServiceLocator.Current != null )
        //        {

        //        }
        //    }
        //    return re;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> ShowResourceListWindow (ResourceTypes type )
        {
            List<string> re = new List<string> ( );
            if ( !IsTest )
            {
                if ( ServiceLocator.Current != null )
                {
                    var vv = ServiceLocator.Current.Resolve<IResourceSelector> ( );
                    if ( vv != null )
                    {

                        try
                        {
                            var vvv = vv.ShowResourceBrowser ( type );
                            if ( vvv != null && !string.IsNullOrEmpty ( vvv.Name ) )
                            {
                                re.Add ( vvv.Name );
                            }
                        }
                        catch ( Exception )
                        {

                        }
                    }
                }
            }
            return re;
        }

        /// <summary>
        /// 执行一个显示代理函数
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        public static object ShowDelagate ( this HelperDelagate gate )
        {
            if ( !IsTest )
            {
                return gate ( );
            }
            else
            {
                return null;
            }
        }



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
