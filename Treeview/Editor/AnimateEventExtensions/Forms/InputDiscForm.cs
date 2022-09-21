#region XDEComponent Begin Template Expansion{76B7852F-D11A-4B16-B049-630804A34D02}

/**********************************************
模块名称：	InputDiscForm.cs
版权所有：	北京九思易自动化软件有限公司
模块描述：	动画连接类
使用说明：	动画连接实现窗口类用于运行环境动画连接输入对话框的实现
原始作者：	鲍伟
日期：		2004-7-27
问题列表：	序号			问题描述
修改记录：	时间			作者			版本			修改内容
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
    /// 离散值输入对话框
    /// </summary>
    internal class InputDiscForm : System.Windows.Forms.Form
    {
        #region ...变量...
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonClose;
        private bool currentValue = false;					// 当前值
        private System.Windows.Forms.Label labelPrompt;

        private AniInputDisc mAniInfo;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion ...变量...

        #region ...构造...
        public InputDiscForm ( bool discValue, BaseShape shape, Control grpControl, AniInputDisc aniInfo )
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();

 
            currentValue = discValue;
            buttonOpen.Text = aniInfo.TrueText;
            buttonClose.Text = aniInfo.FalseText;

            mAniInfo = aniInfo;


            //Location = AniInputText.GetInputFormLocation(shape, grpControl, Bounds);

            LoadResource();
        }
        #endregion ...构造...

        #region ...方法...

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

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
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
        #endregion ...方法...

        #region ...属性...
        /// <summary>
        /// 当前值
        /// </summary>
        public bool Value
        {
            get
            {
                return currentValue;
            }
        }
        #endregion ...属性...
    }
}
