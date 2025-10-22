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
    /// <summary>
    /// 截图主窗体：负责截图显示、选区交互、工具条绘制与保存。
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

        /// <summary>
        /// 释放截图相关资源（图层、位图、钩子等），防止泄漏
        /// </summary>
        private void DelResource()
        {
            if (_captureDatas != null) _captureDatas.Dispose();
            if (m_bmpLayerShow != null) m_bmpLayerShow.Dispose();
            m_layer.Clear();
            imageProcessBox1.DeleResource();
            GC.Collect();
        }

        #region Properties

        private bool isCaptureCursor; // 是否捕捉鼠标指针
        /// <summary>
        /// 获取或设置是否捕获鼠标
        /// </summary>
        public bool IsCaptureCursor
        {
            get { return isCaptureCursor; }
            set { isCaptureCursor = value; }
        }
        private bool isFromClipBoard; // 是否从剪贴板加载图像
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

        private HookHelper m_MHook; // 全局输入钩子（鼠标/键盘）
        private List<Bitmap> m_layer;       //记录历史图层

        //private bool m_bSave;
        private bool m_isStartDraw; // 是否进入绘制状态（按下到抬起）
        private Point m_ptOriginal; // 绘制起点（客户端坐标）
        private Point m_ptCurrent; // 当前鼠标位置（客户端坐标）
        private Bitmap _captureDatas; // 截取的最终图像数据（裁剪结果）
        private Bitmap m_bmpLayerShow; // 当前叠加层显示位图（用于预览）

        // 绘图资源缓存（减少频繁 new Pen 的成本）
        private readonly Dictionary<string, Pen> _penCache = new Dictionary<string, Pen>();
        private readonly System.Drawing.Drawing2D.AdjustableArrowCap _arrowCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5, true);

        Rectangle _virtualScreenBounds;


        /// <summary>
        /// 截图主窗体
        /// </summary>
        public CaptureForm()
        {
            m_layer = new List<Bitmap>();

            InitializeComponent();
            // 开启双缓冲与减少 WM_PAINT 重绘，提高拖拽绘制性能与减少闪烁
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

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
                // 释放绘图缓存资源
                foreach (var kv in _penCache)
                {
                    kv.Value?.Dispose();
                }
                _arrowCap?.Dispose();
                this.DelResource();
            };


            // 设置窗体可以接收键盘输入
            this.KeyPreview = true;
        }

        /// <summary>
        /// 窗体显示后初始化Hook
        /// </summary>
        /// <summary>
        /// 窗体显示后初始化全局Hook（异步），避免阻塞UI线程
        /// </summary>
        /// <param name="sender">截图主窗体</param>
        /// <param name="e">事件参数</param>
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
        /// <summary>
        /// 窗体加载：初始化成员、禁用窗口用于信息显示，并启用定时器
        /// </summary>
        /// <param name="sender">截图主窗体</param>
        /// <param name="e">事件参数</param>
        private void FrmCapture_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() => this.Enabled = false));
            this.InitMember();

            imageProcessBox1.MouseLeave += (s, ce) => this.Cursor = Cursors.Default;
            imageProcessBox1.IsDrawOperationDot = false;
            timer1.Interval = 500;
            timer1.Enabled = true;
        }

        /// <summary>
        /// 全局输入钩子事件：处理 Ctrl+C 复制颜色、Esc 取消等快捷键
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">钩子事件参数</param>
        private void m_MHook_MHookEvent(object sender, MHookEventArgs e)
        {
            // 添加空引用检查
            if (this.IsDisposed || imageProcessBox1 == null)
                return;

            //如果窗体禁用 调用控件的方法设置信息显示位置
            if (!this.Enabled)
            {
                var ptClient = ScreenHelper.ScreenToClient(imageProcessBox1, Control.MousePosition);
                imageProcessBox1.SetInfoPoint(ptClient);
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
                this.FoundAndDrawWindowRect(_virtualScreenBounds);
            #endregion

        }


        //初始化参数
        /// <summary>
        /// 初始化成员与事件绑定：工具条、颜色框、钩子等
        /// </summary>
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
        /// <summary>
        /// 工具条选择事件：矩形、椭圆、箭头、画笔、文字的选择切换
        /// </summary>
        /// <param name="sender">工具按钮</param>
        /// <param name="e">事件参数</param>
        private void selectToolButton_Click(object sender, EventArgs e)
        {
            panel2.Visible = ((ToolButton)sender).IsSelected;
            if (panel2.Visible) imageProcessBox1.CanReset = false;
            else { imageProcessBox1.CanReset = m_layer.Count == 0; }
            this.SetToolBarLocation();
        }

        #region 截图后的一些后期绘制

        /// <summary>
        /// 鼠标按下：开始绘制或移动选区，并限制光标范围
        /// </summary>
        /// <param name="sender">图像控件</param>
        /// <param name="e">鼠标事件参数</param>
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
                        textBox1.Location = ScreenHelper.ClientToScreen(imageProcessBox1, e.Location);
                        textBox1.Visible = true;
                        textBox1.Focus();
                        return;
                    }
                    m_isStartDraw = true;
                    Rectangle selectedRectScreen = ScreenHelper.ClientRectToScreen(imageProcessBox1, imageProcessBox1.SelectedRectangle);
                    Cursor.Clip = selectedRectScreen;
                }
            }
            m_ptOriginal = e.Location;
        }

        /// <summary>
        /// 获得画笔
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private Pen GetPen(Color color, int width)
        {
            Pen pen = new Pen(color);
            pen.Width = width;
            return pen;
        }

        /// <summary>
        /// 鼠标移动：更新选区位置或大小，并进行局部刷新（含节流）
        /// </summary>
        /// <param name="sender">图像控件</param>
        /// <param name="e">鼠标事件参数</param>
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
            {
                //如果在区域内点下那么绘制相应图形
                using (Graphics g = Graphics.FromImage(m_bmpLayerShow))
                {
                    int tempWidth = 1;
                    if (toolButton2.IsSelected) tempWidth = 3;
                    if (toolButton3.IsSelected) tempWidth = 5;
                    var p = GetPen(colorBox1.SelectedColor, tempWidth);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    #region   绘制矩形

                    if (tBtn_Rect.IsSelected)
                    {
                        int tempX = e.X - m_ptOriginal.X > 0 ? m_ptOriginal.X : e.X;
                        int tempY = e.Y - m_ptOriginal.Y > 0 ? m_ptOriginal.Y : e.Y;
                        g.Clear(Color.Transparent);
                        g.DrawRectangle(p, tempX - imageProcessBox1.SelectedRectangle.Left, tempY - imageProcessBox1.SelectedRectangle.Top, Math.Abs(e.X - m_ptOriginal.X), Math.Abs(e.Y - m_ptOriginal.Y));
                        InvalidateSelectionBounds();
                    }

                    #endregion

                    #region    绘制圆形

                    if (tBtn_Ellipse.IsSelected)
                    {
                        g.DrawLine(Pens.Red, 0, 0, 200, 200);
                        g.Clear(Color.Transparent);
                        g.DrawEllipse(p, m_ptOriginal.X - imageProcessBox1.SelectedRectangle.Left, m_ptOriginal.Y - imageProcessBox1.SelectedRectangle.Top, e.X - m_ptOriginal.X, e.Y - m_ptOriginal.Y);
                        InvalidateSelectionBounds();
                    }

                    #endregion

                    #region    绘制箭头

                    if (tBtn_Arrow.IsSelected)
                    {
                        g.Clear(Color.Transparent);
                        p.CustomEndCap = _arrowCap;
                        g.DrawLine(p, (Point)((Size)m_ptOriginal - (Size)imageProcessBox1.SelectedRectangle.Location), (Point)((Size)m_ptCurrent - (Size)imageProcessBox1.SelectedRectangle.Location));
                        InvalidateSelectionBounds();
                    }

                    #endregion

                    #region    绘制线条

                    if (tBtn_Brush.IsSelected)
                    {
                        Point ptTemp = (Point)((Size)m_ptOriginal - (Size)imageProcessBox1.SelectedRectangle.Location);
                        g.DrawLine(p, ptTemp, (Point)((Size)e.Location - (Size)imageProcessBox1.SelectedRectangle.Location));
                        m_ptOriginal = e.Location;
                        InvalidateSelectionBounds();
                    }

                    #endregion

                    p.Dispose();
                }
            }
        }

        /// <summary>
        /// 鼠标抬起：结束绘制或移动选区，根据工具状态执行后续操作
        /// </summary>
        /// <param name="sender">图像控件</param>
        /// <param name="e">鼠标事件参数</param>
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
                _captureDatas = null;
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
                _captureDatas = imageProcessBox1.GetResultBmp();    //获取选取图形
                m_bmpLayerShow = new Bitmap(_captureDatas.Width, _captureDatas.Height);
            }
            //如果移动了选取位置 重新获取选取的图形
            if (imageProcessBox1.Cursor == Cursors.SizeAll && m_ptOriginal != e.Location)
            {
                _captureDatas.Dispose();
                _captureDatas = imageProcessBox1.GetResultBmp();
            }

            if (!m_isStartDraw) return;
            Cursor.Clip = Rectangle.Empty;
            m_isStartDraw = false;
            if (e.Location == m_ptOriginal && !tBtn_Brush.IsSelected) return;
            this.SetLayer();        //将绘制的图形绘制到历史图层中
        }
        //绘制后期操作
        /// <summary>
        /// 图像控件重绘：绘制选区、操作点与工具条预览
        /// </summary>
        /// <param name="sender">图像控件</param>
        /// <param name="e">绘制事件参数</param>
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
        /// <summary>
        /// 文本输入变更：更新文字工具的内容并重绘预览
        /// </summary>
        /// <param name="sender">文本框</param>
        /// <param name="e">事件参数</param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Size se = TextRenderer.MeasureText(textBox1.Text, textBox1.Font);
            textBox1.Size = se.IsEmpty ? new Size(50, textBox1.Font.Height) : se;
        }
        //文本框失去焦点时 绘制文本
        /// <summary>
        /// 文本框验证：结束文字输入并将文字绘制到图层
        /// </summary>
        /// <param name="sender">文本框</param>
        /// <param name="e">验证事件参数</param>
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            textBox1.Visible = false;
            if (string.IsNullOrEmpty(textBox1.Text.Trim())) { textBox1.Text = ""; return; }
            using (Graphics g = Graphics.FromImage(_captureDatas))
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
        /// <summary>
        /// 文本框大小变更：根据内容调整尺寸并避免越界
        /// </summary>
        /// <param name="sender">文本框</param>
        /// <param name="e">事件参数</param>
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
        /// <summary>
        /// 取消并关闭截图窗口（不保存）
        /// </summary>
        /// <param name="sender">“取消”工具按钮</param>
        /// <param name="e">事件参数</param>
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
                    _captureDatas = m_layer[m_layer.Count - 1].Clone() as Bitmap;
                else
                    _captureDatas = imageProcessBox1.GetResultBmp();
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

        /// <summary>
        /// 保存当前截图到文件（弹出保存对话框）
        /// </summary>
        /// <param name="sender">“保存”工具按钮</param>
        /// <param name="e">事件参数</param>
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
                        _captureDatas.Clone(new Rectangle(0, 0, _captureDatas.Width, _captureDatas.Height),
                            PixelFormat.Format32bppArgb).Save(saveDlg.FileName,
                            ImageFormat.Png);
                        this.Close();
                        break;
                    case 2:
                        _captureDatas.Clone(new Rectangle(0, 0, _captureDatas.Width, _captureDatas.Height),
                            PixelFormat.Format24bppRgb).Save(saveDlg.FileName,
                            ImageFormat.Jpeg);
                        this.Close();
                        break;
                    case 3:
                    default:
                        _captureDatas.Clone(new Rectangle(0, 0, _captureDatas.Width, _captureDatas.Height),
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
        /// <summary>
        /// 完成截取：将结果抛出到回调并关闭窗口
        /// </summary>
        /// <param name="sender">“完成”工具按钮</param>
        /// <param name="e">事件参数</param>
        private void tBtn_Finish_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(_captureDatas);
            this.RaiseOnCaptured(Image.FromHbitmap(_captureDatas.GetHbitmap()));
            this.Close();
        }

        /// <summary>
        /// 输出当前截图到独立预览窗体，并关闭当前截图会话
        /// </summary>
        /// <param name="sender">“输出”工具按钮</param>
        /// <param name="e">事件参数</param>
        private void tBtn_Out_Click(object sender, EventArgs e)
        {
            new FrmOut(_captureDatas.Clone() as Bitmap).Show();
            this.Close();
        }

        /// <summary>
        /// 双击图像：将裁剪结果复制到剪贴板并触发Captured回调
        /// </summary>
        /// <param name="sender">图像控件</param>
        /// <param name="e">事件参数</param>
        private void imageProcessBox1_DoubleClick(object sender, EventArgs e)
        {
            if (_captureDatas != null)
            {
                Clipboard.SetImage(_captureDatas);
                this.RaiseOnCaptured(Image.FromHbitmap(_captureDatas.GetHbitmap()));
                this.Close();
            }
        }

        /// <summary>
        /// 定时器：在窗体禁用时更新放大镜信息点（相对虚拟屏幕坐标）
        /// </summary>
        /// <param name="sender">定时器组件</param>
        /// <param name="e">事件参数</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.Enabled)
            {
                Rectangle virtualScreen = ScreenHelper.GetVirtualScreenBounds();
                imageProcessBox1.SetInfoPoint(MousePosition.X - virtualScreen.X, MousePosition.Y - virtualScreen.Y);
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
        /// 绘制图片
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


        /// <summary>
        /// 设置工具条位置
        /// </summary>
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
        /// <summary>
        /// 确定是否工具条上面有被选中的按钮
        /// </summary>
        /// <returns></returns>
        private bool HaveSelectedToolButton()
        {
            return tBtn_Rect.IsSelected || tBtn_Ellipse.IsSelected
                || tBtn_Arrow.IsSelected || tBtn_Brush.IsSelected
                || tBtn_Text.IsSelected;
        }
        //清空选中的工具条上的工具
        /// <summary>
        /// 清空工具条上的选中状态，避免工具状态冲突
        /// </summary>
        private void ClearToolBarBtnSelected()
        {
            tBtn_Rect.IsSelected = tBtn_Ellipse.IsSelected = tBtn_Arrow.IsSelected =
                tBtn_Brush.IsSelected = tBtn_Text.IsSelected = false;
        }
        //设置历史图层
        /// <summary>
        /// 将当前叠加层合并到最终位图，并记录历史图层用于撤销
        /// </summary>
        private void SetLayer()
        {
            if (this.IsDisposed) return;
            using (Graphics g = Graphics.FromImage(_captureDatas))
            {
                g.DrawImage(m_bmpLayerShow, 0, 0);
            }
            Bitmap bmpTemp = _captureDatas.Clone() as Bitmap;
            m_layer.Add(bmpTemp);
        }
        //保存时获取当前时间字符串作文默认文件名
        /// <summary>
        /// 获取当前时间的字符串表达（作为默认保存文件名）
        /// </summary>
        /// <returns>时间字符串，格式示例：yyyyMMdd_HH:mm:ss</returns>
        private string GetTimeString()
        {
            DateTime time = DateTime.Now;
            return time.Date.ToShortDateString().Replace("/", "") + "_" +
                time.ToLongTimeString().Replace(":", "");
        }


        /// <summary>
        /// 显示对话框
        /// </summary>
        public new void ShowDialog()
        {
            _virtualScreenBounds = ScreenHelper.GetVirtualScreenBounds();
            this.Bounds = _virtualScreenBounds;
            imageProcessBox1.Dock = DockStyle.Fill;
            imageProcessBox1.VirtualScreenBounds = _virtualScreenBounds;
            try
            {
                Image screenImage = LoadImage(_virtualScreenBounds, isCaptureCursor, isFromClipBoard);
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

        /// <summary>
        /// 窗体快捷键
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            // 在放大镜出现时，Ctrl+C 复制当前颜色
            if (e.Control && e.KeyCode == Keys.C)
            {
                TryCopyMagnifierColor();
            }
        }

        /// <summary>
        /// 复制放大镜选取的颜色
        /// </summary>
        private void TryCopyMagnifierColor()
        {
            try
            {
                // 仅在放大镜信息显示阶段生效（未锁定选区）
                if (imageProcessBox1 == null || imageProcessBox1.WorkImage == null) return;
                if (!imageProcessBox1.IsShowInfo || imageProcessBox1.IsDrawed) return;

                // 将全局坐标转换为控件坐标，避免多屏/DPI误差
                Point clientPt = imageProcessBox1.PointToClient(MousePosition);
                Bitmap bmp = imageProcessBox1.WorkImage as Bitmap;
                if (bmp == null) return;

                int x = Math.Max(0, Math.Min(clientPt.X, bmp.Width - 1));
                int y = Math.Max(0, Math.Min(clientPt.Y, bmp.Height - 1));
                Color clr = bmp.GetPixel(x, y);

                // 复制为常用格式：#RRGGBB
                string hexRgb = $"#{clr.R:X2}{clr.G:X2}{clr.B:X2}";
                Clipboard.SetText(hexRgb);
            }
            catch { }
        }


        private void InvalidateSelectionBounds(int inflate = 2)
        {
            var r = imageProcessBox1.SelectedRectangle;
            r.Inflate(inflate, inflate);
            imageProcessBox1.Invalidate(r);
        }



    }
}

