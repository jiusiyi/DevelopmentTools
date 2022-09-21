using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.AniInfos;
using Properties = ControlEase.Inspec.ViewCore.Properties;
using ControlEase.Nexus.ComponentModel;
using ControlEase.Inspec.Extension;
using ControlEase.Inspec.Animates;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// 模拟值有虚拟键盘的输入对话框
    /// </summary>
    internal class InputAnalogWithVirkeyForm : System.Windows.Forms.Form
    {
        #region ...变量...
        private Decimal minValue = Decimal.MinValue;	// 模拟值输入的最小值
        private Decimal maxValue = Decimal.MaxValue;	// 模拟值输入的最大值

        private System.Windows.Forms.Label labelPrompt;
        private System.Windows.Forms.Label labelMinValuePrompt;
        private System.Windows.Forms.Label labelMaxValuePrompt;
        private System.Windows.Forms.Label labelMinValue;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private ToggleButton button1;
        private ToggleButton button2;
        private ToggleButton button3;
        private ToggleButton button4;
        private ToggleButton button5;
        private ToggleButton button6;
        private ToggleButton button7;
        private ToggleButton button8;
        private ToggleButton button9;
        private System.Windows.Forms.Label labelMaxValue;
        private NumEdit textBoxAnalog;
        private System.Windows.Forms.ToolTip toolTip1;
        private ToggleButton buttonPoint;
        private ToggleButton buttonMinus;
        private ToggleButton buttonBackSpace;
        private ToggleButton button0;
        private TableLayoutPanel tableLayoutPanel1;
        private System.ComponentModel.IContainer components;

        private AniInputAnalog mAniInfo;

        private bool mHasErro = false;

        #endregion ...变量...

        #region ...构造...
        /// <summary>
        /// 构造 InputAnalogWithVirkeyForm 的一个实例
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shape"></param>
        /// <param name="grpControl"></param>
        /// <param name="aniInfo"></param>
        /// <param name="tagType"></param>
        public InputAnalogWithVirkeyForm ( Decimal value, BaseShape shape, Control grpControl, AniInputAnalog aniInfo, Type tagType )
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();

            mAniInfo = aniInfo;


            this.textBoxAnalog.Text = value.ToString();
            textBoxAnalog.SelectAll();

            minValue = aniInfo.ValueMin;
            maxValue = aniInfo.ValueMax;
            textBoxAnalog.DecimalNum = aniInfo.DecimalNum;

            string format = string.Format("f{0}", aniInfo.DecimalNum);

            this.labelMinValue.Text = minValue.ToString(format);
            this.labelMaxValue.Text = maxValue.ToString(format);

            //this.labelMinValue.Text = ((dynamic)Convert.ChangeType(minValue, tagType)).ToString(format);
            //this.labelMaxValue.Text = ((dynamic)Convert.ChangeType(maxValue, tagType)).ToString(format);
            
            //Location = AniInputText.GetInputFormLocation(shape, grpControl, Bounds);


            LoadResource();
        }

        
        #endregion ...构造...

        #region ...属性...
        /// <summary>
        /// 文本框的内容
        /// </summary>
        public Decimal Value
        {
            get
            {
                try
                {
                    mHasErro = false;
                    return Convert.ToDecimal(textBoxAnalog.Text);
                }
                catch (Exception)
                {
                    mHasErro = true;
                    return Decimal.MaxValue;
                }
            }
        }
        #endregion ...属性...

        #region ...方法...

        private void mAniInfo_NeedUpdateResource(object sender, EventArgs e)
        {
            LoadResource();
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadResource()
        {
            this.Text = SR.GetString("InputAnalog_Title");
            this.labelPrompt.Text = SR.GetString ( "InputAnalog_pleasesInput" );
            
            this.labelMaxValuePrompt.Text = SR.GetString("InputAnalog_max");
            this.labelMinValuePrompt.Text = SR.GetString("InputAnalog_min");
            this.buttonOK.Text = SR.GetString("OK");
            this.buttonCancel.Text = SR.GetString("Cancel");
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputAnalogWithVirkeyForm));
            this.labelPrompt = new System.Windows.Forms.Label();
            this.labelMinValuePrompt = new System.Windows.Forms.Label();
            this.labelMaxValuePrompt = new System.Windows.Forms.Label();
            this.labelMinValue = new System.Windows.Forms.Label();
            this.labelMaxValue = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button2 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button3 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button4 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonBackSpace = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button5 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button6 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button7 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button8 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonMinus = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonPoint = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button0 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button9 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxAnalog = new ControlEase.Inspec.ViewCore.NumEdit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPrompt
            // 
            resources.ApplyResources(this.labelPrompt, "labelPrompt");
            this.labelPrompt.Name = "labelPrompt";
            // 
            // labelMinValuePrompt
            // 
            resources.ApplyResources(this.labelMinValuePrompt, "labelMinValuePrompt");
            this.labelMinValuePrompt.Name = "labelMinValuePrompt";
            // 
            // labelMaxValuePrompt
            // 
            resources.ApplyResources(this.labelMaxValuePrompt, "labelMaxValuePrompt");
            this.labelMaxValuePrompt.Name = "labelMaxValuePrompt";
            // 
            // labelMinValue
            // 
            resources.ApplyResources(this.labelMinValue, "labelMinValue");
            this.labelMinValue.Name = "labelMinValue";
            // 
            // labelMaxValue
            // 
            resources.ApplyResources(this.labelMaxValue, "labelMaxValue");
            this.labelMaxValue.Name = "labelMaxValue";
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.Click += new System.EventHandler(this.button_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.toolTip1.SetToolTip(this.button2, resources.GetString("button2.ToolTip"));
            this.button2.Click += new System.EventHandler(this.button_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.Click += new System.EventHandler(this.button_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.toolTip1.SetToolTip(this.button4, resources.GetString("button4.ToolTip"));
            this.button4.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonBackSpace
            // 
            resources.ApplyResources(this.buttonBackSpace, "buttonBackSpace");
            this.buttonBackSpace.Name = "buttonBackSpace";
            this.toolTip1.SetToolTip(this.buttonBackSpace, resources.GetString("buttonBackSpace.ToolTip"));
            this.buttonBackSpace.Click += new System.EventHandler(this.button_Click);
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.toolTip1.SetToolTip(this.button5, resources.GetString("button5.ToolTip"));
            this.button5.Click += new System.EventHandler(this.button_Click);
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            this.toolTip1.SetToolTip(this.button6, resources.GetString("button6.ToolTip"));
            this.button6.Click += new System.EventHandler(this.button_Click);
            // 
            // button7
            // 
            resources.ApplyResources(this.button7, "button7");
            this.button7.Name = "button7";
            this.toolTip1.SetToolTip(this.button7, resources.GetString("button7.ToolTip"));
            this.button7.Click += new System.EventHandler(this.button_Click);
            // 
            // button8
            // 
            resources.ApplyResources(this.button8, "button8");
            this.button8.Name = "button8";
            this.toolTip1.SetToolTip(this.button8, resources.GetString("button8.ToolTip"));
            this.button8.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonMinus
            // 
            resources.ApplyResources(this.buttonMinus, "buttonMinus");
            this.buttonMinus.Name = "buttonMinus";
            this.toolTip1.SetToolTip(this.buttonMinus, resources.GetString("buttonMinus.ToolTip"));
            this.buttonMinus.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPoint
            // 
            resources.ApplyResources(this.buttonPoint, "buttonPoint");
            this.buttonPoint.Name = "buttonPoint";
            this.toolTip1.SetToolTip(this.buttonPoint, resources.GetString("buttonPoint.ToolTip"));
            this.buttonPoint.Click += new System.EventHandler(this.button_Click);
            // 
            // button0
            // 
            resources.ApplyResources(this.button0, "button0");
            this.button0.Name = "button0";
            this.toolTip1.SetToolTip(this.button0, resources.GetString("button0.ToolTip"));
            this.button0.Click += new System.EventHandler(this.button_Click);
            // 
            // button9
            // 
            resources.ApplyResources(this.button9, "button9");
            this.button9.Name = "button9";
            this.toolTip1.SetToolTip(this.button9, resources.GetString("button9.ToolTip"));
            this.button9.Click += new System.EventHandler(this.button_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonBackSpace, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.button5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button6, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.button7, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.button8, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonMinus, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonPoint, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.button0, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.button9, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // textBoxAnalog
            // 
            resources.ApplyResources(this.textBoxAnalog, "textBoxAnalog");
            this.textBoxAnalog.DecimalNum = -1;
            this.textBoxAnalog.ForeColor = System.Drawing.Color.Black;
            this.textBoxAnalog.InputType = ControlEase.Inspec.ViewCore.NumEdit.NumEditType.Double;
            this.textBoxAnalog.Name = "textBoxAnalog";
            // 
            // InputAnalogWithVirkeyForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelMinValuePrompt);
            this.Controls.Add(this.textBoxAnalog);
            this.Controls.Add(this.labelMaxValuePrompt);
            this.Controls.Add(this.labelMinValue);
            this.Controls.Add(this.labelMaxValue);
            this.Controls.Add(this.labelPrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputAnalogWithVirkeyForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.InputAnalogWithVirkeyForm_Closing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        #endregion ...方法...

        #region ...事件...
        /// <summary>
        /// 数字按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, System.EventArgs e)
        {
            if (!this.textBoxAnalog.Focused)
                this.textBoxAnalog.Focus();

            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;

            byte cKey = 0;
            if (button == buttonBackSpace)	// BackSpace
            {
                cKey = (byte)VirtualKeys.VK_BACK;
            }
            else
            {
                cKey = Convert.ToByte(button.Text[0]);
            }

            short ks = Win32API.VkKeyScan((char)cKey);
            byte key = (byte)(ks & 0xFF);

            Win32API.keybd_event(key, 0, 0, (IntPtr)0);
            Win32API.keybd_event(key, 0, (uint)KeyEventTF.KEYEVENTF_KEYUP, (IntPtr)0);

            this.textBoxAnalog.Focus();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputAnalogWithVirkeyForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                return;

            Decimal dValue = Value;
            if (mHasErro)
            {
                ControlEase.Nexus.Windows.MessageBox.Show(Properties.Resources.InvalidInput,
                    Properties.Resources.Error, System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                textBoxAnalog.SelectAll();
                this.textBoxAnalog.Focus();

                e.Cancel = true;
            }
            else if (dValue < this.minValue)
            {
                ControlEase.Nexus.Windows.MessageBox.Show(Properties.Resources.SmallerthanMinValue,
                    Properties.Resources.Error, System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                textBoxAnalog.SelectAll();
                this.textBoxAnalog.Focus();

                e.Cancel = true;
            }
            else if (dValue > this.maxValue)
            {
                ControlEase.Nexus.Windows.MessageBox.Show(Properties.Resources.GreaterthanMaxValue,
                    Properties.Resources.Error, System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                textBoxAnalog.SelectAll();
                this.textBoxAnalog.Focus();

                e.Cancel = true;
            }

        }
        #endregion ...事件...

        private void buttonOK_Click ( object sender, EventArgs e )
        {

        }
    }
}
