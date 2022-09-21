using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Class VCComboBoxItemViewModel
    /// </summary>
    public class VCComboBoxItemViewModel : ComboboxItemViewModel
    {

        #region ...Variables  ...

        /// <summary>
        /// The camera ID
        /// </summary>
        private Guid mCameraID;

        #endregion ...Variables  ...

        #region ...Constructor...
        /// <summary>
        /// Initializes a new instance of the <see cref="VCComboBoxItemViewModel"/> class.
        /// </summary>
        /// <param name="cameraid">The cameraid.</param>
        public VCComboBoxItemViewModel(Guid cameraid)
        {

            mCameraID = cameraid;
            if (!cameraid.Equals(Guid.Empty))
            {
                //VSAgent agent = VSAgentManagerHelper.GetVSAgentByCameraID(mCameraID);
                //if (agent != null && agent.CameraDatas.ContainsKey(mCameraID))
                //{
                //    Name = agent.CameraDatas[mCameraID].Name;
                //}
                //else
                //{
                //    Name = string.Empty;
                //}
 
            }
        }
        #endregion ...Constructor...

        #region ...Properties ...

        /// <summary>
        /// Gets the camera ID.
        /// </summary>
        /// <value>The camera ID.</value>
        public Guid CameraID
        {
            get { return mCameraID; }
        }
        #endregion ...Properties ...

        #region ...Methods    ...
        #endregion ...Methods    ...

        #region ...Events     ...
        #endregion ...Events     ...

        #region ...Interfaces ...
        #endregion ...Interfaces ...


    }
}
