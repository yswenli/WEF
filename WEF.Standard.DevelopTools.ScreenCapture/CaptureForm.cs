using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Common.Win32;

namespace WEF.Standard.DevelopTools.Capture
{
    /// <summary>
    /// 截图功能窗体
    /// </summary>
    public partial class CaptureForm : Form
    {

        public delegate void OnCapturedHandler(Image imgDatas);
        /// <summary>
        /// 抓屏结束事件
        /// </summary>
        public event OnCapturedHandler OnCaptured;
        /// <summary>
        /// 抓屏结束事件
        /// </summary>
        /// <param name="picDatas"></param>
        protected void RaiseOnCaptured(Image imgDatas)
        {
            if (this.OnCaptured != null) this.OnCaptured(imgDatas);
        }

        private void DelResource()
        {
            if (captureDatas != null) captureDatas.Dispose();
            if (m_bmpLayerShow != null) m_bmpLayerShow.Dispose();
            m_layer.Clear();
            imageProcessBox1.DeleResource();
            GC.Collect();
        }

        #region Properties

        private bool isCaptureCursor;
        /// <summary>
        /// 获取或设置是否捕获鼠标
        /// </summary>
        public bool IsCaptureCursor
        {
            get { return isCaptureCursor; }
            set { isCaptureCursor = value; }
        }
        private bool isFromClipBoard;
        /// <summary>
        /// 获取或设置是否从剪切板获取图像
        /// </summary>
        public bool IsFromClipBoard
        {
            get { return isFromClipBoard; }
            set { isFromClipBoard = value; }
        }
        /// <summary>
        /// 获取或设置是否显示图像信息
        /// </summary>
        public bool ImgProcessBoxIsShowInfo
        {
            get { return imageProcessBox1.IsShowInfo; }
            set { imageProcessBox1.IsShowInfo = value; }
        }
        /// <summary>
        /// 获取或设置操作框点的颜色
        /// </summary>
        public Color ImgProcessBoxDotColor
        {
            get { return imageProcessBox1.DotColor; }
            set { imageProcessBox1.DotColor = value; }
        }
        /// <summary>
        /// 获取或设置操作框边框颜色
        /// </summary>
        public Color ImgProcessBoxLineColor
        {
            get { return imageProcessBox1.LineColor; }
            set { imageProcessBox1.LineColor = value; }
        }
        /// <summary>
        /// 获取或设置放大图形的原始尺寸
        /// </summary>
        public Size ImgProcessBoxMagnifySize
        {
            get { return imageProcessBox1.MagnifySize; }
            set { imageProcessBox1.MagnifySize = value; }
        }
        /// <summary>
        /// 获取或设置放大图像的倍数
        /// </summary>
        public int ImgProcessBoxMagnifyTimes
        {
            get { return imageProcessBox1.MagnifyTimes; }
            set { imageProcessBox1.MagnifyTimes = value; }
        }

        #endregion

        private HookHelper m_MHook;
        private List<Bitmap> m_layer;       //记录历史图层

        //private bool m_bSave;
        private bool m_isStartDraw;
        private Point m_ptOriginal;
        private Point m_ptCurrent;
        private Bitmap captureDatas;
        private Bitmap m_bmpLayerShow;

        Rectangle _virtualScreen;


        /// <summary>
        /// 截图主窗体
        /// </summary>
        public CaptureForm()
        {
            m_layer = new List<Bitmap>();

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;
            this.ShowInTaskbar = false;

            // 优化：延迟Hook初始化，避免阻塞UI
            this.Shown += CaptureForm_Shown;
            this.FormClosing += (s, ce) =>
            {
                if (m_MHook != null)
                {
                    m_MHook.UnLoadMHook();
                    m_MHook.MHookEvent -= m_MHook_MHookEvent;
                }
                this.DelResource();
            };
        }

