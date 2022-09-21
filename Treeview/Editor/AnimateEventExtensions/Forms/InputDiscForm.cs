#region XDEComponent Begin Template Expansion{76B7852F-D11A-4B16-B049-630804A34D02}

/**********************************************
ģ�����ƣ�	InputDiscForm.cs
��Ȩ���У�	������˼���Զ���������޹�˾
ģ��������	����������
ʹ��˵����	��������ʵ�ִ������������л���������������Ի����ʵ��
ԭʼ���ߣ�	��ΰ
���ڣ�		2004-7-27
�����б�	���			��������
�޸ļ�¼��	ʱ��			����			�汾			�޸�����
**********************************************/
#endregion XDEComponent End Template Expansion{76B7852F-D11A-4B16-B049-630804A34D02}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.AniInfos;
using ControlEase.Inspec.TreeView;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// ��ɢֵ����Ի���
    /// </summary>
    internal class InputDiscForm : System.Windows.Forms.Form
    {
        #region ...����...
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonClose;
        private bool currentValue = false;					// ��ǰֵ
        private System.Windows.Forms.Label labelPrompt;

        private AniInputDisc mAniInfo;

        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion ...����...

        #region ...����...
        public InputDiscForm ( bool discValue, BaseShape shape, Control grpControl, AniInputDisc aniInfo )
        {
            //
            // Windows ���������֧���������
            //
            InitializeComponent();

 
            currentValue = discValue;
            buttonOpen.Text = aniInfo.TrueText;
            buttonClose.Text = aniInfo.FalseText;

            mAniInfo = aniInfo;


            //Location = AniInputText.GetInputFormLocation(shape, grpControl, Bounds);

            LoadResource();
        }
        #endregion ...����...

        #region ...����...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        /// <summary>
        /// ������������ʹ�õ���Դ��
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
            this.Text = SR.GetString("InputDisc_Title");
            this.buttonCancel.Text = SR.GetString("Cancel");
        }

        #region Windows ������������ɵĴ���
        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputDiscForm));
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOpen
            // 
            resources.ApplyResources(this.buttonOpen, "buttonOpen");
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            // 
            // labelPrompt
            // 
            this.labelPrompt.AutoEllipsis = true;
            resources.ApplyResources(this.labelPrompt, "labelPrompt");
            this.labelPrompt.Name = "labelPrompt";
            // 
            // InputDiscForm
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.labelPrompt);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputDiscForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);

        }
        #endregion

        private void buttonOpen_Click(object sender, System.EventArgs e)
        {
            currentValue = true;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            currentValue = false;
            this.DialogResult = DialogResult.OK;
            Close();
        }
        #endregion ...����...

        #region ...����...
        /// <summary>
        /// ��ǰֵ
        /// </summary>
        public bool Value
        {
            get
            {
                return currentValue;
            }
        }
        #endregion ...����...
    }
}
