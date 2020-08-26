/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：508dafaf-8d9d-445b-94a8-1ab08bb7a9f0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：HttpTool
 * 类名称：MyTextBox
 * 创建时间：2016/11/14 20:26:34
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace WEF.ModelGenerator.Controls
{
    public partial class MyTextBox : UserControl
    {
        private bool _isLeftDown; //鼠标左键是否点下   


        private int _pageLine; //当前文本框内容所能显示的行数  

        public MyTextBox()
        {
            InitializeComponent();
            timer1 = new Timer
            {
                Interval = 50
            };
        }

        private void MyTextBox_Load(object sender, EventArgs e)
        {
            txtContent.MouseWheel += txtContect_MouseWheel;
            txtContent.TextChanged += txtContect_TextChanged;
            txtContent.KeyDown += txtContent_KeyDown;
            txtContent.KeyUp += txtContent_KeyUp;
            txtContent.MouseDown += txtContent_MouseDown;
            txtContent.MouseMove += txtContent_MouseMove;
            txtContent.MouseUp += txtContent_MouseUp;
            txtContent.TextChanged += txtRow_TextChanged;

            timer1.Tick += timer1_Tick;
        }

        private void txtContect_TextChanged(object sender, EventArgs e)
        {
            //调用顺序不可变  
            SetScrollBar();
            ShowRow();
            ShowCursorLine();
        }

        //鼠标滚动  
        private void txtContect_MouseWheel(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
        }

        // 上、下键  
        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Up) || (e.KeyData == Keys.Down))
                SetScrollBar();

            if ((e.Modifiers == Keys.Control) && (e.KeyCode == Keys.A))
                ((TextBox)sender).SelectAll();
        }

        private void txtContent_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Up) || (e.KeyData == Keys.Down))
                ShowCursorLine();
        }

        //点击滚动条  
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            var t = SetScrollPos(txtContent.Handle, 1, vScrollBar1.Value, true);
            SendMessage(txtContent.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * vScrollBar1.Value, 0);
            ShowRow();
        }

        //显示光标行  
        private void txtContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _isLeftDown = true;
            ShowCursorLine();
        }

        //鼠标选择内容上下移动  
        private void txtContent_MouseMove(object sender, MouseEventArgs e)
        {
            SetScrollBar();
        }

        private void txtContent_MouseUp(object sender, MouseEventArgs e)
        {
            _isLeftDown = false;
        }

        //文本框大小改变  
        private void txtContent_SizeChanged(object sender, EventArgs e)
        {
            var si = new SCROLLINFO();
            si.cbSize = (uint)Marshal.SizeOf(si);
            si.fMask = SIF_ALL;
            var r = GetScrollInfo(txtContent.Handle, SB_VERT, ref si);
            _pageLine = (int)si.nPage;
            timer1.Enabled = true;
            ShowRow();
        }

        //行显示栏宽度自适应  
        private void txtRow_TextChanged(object sender, EventArgs e)
        {
            if (txtRow.Lines.Length > 0)
            {
                var s = txtRow.CreateGraphics().MeasureString(txtRow.Lines[txtRow.Lines.Length - 1], txtRow.Font);
                txtRow.Width = (int)s.Width + 25;
                txtRow.Width = 43;
            }
        }

        private void txtRow_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret((sender as TextBox).Handle);
        }

        #region 自定义属性/方法

        public string SelectedText
        {
            get
            {
                return txtContent.SelectedText;
                ;
            }
            set
            {
                txtContent.SelectedText = value;
            }
        }

        public int SelectionLength
        {
            get
            {
                return txtContent.SelectionLength;
                ;
            }
            set
            {
                txtContent.SelectionLength = value;
            }
        }

        public int SelectionStart
        {
            get
            {
                return txtContent.SelectionStart;
                ;
            }
            set
            {
                txtContent.SelectionStart = value;
            }
        }

        public new string Text
        {
            get
            {
                return txtContent.Text;
            }
            set
            {
                txtContent.Text = value;
            }
        }
        public int MaxLength
        {
            get
            {
                return txtContent.MaxLength;
            }
            set
            {
                txtContent.MaxLength = value;
            }
        }
        public System.Windows.Forms.ScrollBars ScrollBars
        {
            get
            {
                return txtContent.ScrollBars;
            }
            set
            {
                txtContent.ScrollBars = value;
            }
        }
        public new ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return txtContent.ContextMenuStrip;
            }
            set
            {
                txtContent.ContextMenuStrip = value;
            }
        }

        public bool Multiline
        {
            get; set;
        }

        public void ScrollToCaret(int position)
        {
            txtContent.SelectionStart = position;
            txtContent.SelectionLength = 1;
            txtContent.ScrollToCaret();
        }

        public int TextLength
        {
            get
            {
                return txtContent.TextLength;
            }
        }

        #endregion

        #region Method  

        private void ShowCursorLine()
        {
            //toolStripStatusLabel1.Text = "行: " + (this.txtContent.GetLineFromCharIndex(this.txtContent.SelectionStart) + 1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SetScrollBar();
            timer1.Enabled = false;
        }

        private void SetScrollBar()
        {
            var si = new SCROLLINFO();
            si.cbSize = (uint)Marshal.SizeOf(si);
            si.fMask = SIF_ALL;
            var r = GetScrollInfo(txtContent.Handle, SB_VERT, ref si);
            _pageLine = (int)si.nPage;
            vScrollBar1.LargeChange = _pageLine;

            if (si.nMax >= si.nPage)
            {
                vScrollBar1.Visible = true;
                vScrollBar1.Maximum = si.nMax;
                vScrollBar1.Value = si.nPos;
            }
            else
                vScrollBar1.Visible = false;
        }

        private void ShowRow()
        {
            var firstLine = txtContent.GetLineFromCharIndex(txtContent.GetCharIndexFromPosition(new Point(0, 2)));
            var lin = new string[_pageLine];
            for (var i = 0; i < _pageLine; i++)
                lin[i] = (i + firstLine + 1).ToString();
            txtRow.Lines = lin;
        }

        #endregion

        #region 调用 API  

        public static uint SIF_RANGE = 0x0001;
        public static uint SIF_PAGE = 0x0002;
        public static uint SIF_POS = 0x0004;
        public static uint SIF_TRACKPOS = 0x0010;
        public static uint SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
        public int SB_THUMBPOSITION = 4;
        public int SB_VERT = 1;
        public int WM_VSCROLL = 0x0115;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetScrollInfo(IntPtr hwnd, int bar, ref SCROLLINFO si);

        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hwnd, int nbar);

        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool Rush);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);


        [DllImport("user32", EntryPoint = "HideCaret")]
        private static extern bool HideCaret(IntPtr hWnd);

        #endregion
    }
}