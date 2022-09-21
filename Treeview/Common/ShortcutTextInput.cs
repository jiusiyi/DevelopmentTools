using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using ControlEase.Inspec.ViewCore;
using System.Windows.Forms.Integration;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 
    /// </summary>
    public class ShortcutTextInput : WindowsFormsHost
    {
        #region ... Variables  ...
        
        private ShortcutTextbox textbox;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public ShortcutTextInput()
        {
            textbox = new ShortcutTextbox();
            textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textbox.ShortcutChanged += (textbox_ShortcutChanged);
            this.Child = textbox;
        }

        /// <summary>
        /// 
        /// </summary>
        void textbox_ShortcutChanged()
        {
            Keys = textbox.ShortcutValue;
        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.Keys Keys
        {
            get { return (System.Windows.Forms.Keys)GetValue(KeysProperty); }
            set { SetValue(KeysProperty, value); }
        }

        
        public static readonly DependencyProperty KeysProperty = DependencyProperty.Register("Keys", typeof(System.Windows.Forms.Keys), typeof(ShortcutTextInput), new UIPropertyMetadata(System.Windows.Forms.Keys.None,new PropertyChangedCallback(KeysPropertyChanged)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private static void KeysPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as ShortcutTextInput).UpdateShortcut();
            //(sender as ShortcutTextInput).Text = arg.NewValue.ToString();
        }

        #endregion ...Properties...

        #region ... Methods    ...

        private void UpdateShortcut()
        {
            textbox.ShortcutValue = Keys;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        //{
        //    string stmp = e.Key.ToString();
        //    if (e.Key == System.Windows.Input.Key.LeftCtrl)
        //    {
        //        stmp = System.Windows.Forms.Keys.LControlKey.ToString();
        //    }
        //    switch (e.Key)
        //    {
        //        case System.Windows.Input.Key.LeftCtrl:
        //            stmp = System.Windows.Forms.Keys.LControlKey.ToString();
        //            break;
        //        case System.Windows.Input.Key.RightCtrl:
        //            stmp = System.Windows.Forms.Keys.RControlKey.ToString();
        //            break;
        //        case System.Windows.Input.Key.LeftShift:
        //            stmp = System.Windows.Forms.Keys.LShiftKey.ToString();
        //            break;
        //        case System.Windows.Input.Key.RightShift:
        //            stmp = System.Windows.Forms.Keys.RShiftKey.ToString();
        //            break;
        //        case System.Windows.Input.Key.LeftAlt:
        //            stmp = System.Windows.Forms.Keys.Alt.ToString();
        //            break;
        //        case System.Windows.Input.Key.RightAlt:
        //            stmp = System.Windows.Forms.Keys.Alt.ToString();
        //            break;
        //    }
        //    try
        //    {
        //        this.Keys = (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), e.Key.ToString());
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    e.Handled = true;

        //}

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
