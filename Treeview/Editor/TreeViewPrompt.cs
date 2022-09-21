using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ControlEase.Nexus.Windows;
using System.Windows.Data;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 提示信息
    /// </summary>
    public class TreeViewPrompt : INotifyPropertyChanged
    {
        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private string mDialogInfoText;
        /// <summary>
        /// 
        /// </summary>
        private static TreeViewPrompt mInstance = new TreeViewPrompt ( );

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// Gets or sets the dialog info text.
        /// </summary>
        /// <value>
        /// The dialog info text.
        /// </value>
        public string DialogInfoText
        {
            get { return mDialogInfoText; }
            set
            {
                if ( mDialogInfoText != value )
                {
                    mDialogInfoText = value;
                    OnPropertyChanged ( "DialogInfoText" );
                }
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static TreeViewPrompt Instance
        {
            get { return mInstance; }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// Sets the binding.
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        public static void SetBinding ( OkCancelDialog dialog )
        {
            Binding bind = new Binding ( "DialogInfoText" );
            bind.Mode = BindingMode.TwoWay;
            bind.Source = mInstance;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dialog.SetBinding ( OkCancelDialog.InfoTextProperty, bind );
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged ( string propertyName )
        {
            if ( PropertyChanged != null )
                PropertyChanged ( this, new PropertyChangedEventArgs ( propertyName ) );
        }

        #endregion ...Interfaces...
    }
}
