using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlEase.Nexus.Presentation;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Class ComboboxItemViewModel
    /// </summary>
    public class ComboboxItemViewModel:ViewModel
    {
        #region ...Variables  ...
        #endregion ...Variables  ...

        #region ...Constructor...
        #endregion ...Constructor...

        #region ...Properties ...

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        #endregion ...Properties ...

        #region ...Methods    ...

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Name;
        }
        #endregion ...Methods    ...

        #region ...Events     ...
        #endregion ...Events     ...

        #region ...Interfaces ...
        #endregion ...Interfaces ...
    }
}
