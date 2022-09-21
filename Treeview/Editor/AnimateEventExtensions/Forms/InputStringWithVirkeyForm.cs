using System;
using System.Drawing;using System.Drawing.Drawing2D;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.AniInfos;
using ControlEase.Inspec.Animates;

namespace ControlEase.Inspec.TreeView
{
	/// <summary>
	/// 字符串值有虚拟键盘的输入对话框
	/// </summary>
    internal class InputStringWithVirkeyForm : System.Windows.Forms.Form
	{
		#region ...嵌入类...
		/// <summary>
		/// Shift 按下前后输入键的内容
		/// </summary>
		public struct ShiftKeys
		{
			public char NormalKey;
			public char ShiftKey;

			public ShiftKeys(char normalKey, char shiftKey)
			{
				NormalKey	= normalKey;
				ShiftKey	= shiftKey;
			}
		}
		#endregion ...嵌入类...

		#region ...变量...
		private ShiftKeys[] mShiftKeys = new ShiftKeys[]{new ShiftKeys('`', '~'), new ShiftKeys('1', '!'),
															new ShiftKeys('2', '@'), new ShiftKeys('3', '#'),
															new ShiftKeys('4', '$'), new ShiftKeys('5', '%'),
															new ShiftKeys('6', '^'), new ShiftKeys('7', '&'),
															new ShiftKeys('8', '*'), new ShiftKeys('9', '('),
															new ShiftKeys('0', ')'), new ShiftKeys('-', '_'),
															new ShiftKeys('=', '+'), new ShiftKeys('\\', '|'),
															new ShiftKeys('[', '{'), new ShiftKeys(']', '}'),
															new ShiftKeys(';', ':'), new ShiftKeys('\'', '"'),
															new ShiftKeys(',', '<'), new ShiftKeys('.', '>'),
															new ShiftKeys('/', '?'),};

		private System.Windows.Forms.Label labelPrompt;
		private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private ToggleButton buttonBackSpace;
		private ToggleButton buttonPipe;
		private ToggleButton buttonOpenBrackets;
		private ToggleButton buttonCloseBrackets;
		private ToggleButton buttonO;
		private ToggleButton buttonP;
		private ToggleButton buttonU;
		private ToggleButton buttonI;
		private ToggleButton buttonT;
		private ToggleButton buttonY;
		private ToggleButton buttonE;
		private ToggleButton buttonR;
		private ToggleButton buttonQ;
		private ToggleButton buttonW;
		private ToggleButton buttonD;
		private ToggleButton buttonA;
		private ToggleButton buttonS;
		private ToggleButton buttonSpace;
		private ToggleButton buttonShift;
		private ToggleButton buttonZ;
		private ToggleButton buttonQuotes;
		private ToggleButton buttonL;
		private ToggleButton buttonSemicolon;
		private ToggleButton buttonJ;
		private ToggleButton buttonK;
		private ToggleButton buttonG;
		private ToggleButton buttonH;
		private ToggleButton buttonF;
		private ToggleButton buttonN;
		private ToggleButton buttonM;
		private ToggleButton buttonV;
		private ToggleButton buttonB;
		private ToggleButton buttonX;
		private ToggleButton buttonC;
		private ToggleButton buttonSubtract;
		private ToggleButton buttonDivide;
		private ToggleButton buttonComma;
		private ToggleButton buttonPeriod;
		private ToggleButton buttonAdd;
		private ToggleButton buttonTilde;
		private ToggleButton button1;
		private ToggleButton button2;
		private ToggleButton button3;
		private ToggleButton button6;
		private ToggleButton button7;
		private ToggleButton button4;
		private ToggleButton button5;
		private ToggleButton button0;
		private ToggleButton button8;
		private ToggleButton button9;
		private System.Windows.Forms.TextBox textBoxString;
		private ToggleButton buttonCapsLock;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel6;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel4;
        private ToolTip toolTip1;
        private IContainer components;

        private AniInputString mAniInfo;

        #endregion ...变量...

        #region ...构造...
        public InputStringWithVirkeyForm ( string value, BaseShape shape, Control grpControl, AniInputString aniInfo )
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
                textBoxString.Text = string.Empty;
            }
            else
            {
                this.textBoxString.Text = value;    
            }
				
            
			
