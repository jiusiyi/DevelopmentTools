using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ControlEase.Nexus;
using ControlEase.Inspec.ViewPresentation;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 气泡转状态信息
    /// </summary>
    public class ViewToolTipHelper : DependencyObject
    {
        #region ... Variables  ...

        private static Dictionary<DependencyObject, string> TextDictionary = new Dictionary<DependencyObject, string> ( );


        private static Dictionary<DependencyObject, string> StaticTextDictionary = new Dictionary<DependencyObject, string> ( );

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public static IMessageMediator Mediator
        {
            get
            {
                return MessageMediatorHelper.Mediator;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetText ( DependencyObject element, string value )
        {
            element.SetValue ( TextProperty, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetText ( DependencyObject element )
        {
            return ( string ) element.GetValue ( TextProperty );
        }

        /// <summary>
        /// 鼠标进入时提示信息
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached ( "Text", typeof ( string ), typeof ( ViewToolTipHelper ), new UIPropertyMetadata ( "", new PropertyChangedCallback ( OnTextChanged ) ) );


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetStaticText ( DependencyObject element, string value )
        {
            element.SetValue ( StaticTextProperty, value );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetStaticText ( DependencyObject element )
        {
            return ( string ) element.GetValue ( StaticTextProperty );
        }

        /// <summary>
        /// 静态鼠标进入时提示信息
        /// </summary>
        public static readonly DependencyProperty StaticTextProperty =
            DependencyProperty.RegisterAttached ( "StaticText", typeof ( string ), typeof ( ViewToolTipHelper ), new UIPropertyMetadata ( "", new PropertyChangedCallback ( OnStaticTextChanged ) ) );


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetFouseText ( DependencyObject element, string value )
        {
            element.SetValue ( FouseTextProperty, value );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetFouseText ( DependencyObject element )
        {
            return ( string ) element.GetValue ( FouseTextProperty );
        }

        /// <summary>
        /// 获取焦点是提示信息
        /// </summary>
        public static readonly DependencyProperty FouseTextProperty =
            DependencyProperty.RegisterAttached ( "FouseText", typeof ( string ), typeof ( ViewToolTipHelper ), new UIPropertyMetadata ( "", new PropertyChangedCallback ( OnFouseTextChanged ) ) );


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetReciver ( DependencyObject element, string value )
        {
            element.SetValue ( ReciverProperty, value );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetReciver ( DependencyObject element )
        {
            return ( string ) element.GetValue ( ReciverProperty );
        }

        /// <summary>
        /// 提示文本处理消息名
        /// </summary>
        public static readonly DependencyProperty ReciverProperty =
            DependencyProperty.RegisterAttached ( "Reciver", typeof ( string ), typeof ( ViewToolTipHelper ), new UIPropertyMetadata ( "" ) );



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnStaticTextChanged ( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {

            ( d as FrameworkElement ).MouseLeave += ( ToolTipHelper_StaticMouseLeave );
            ( d as FrameworkElement ).MouseEnter += ( ToolTipHelper_StaticMouseEnter );
            if ( !StaticTextDictionary.ContainsKey ( d ) )
            {
                StaticTextDictionary.Add ( d, GetStaticText ( d ) );
            }
        }


        static void ToolTipHelper_StaticMouseLeave ( object sender, System.Windows.Input.MouseEventArgs e )
        {
            DependencyObject obj = sender as DependencyObject;

            string rec = GetReciver ( obj );

            if ( string.IsNullOrEmpty ( rec ) )
            {
                rec = "ViewToolTipHelperMessage";
            }

            Mediator.SendMessage<string> ( rec, "" );
        }

        static void ToolTipHelper_StaticMouseEnter ( object sender, System.Windows.Input.MouseEventArgs e )
        {
            DependencyObject obj = sender as DependencyObject;

            string rec = GetReciver ( obj );

            if ( string.IsNullOrEmpty ( rec ) )
            {
                rec = "ViewToolTipHelperMessage";
            }

            string stmp = "";
            if ( StaticTextDictionary.ContainsKey ( obj ) )
            {
                stmp = StaticTextDictionary[obj];
            }
            Mediator.SendMessage<string> ( rec, stmp );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnTextChanged ( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {

            ( d as FrameworkElement ).MouseLeave += new System.Windows.Input.MouseEventHandler ( ToolTipHelper_MouseLeave );
            ( d as FrameworkElement ).MouseEnter += new System.Windows.Input.MouseEventHandler ( ToolTipHelper_MouseEnter );
            ( d as FrameworkElement ).Unloaded += new RoutedEventHandler ( ToolTipHelper_Unloaded );
            if ( !TextDictionary.ContainsKey ( d ) )
            {
                TextDictionary.Add ( d, GetText ( d ) );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnFouseTextChanged ( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {

            ( d as FrameworkElement ).GotFocus += new RoutedEventHandler ( ToolTipHelper_GotFocus );
            ( d as FrameworkElement ).LostFocus += new RoutedEventHandler ( ToolTipHelper_LostFocus );
            ( d as FrameworkElement ).Unloaded += new RoutedEventHandler ( FouseToolTipHelper_Unloaded );
            if ( !TextDictionary.ContainsKey ( d ) )
            {
                TextDictionary.Add ( d, GetFouseText ( d ) );
            }
        }

        static void ToolTipHelper_LostFocus ( object sender, RoutedEventArgs e )
        {
            DependencyObject obj = sender as DependencyObject;

            string rec = GetReciver ( obj );

            if ( string.IsNullOrEmpty ( rec ) )
            {
                rec = "ViewToolTipHelperMessage";
            }

            Mediator.SendMessage<string> ( rec, "" );
        }

        static void ToolTipHelper_GotFocus ( object sender, RoutedEventArgs e )
        {
            DependencyObject obj = sender as DependencyObject;

            string rec = GetReciver ( obj );

            if ( string.IsNullOrEmpty ( rec ) )
            {
                rec = "ViewToolTipHelperMessage";
            }

            string stmp = "";
            if ( TextDictionary.ContainsKey ( obj ) )
            {
                stmp = TextDictionary[obj];
            }
            Mediator.SendMessage<string> ( rec, stmp );
        }

        static void ToolTipHelper_MouseLeave ( object sender, System.Windows.Input.MouseEventArgs e )
        {
            DependencyObject obj = sender as DependencyObject;

            string rec = GetReciver ( obj );

            if ( string.IsNullOrEmpty ( rec ) )
            {
                rec = "ViewToolTipHelperMessage";
            }

            Mediator.SendMessage<string> ( rec, "" );
        }

        static void ToolTipHelper_MouseEnter ( object sender, System.Windows.Input.MouseEventArgs e )
        {
            DependencyObject obj = sender as DependencyObject;

            string rec = GetReciver ( obj );

            if ( string.IsNullOrEmpty ( rec ) )
            {
                rec = "ViewToolTipHelperMessage";
            }

            string stmp = "";
            if ( TextDictionary.ContainsKey ( obj ) )
            {
                stmp = TextDictionary[obj];
            }
            Mediator.SendMessage<string> ( rec, stmp );
        }

        static void FouseToolTipHelper_Unloaded ( object sender, RoutedEventArgs e )
        {
            DependencyObject obj = sender as DependencyObject;
            if ( TextDictionary.ContainsKey ( obj ) )
            {
                ( obj as FrameworkElement ).Unloaded -= ( FouseToolTipHelper_Unloaded );

                ( obj as FrameworkElement ).GotFocus -= ( ToolTipHelper_GotFocus );
                ( obj as FrameworkElement ).LostFocus -= ( ToolTipHelper_LostFocus );
                TextDictionary.Remove ( obj );
            }
        }

        static void ToolTipHelper_Unloaded ( object sender, RoutedEventArgs e )
        {
            DependencyObject obj = sender as DependencyObject;
            if ( TextDictionary.ContainsKey ( obj ) )
            {
                ( obj as FrameworkElement ).Unloaded -= ( ToolTipHelper_Unloaded );

                ( obj as FrameworkElement ).MouseLeave -= ( ToolTipHelper_MouseLeave );
                ( obj as FrameworkElement ).MouseEnter -= ( ToolTipHelper_MouseEnter );
                TextDictionary.Remove ( obj );
            }
        }


        /// <summary>
        /// 清除所有采用静态引用方式的部分
        /// </summary>
        public static void ClearStatic ( )
        {
            foreach ( var vv in StaticTextDictionary.Keys )
            {
                ( vv as FrameworkElement ).MouseLeave -= ( ToolTipHelper_StaticMouseLeave );
                ( vv as FrameworkElement ).MouseEnter -= ( ToolTipHelper_StaticMouseEnter );
            }
            StaticTextDictionary.Clear ( );
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
