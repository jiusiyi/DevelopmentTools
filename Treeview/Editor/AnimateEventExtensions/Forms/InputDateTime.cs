#region  ...模块说明...
//**********************************************
//
//模块名称： InputDateTime.cs
//
//版权所有： Beijing ControlEase Automation Software Co., Ltd.
//
//模块描述： （对该模块下的类、主要功能作简单描述）
//
//问题列表：	时间	描述	提交者	解决时间
//
//修改记录：	时间	作者	版本	修改内容
//			2009-06-17	yafeya	0.1		初始版本
//                                      将对话框的Localize属性设置为true，并将其anchor调整。
//**********************************************
#endregion 	...模块说明...

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.AniInfos;
using ControlEase.Inspec.TreeView;

namespace ControlEase.Inspec.TreeView
{
    internal class InputDateTime : Form
    {
        #region ...变量...
        private MonthCalendar mcDate;
        private Label lblInput;
        private DateTimePicker dtpicker;
        private Label lblSelectedDateTime;
        private System.Windows.Forms.Button buttonCancel;
        private Label lblTime;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonOK;

        private AniInputDateTime mAniInfo;

        //private System.ComponentModel.IContainer components;
        #endregion ...变量...

        #region ...属性...
        /// <summary>
        /// 获取输入的时间
        /// </summary>
        public DateTime Value
        {
            get
            {
                DateTime value = mcDate.SelectionStart.Date;
                int hours = dtpicker.Value.Hour;
                int minutes = dtpicker.Value.Minute;
                int seconds = dtpicker.Value.Second;
                TimeSpan ts = new TimeSpan(hours, minutes, seconds);
                value = value.Add(ts);
                return value;
            }
        }
        #endregion ...属性...

        #region ...构造...
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shape"></param>
        /// <param name="grpControl"></param>
        /// <param name="aniInfo"></param>
        public InputDateTime ( DateTime value, BaseShape shape, Control grpControl, AniInputDateTime aniInfo )
        {
            InitializeComponent();

            DateTime now = DateTime.Now;
            if (value >= dtpicker.MinDate && value <= dtpicker.MaxDate)
            {
                dtpicker.Value = value;
            }
            else
            {
                dtpicker.Value = now;
            }

            if (value >= mcDate.MinDate && value <= mcDate.MaxDate)
            {
                mcDate.SelectionStart = value;
                mcDate.SelectionEnd = value;
            }
            else
            {
                mcDate.SelectionStart = now;
                mcDate.SelectionEnd = now;
            }


            //Location = AniInputText.GetInputFormLocation(shape, grpControl, Bounds);

            mAniInfo = aniInfo;


           // checkBoxTime.Checked = true;
            InitResource();
            UpdateDateTime();
        }

        

        #endregion ...构造...

        #region ...方法...