			textBoxString.SelectAll();
            //Location = AniInputText.GetInputFormLocation(shape, grpControl, Bounds);

            mAniInfo = aniInfo;


			UpdateCapsButton();

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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				TurnoffShiftKey();

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputStringWithVirkeyForm));
            this.labelPrompt = new System.Windows.Forms.Label();
            this.textBoxString = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAdd = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonBackSpace = new ControlEase.Inspec.TreeView.ToggleButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTilde = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button1 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPipe = new ControlEase.Inspec.TreeView.ToggleButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonQ = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonW = new ControlEase.Inspec.TreeView.ToggleButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonQuotes = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonShift = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonDivide = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button2 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonSpace = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonComma = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button3 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button4 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonM = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonN = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button5 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button6 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonB = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonV = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button7 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonC = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonX = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button8 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonO = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button9 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.button0 = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonZ = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonSubtract = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonCapsLock = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonSemicolon = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonL = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonE = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonR = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonK = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonJ = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonT = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonY = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonH = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonG = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonU = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonI = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonF = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonD = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonP = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonS = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonA = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonOpenBrackets = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonCloseBrackets = new ControlEase.Inspec.TreeView.ToggleButton();
            this.buttonPeriod = new ControlEase.Inspec.TreeView.ToggleButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPrompt
            // 
            resources.ApplyResources(this.labelPrompt, "labelPrompt");
            this.labelPrompt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelPrompt.Name = "labelPrompt";
            // 
            // textBoxString
            // 
            resources.ApplyResources(this.textBoxString, "textBoxString");
            this.textBoxString.Name = "textBoxString";
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
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 11, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 11, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 11, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonShift, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonDivide, 10, 3);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSpace, 11, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonComma, 9, 3);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonM, 7, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonN, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.button5, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.button6, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonB, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonV, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.button7, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonC, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonX, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.button8, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonO, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.button9, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.button0, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonZ, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonSubtract, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCapsLock, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonSemicolon, 10, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonL, 9, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonE, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonR, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonK, 8, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonJ, 7, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonT, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonY, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonH, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonG, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonU, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonI, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonF, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonD, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonP, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonS, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonA, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonOpenBrackets, 9, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonCloseBrackets, 10, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonPeriod, 8, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.buttonAdd, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.buttonBackSpace, 1, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonBackSpace
            // 
            resources.ApplyResources(this.buttonBackSpace, "buttonBackSpace");
            this.buttonBackSpace.Name = "buttonBackSpace";
            this.toolTip1.SetToolTip(this.buttonBackSpace, resources.GetString("buttonBackSpace.ToolTip"));
            this.buttonBackSpace.Click += new System.EventHandler(this.button_Click);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.buttonTilde, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // buttonTilde
            // 
            resources.ApplyResources(this.buttonTilde, "buttonTilde");
            this.buttonTilde.Name = "buttonTilde";
            this.buttonTilde.Click += new System.EventHandler(this.button_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.Click += new System.EventHandler(this.button_Click);
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.buttonPipe, 0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // buttonPipe
            // 
            resources.ApplyResources(this.buttonPipe, "buttonPipe");
            this.buttonPipe.Name = "buttonPipe";
            this.buttonPipe.Click += new System.EventHandler(this.button_Click);
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.buttonQ, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonW, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // buttonQ
            // 
            resources.ApplyResources(this.buttonQ, "buttonQ");
            this.buttonQ.Name = "buttonQ";
            this.buttonQ.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonW
            // 
            resources.ApplyResources(this.buttonW, "buttonW");
            this.buttonW.Name = "buttonW";
            this.buttonW.Click += new System.EventHandler(this.button_Click);
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.buttonQuotes, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // buttonQuotes
            // 
            resources.ApplyResources(this.buttonQuotes, "buttonQuotes");
            this.buttonQuotes.Name = "buttonQuotes";
            this.buttonQuotes.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonShift
            // 
            resources.ApplyResources(this.buttonShift, "buttonShift");
            this.buttonShift.IsToggleButton = true;
            this.buttonShift.Name = "buttonShift";
            this.toolTip1.SetToolTip(this.buttonShift, resources.GetString("buttonShift.ToolTip"));
            this.buttonShift.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonDivide
            // 
            resources.ApplyResources(this.buttonDivide, "buttonDivide");
            this.buttonDivide.Name = "buttonDivide";
            this.buttonDivide.Click += new System.EventHandler(this.button_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonSpace
            // 
            resources.ApplyResources(this.buttonSpace, "buttonSpace");
            this.buttonSpace.Name = "buttonSpace";
            this.toolTip1.SetToolTip(this.buttonSpace, resources.GetString("buttonSpace.ToolTip"));
            this.buttonSpace.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonComma
            // 
            resources.ApplyResources(this.buttonComma, "buttonComma");
            this.buttonComma.Name = "buttonComma";
            this.buttonComma.Click += new System.EventHandler(this.button_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.Click += new System.EventHandler(this.button_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonM
            // 
            resources.ApplyResources(this.buttonM, "buttonM");
            this.buttonM.Name = "buttonM";
            this.buttonM.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonN
            // 
            resources.ApplyResources(this.buttonN, "buttonN");
            this.buttonN.Name = "buttonN";
            this.buttonN.Click += new System.EventHandler(this.button_Click);
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.Click += new System.EventHandler(this.button_Click);
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            this.button6.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonB
            // 
            resources.ApplyResources(this.buttonB, "buttonB");
            this.buttonB.Name = "buttonB";
            this.buttonB.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonV
            // 
            resources.ApplyResources(this.buttonV, "buttonV");
            this.buttonV.Name = "buttonV";
            this.buttonV.Click += new System.EventHandler(this.button_Click);
            // 
            // button7
            // 
            resources.ApplyResources(this.button7, "button7");
            this.button7.Name = "button7";
            this.button7.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonC
            // 
            resources.ApplyResources(this.buttonC, "buttonC");
            this.buttonC.Name = "buttonC";
            this.buttonC.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonX
            // 
            resources.ApplyResources(this.buttonX, "buttonX");
            this.buttonX.Name = "buttonX";
            this.buttonX.Click += new System.EventHandler(this.button_Click);
            // 
            // button8
            // 
            resources.ApplyResources(this.button8, "button8");
            this.button8.Name = "button8";
            this.button8.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonO
            // 
            resources.ApplyResources(this.buttonO, "buttonO");
            this.buttonO.Name = "buttonO";
            this.buttonO.Click += new System.EventHandler(this.button_Click);
            // 
            // button9
            // 
            resources.ApplyResources(this.button9, "button9");
            this.button9.Name = "button9";
            this.button9.Click += new System.EventHandler(this.button_Click);
            // 
            // button0
            // 
            resources.ApplyResources(this.button0, "button0");
            this.button0.Name = "button0";
            this.button0.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonZ
            // 
            resources.ApplyResources(this.buttonZ, "buttonZ");
            this.buttonZ.Name = "buttonZ";
            this.buttonZ.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonSubtract
            // 
            resources.ApplyResources(this.buttonSubtract, "buttonSubtract");
            this.buttonSubtract.Name = "buttonSubtract";
            this.buttonSubtract.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCapsLock
            // 
            resources.ApplyResources(this.buttonCapsLock, "buttonCapsLock");
            this.buttonCapsLock.Name = "buttonCapsLock";
            this.toolTip1.SetToolTip(this.buttonCapsLock, resources.GetString("buttonCapsLock.ToolTip"));
            this.buttonCapsLock.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonSemicolon
            // 
            resources.ApplyResources(this.buttonSemicolon, "buttonSemicolon");
            this.buttonSemicolon.Name = "buttonSemicolon";
            this.buttonSemicolon.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonL
            // 
            resources.ApplyResources(this.buttonL, "buttonL");
            this.buttonL.Name = "buttonL";
            this.buttonL.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonE
            // 
            resources.ApplyResources(this.buttonE, "buttonE");
            this.buttonE.Name = "buttonE";
            this.buttonE.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonR
            // 
            resources.ApplyResources(this.buttonR, "buttonR");
            this.buttonR.Name = "buttonR";
            this.buttonR.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonK
            // 
            resources.ApplyResources(this.buttonK, "buttonK");
            this.buttonK.Name = "buttonK";
            this.buttonK.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonJ
            // 
            resources.ApplyResources(this.buttonJ, "buttonJ");
            this.buttonJ.Name = "buttonJ";
            this.buttonJ.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonT
            // 
            resources.ApplyResources(this.buttonT, "buttonT");
            this.buttonT.Name = "buttonT";
            this.buttonT.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonY
            // 
            resources.ApplyResources(this.buttonY, "buttonY");
            this.buttonY.Name = "buttonY";
            this.buttonY.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonH
            // 
            resources.ApplyResources(this.buttonH, "buttonH");
            this.buttonH.Name = "buttonH";
            this.buttonH.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonG
            // 
            resources.ApplyResources(this.buttonG, "buttonG");
            this.buttonG.Name = "buttonG";
            this.buttonG.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonU
            // 
            resources.ApplyResources(this.buttonU, "buttonU");
            this.buttonU.Name = "buttonU";
            this.buttonU.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonI
            // 
            resources.ApplyResources(this.buttonI, "buttonI");
            this.buttonI.Name = "buttonI";
            this.buttonI.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonF
            // 
            resources.ApplyResources(this.buttonF, "buttonF");
            this.buttonF.Name = "buttonF";
            this.buttonF.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonD
            // 
            resources.ApplyResources(this.buttonD, "buttonD");
            this.buttonD.Name = "buttonD";
            this.buttonD.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonP
            // 
            resources.ApplyResources(this.buttonP, "buttonP");
            this.buttonP.Name = "buttonP";
            this.buttonP.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonS
            // 
            resources.ApplyResources(this.buttonS, "buttonS");
            this.buttonS.Name = "buttonS";
            this.buttonS.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonA
            // 
            resources.ApplyResources(this.buttonA, "buttonA");
            this.buttonA.Name = "buttonA";
            this.buttonA.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonOpenBrackets
            // 
            resources.ApplyResources(this.buttonOpenBrackets, "buttonOpenBrackets");
            this.buttonOpenBrackets.Name = "buttonOpenBrackets";
            this.buttonOpenBrackets.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCloseBrackets
            // 
            resources.ApplyResources(this.buttonCloseBrackets, "buttonCloseBrackets");
            this.buttonCloseBrackets.Name = "buttonCloseBrackets";
            this.buttonCloseBrackets.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPeriod
            // 
            resources.ApplyResources(this.buttonPeriod, "buttonPeriod");
            this.buttonPeriod.Name = "buttonPeriod";
            this.buttonPeriod.Click += new System.EventHandler(this.button_Click);
            // 
            // InputStringWithVirkeyForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxString);
            this.Controls.Add(this.labelPrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputStringWithVirkeyForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void button_Click(object sender, System.EventArgs e)
		{
			//Debug.Assert( sender is Button );
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;

			byte cKey = 0;
			bool bWordKey = false;

			if( button == buttonCapsLock )			// CapsLock
			{
				UpdateCapsButton(!IsCaps);

				cKey = (byte)VirtualKeys.VK_CAPITAL;
			}
			else if( button == buttonBackSpace )	// BackSpace
			{
				cKey = (byte)VirtualKeys.VK_BACK;
			}
			else if( button == buttonShift )		// Shift
			{
				cKey = (byte)VirtualKeys.VK_SHIFT;
			}
			else if( button == buttonSpace )		// Space
			{
				cKey = (byte)VirtualKeys.VK_SPACE;
			}
			else
			{
				Debug.Assert( button.Text != string.Empty );
				cKey = Convert.ToByte(button.Text[0]);
				bWordKey = true;
			}

			if( bWordKey )//common key
			{
				short ks = Win32API.VkKeyScan((char)cKey);
				byte key = (byte)(ks & 0xFF);

				Win32API.keybd_event(key,0,0,(IntPtr)0);
				Win32API.keybd_event(key,0, (uint)KeyEventTF.KEYEVENTF_KEYUP,(IntPtr)0);

				TurnoffShiftKey();
			}
			else
			{
				if( cKey != (byte)VirtualKeys.VK_SHIFT )
				{
					Win32API.keybd_event(cKey,0,0,(IntPtr)0);
					Win32API.keybd_event(cKey,0, (uint)KeyEventTF.KEYEVENTF_KEYUP,(IntPtr)0);

					TurnoffShiftKey();
				}
				else
				{
					if( (Win32API.GetKeyState(cKey) & 0xF000) > 0 )
					{
						Win32API.keybd_event(cKey,0, (uint)KeyEventTF.KEYEVENTF_KEYUP,(IntPtr)0);
//						buttonShift.SetToggleState(false);
					}
					else
					{
						Win32API.keybd_event(cKey,0,0,(IntPtr)0);
//						buttonShift.SetToggleState(true);
					}

					UpdateShiftButton();
				}
			}
		}

		/// <summary>
		/// 更新按钮文本大小写
		/// </summary>
		/// <param name="isCaps"></param>
		private void UpdateCapsButton(bool isCaps)
		{
			foreach( Control control in this.Controls )
			{
				if( control is System.Windows.Forms.Button )
				{
                    System.Windows.Forms.Button button = control as System.Windows.Forms.Button;
					if( button.Text.Length == 1 && char.IsLetter(button.Text, 0) )
					{
						button.Text = isCaps ? button.Text.ToUpper() : button.Text.ToLower();
					}
				}
			}
		}

		/// <summary>
		/// 更新按钮文本大小写状态
		/// </summary>
		private void UpdateCapsButton()
		{
			UpdateCapsButton(IsCaps);
		}

		/// <summary>
		/// 更新按钮文本 Shift 状态
		/// </summary>
		private void UpdateShiftButton()
		{
			bool isShift = ( (Win32API.GetKeyState((int)VirtualKeys.VK_SHIFT) & 0xF000) > 0 );

			foreach( Control control in this.Controls )
			{
                if ( control is System.Windows.Forms.Button )
				{
                    System.Windows.Forms.Button button = control as System.Windows.Forms.Button;
					if( button.Text.Length >= 1 && button.Text.Length <= 2 )
					{
						if( char.IsLetter(button.Text, 0) )
						{
							bool shift = IsCaps ? isShift : !isShift;
							button.Text = shift ? button.Text.ToUpper() : button.Text.ToLower();
						}
						else
						{
							foreach( ShiftKeys key in mShiftKeys )
							{
								if( !isShift )
								{
									if( button.Text[0] == key.NormalKey )
									{
										if( key.ShiftKey == '&' )
											button.Text = new string(key.ShiftKey, 2);
										else
											button.Text = new string(key.ShiftKey, 1);
										break;
									}
								}
								else
								{
									if( button.Text[0] == key.ShiftKey )
									{
										button.Text = new string(key.NormalKey, 1);
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 如果Shift 键被按下关闭 Shift 键
		/// </summary>
		private void TurnoffShiftKey()
		{
			//turn off the shift if they were down
			int vk = (int)VirtualKeys.VK_SHIFT;
			if( (Win32API.GetKeyState(vk) & 0xF000) > 0 )
			{
				UpdateShiftButton();

				Win32API.keybd_event((byte)vk,0, (uint)KeyEventTF.KEYEVENTF_KEYUP,(IntPtr)0);
                buttonShift.SetToggleState(false);
			}	
		}

        /// <summary>
        /// 
        /// </summary>
        public void LoadResource()
        {
            this.Text = SR.GetString("InputString_Title");
            this.labelPrompt.Text = SR.GetString ( "InputAnalog_pleasesInput" );
            this.buttonOK.Text = SR.GetString("OK");
            this.buttonCancel.Text = SR.GetString("Cancel");
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

		/// <summary>
		/// 获取是否为大写状态
		/// </summary>
		private bool IsCaps
		{
			get
			{
				return Win32API.GetKeyState((byte)VirtualKeys.VK_CAPITAL) == 0 ? false : true;
			}
		}
		#endregion ...属性...
	}
}
