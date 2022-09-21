using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Timers;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 
    /// </summary>
    public static class ClickExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SupportDoubleClickProperty = DependencyProperty.RegisterAttached ( "SupportDoubleClick", typeof ( bool ),
            typeof ( ClickExtension ), new PropertyMetadata ( false, new PropertyChangedCallback ( OnSupportDoubleClickChanged ) ) );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetSupportDoubleClick ( DependencyObject d, bool value )
        {
            d.SetValue ( SupportDoubleClickProperty, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetSupportDoubleClick ( DependencyObject d )
        {
            return ( bool ) d.GetValue ( SupportDoubleClickProperty );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSupportDoubleClickChanged ( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            if ( ( bool ) e.NewValue )
            {
                UIElement ue = d as UIElement;
                if ( ue != null )
                {
                    ue.AddHandler ( UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler ( Border_MouseLeftButtonDown ), true );
                    ue.AddHandler ( UIElement.MouseRightButtonDownEvent, new MouseButtonEventHandler ( Border_MouseRightButtonDown ), true );
                }
            }
        }

        /// <summary>
        /// 明确一个行为，在执行动作之前先执行验证，没有动作就没有验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Border_MouseLeftButtonDown ( object sender, MouseButtonEventArgs e )
        {
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            e.Handled = true;
            var element = ( FrameworkElement ) sender;
            {
                if ( e.ClickCount == 1 )
                {
                    var timer = new System.Timers.Timer ( 200 );
                    timer.AutoReset = false;
                    timer.Elapsed += new ElapsedEventHandler ( ( o, ex ) => element.Dispatcher.Invoke ( new Action ( ( ) =>
                    {
                        var timer2 = ( System.Timers.Timer ) element.Tag;
                        timer2.Stop ( );
                        timer2.Dispose ( );
                        if ( element.DataContext is TreeNodeViewModel )
                        {
                            var dc = element.DataContext as TreeNodeViewModel;
                            if ( dc != null )
                            {
                                if ( !element.DataContext.Equals ( ( e.OriginalSource as FrameworkElement ).DataContext ) ) return;
                                dc.IsSelected = true;
                                if ( e.OriginalSource is System.Windows.Controls.Border )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Border ).Name == "Bd" )
                                        LeftSingleClick ( dc, p.X, p.Y );
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.Image )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Image ).Name == "WorkImage" )
                                        LeftSingleClick ( dc, p.X, p.Y );
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.TextBlock )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.TextBlock ).Name == "WorkTextBlock" )
                                        LeftSingleClick ( dc, p.X, p.Y );
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.Grid )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Grid ).Name == "TreeViewItemGrid" )
                                        LeftSingleClick ( dc, p.X, p.Y );
                                }
                            }
                        }
                        else if ( element.DataContext is TreeViewControlViewModel )
                        {
                            var dc = element.DataContext as TreeViewControlViewModel;
                            if ( dc != null && e.OriginalSource is System.Windows.Controls.Grid )
                            {
                                LeftSingleClick ( dc, p.X, p.Y );
                            }
                        }
                    } ) ) );
                    timer.Start ( );
                    element.Tag = timer;
                }
                if ( e.ClickCount > 1 )
                {
                    var timer = element.Tag as System.Timers.Timer;
                    if ( timer != null )
                    {
                        timer.Stop ( );
                        timer.Dispose ( );
                        if ( element.DataContext is TreeNodeViewModel )
                        {
                            var dc = element.DataContext as TreeNodeViewModel;
                            if ( dc != null )
                            {
                                if ( !element.DataContext.Equals ( ( e.OriginalSource as FrameworkElement ).DataContext ) ) return;
                                dc.IsSelected = true;
                                if ( e.OriginalSource is System.Windows.Controls.Border )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Border ).Name == "Bd" )
                                        LeftDoubleClick ( dc, p.X,p.Y);
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.Image )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Image ).Name == "WorkImage" )
                                        LeftDoubleClick ( dc,p.X,p.Y );
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.TextBlock )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.TextBlock ).Name == "WorkTextBlock" )
                                        LeftDoubleClick ( dc,p.X,p.Y );
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.Grid )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Grid ).Name == "TreeViewItemGrid" )
                                        LeftDoubleClick ( dc,p.X,p.Y );
                                }
                            }
                        }
                        else if ( element.DataContext is TreeViewControlViewModel )
                        {
                            var dc = element.DataContext as TreeViewControlViewModel;
                            if ( dc != null && e.OriginalSource is System.Windows.Controls.Grid )
                            {
                                LeftDoubleClick ( dc,p.X,p.Y );
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        private static void LeftDoubleClick ( object dc,int x,int y )
        {
            if ( dc is TreeNodeViewModel )
            {
                TreeNodeViewModel tnv = dc as TreeNodeViewModel;
                if ( tnv == null ) return;
                if ( tnv.LeftMouseDoubleClickInput != null )
                {
                    if ( tnv.ValidateSignature ( ) )
                    {
                        if ( tnv.LeftMouseDoubleClickInput is AniInputAnalog )
                        {
                            ( tnv.LeftMouseDoubleClickInput as AniInputAnalog ).LinkCaculate ( x,y );
                        }
                        else if ( tnv.LeftMouseDoubleClickInput is AniInputDateTime )
                        {
                            ( tnv.LeftMouseDoubleClickInput as AniInputDateTime ).LinkCaculate ( x, y );
                        }
                        else if ( tnv.LeftMouseDoubleClickInput is AniInputDisc )
                        {
                            ( tnv.LeftMouseDoubleClickInput as AniInputDisc ).LinkCaculate ( x, y );
                        }
                        else if ( tnv.LeftMouseDoubleClickInput is AniInputString )
                        {
                            ( tnv.LeftMouseDoubleClickInput as AniInputString ).LinkCaculate ( x, y );
                        }
                    }
                }
                if ( tnv.LeftMouseDoubleClick != null )
                {
                    if ( tnv.ValidateSignature ( ) )
                    {
                        tnv.LeftMouseDoubleClick.LinkCaculate ( );
                    }
                }
            }
            else if ( dc is TreeViewControlViewModel )
            {
                TreeViewControlViewModel tvc = dc as TreeViewControlViewModel;
                if ( tvc == null ) return;
                if ( tvc.LeftMouseDoubleClickInput != null )
                {
                    if ( !tvc.ValidateSignature ( ) ) return;
                    if ( tvc.LeftMouseDoubleClickInput is AniInputAnalog )
                    {
                        ( tvc.LeftMouseDoubleClickInput as AniInputAnalog ).LinkCaculate ( x, y );
                    }
                    else if ( tvc.LeftMouseDoubleClickInput is AniInputDateTime )
                    {
                        ( tvc.LeftMouseDoubleClickInput as AniInputDateTime ).LinkCaculate ( x, y );
                    }
                    else if ( tvc.LeftMouseDoubleClickInput is AniInputDisc )
                    {
                        ( tvc.LeftMouseDoubleClickInput as AniInputDisc ).LinkCaculate ( x, y );
                    }
                    else if ( tvc.LeftMouseDoubleClickInput is AniInputString )
                    {
                        ( tvc.LeftMouseDoubleClickInput as AniInputString ).LinkCaculate ( x, y );
                    }
                }
                if ( tvc.LeftMouseDoubleClick != null )
                {
                    if ( !tvc.ValidateSignature ( ) ) return;
                    tvc.LeftMouseDoubleClick.LinkCaculate ( );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        private static void LeftSingleClick ( object dc, double X, double Y )
        {
            if ( dc is TreeNodeViewModel )
            {
                TreeNodeViewModel mtn = dc as TreeNodeViewModel;
                //单击输入工程变量
                if ( mtn.LeftMouseClickInput != null )
                {
                        if ( !mtn.ValidateSignature ( ) ) return;

                        if ( mtn.LeftMouseClickInput is AniInputAnalog )
                        {
                            ( mtn.LeftMouseClickInput as AniInputAnalog ).LinkCaculate ( ( int ) X, ( int ) Y );
                        }
                        else if ( mtn.LeftMouseClickInput is AniInputDateTime )
                        {
                            ( mtn.LeftMouseClickInput as AniInputDateTime ).LinkCaculate ( ( int ) X, ( int ) Y );
                        }
                        else if ( mtn.LeftMouseClickInput is AniInputDisc )
                        {
                            ( mtn.LeftMouseClickInput as AniInputDisc ).LinkCaculate ( ( int ) X, ( int ) Y );
                        }
                        else if ( mtn.LeftMouseClickInput is AniInputString )
                        {
                            ( mtn.LeftMouseClickInput as AniInputString ).LinkCaculate ( ( int ) X, ( int ) Y );
                        }
                }
                //单击执行程序
                if ( mtn.LeftMouseClick != null )
                {
                    if ( mtn.ValidateSignature ( ) )
                        mtn.LeftMouseClick.LinkCaculate ( );
                }
                //一种是加在这里，一种是
                if (mtn.Root.NodeSelectedEvent != null)
                {
                    if (mtn.Root.ValidateSignature())
                    mtn.Root.NodeSelectedEvent.LinkCaculate();
                }
                //单机弹出菜单
                if ( !string.IsNullOrEmpty ( mtn.LeftMouseClickMenu ) )
                {
                    if ( mtn.ValidateSignature ( ) )
                        ( mtn.Root ).ExeMenuItem ( mtn.LeftMouseClickMenu, X, Y );
                }
            }
            else if ( dc is TreeViewControlViewModel )
            {
                TreeViewControlViewModel mtvc = dc as TreeViewControlViewModel;
                if ( mtvc.SelectItem != null )
                {
                    mtvc.SelectItem.IsSelected = false;
                    mtvc.SelectItem = null;
                }
                //单击输入工程变量
                if ( mtvc.LeftMouseClickInput != null )
                {
                    if ( !mtvc.ValidateSignature ( ) ) return;
                    if ( mtvc.LeftMouseClickInput is AniInputAnalog )
                    {
                        ( mtvc.LeftMouseClickInput as AniInputAnalog ).LinkCaculate ( ( int ) X, ( int ) Y );
                    }
                    else if ( mtvc.LeftMouseClickInput is AniInputDateTime )
                    {
                        ( mtvc.LeftMouseClickInput as AniInputDateTime ).LinkCaculate ( ( int ) X, ( int ) Y );
                    }
                    else if ( mtvc.LeftMouseClickInput is AniInputDisc )
                    {
                        ( mtvc.LeftMouseClickInput as AniInputDisc ).LinkCaculate ( ( int ) X, ( int ) Y );
                    }
                    else if ( mtvc.LeftMouseClickInput is AniInputString )
                    {
                        ( mtvc.LeftMouseClickInput as AniInputString ).LinkCaculate ( (int)X ,(int)Y);
                    }
                }
                //单击执行程序
                if ( mtvc.LeftMouseClick != null )
                {
                    if ( !mtvc.ValidateSignature ( ) ) return;
                    mtvc.LeftMouseClick.LinkCaculate ( );
                }
              
                    
                //单机弹出菜单
                if ( !string.IsNullOrEmpty ( mtvc.LeftMouseClickMenu ) )
                {
                    if ( !mtvc.ValidateSignature ( ) ) return;
                    mtvc.ExeMenuItem ( mtvc.LeftMouseClickMenu, X, Y );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Border_MouseRightButtonDown ( object sender, MouseButtonEventArgs e )
        {
            System.Windows.Point mpoint = new Point ( ); ;
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            mpoint.X = p.X;
            mpoint.Y = p.Y;
            e.Handled = true;
            var element = ( FrameworkElement ) sender;
            {
                if ( e.ClickCount == 1 )
                {
                    var timer = new System.Timers.Timer ( 200 );
                    timer.AutoReset = false;
                    timer.Elapsed += new ElapsedEventHandler ( ( o, ex ) => element.Dispatcher.Invoke ( new Action ( ( ) =>
                    {
                        var timer2 = ( System.Timers.Timer ) element.Tag;
                        timer2.Stop ( );
                        timer2.Dispose ( );
                        if ( element.DataContext is TreeNodeViewModel )
                        {
                            var dc = element.DataContext as TreeNodeViewModel;
                            if ( dc != null )
                            {
                                if ( !element.DataContext.Equals ( ( e.OriginalSource as FrameworkElement ).DataContext ) ) return;

                                dc.IsSelected = true;
                                if ( ( e.OriginalSource is System.Windows.Controls.Border ) )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Border ).Name == "Bd" )

                                        RightSingleClick ( e.OriginalSource, dc, mpoint.X, mpoint.Y );
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.Image )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Image ).Name == "WorkImage" )
                                        RightSingleClick ( e.OriginalSource, dc, mpoint.X, mpoint.Y );
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.TextBlock )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.TextBlock ).Name == "WorkTextBlock" )
                                        RightSingleClick ( e.OriginalSource, dc, mpoint.X, mpoint.Y );
                                }
                                else if ( e.OriginalSource is System.Windows.Controls.Grid )
                                {
                                    if ( ( e.OriginalSource as System.Windows.Controls.Grid ).Name == "TreeViewItemGrid" )
                                        RightSingleClick ( e.OriginalSource, dc, mpoint.X, mpoint.Y );
                                }
                            }
                        }
                        else if ( element.DataContext is TreeViewControlViewModel )
                        {
                            var dc = element.DataContext as TreeViewControlViewModel;
                            if ( dc != null && e.OriginalSource is System.Windows.Controls.Grid )
                            {
                               
                                RightSingleClick ( e.OriginalSource, dc, mpoint.X, mpoint.Y );
                            }
                        }
                    } ) ) );
                    timer.Start ( );
                    element.Tag = timer;
                }
                if ( e.ClickCount > 1 )
                {
                    var timer = element.Tag as System.Timers.Timer;
                    if ( timer != null )
                    {
                        timer.Stop ( );
                        timer.Dispose ( );
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        private static void RightSingleClick ( object originalSource, object dc, double X, double Y )
        {
            if ( dc is TreeNodeViewModel )
            {
                TreeNodeViewModel mtn = dc as TreeNodeViewModel;
                if ( mtn == null ) return;
                //单击输入工程变量
                if ( mtn.RightMouseClickInput != null )
                {
                    if ( mtn.ValidateSignature ( ) )
                    {
                        if ( mtn.RightMouseClickInput is AniInputAnalog )
                        {
                            ( mtn.RightMouseClickInput as AniInputAnalog ).LinkCaculate ( ( int ) X, ( int ) Y );
                        }
                        else if ( mtn.RightMouseClickInput is AniInputDateTime )
                        {
                            ( mtn.RightMouseClickInput as AniInputDateTime ).LinkCaculate ( ( int ) X, ( int ) Y );
                        }
                        else if ( mtn.RightMouseClickInput is AniInputDisc )
                        {
                            ( mtn.RightMouseClickInput as AniInputDisc ).LinkCaculate ( ( int ) X, ( int ) Y );
                        }
                        else if ( mtn.RightMouseClickInput is AniInputString )
                        {
                            ( mtn.RightMouseClickInput as AniInputString ).LinkCaculate ( ( int ) X, ( int ) Y );
                        }
                    }
                    else
                    {
                        if ( originalSource is FrameworkElement )
                            ( originalSource as FrameworkElement ).Focus ( );
                    }
                }
                //单击执行程序
                if ( mtn.RightMouseClick != null )
                {
                    if ( mtn.ValidateSignature ( ) )
                        mtn.RightMouseClick.LinkCaculate ( );
                    else
                    {
                        if ( originalSource is FrameworkElement )
                            ( originalSource as FrameworkElement ).Focus ( );
                    }
                }
                //单机弹出菜单
                if ( !string.IsNullOrEmpty ( mtn.RightMouseClickMenu ) )
                {
                    if ( mtn.ValidateSignature ( ) )
                        ( mtn.Root ).ExeMenuItem ( mtn.RightMouseClickMenu, X, Y );
                    else
                    {
                        if ( originalSource is FrameworkElement )
                            ( originalSource as FrameworkElement ).Focus ( );
                    }
                }
            }
            else if ( dc is TreeViewControlViewModel )
            {
                TreeViewControlViewModel mtvc = dc as TreeViewControlViewModel;
                if ( mtvc == null ) return;
                if ( mtvc.SelectItem != null )
                {
                    mtvc.SelectItem.IsSelected = false;
                    mtvc.SelectItem = null;
                }
                //单击输入工程变量
                if ( mtvc.RightMouseClickInput != null )
                {
                    if ( !mtvc.ValidateSignature ( ) )  return;
                    if ( mtvc.RightMouseClickInput is AniInputAnalog )
                    {
                        ( mtvc.RightMouseClickInput as AniInputAnalog ).LinkCaculate ( ( int ) X, ( int ) Y );
                    }
                    else if ( mtvc.RightMouseClickInput is AniInputDateTime )
                    {
                        ( mtvc.RightMouseClickInput as AniInputDateTime ).LinkCaculate ( ( int ) X, ( int ) Y );
                    }
                    else if ( mtvc.RightMouseClickInput is AniInputDisc )
                    {
                        ( mtvc.RightMouseClickInput as AniInputDisc ).LinkCaculate ( ( int ) X, ( int ) Y );
                    }
                    else if ( mtvc.RightMouseClickInput is AniInputString )
                    {
                        ( mtvc.RightMouseClickInput as AniInputString ).LinkCaculate ( ( int ) X, ( int ) Y );
                    }
                }
                //单击执行程序
                if ( mtvc.RightMouseClick != null )
                {
                    if ( !mtvc.ValidateSignature ( ) ) return;
                    mtvc.RightMouseClick.LinkCaculate ( );
                }
                //单机弹出菜单
                if ( !string.IsNullOrEmpty ( mtvc.RightMouseClickMenu ) )
                {
                    if ( !mtvc.ValidateSignature ( ) ) return;
                    mtvc.ExeMenuItem ( mtvc.RightMouseClickMenu, X, Y );
                }
            }
        }
    }
}