        /// <summary>
        /// 初始化构造器
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( InputDateTime ) );
            this.mcDate = new System.Windows.Forms.MonthCalendar ( );
            this.lblInput = new System.Windows.Forms.Label ( );
            this.dtpicker = new System.Windows.Forms.DateTimePicker ( );
            this.lblSelectedDateTime = new System.Windows.Forms.Label ( );
            this.buttonCancel = new System.Windows.Forms.Button ( );
            this.buttonOK = new System.Windows.Forms.Button ( );
            this.lblTime = new System.Windows.Forms.Label ( );
            this.pictureBox1 = new System.Windows.Forms.PictureBox ( );
            ( ( System.ComponentModel.ISupportInitialize ) ( this.pictureBox1 ) ).BeginInit ( );
            this.SuspendLayout ( );
            // 
            // mcDate
            // 
            this.mcDate.Location = new System.Drawing.Point ( 14, 27 );
            this.mcDate.Name = "mcDate";
            this.mcDate.TabIndex = 0;
            this.mcDate.DateChanged += new System.Windows.Forms.DateRangeEventHandler ( this.mcDate_DateChanged );
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.BackColor = System.Drawing.Color.Transparent;
            this.lblInput.Location = new System.Drawing.Point ( 12, 9 );
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size ( 23, 12 );
            this.lblInput.TabIndex = 1;
            this.lblInput.Text = "$$$";
            // 
            // dtpicker
            // 
            this.dtpicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpicker.Location = new System.Drawing.Point ( 287, 168 );
            this.dtpicker.Name = "dtpicker";
            this.dtpicker.ShowUpDown = true;
            this.dtpicker.Size = new System.Drawing.Size ( 112, 21 );
            this.dtpicker.TabIndex = 2;
            this.dtpicker.ValueChanged += new System.EventHandler ( this.dtpicker_ValueChanged );
            // 
            // lblSelectedDateTime
            // 
            this.lblSelectedDateTime.AutoSize = true;
            this.lblSelectedDateTime.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectedDateTime.Location = new System.Drawing.Point ( 12, 196 );
            this.lblSelectedDateTime.Name = "lblSelectedDateTime";
            this.lblSelectedDateTime.Size = new System.Drawing.Size ( 23, 12 );
            this.lblSelectedDateTime.TabIndex = 3;
            this.lblSelectedDateTime.Text = "$$$";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point ( 324, 215 );
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size ( 75, 23 );
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point ( 241, 215 );
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size ( 75, 23 );
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Location = new System.Drawing.Point ( 291, 9 );
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size ( 23, 12 );
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "$$$";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ( ( System.Drawing.Image ) ( resources.GetObject ( "pictureBox1.Image" ) ) );
            this.pictureBox1.Location = new System.Drawing.Point ( 287, 42 );
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size ( 100, 100 );
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // InputDateTime
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 96F, 96F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size ( 415, 243 );
            this.Controls.Add ( this.pictureBox1 );
            this.Controls.Add ( this.lblTime );
            this.Controls.Add ( this.mcDate );
            this.Controls.Add ( this.lblInput );
            this.Controls.Add ( this.dtpicker );
            this.Controls.Add ( this.lblSelectedDateTime );
            this.Controls.Add ( this.buttonCancel );
            this.Controls.Add ( this.buttonOK );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputDateTime";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            ( ( System.ComponentModel.ISupportInitialize ) ( this.pictureBox1 ) ).EndInit ( );
            this.ResumeLayout ( false );
            this.PerformLayout ( );

        }

        /// <summary>
        /// 
        /// </summary>
        private void InitResource()
        {
            Text = SR.GetString("DateTimeInputTitle");
            lblInput.Text = SR.GetString ( "DateTimeInputDate" );
            lblTime.Text = SR.GetString("DateTimeInputTime");
            buttonOK.Text = SR.GetString("OK");
            buttonCancel.Text = SR.GetString("Cancel");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 形成显示日期+时间
        /// </summary>
        /// <param name="dtSeleted"></param>
        /// <param name="showTime">是否显示时间</param>
        /// <returns></returns>
        private string FormatTime(DateTime dtSeleted, bool showTime)
        {
            StringBuilder strbResult = new StringBuilder();
            strbResult.Append(dtSeleted.ToString("D"));

            if (showTime)
            {
                strbResult.Append(" ");
                strbResult.Append(dtSeleted.ToString("T"));
            }

            return strbResult.ToString();
        }

        private void UpdateDateTime()
        {
            lblSelectedDateTime.Text = FormatTime(Value, true);
        }
        #endregion ...方法...

        #region ...事件...
        /// <summary>
        /// 日期控件的选择变化后发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcDate_DateChanged(object sender, DateRangeEventArgs e)
        {
            UpdateDateTime();
        }

        /// <summary>
        /// 时间控件属性改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpicker_ValueChanged(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void checkBoxTime_CheckedChanged(object sender, EventArgs e)
        {
            dtpicker.Enabled = true;

            UpdateDateTime();
        }
        #endregion ...事件...
    }
}
