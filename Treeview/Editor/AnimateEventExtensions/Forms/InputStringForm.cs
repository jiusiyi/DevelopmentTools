#region XDEComponent Begin Template Expansion{4ACCF9E5-99B9-4A99-BD71-A875E48E8CAD}

/**********************************************
ģ�����ƣ�	InputStringForm.cs
��Ȩ���У�	������˼���Զ���������޹�˾
ģ��������	����������
ʹ��˵����	��������ʵ�ִ������������л���������������Ի����ʵ��
ԭʼ���ߣ�	��ΰ
���ڣ�		2004-7-27
�����б�	���			��������
�޸ļ�¼��	ʱ��			����			�汾			�޸�����
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
	/// �ַ���ֵ��������̵�����Ի���
	/// </summary>
    internal class InputStringForm : System.Windows.Forms.Form
	{
		#region ...����...
		private TextBox textBoxString;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion ...����...

		#region ...����...
        public InputStringForm(string value, BaseShape shape, Control grpControl, AniInfoStringInput aniInfo)
		{
			//
			// Windows ���������֧���������
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
		#endregion ...����...

		#region ...����...
		/// <summary>
		/// ������������ʹ�õ���Դ��
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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
			if( keyData == Keys.Escape )		// ȡ����β���
			{
				if( OnCancel() )
					return true;
			}
			else if( keyData == Keys.Return )	// ȷ����β���
			{
				if( OnOK() )
					return true;
			}
			return base.ProcessDialogKey (keyData);
		}

		/// <summary>
		/// ȡ����β���
		/// </summary>
		/// <returns></returns>
		private bool OnCancel()
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
			return true;
		}

		/// <summary>
		/// ȷ����β���
		/// </summary>
		/// <returns></returns>
		private bool OnOK()
		{
			this.DialogResult = DialogResult.OK;
			Close();
			return true;
		}
		#endregion ...����...

		#region ...����...
		/// <summary>
		/// �õ���ǰ������ı�
		/// </summary>
		public string Value
		{
			get
			{
				return this.textBoxString.Text;
			}
		}
		#endregion ...����...
	}
}
