#region XDEComponent Begin Template Expansion{4ACCF9E5-99B9-4A99-BD71-A875E48E8CAD}

/**********************************************
模块名称：	InputStringForm.cs
版权所有：	北京九思易自动化软件有限公司
模块描述：	动画连接类
使用说明：	动画连接实现窗口类用于运行环境动画连接输入对话框的实现
原始作者：	鲍伟
日期：		2004-7-27
问题列表：	序号			问题描述
修改记录：	时间			作者			版本			修改内容
**********************************************/
#endregion XDEComponent End Template Expansion{4ACCF9E5-99B9-4A99-BD71-A875E48E8CAD}
using System;
using System.Drawing;using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.AniInfos;

namespace ControlEase.Inspec.TreeView
{
	/// <summary>
	/// 字符串值无虚拟键盘的输入对话框
	/// </summary>
    internal class InputStringForm : System.Windows.Forms.Form
	{
		#region ...变量...
		private TextBox textBoxString;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion ...变量...

		#region ...构造...
        public InputStringForm(string value, BaseShape shape, Control grpControl, AniInfoStringInput aniInfo)
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//

			//
            if (aniInfo.PasswordInput)
            {
                textBoxString.PasswordChar = '*';
                this.textBoxString.Text = string.Empty;
            }
            else
            {
                this.textBoxString.Text = value;
            }

			textBoxString.SelectAll();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputStringForm));
            this.textBoxString = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxString
            // 
            resources.ApplyResources(this.textBoxString, "textBoxString");
            this.textBoxString.Name = "textBoxString";
            // 
            // InputStringForm
            // 
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.textBoxString);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputStringForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

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
			this.DialogResult = DialogResult.OK;
			Close();
			return true;
		}
		#endregion ...方法...

		#region ...属性...
		/// <summary>
		/// 得到当前输入的文本
		/// </summary>
		public string Value
		{
			get
			{
				return this.textBoxString.Text;
			}
		}
		#endregion ...属性...
	}
}