        /// <summary>
        /// 窗体显示后初始化Hook（性能优化）
        /// </summary>
        private void CaptureForm_Shown(object sender, EventArgs e)
        {
            // 在UI线程上异步初始化Hook，避免阻塞主线程
            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    m_MHook = new HookHelper();
                    if (m_MHook.SetMHook())
                    {
                        m_MHook.MHookEvent += m_MHook_MHookEvent;
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但不中断启动流程
                    System.Diagnostics.Debug.WriteLine($"Hook初始化失败: {ex.Message}");
                }
            }));
        }

        /// <summary>
        /// 截图主窗体
        /// </summary>
        /// <param name="captureCursor"></param>
        public CaptureForm(bool captureCursor) : this()
        {
            this.IsCaptureCursor = captureCursor;
        }

        /// <summary>
        /// 窗体加载后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCapture_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() => this.Enabled = false));
            this.InitMember();

            imageProcessBox1.MouseLeave += (s, ce) => this.Cursor = Cursors.Default;
            imageProcessBox1.IsDrawOperationDot = false;
            timer1.Interval = 500;
            timer1.Enabled = true;
        }

        private void m_MHook_MHookEvent(object sender, MHookEventArgs e)
        {
            // 添加空引用检查
            if (this.IsDisposed || imageProcessBox1 == null)
                return;

            //如果窗体禁用 调用控件的方法设置信息显示位置
            if (!this.Enabled)
            {
                imageProcessBox1.SetInfoPoint(MousePosition.X - _virtualScreen.X, MousePosition.Y - _virtualScreen.Y);
            }
            //鼠标点下恢复窗体禁用
            if (e.MButton == ButtonStatus.LeftDown || e.MButton == ButtonStatus.RightDown)
            {
                this.Enabled = true;
                imageProcessBox1.IsDrawOperationDot = true;
            }

            #region 右键抬起

            if (e.MButton == ButtonStatus.RightUp)
            {
                if (!imageProcessBox1.IsDrawed) //没有绘制那么退出(直接this.Close右键将传递到下面)
                    this.BeginInvoke(new MethodInvoker(() => this.Close()));
            }

            #endregion

            #region 找寻窗体

            if (!this.Enabled)
                this.FoundAndDrawWindowRect(_virtualScreen);
            #endregion

        }


        //初始化参数
        private void InitMember()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.White;
            panel1.Height = tBtn_Finish.Bottom + 3;
            panel1.Width = tBtn_Finish.Right + 3;
            panel2.Height = colorBox1.Height;
            panel1.Paint += (s, e) => e.Graphics.DrawRectangle(Pens.SteelBlue, 0, 0, panel1.Width - 1, panel1.Height - 1);
            panel2.Paint += (s, e) => e.Graphics.DrawRectangle(Pens.SteelBlue, 0, 0, panel2.Width - 1, panel2.Height - 1);

            tBtn_Rect.Click += new EventHandler(selectToolButton_Click);
            tBtn_Ellipse.Click += new EventHandler(selectToolButton_Click);
            tBtn_Arrow.Click += new EventHandler(selectToolButton_Click);
            tBtn_Brush.Click += new EventHandler(selectToolButton_Click);
            tBtn_Text.Click += new EventHandler(selectToolButton_Click);
            tBtn_Close.Click += (s, e) => this.Close();

            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Visible = false;
            textBox1.ForeColor = Color.Red;
            colorBox1.ColorChanged += (s, e) => textBox1.ForeColor = e.Color;
        }
        //工具条前五个按钮绑定的公共事件
        private void selectToolButton_Click(object sender, EventArgs e)
        {
            panel2.Visible = ((ToolButton)sender).IsSelected;
            if (panel2.Visible) imageProcessBox1.CanReset = false;
            else { imageProcessBox1.CanReset = m_layer.Count == 0; }
            this.SetToolBarLocation();
        }

        #region 截图后的一些后期绘制

        private void imageProcessBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (imageProcessBox1.Cursor != Cursors.SizeAll &&
                imageProcessBox1.Cursor != Cursors.Default)
                panel1.Visible = false;         //表示改变选取大小 隐藏工具条
            //若果在选区内点击 并且有选择工具
            if (e.Button == MouseButtons.Left && imageProcessBox1.IsDrawed && HaveSelectedToolButton())
            {
                if (imageProcessBox1.SelectedRectangle.Contains(e.Location))
                {
                    if (tBtn_Text.IsSelected)
                    {
                        //如果选择的是绘制文本 弹出文本框
                        textBox1.Location = e.Location;
                        textBox1.Visible = true;
                        textBox1.Focus();
                        return;
                    }
                    m_isStartDraw = true;
                    Cursor.Clip = imageProcessBox1.SelectedRectangle;
                }
            }
            m_ptOriginal = e.Location;
        }

        private void imageProcessBox1_MouseMove(object sender, MouseEventArgs e)
        {
            m_ptCurrent = e.Location;
            //根据是否选择有工具决定 鼠标指针样式
            if (imageProcessBox1.SelectedRectangle.Contains(e.Location) && HaveSelectedToolButton() && imageProcessBox1.IsDrawed)
                this.Cursor = Cursors.Cross;
            else if (!imageProcessBox1.SelectedRectangle.Contains(e.Location))
                this.Cursor = Cursors.Default;

            if (imageProcessBox1.IsStartDraw && panel1.Visible)   //在重置选取的时候 重置工具条位置(成立于移动选取的时候)
                this.SetToolBarLocation();
            if (m_isStartDraw && m_bmpLayerShow != null)
            {        //如果在区域内点下那么绘制相应图形
                using (Graphics g = Graphics.FromImage(m_bmpLayerShow))
                {
                    int tempWidth = 1;
                    if (toolButton2.IsSelected) tempWidth = 3;
                    if (toolButton3.IsSelected) tempWidth = 5;
                    Pen p = new Pen(colorBox1.SelectedColor, tempWidth);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    #region   绘制矩形

                    if (tBtn_Rect.IsSelected)
                    {
                        int tempX = e.X - m_ptOriginal.X > 0 ? m_ptOriginal.X : e.X;
                        int tempY = e.Y - m_ptOriginal.Y > 0 ? m_ptOriginal.Y : e.Y;
                        g.Clear(Color.Transparent);
                        g.DrawRectangle(p, tempX - imageProcessBox1.SelectedRectangle.Left, tempY - imageProcessBox1.SelectedRectangle.Top, Math.Abs(e.X - m_ptOriginal.X), Math.Abs(e.Y - m_ptOriginal.Y));
                        imageProcessBox1.Invalidate();
                    }

                    #endregion

                    #region    绘制圆形

                    if (tBtn_Ellipse.IsSelected)
                    {
                        g.DrawLine(Pens.Red, 0, 0, 200, 200);
                        g.Clear(Color.Transparent);
                        g.DrawEllipse(p, m_ptOriginal.X - imageProcessBox1.SelectedRectangle.Left, m_ptOriginal.Y - imageProcessBox1.SelectedRectangle.Top, e.X - m_ptOriginal.X, e.Y - m_ptOriginal.Y);
                        imageProcessBox1.Invalidate();
                    }

                    #endregion

                    #region    绘制箭头

                    if (tBtn_Arrow.IsSelected)
                    {
                        g.Clear(Color.Transparent);
                        System.Drawing.Drawing2D.AdjustableArrowCap lineArrow =
                            new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5, true);
                        p.CustomEndCap = lineArrow;
                        g.DrawLine(p, (Point)((Size)m_ptOriginal - (Size)imageProcessBox1.SelectedRectangle.Location), (Point)((Size)m_ptCurrent - (Size)imageProcessBox1.SelectedRectangle.Location));
                        imageProcessBox1.Invalidate();
                    }

                    #endregion

                    #region    绘制线条

                    if (tBtn_Brush.IsSelected)
                    {
                        Point ptTemp = (Point)((Size)m_ptOriginal - (Size)imageProcessBox1.SelectedRectangle.Location);
                        p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        g.DrawLine(p, ptTemp, (Point)((Size)e.Location - (Size)imageProcessBox1.SelectedRectangle.Location));
                        m_ptOriginal = e.Location;
                        imageProcessBox1.Invalidate();
                    }

                    #endregion

                    p.Dispose();
                }
            }
        }

        private void imageProcessBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.IsDisposed) return;
            if (e.Button == MouseButtons.Right)
            {   //右键清空绘制
                this.Enabled = false;
                imageProcessBox1.ClearDraw();
                imageProcessBox1.CanReset = true;
                imageProcessBox1.IsDrawOperationDot = false;
                m_layer.Clear();    //清空历史记录
                captureDatas = null;
                m_bmpLayerShow = null;
                ClearToolBarBtnSelected();
                panel1.Visible = false;
                panel2.Visible = false;
            }
            if (!imageProcessBox1.IsDrawed)
            {       //如果没有成功绘制选取 继续禁用窗体
                this.Enabled = false;
                imageProcessBox1.IsDrawOperationDot = false;
            }
            else if (!panel1.Visible)
            {           //否则显示工具条
                this.SetToolBarLocation();          //重置工具条位置
                panel1.Visible = true;
                captureDatas = imageProcessBox1.GetResultBmp();    //获取选取图形
                m_bmpLayerShow = new Bitmap(captureDatas.Width, captureDatas.Height);
            }
            //如果移动了选取位置 重新获取选取的图形
            if (imageProcessBox1.Cursor == Cursors.SizeAll && m_ptOriginal != e.Location)
            {
                captureDatas.Dispose();
                captureDatas = imageProcessBox1.GetResultBmp();
            }

            if (!m_isStartDraw) return;
            Cursor.Clip = Rectangle.Empty;
            m_isStartDraw = false;
            if (e.Location == m_ptOriginal && !tBtn_Brush.IsSelected) return;
            this.SetLayer();        //将绘制的图形绘制到历史图层中
        }
        //绘制后期操作
        private void imageProcessBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (m_layer.Count > 0)  //绘制保存的历史记录的最后一张图
                g.DrawImage(m_layer[m_layer.Count - 1], imageProcessBox1.SelectedRectangle.Location);
            if (m_bmpLayerShow != null)     //绘制当前正在拖动绘制的图形(即鼠标点下还没有抬起确认的图形)
                g.DrawImage(m_bmpLayerShow, imageProcessBox1.SelectedRectangle.Location);
        }

        #endregion

        //文本改变时重置文本框大小
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Size se = TextRenderer.MeasureText(textBox1.Text, textBox1.Font);
            textBox1.Size = se.IsEmpty ? new Size(50, textBox1.Font.Height) : se;
        }
        //文本框失去焦点时 绘制文本
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            textBox1.Visible = false;
            if (string.IsNullOrEmpty(textBox1.Text.Trim())) { textBox1.Text = ""; return; }
            using (Graphics g = Graphics.FromImage(captureDatas))
            {
                SolidBrush sb = new SolidBrush(colorBox1.SelectedColor);
                g.DrawString(textBox1.Text, textBox1.Font, sb,
                    textBox1.Left - imageProcessBox1.SelectedRectangle.Left,
                    textBox1.Top - imageProcessBox1.SelectedRectangle.Top);
                sb.Dispose();
                textBox1.Text = "";
                this.SetLayer();        //将文本绘制到当前图层并存入历史记录
                imageProcessBox1.Invalidate();
            }
        }
        //窗体大小改变时重置字体 从控件中获取字体大小
        private void textBox1_Resize(object sender, EventArgs e)
        {
            //在三个大小选择的按钮点击中设置字体大小太麻烦 所以Resize中获取设置
            int se = 10;
            if (toolButton2.IsSelected) se = 12;
            if (toolButton3.IsSelected) se = 14;
            if (this.textBox1.Font.Height == se) return;
            textBox1.Font = new Font(this.Font.FontFamily, se);
        }
        //撤销
        private void tBtn_Cancel_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(m_bmpLayerShow))
            {
                g.Clear(Color.Transparent);     //清空当前临时显示的图像
            }
            if (m_layer.Count > 0)
            {            //删除最后一层
                m_layer.RemoveAt(m_layer.Count - 1);
                if (m_layer.Count > 0)
                    captureDatas = m_layer[m_layer.Count - 1].Clone() as Bitmap;
                else
                    captureDatas = imageProcessBox1.GetResultBmp();
                imageProcessBox1.Invalidate();
                imageProcessBox1.CanReset = m_layer.Count == 0 && !HaveSelectedToolButton();
            }
            else
            {                            //如果没有历史记录则取消本次截图
                this.Enabled = false;
                imageProcessBox1.ClearDraw();
                imageProcessBox1.IsDrawOperationDot = false;
                panel1.Visible = false;
                panel2.Visible = false;
            }
        }

        private void tBtn_Save_Click(object sender, EventArgs e)
        {
            //m_bSave = true;
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "PNG(*.png)|*.png|JPEG(*.jpg)|*.jpg|Bitmap(*.bmp)|*.bmp";
            saveDlg.FilterIndex = 1;
            saveDlg.FileName = "YSWENLI_WEF_CAPTURE_" + GetTimeString();
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                switch (saveDlg.FilterIndex)
                {
                    case 1:
                        captureDatas.Clone(new Rectangle(0, 0, captureDatas.Width, captureDatas.Height),
                            PixelFormat.Format32bppArgb).Save(saveDlg.FileName,
                            ImageFormat.Png);
                        this.Close();
                        break;
                    case 2:
                        captureDatas.Clone(new Rectangle(0, 0, captureDatas.Width, captureDatas.Height),
                            PixelFormat.Format24bppRgb).Save(saveDlg.FileName,
                            ImageFormat.Jpeg);
                        this.Close();
                        break;
                    case 3:
                    default:
                        captureDatas.Clone(new Rectangle(0, 0, captureDatas.Width, captureDatas.Height),
                            PixelFormat.Format24bppRgb).Save(saveDlg.FileName,
                            ImageFormat.Bmp);
                        this.Close();
                        break;
                }
            }
            //m_bSave = false;
        }
        /// <summary>
        /// 点击完成： 将图像保存到剪贴板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBtn_Finish_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(captureDatas);
            this.RaiseOnCaptured(Image.FromHbitmap(captureDatas.GetHbitmap()));
            this.Close();
        }

        private void tBtn_Out_Click(object sender, EventArgs e)
        {
            new FrmOut(captureDatas.Clone() as Bitmap).Show();
            this.Close();
        }

        private void imageProcessBox1_DoubleClick(object sender, EventArgs e)
        {
            if (captureDatas != null)
            {
                Clipboard.SetImage(captureDatas);
                this.RaiseOnCaptured(Image.FromHbitmap(captureDatas.GetHbitmap()));
                this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.Enabled)
            {
                imageProcessBox1.SetInfoPoint(MousePosition.X - _virtualScreen.X, MousePosition.Y - _virtualScreen.Y);
            }
        }
        /// <summary>
        /// 根据鼠标位置找寻窗体平绘制边框
        /// </summary>
        private void FoundAndDrawWindowRect(Rectangle virtualScreen)
        {
            MouseAndKeyHelper.LPPOINT pt = new MouseAndKeyHelper.LPPOINT();
            pt.X = MousePosition.X; pt.Y = MousePosition.Y;
            IntPtr hWnd = MouseAndKeyHelper.ChildWindowFromPointEx(MouseAndKeyHelper.GetDesktopWindow(), pt,
                MouseAndKeyHelper.CWP_SKIPINVISIBL | MouseAndKeyHelper.CWP_SKIPDISABLED);
            if (hWnd != IntPtr.Zero)
            {
                IntPtr hTemp = hWnd;
                while (true)
                {
                    MouseAndKeyHelper.ScreenToClient(hTemp, out pt);
                    hTemp = MouseAndKeyHelper.ChildWindowFromPointEx(hWnd, pt, MouseAndKeyHelper.CWP_SKIPINVISIBL);
                    if (hTemp == IntPtr.Zero || hTemp == hWnd)
                        break;
                    hWnd = hTemp;
                    pt.X = MousePosition.X; pt.Y = MousePosition.Y; //坐标还原为屏幕坐标
                }
                MouseAndKeyHelper.LPRECT rect = new MouseAndKeyHelper.LPRECT();
                MouseAndKeyHelper.GetWindowRect(hWnd, out rect);
                Rectangle windowRect = new Rectangle(
                    rect.Left - virtualScreen.X,
                    rect.Top - virtualScreen.Y,
                    rect.Right - rect.Left,
                    rect.Bottom - rect.Top);

                imageProcessBox1.SetSelectRect(windowRect);
            }
        }


        /// <summary>
        /// 绘制图片 - 优化版本，提升性能
        /// </summary>
        /// <param name="virtualScreen"></param>
        /// <param name="bCaptureCursor"></param>
        /// <param name="bFromClipBoard"></param>
        /// <returns></returns>
        private static Bitmap LoadImage(Rectangle virtualScreen, bool bCaptureCursor, bool bFromClipBoard)
        {
            Bitmap bmp = new Bitmap(virtualScreen.Width, virtualScreen.Height, PixelFormat.Format32bppArgb);
            try
            {
                if (bFromClipBoard)
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        // 优化：使用高效的Graphics设置
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                        using (Image img_clip = Clipboard.GetImage())
                        {
                            if (img_clip != null)
                            {
                                using (SolidBrush sb = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                                {
                                    g.FillRectangle(sb, 0, 0, bmp.Width, bmp.Height);
                                    g.DrawImage(img_clip, 0, 0);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ScreenHelper.CaptureScreen(bmp, virtualScreen, MousePosition, bCaptureCursor);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                // 清理资源
                bmp?.Dispose();
                System.Diagnostics.Debug.WriteLine($"LoadImage失败: {ex.Message}");
                throw;
            }
        }




        //设置工具条位置
        private void SetToolBarLocation()
        {
            int tempX = imageProcessBox1.SelectedRectangle.Left;
            int tempY = imageProcessBox1.SelectedRectangle.Bottom + 5;
            int tempHeight = panel2.Visible ? panel2.Height + 2 : 0;
            if (tempY + panel1.Height + tempHeight >= this.Height)
                tempY = imageProcessBox1.SelectedRectangle.Top - panel1.Height - 10 - imageProcessBox1.Font.Height;

            if (tempY - tempHeight <= 0)
            {
                if (imageProcessBox1.SelectedRectangle.Top - 5 - imageProcessBox1.Font.Height >= 0)
                    tempY = imageProcessBox1.SelectedRectangle.Top + 5;
                else
                    tempY = imageProcessBox1.SelectedRectangle.Top + 10 + imageProcessBox1.Font.Height;
            }
            if (tempX + panel1.Width >= this.Width)
                tempX = this.Width - panel1.Width - 5;
            panel1.Left = tempX;
            panel2.Left = tempX;
            panel1.Top = tempY;
            panel2.Top = imageProcessBox1.SelectedRectangle.Top > tempY ? tempY - tempHeight : panel1.Bottom + 2;
        }
        //确定是否工具条上面有被选中的按钮
        private bool HaveSelectedToolButton()
        {
            return tBtn_Rect.IsSelected || tBtn_Ellipse.IsSelected
                || tBtn_Arrow.IsSelected || tBtn_Brush.IsSelected
                || tBtn_Text.IsSelected;
        }
        //清空选中的工具条上的工具
        private void ClearToolBarBtnSelected()
        {
            tBtn_Rect.IsSelected = tBtn_Ellipse.IsSelected = tBtn_Arrow.IsSelected =
                tBtn_Brush.IsSelected = tBtn_Text.IsSelected = false;
        }
        //设置历史图层
        private void SetLayer()
        {
            if (this.IsDisposed) return;
            using (Graphics g = Graphics.FromImage(captureDatas))
            {
                g.DrawImage(m_bmpLayerShow, 0, 0);
            }
            Bitmap bmpTemp = captureDatas.Clone() as Bitmap;
            m_layer.Add(bmpTemp);
        }
        //保存时获取当前时间字符串作文默认文件名
        private string GetTimeString()
        {
            DateTime time = DateTime.Now;
            return time.Date.ToShortDateString().Replace("/", "") + "_" +
                time.ToLongTimeString().Replace(":", "");
        }


        /// <summary>
        /// 显示窗口
        /// </summary>
        public new void Show()
        {
            _virtualScreen = ScreenHelper.GetVirtualScreenBounds();
            this.Bounds = _virtualScreen;
            imageProcessBox1.Dock = DockStyle.Fill;
            Image screenImage = LoadImage(_virtualScreen, isCaptureCursor, isFromClipBoard);
            if (!this.IsDisposed && imageProcessBox1 != null)
            {
                imageProcessBox1.WorkImage = screenImage;
                base.Show();
            }
            else
            {
                screenImage?.Dispose();
            }
        }

        /// <summary>
        /// 显示对话框 - 优化版本，快速启动
        /// </summary>
        public new void ShowDialog()
        {
            _virtualScreen = ScreenHelper.GetVirtualScreenBounds();
            this.Bounds = _virtualScreen;
            imageProcessBox1.Dock = DockStyle.Fill;
            try
            {
                Image screenImage = LoadImage(_virtualScreen, isCaptureCursor, isFromClipBoard);
                if (!this.IsDisposed && imageProcessBox1 != null)
                {
                    imageProcessBox1.WorkImage = screenImage;
                }
                else
                {
                    screenImage?.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"异步加载截图失败: {ex.Message}");
            }

            base.ShowDialog();
        }

    }
}
