#region XDEComponent Begin Template Expansion{943342FA-8D91-4137-93F6-029866C2431E}

/**********************************************
ģ�����ƣ�	InputAnalogForm.cs
��Ȩ���У�	������˼���Զ���������޹�˾
ģ��������	����������
ʹ��˵����	��������ʵ�ִ������������л���������������Ի����ʵ��
ԭʼ���ߣ�	��ΰ
���ڣ�		2004-7-27
�����б�	���			��������
�޸ļ�¼��	ʱ��			����			�汾			�޸�����
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
	/// ģ��ֵ��������̵�����Ի���
	/// </summary>
    internal class InputAnalogForm : System.Windows.Forms.Form
	{
		#region ...����...
		private NumEdit textBoxAnalog;

        private Decimal minValue = Decimal.MinValue;	// ģ��ֵ�������Сֵ

        private Decimal maxValue = Decimal.MaxValue;	// ģ��ֵ��������ֵ

		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

        private bool mIsErro = false;

		#endregion ...����...

		#region ...����...
        /// <summary>
        /// ����һ�� InputAnalogForm ���ʵ��
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shape"></param>
        /// <param name="grpControl"></param>
        /// <param name="aniInfo"></param>
        public InputAnalogForm ( Decimal value, BaseShape shape, Control grpControl, AniInputAnalog aniInfo )
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			this.textBoxAnalog.Text = value.ToString();

			textBoxAnalog.SelectAll();

            minValue = aniInfo.ValueMin;
            maxValue = aniInfo.ValueMax;
            textBoxAnalog.DecimalNum = aniInfo.DecimalNum;

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
			this.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
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
		#endregion ...����...

		#region ...����...
		/// <summary>
		/// �ı��������
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
		#endregion ...����...
	}
}
