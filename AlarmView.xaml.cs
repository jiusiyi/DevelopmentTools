using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlEase.AI.IO;

namespace ControlEase.IoDrive.Hikvision
{
    /// <summary>
    /// AlarmView.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmView : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public AlarmView ( )
        {
            InitializeComponent ( );
        }
         /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        public AlarmView ( DeviceDev device )
            : this ( )
        {
            DataContext = new AlarmViewModel ( device.ConfigData as AlarmConfigData );
        }
    }
}
