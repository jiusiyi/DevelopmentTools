using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms.VisualStyles;
using ControlEase.Inspec.ViewCore;
using ControlEase.Inspec.Animates;

namespace ControlEase.Inspec.TreeView
{
	/// <summary>
	/// 为了解决button的某些问题，重载了button的方法
	/// </summary>
	public class ToggleButton : System.Windows.Forms.Button
	{
		#region ...变量...
		/// <summary>
		/// 是否为按下状态
		/// </summary>
		private bool isPressed = false;
		/// <summary>
		/// 是否为 Toggle Button
		/// </summary>
		protected bool isToggleButton = false;
		/// <summary>
		/// Toggle 状态
		/// </summary>
		protected bool toggleState = false;

        private bool mIsHoted = false;
		#endregion ...变量...

		#region ...构造...
		/// <summary>
		/// 构造
		/// </summary>
		public ToggleButton()
		{
			// Set the value of the double-buffering style bits to true.
			this.SetStyle(ControlStyles.DoubleBuffer | 
				ControlStyles.UserPaint | 
				ControlStyles.AllPaintingInWmPaint,
				true);
			this.UpdateStyles();
		}
		#endregion ...构造...

		#region ...属性...
		///<summary>
		/// 是否为 Toggle Button
		///</summary>
		[DefaultValue(false)]
		public bool IsToggleButton
		{
			get
			{
				return this.isToggleButton;
			}
			set
			{
				this.isToggleButton = value;
			}
		}

		///<summary>
		/// Toggle 状态
		///</summary>
		[DefaultValue(false)]
		public bool ToggleState
		{
			get
			{
				return this.toggleState;
			}
			set
			{
				this.toggleState = value;
			}
		}
		#endregion ...属性...

		#region ...方法...
		/// <summary>
		/// 设置 Toogle 状态
		/// </summary>
		/// <param name="state"></param>
		public void SetToggleState(bool state)
		{
			if(this.isToggleButton )
			{
				this.toggleState = state;
				this.Refresh();
			}
		}
		#endregion ...方法...

		#region ...事件...
		/// <summary>
		/// 绘制事件
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
            if (!isToggleButton)
            {
                base.OnPaint(e);
            }
            else
            {
                System.Drawing.Rectangle rect = ClientRectangle;
                Graphics g = e.Graphics;
                bool isPressed = false;

                if (this.isPressed)
                    isPressed = true;
                else if (this.isToggleButton && this.toggleState)
                    isPressed = true;

                PushButtonState state = PushButtonState.Normal;
                if (mIsHoted)
                    state = PushButtonState.Hot;
                if (isPressed)
                    state = PushButtonState.Pressed;
                if (!Enabled)
                    state = PushButtonState.Disabled;

                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }

                ButtonRenderer.DrawButton(e.Graphics, rect, state);

                TextFormatFlags flags = GetTextFormatFlags();
                TextRenderer.DrawText(e.Graphics, Text, Font, rect, ForeColor, flags);

                if (this.Focused)
                {
                    rect.Inflate(-2, -2);
                    ControlPaint.DrawFocusRectangle(e.Graphics, rect);
                }
            }
		}

        /// <summary>
        /// 获取指定文本字符串的显示和布局信息
        /// </summary>
        /// <returns></returns>
        private TextFormatFlags GetTextFormatFlags()
        {
            TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine;
            switch (TextAlign)
            {
                case System.Drawing.ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case System.Drawing.ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case System.Drawing.ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
                default:
                    break;
            }
            return flags;
        }

		/// <summary>
		/// 鼠标抬起事件
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if( this.isPressed )
			{
				this.isPressed = false;

				if(this.isToggleButton)
				{
					this.toggleState = !this.toggleState;
				}

				Invalidate();

				if(ClientRectangle.Contains(PointToClient(MousePosition)))
				{
					if(e.Button == MouseButtons.Left)
						PerformClick();
				}
			}

			base.OnMouseUp (e);
		}

		/// <summary>
		/// 鼠标按下事件
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.Capture = true;
			this.isPressed = true;

			Invalidate();

			base.OnMouseDown (e);
		}

		/// <summary>
		/// 鼠标移动事件
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if( this.isPressed )
			{
				if( !ClientRectangle.Contains(e.X, e.Y) )
				{
					this.isPressed = false;
					this.Capture = false;
					Invalidate();
				}
			}
			base.OnMouseMove (e);
		}

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            mIsHoted = true;
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            mIsHoted = false;
        }

		/// <summary>
		/// 消息过程
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if( m.Msg == (int)Msgs.WM_SETFOCUS )
			{
				IntPtr hwndPrevious = m.WParam;       // window handle
				Win32API.SetFocus(hwndPrevious);

				m.Result = (IntPtr)0;
				return;
			}
			else if( m.Msg == (int)Msgs.WM_SYSCOMMAND )
			{
				if( (int)SystemMenuCommand.SC_CLOSE == (int)m.WParam )
				{
					m.Result = (IntPtr)1;
					return;
				}
			}
			else if( m.Msg == (int)Msgs.WM_MOUSEACTIVATE )
			{
				m.Result = (IntPtr)MouseActivateFlags.MA_NOACTIVATE ;
				return;
			}
			else if( m.Msg == (int)Msgs.WM_ACTIVATE )
			{
				if( (uint)ActiveState.WA_ACTIVE == (uint)((uint)m.WParam & 0xffff) )
				{
					IntPtr hwndPrevious = m.LParam;       // window handle
					Win32API.SetActiveWindow( hwndPrevious ) ;
				}
				m.Result = (IntPtr)0;
				return;
			}

			base.WndProc (ref m);
		}
		#endregion ...事件...
	}
}
