using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ControlEase.Nexus.Presentation;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Class CommandViewModel
    /// </summary>
    public class CommandViewModel:ViewModel
    {
        #region ... Variables  ...

        private ICommand mCommand;
        private string mIconPath;
        private string mName;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class.
        /// </summary>
        public CommandViewModel()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        public CommandViewModel(ICommand command)
            : this()
        {
            mCommand = command;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandViewModel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="command">The command.</param>
        public CommandViewModel(string name, ICommand command)
            : this(command)
        {
            Name = name;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// Gets the command.
        /// </summary>
        public ICommand Command
        {
            get { return mCommand; }
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name 
        {
            get { return mName; }
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
        /// Gets or sets the icon path.
        /// </summary>
        /// <value>
        /// The icon path.
        /// </value>
        public string IconPath
        {
            get
            {
                return mIconPath;
            }
            set
            {
                if(mIconPath!=value)
                {
                    mIconPath=value;
                    OnPropertyChanged("IconPath");
                }
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
