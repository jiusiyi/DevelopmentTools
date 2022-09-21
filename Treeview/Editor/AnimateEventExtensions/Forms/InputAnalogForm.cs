#region XDEComponent Begin Template Expansion{943342FA-8D91-4137-93F6-029866C2431E}

/**********************************************
模块名称：	InputAnalogForm.cs
版权所有：	北京九思易自动化软件有限公司
模块描述：	动画连接类
使用说明：	动画连接实现窗口类用于运行环境动画连接输入对话框的实现
原始作者：	鲍伟
日期：		2004-7-27
问题列表：	序号			问题描述
修改记录：	时间			作者			版本			修改内容
**********************************************/
#endregion XDEComponent End Template Expansion{943342FA-8D91-4137-93F6-029866C2431E}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.AniInfos;
using Properties = ControlEase.Inspec.ViewCore.Properties;

namespace ControlEase.Inspec.TreeView
{
	/// <summary>
	/// 模拟值无虚拟键盘的输入对话框
	/// </summary>
    internal class InputAnalogForm : System.Windows.Forms.Form
	{
		#region ...变量...
		private NumEdit textBoxAnalog;

        private Decimal minValue = Decimal.MinValue;	// 模拟值输入的最小值

        private Decimal maxValue = Decimal.MaxValue;	// 模拟值输入的最大值

		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

        private bool mIsErro = false;

		#endregion ...变量...

		#region ...构造...
        /// <summary>
        /// 构造一个 InputAnalogForm 类的实例
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shape"></param>
        /// <param name="grpControl"></param>
        /// <param name="aniInfo"></param>
        public InputAnalogForm ( Decimal value, BaseShape shape, Control grpControl, AniInputAnalog aniInfo )
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			this.textBoxAnalog.Text = value.ToString();

			textBoxAnalog.SelectAll();

            minValue = aniInfo.ValueMin;
            maxValue = aniInfo.ValueMax;
            textBoxAnalog.DecimalNum = aniInfo.DecimalNum;

            //Location = AniInputText.GetInputFormLocation(shape, grpControl, Bounds);
		}
		#endregion ...构造...

		#region ...方法...
		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxAnalog = new ControlEase.Inspec.ViewCore.NumEdit();
			this.SuspendLayout();
			// 
			// textBoxAnalog
			// 
			this.textBoxAnalog.InputType = ControlEase.Inspec.ViewCore.NumEdit.NumEditType.Double;
			this.textBoxAnalog.Location = new System.Drawing.Point(2, 3);
			this.textBoxAnalog.Name = "textBoxAnalog";
			this.textBoxAnalog.Size = new System.Drawing.Size(168, 21);
			this.textBoxAnalog.TabIndex = 0;
			this.textBoxAnalog.Text = "";
			// 
			// InputAnalogForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(172, 27);
			this.ControlBox = false;
			this.Controls.Add(this.textBoxAnalog);
			this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "InputAnalogForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.ResumeLayout(false);

		}
		#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if( keyData == Keys.Escape )		// 取消这次操作
			{
				if( OnCancel() )
					return true;
			}
			else if( keyData == Keys.Return )	// 确认这次操作
			{
				if( OnOK() )
					return true;
			}
			return base.ProcessDialogKey (keyData);
		}

		/// <summary>
		/// 取消这次操作
		/// </summary>
		/// <returns></returns>
		private bool OnCancel()
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
			return true;
		}

		/// <summary>
		/// 确认这次操作
		/// </summary>
		/// <returns></returns>
		private bool OnOK()
		{
            Decimal dValue = Value;
            if (mIsErro)
			{
                ControlEase.Nexus.Windows.MessageBox.Show(Properties.Resources.InvalidInput,
					Properties.Resources.Error, System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
				textBoxAnalog.SelectAll();
				this.textBoxAnalog.Focus();

				return false;
			}
			else if( dValue < this.minValue )
			{
                ControlEase.Nexus.Windows.MessageBox.Show(Properties.Resources.SmallerthanMinValue,
                    Properties.Resources.Error, System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
				textBoxAnalog.SelectAll();
				this.textBoxAnalog.Focus();

				return false;
			}
			else if( dValue > this.maxValue )
			{
                ControlEase.Nexus.Windows.MessageBox.Show(Properties.Resources.GreaterthanMaxValue,
                    Properties.Resources.Error, System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
				textBoxAnalog.SelectAll();
				this.textBoxAnalog.Focus();

				return false;
			}

			this.DialogResult = DialogResult.OK;
			Close();
			return true;
		}
		#endregion ...方法...

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
                    mIsErro = false;
					return Convert.ToDecimal(textBoxAnalog.Text);
				}
				catch( Exception )
				{
                    mIsErro = true;
                    return Decimal.MaxValue;
				}
			}
		}
		#endregion ...属性...
	}
}
