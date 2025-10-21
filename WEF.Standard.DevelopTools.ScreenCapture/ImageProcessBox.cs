using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Common.Win32;
using WEF.Standard.DevelopTools.ScreenCapture;

namespace WEF.Standard.DevelopTools.Capture
{
    public partial class ImageProcessBox : Control
    {
        Rectangle _virtualScreenBounds;

        public Rectangle VirtualScreenBounds
        {
            get { return _virtualScreenBounds; }
            set { _virtualScreenBounds = value; }
        }

        public ImageProcessBox()
        {
            InitializeComponent();
            InitMember();

            this.ForeColor = Color.White;
            this.BackColor = Color.Black;
            this.Dock = DockStyle.Fill;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private void InitMember()
        {
            this.dotColor = Color.FromArgb(255, 0, 255, 255);
            this.lineColor = Color.FromArgb(255, 0, 255, 255);
            this._magnifySize = new Size(15, 15);
            this.magnifyTimes = 5;
            this.isDrawOperationDot = true;
            this.isSetClip = false;  // 修复跨屏幕问题：禁用裁剪限制
            this.isShowInfo = true;
            this.autoSizeFromImage = true;
            this.canReset = true;
            m_pen = new Pen(lineColor, 1);
            m_sb = new SolidBrush(dotColor);
            this.Font = new Font("微软雅黑", 5);
            this.selectedRectangle = new Rectangle();
            this.ClearDraw();
            m_rectDots = new Rectangle[8];
            for (int i = 0; i < 8; i++)
            {
                m_rectDots[i] = new Rectangle(-10, -10, 15, 15);
            }
        }

        ~ImageProcessBox()
        {
            m_pen?.Dispose();
            m_sb?.Dispose();
            if (this.baseImage != null)
            {
                m_bmpDark?.Dispose();
                this.baseImage.Dispose();
            }
            _magnifyForm?.Dispose();
        }

        public void DeleResource()
        {
            m_pen?.Dispose();
            m_sb?.Dispose();
            if (this.baseImage != null)
            {
                m_bmpDark?.Dispose();
                this.baseImage.Dispose();
            }
            _magnifyForm?.Dispose();
        }

        #region Properties

        private Image baseImage;
        /// <summary>
        /// 获取或设置用于被操作的图像
        /// </summary>
        [Category("Custom"), Description("获取或设置用于被操作的图像")]
        public Image WorkImage
        {
            get { return baseImage; }
            set
            {
                baseImage = value;
                this.DrawImage();
            }
        }

        private Color dotColor;
        /// <summary>
        /// 获取或设置操作框点的颜色
        /// </summary>
        [Description("获取或设置操作框点的颜色")]
        [DefaultValue(typeof(Color), "Yellow"), Category("Custom")]
        public Color DotColor
        {
            get { return dotColor; }
            set { dotColor = value; }
        }

        private Color lineColor;
        /// <summary>
        /// 获取或设置操作框线条的颜色
        /// </summary>
        [Description("获取或设置操作框线条的颜色")]
        [DefaultValue(typeof(Color), "Cyan"), Category("Custom")]
        public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        private Rectangle selectedRectangle;
        /// <summary>
        /// 获取当前选中的区域
        /// </summary>
        [Browsable(false)]
        public Rectangle SelectedRectangle
        {
            get
            {
                Rectangle rectTemp = selectedRectangle;
                rectTemp.Width++; rectTemp.Height++;
                return rectTemp;
            }
        }

        private Size _magnifySize;
        /// <summary>
        /// 获取或设置放大图像的原图大小尺寸
        /// </summary>
        [Description("获取或设置放大图像的原图大小尺寸")]
        [DefaultValue(typeof(Size), "20,20"), Category("Custom")]
        public Size MagnifySize
        {
            get { return _magnifySize; }
            set
            {
                _magnifySize = value;
                if (_magnifySize.Width < 5) _magnifySize.Width = 5;
                if (_magnifySize.Width > 20) _magnifySize.Width = 20;
                if (_magnifySize.Height < 5) _magnifySize.Height = 5;
                if (_magnifySize.Height > 20) _magnifySize.Height = 20;
            }
        }

        private int magnifyTimes;
        /// <summary>
        /// 获取或设置图像放大的倍数
        /// </summary>
        [Description("获取或设置图像放大的倍数")]
        [DefaultValue(10), Category("Custom")]
        public int MagnifyTimes
        {
            get { return magnifyTimes; }
            set
            {
                magnifyTimes = value;
                if (magnifyTimes < 3) magnifyTimes = 3;
                if (magnifyTimes > 10) magnifyTimes = 10;
            }
        }

        private bool isDrawOperationDot;
        /// <summary>
        /// 获取或设置是否绘制操作框点
        /// </summary>
        [Description("获取或设置是否绘制操作框点")]
        [DefaultValue(true), Category("Custom")]
        public bool IsDrawOperationDot
        {
            get { return isDrawOperationDot; }
            set
            {
                if (value == isDrawOperationDot) return;
                isDrawOperationDot = value;
                this.Invalidate();
            }
        }

        private bool isSetClip;
        /// <summary>
        /// 获取或设置是否限制鼠标操作区域
        /// </summary>
        [Description("获取或设置是否限制鼠标操作区域")]
        [DefaultValue(true), Category("Custom")]
        public bool IsSetClip
        {
            get { return isSetClip; }
            set { isSetClip = value; }
        }

        private bool isShowInfo;
        /// <summary>
        /// 获取或设置是否绘制信息展示
        /// </summary>
        [Description("获取或设置是否绘制信息展示")]
        [DefaultValue(true), Category("Custom")]
        public bool IsShowInfo
        {
            get { return isShowInfo; }
            set { isShowInfo = value; }
        }

        private bool autoSizeFromImage;
        /// <summary>
        /// 获取或设置是否根据图像大小自动调整控件尺寸
        /// </summary>
        [Description("获取或设置是否根据图像大小自动调整控件尺寸")]
        [DefaultValue(true), Category("Custom")]
        public bool AutoSizeFromImage
        {
            get { return autoSizeFromImage; }
            set
            {
                if (value && this.baseImage != null)
                {
                    this.Width = this.baseImage.Width;
                    this.Height = this.baseImage.Height;
                }
                autoSizeFromImage = value;
            }
        }

        private bool _isDrawed;
        /// <summary>
        /// 获取当前是否绘制的有区域
        /// </summary>
        [Browsable(false)]
        public bool IsDrawed
        {
            get { return _isDrawed; }
        }

        private bool isStartDraw;
        /// <summary>
        /// 获取当前是否开始绘制
        /// </summary>
        [Browsable(false)]
        public bool IsStartDraw
        {
            get { return isStartDraw; }
        }

        private bool _isMoving;
        /// <summary>
        /// 获取当前操作框是否正在移动
        /// </summary>
        [Browsable(false)]
        public bool IsMoving
        {
            get { return _isMoving; }
        }

        private bool canReset;
        /// <summary>
        /// 获取或设置操作框是否锁定
        /// </summary>
        [Browsable(false)]
        public bool CanReset
        {
            get { return canReset; }
            set
            {
                canReset = value;
                if (!canReset) this.Cursor = Cursors.Default;
            }
        }
        #endregion

        #region Member variable

        private bool m_bMouseEnter;
        private bool m_bLockH;
        private bool m_bLockW;
        private Point m_ptOriginal;
        private Point m_ptCurrent;
        private Point m_ptTempStarPos;
        private Rectangle[] m_rectDots;
        private Rectangle m_rectClip;

        private Bitmap m_bmpDark;
        private Pen m_pen;
        private SolidBrush m_sb;

        // 放大镜窗体
        private MagnifyForm _magnifyForm;

        #endregion

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //根据情况是否开始绘制操作框
                if (!this.IsDrawed || this.Cursor != Cursors.Default)
                {
                    m_rectClip = this.DisplayRectangle;
                    if (this.baseImage != null)
                    {
                        if (this.isSetClip)
                        {
                            if (e.X > this.baseImage.Width || e.Y > this.baseImage.Height) return;
                            m_rectClip.Intersect(new Rectangle(0, 0, this.baseImage.Width, this.baseImage.Height));
                        }
                        else
                        {
                            // 修复跨屏幕问题：使用整个图像区域作为操作区域
                            m_rectClip = new Rectangle(0, 0, this.baseImage.Width, this.baseImage.Height);
                        }
                    }
                    // 修复多屏环境下鼠标乱跳问题：使用更安全的坐标计算
                    try
                    {
                        // 在多屏环境下，先检查是否应该在当前屏幕上设置clip
                        if (this.FindForm() != null && Screen.AllScreens.Length > 1)
                        {
                            Screen currentScreen = Screen.FromControl(this);
                            if (currentScreen != null)
                            {
                                Rectangle clipRect = RectangleToScreen(m_rectClip);

                                // 验证clip区域是否在当前屏幕或相邻屏幕上
                                if (clipRect.Width > 0 && clipRect.Height > 0)
                                {
                                    // 检查clip区域是否与当前屏幕相交
                                    Rectangle currentBounds = currentScreen.Bounds;
                                    if (clipRect.IntersectsWith(currentBounds) ||
                                        clipRect.Contains(currentBounds) ||
                                        currentBounds.Contains(clipRect))
                                    {
                                        Cursor.Clip = clipRect;
                                    }
                                    // 如果clip区域不在合理范围内，则不设置，避免鼠标乱跳
                                }
                            }
                        }
                        else
                        {
                            // 单屏环境或无法确定屏幕信息时，使用原始方法
                            Rectangle clipRect = RectangleToScreen(m_rectClip);
                            if (clipRect.Width > 0 && clipRect.Height > 0)
                            {
                                Cursor.Clip = clipRect;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // 如果设置Cursor.Clip失败，则不设置，避免鼠标乱跳
                        System.Diagnostics.Debug.WriteLine($"设置Cursor.Clip失败: {ex.Message}");
                    }
                    isStartDraw = true;
                    m_ptOriginal = e.Location;
                }
            }
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            m_ptCurrent = e.Location;
            m_bMouseEnter = true;

            #region Process OperationBox

            if (_isDrawed && this.canReset)
            {
                //如果已经绘制 并且可以操作选区 判断操作类型
                this.SetCursorStyle(e.Location);
                if (isStartDraw && this.isDrawOperationDot)
                {
                    if (m_rectDots[0].Contains(e.Location))
                    {
                        _isDrawed = false;
                        m_ptOriginal.X = this.selectedRectangle.Right;
                        m_ptOriginal.Y = this.selectedRectangle.Bottom;
                    }
                    else if (m_rectDots[1].Contains(e.Location))
                    {
                        _isDrawed = false;
                        m_ptOriginal.Y = this.selectedRectangle.Bottom;
                        m_bLockW = true;
                    }
                    else if (m_rectDots[2].Contains(e.Location))
                    {
                        _isDrawed = false;
                        m_ptOriginal.X = this.selectedRectangle.X;
                        m_ptOriginal.Y = this.selectedRectangle.Bottom;
                    }
                    else if (m_rectDots[3].Contains(e.Location))
                    {
                        _isDrawed = false;
                        m_ptOriginal.X = this.selectedRectangle.Right;
                        m_bLockH = true;
                    }
                    else if (m_rectDots[4].Contains(e.Location))
                    {
                        _isDrawed = false;
                        m_ptOriginal.X = this.selectedRectangle.X;
                        m_bLockH = true;
                    }
                    else if (m_rectDots[5].Contains(e.Location))
                    {
                        _isDrawed = false;
                        m_ptOriginal.X = this.selectedRectangle.Right;
                        m_ptOriginal.Y = this.selectedRectangle.Y;
                    }
                    else if (m_rectDots[6].Contains(e.Location))
                    {
                        _isDrawed = false;
                        m_ptOriginal.Y = this.selectedRectangle.Y;
                        m_bLockW = true;
                    }
                    else if (m_rectDots[7].Contains(e.Location))
                    {
                        _isDrawed = false;
                        m_ptOriginal = this.selectedRectangle.Location;
                    }
                    else if (this.selectedRectangle.Contains(e.Location))
                    {
                        _isDrawed = false;
                        _isMoving = true;
                    }
                }
                base.OnMouseMove(e);
                return;
            }

            #endregion

            #region Calculate the operationbox

            if (isStartDraw)
            {
                if (_isMoving)
                {
                    //如果移动选区 只重置 location
                    //Point ptLast = this.selectedRectangle.Location;
                    this.selectedRectangle.X = m_ptTempStarPos.X + e.X - m_ptOriginal.X;
                    this.selectedRectangle.Y = m_ptTempStarPos.Y + e.Y - m_ptOriginal.Y;

                    // 修复跨屏幕问题：使用整个图像区域作为边界
                    if (this.selectedRectangle.X < 0) this.selectedRectangle.X = 0;
                    if (this.selectedRectangle.Y < 0) this.selectedRectangle.Y = 0;
                    if (this.selectedRectangle.Right > m_rectClip.Width)
                        this.selectedRectangle.X = m_rectClip.Width - this.selectedRectangle.Width - 1;
                    if (this.selectedRectangle.Bottom > m_rectClip.Height)
                        this.selectedRectangle.Y = m_rectClip.Height - this.selectedRectangle.Height - 1;
                    //if (this.Location == ptLast) return;
                }
                else
                {
                    //其他情况 判断是锁定高 还是锁定宽
                    if (Math.Abs(e.X - m_ptOriginal.X) > 1 || Math.Abs(e.Y - m_ptOriginal.Y) > 1)
                    {
                        if (!m_bLockW)
                        {
                            selectedRectangle.X = m_ptOriginal.X - e.X < 0 ? m_ptOriginal.X : e.X;
                            selectedRectangle.Width = Math.Abs(m_ptOriginal.X - e.X);
                        }
                        if (!m_bLockH)
                        {
                            selectedRectangle.Y = m_ptOriginal.Y - e.Y < 0 ? m_ptOriginal.Y : e.Y;
                            selectedRectangle.Height = Math.Abs(m_ptOriginal.Y - e.Y);
                        }
                    }
                }
                this.Invalidate();
            }

            #endregion
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            m_bMouseEnter = false;
            // 隐藏放大镜窗体
            if (_magnifyForm != null && !_magnifyForm.IsDisposed && _magnifyForm.Visible)
            {
                _magnifyForm.Hide();
            }
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //如果绘制太小 则视为无效
                if (this.selectedRectangle.Width >= 4 && this.selectedRectangle.Height >= 4)
                    _isDrawed = true;
                else
                    this.ClearDraw();
                _isMoving = m_bLockH = m_bLockW = false; //取消锁定
                isStartDraw = false;
                m_ptTempStarPos = this.selectedRectangle.Location;
                // 安全地清理Cursor.Clip，避免多屏环境下的问题
                try
                {
                    Cursor.Clip = new Rectangle();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"清理Cursor.Clip失败: {ex.Message}");
                }
            }
            else if (e.Button == MouseButtons.Right)
                this.ClearDraw();
            this.Invalidate();
            base.OnMouseUp(e);
        }

        //响应四个按键实现精确移动
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
                MouseAndKeyHelper.SetCursorPos(MousePosition.X, MousePosition.Y - 1);
            else if (e.KeyChar == 's')
                MouseAndKeyHelper.SetCursorPos(MousePosition.X, MousePosition.Y + 1);
            else if (e.KeyChar == 'a')
                MouseAndKeyHelper.SetCursorPos(MousePosition.X - 1, MousePosition.Y);
            else if (e.KeyChar == 'd')
                MouseAndKeyHelper.SetCursorPos(MousePosition.X + 1, MousePosition.Y);
            base.OnKeyPress(e);
        }

        /// <summary>
        /// 重写控件的绘制图像
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.baseImage == null || e == null || e.Graphics == null || IsDisposed) return;
            Graphics g = e.Graphics;

            if (this.baseImage != null)
            {
                // 修复图像质量问题：确保1:1像素映射，避免失真
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.SmoothingMode = SmoothingMode.None;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // 绘制带半透明遮罩的整个图像
                if (m_bmpDark == null) return;
                g.DrawImage(m_bmpDark, 0, 0, m_bmpDark.Width, m_bmpDark.Height);

                // 绘制选取框内的原始清晰图像
                if (this.selectedRectangle.Width > 0 && this.selectedRectangle.Height > 0 &&
                    this.baseImage != null &&
                    this.selectedRectangle.X >= 0 && this.selectedRectangle.Y >= 0 &&
                    this.selectedRectangle.Right <= this.baseImage.Width &&
                    this.selectedRectangle.Bottom <= this.baseImage.Height)
                {
                    g.DrawImage(this.baseImage, this.selectedRectangle, this.selectedRectangle, GraphicsUnit.Pixel);
                }
            }
            this.DrawOperationBox(g);
            if (!_isMoving && m_bMouseEnter && isShowInfo)
            {
                DrawMagnifyInfo(e.Graphics);
            }
            base.OnPaint(e);
        }
        /// <summary>
        /// 绘制操作框
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawOperationBox(Graphics g)
        {

            #region Draw SizeInfo

            string strDrawSize = "X:" + this.selectedRectangle.X + " Y:" + this.selectedRectangle.Y +
                " W:" + (this.selectedRectangle.Width + 1) + " H:" + (this.selectedRectangle.Height + 1);
            Size seStr = TextRenderer.MeasureText(strDrawSize, this.Font);
            int tempX = this.selectedRectangle.X;
            int tempY = this.selectedRectangle.Y - seStr.Height - 5;
            if (!m_rectClip.IsEmpty)
                if (tempX + seStr.Width >= m_rectClip.Right) tempX -= seStr.Width;
            if (tempY <= 0) tempY += seStr.Height + 10;

            m_sb.Color = Color.FromArgb(125, 0, 0, 0);
            g.FillRectangle(m_sb, tempX, tempY, seStr.Width, seStr.Height);
            m_sb.Color = this.ForeColor;
            g.DrawString(strDrawSize, this.Font, m_sb, tempX, tempY);

            #endregion

            if (!this.isDrawOperationDot)
            {
                m_pen.Width = 3; m_pen.Color = this.lineColor;
                g.DrawRectangle(m_pen, this.selectedRectangle);
                return;
            }
            //计算八个顶点位置 - 优化：将所有控制点移到选择框线内，避免选择失败
            const int dotSize = 15;
            const int margin = 2;

            // 控制点布局说明：
            // [0]左上角  [1]上中  [2]右上角
            // [3]左中              [4]右中
            // [5]左下角  [6]下中  [7]右下角

            // 上边三个点：移到框内顶部
            m_rectDots[0].Y = m_rectDots[1].Y = m_rectDots[2].Y = this.selectedRectangle.Y + margin;
            // 下边三个点：移到框内底部（边界检查）
            int bottomY = this.selectedRectangle.Height > dotSize + margin * 2
                ? this.selectedRectangle.Bottom - dotSize - margin
                : this.selectedRectangle.Y + margin;
            m_rectDots[5].Y = m_rectDots[6].Y = m_rectDots[7].Y = bottomY;

            // 左边三个点：移到框内左侧
            m_rectDots[0].X = m_rectDots[3].X = m_rectDots[5].X = this.selectedRectangle.X + margin;
            // 右边三个点：移到框内右侧（边界检查）
            int rightX = this.selectedRectangle.Width > dotSize + margin * 2
                ? this.selectedRectangle.Right - dotSize - margin
                : this.selectedRectangle.X + margin;
            m_rectDots[2].X = m_rectDots[4].X = m_rectDots[7].X = rightX;

            // 中间左右两个点：垂直居中
            int centerY = this.selectedRectangle.Y + this.selectedRectangle.Height / 2 - dotSize / 2;
            m_rectDots[3].Y = m_rectDots[4].Y = centerY;

            // 中间上下两个点：水平居中
            int centerX = this.selectedRectangle.X + this.selectedRectangle.Width / 2 - dotSize / 2;
            m_rectDots[1].X = m_rectDots[6].X = centerX;
            m_pen.Width = 1; m_pen.Color = this.lineColor;
            g.DrawRectangle(m_pen, this.selectedRectangle);
            m_sb.Color = this.dotColor;
            foreach (Rectangle rect in m_rectDots)
            {
                g.FillRectangle(m_sb, rect);
            }
            if (this.selectedRectangle.Width <= 10 || this.selectedRectangle.Height <= 10)
                g.DrawRectangle(m_pen, this.selectedRectangle);
        }

        /// <summary>
        /// 绘制图像放大信息 - 使用MagnifyForm窗体实现
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawMagnifyInfo(Graphics g)
        {
            if (this.baseImage == null)
            {
                if (_magnifyForm != null && !_magnifyForm.IsDisposed && _magnifyForm.Visible)
                {
                    _magnifyForm.Hide();
                }
                return;
            }

            try
            {
                // 生成放大图像
                Bitmap magnifyBitmap = null;
                using (Bitmap bmpSrc = new Bitmap(this._magnifySize.Width, this._magnifySize.Height, PixelFormat.Format32bppArgb))
                {
                    using (Graphics gp = Graphics.FromImage(bmpSrc))
                    {
                        // 优化Graphics设置
                        gp.CompositingQuality = CompositingQuality.HighSpeed;
                        gp.InterpolationMode = InterpolationMode.NearestNeighbor;
                        gp.SmoothingMode = SmoothingMode.None;

                        gp.DrawImage(this.baseImage, -(m_ptCurrent.X - this._magnifySize.Width / 2), -(m_ptCurrent.Y - this._magnifySize.Height / 2));
                    }
                    magnifyBitmap = GetMagnifyImage(bmpSrc, this.magnifyTimes);
                }

                // 获取像素颜色信息
                int pixelX = Math.Max(0, Math.Min(m_ptCurrent.X, this.baseImage.Width - 1));
                int pixelY = Math.Max(0, Math.Min(m_ptCurrent.Y, this.baseImage.Height - 1));

                Color clr;
                try
                {
                    clr = ((Bitmap)this.baseImage).GetPixel(pixelX, pixelY);
                }
                catch
                {
                    clr = Color.Black; // 默认颜色
                }

                // 生成颜色信息字符串
                string colorInfo = $"0x: {clr.A:X2}{clr.R:X2}{clr.G:X2}{clr.B:X2}";

                // 创建或更新MagnifyForm
                if (_magnifyForm == null || _magnifyForm.IsDisposed)
                {
                    _magnifyForm = new MagnifyForm();
                    _magnifyForm.TopMost = true;
                    _magnifyForm.ShowInTaskbar = false;
                    _magnifyForm.FormBorderStyle = FormBorderStyle.None;
                }

                // 计算窗体最佳位置
                Point optimalLocation = CalculateMagnifyFormLocation();
                _magnifyForm.Location = optimalLocation;

                // 更新放大信息
                _magnifyForm.SetMagnifyInfo(magnifyBitmap, m_ptCurrent.X, m_ptCurrent.Y, colorInfo);

                // 显示窗体
                if (!_magnifyForm.Visible)
                {
                    _magnifyForm.Show();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DrawMagnifyInfo失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 计算放大镜窗体的最佳位置 - 支持多屏环境和边界优化
        /// </summary>
        /// <returns>窗体的屏幕坐标位置</returns>
        private Point CalculateMagnifyFormLocation()
        {
            try
            {
                // MagnifyForm的尺寸 (267, 419)
                int formWidth = 267;
                int formHeight = 419;
                int margin = 10; // 距离鼠标或选取框的边距

                // 获取当前屏幕信息
                Screen currentScreen = Screen.FromControl(this);
                Rectangle screenBounds = _virtualScreenBounds; //currentScreen.Bounds;

                // 基础位置：鼠标位置转换为屏幕坐标
                Point mouseScreenPos = this.PointToScreen(m_ptCurrent);
                Point preferredLocation;
                //分屏坐标正负值问题todo

                // 判断是否有有效的选取框
                bool hasValidSelection = this._isDrawed &&
                    this.selectedRectangle.Width > 4 &&
                    this.selectedRectangle.Height > 4 &&
                    this.selectedRectangle.X >= 0 &&
                    this.selectedRectangle.Y >= 0 &&
                    this.selectedRectangle.Right <= this.baseImage.Width &&
                    this.selectedRectangle.Bottom <= this.baseImage.Height;

                if (hasValidSelection)
                {
                    // 有选取框时：在选取框周围且可视范围内
                    Point selectionCenter = this.PointToScreen(new Point(
                        this.selectedRectangle.X + this.selectedRectangle.Width / 2,
                        this.selectedRectangle.Y + this.selectedRectangle.Height / 2));

                    // 尝试在选取框右侧显示
                    preferredLocation = new Point(selectionCenter.X + this.selectedRectangle.Width / 2 + margin,
                                                selectionCenter.Y - formHeight / 2);

                    // 检查右边界，如果超出则放到左侧
                    if (preferredLocation.X + formWidth > screenBounds.Right)
                    {
                        preferredLocation.X = selectionCenter.X - this.selectedRectangle.Width / 2 - formWidth - margin;
                    }

                    // 检查左边界
                    if (preferredLocation.X < screenBounds.Left)
                    {
                        preferredLocation.X = Math.Max(screenBounds.Left + margin,
                            selectionCenter.X - formWidth / 2);
                    }
                }
                else
                {
                    // 无选取框时：在鼠标光标周围且可视范围
                    preferredLocation = new Point(mouseScreenPos.X + 20, mouseScreenPos.Y + 20);
                }

                // 边界检查和调整
                // 检查右边界
                if (preferredLocation.X + formWidth > screenBounds.Right)
                {
                    preferredLocation.X = screenBounds.Right - formWidth - margin;
                }

                // 检查左边界
                if (preferredLocation.X < screenBounds.Left + margin)
                {
                    preferredLocation.X = screenBounds.Left + margin;
                }

                // 检查下边界
                if (preferredLocation.Y + formHeight > screenBounds.Bottom)
                {
                    preferredLocation.Y = mouseScreenPos.Y - formHeight - margin;
                }

                // 检查上边界
                if (preferredLocation.Y < screenBounds.Top + margin)
                {
                    preferredLocation.Y = screenBounds.Top + margin;
                }

                // 确保位置在有效范围内
                preferredLocation.X = Math.Max(screenBounds.Left, Math.Min(preferredLocation.X, screenBounds.Right - formWidth));
                preferredLocation.Y = Math.Max(screenBounds.Top, Math.Min(preferredLocation.Y, screenBounds.Bottom - formHeight));

                return preferredLocation;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"计算放大镜位置失败: {ex.Message}");
                // 发生异常时返回默认位置
                return this.PointToScreen(new Point(m_ptCurrent.X + 20, m_ptCurrent.Y + 20));
            }
        }

        /// <summary>
        /// 获取放大的图形
        /// </summary>
        /// <param name="bmpSrc"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        private static Bitmap GetMagnifyImage(Bitmap bmpSrc, int times)
        {
            Bitmap bmpNew = new Bitmap(bmpSrc.Width * times, bmpSrc.Height * times, PixelFormat.Format32bppArgb);
            BitmapData bmpSrcData = bmpSrc.LockBits(new Rectangle(0, 0, bmpSrc.Width, bmpSrc.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData bmpNewData = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte[] bySrcData = new byte[bmpSrcData.Height * bmpSrcData.Stride];
            Marshal.Copy(bmpSrcData.Scan0, bySrcData, 0, bySrcData.Length);
            byte[] byNewData = new byte[bmpNewData.Height * bmpNewData.Stride];
            Marshal.Copy(bmpNewData.Scan0, byNewData, 0, byNewData.Length);
            for (int y = 0, lenY = bmpSrc.Height; y < lenY; y++)
            {
                for (int x = 0, lenX = bmpSrc.Width; x < lenX; x++)
                {
                    for (int cy = 0; cy < times; cy++)
                    {
                        for (int cx = 0; cx < times; cx++)
                        {
                            byNewData[(x * times + cx) * 4 + ((y * times + cy) * bmpNewData.Stride)] = bySrcData[x * 4 + y * bmpSrcData.Stride];
                            byNewData[(x * times + cx) * 4 + ((y * times + cy) * bmpNewData.Stride) + 1] = bySrcData[x * 4 + y * bmpSrcData.Stride + 1];
                            byNewData[(x * times + cx) * 4 + ((y * times + cy) * bmpNewData.Stride) + 2] = bySrcData[x * 4 + y * bmpSrcData.Stride + 2];
                            byNewData[(x * times + cx) * 4 + ((y * times + cy) * bmpNewData.Stride) + 3] = bySrcData[x * 4 + y * bmpSrcData.Stride + 3];
                        }
                    }
                }
            }
            Marshal.Copy(byNewData, 0, bmpNewData.Scan0, byNewData.Length);
            bmpSrc.UnlockBits(bmpSrcData);
            bmpNew.UnlockBits(bmpNewData);
            return bmpNew;
        }

        //设置鼠标指针样式
        private void SetCursorStyle(Point loc)
        {
            if (m_rectDots[0].Contains(loc) || m_rectDots[7].Contains(loc))
                this.Cursor = Cursors.SizeNWSE;
            else if (m_rectDots[1].Contains(loc) || m_rectDots[6].Contains(loc))
                this.Cursor = Cursors.SizeNS;
            else if (m_rectDots[2].Contains(loc) || m_rectDots[5].Contains(loc))
                this.Cursor = Cursors.SizeNESW;
            else if (m_rectDots[3].Contains(loc) || m_rectDots[4].Contains(loc))
                this.Cursor = Cursors.SizeWE;
            else if (this.selectedRectangle.Contains(loc) /*&& this.canReset*/)
                this.Cursor = Cursors.SizeAll;
            else
                this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 在当前控件上画图
        /// </summary>
        private void DrawImage()
        {
            if (this.baseImage == null) return;

            try
            {
                m_bmpDark = new Bitmap(this.baseImage);
                using (Graphics g = Graphics.FromImage(m_bmpDark))
                {
                    g.SmoothingMode = SmoothingMode.HighSpeed;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = PixelOffsetMode.None;
                    g.CompositingQuality = CompositingQuality.HighSpeed;

                    using (SolidBrush sb = new SolidBrush(Color.FromArgb(125, 0, 0, 0)))
                    {
                        g.FillRectangle(sb, 0, 0, m_bmpDark.Width, m_bmpDark.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DrawImage失败: {ex.Message}");
                m_bmpDark?.Dispose();
                m_bmpDark = null;
            }
        }


        /// <summary>
        /// 清空所有操作
        /// </summary>
        public void ClearDraw()
        {
            _isDrawed = false;
            this.selectedRectangle.X = this.selectedRectangle.Y = -100;
            this.selectedRectangle.Width = this.selectedRectangle.Height = 0;
            this.Cursor = Cursors.Default;
            this.Invalidate();
        }
        /// <summary>
        /// 手动设置一个块选中区域
        /// </summary>
        /// <param name="rect">要选中区域</param>
        public void SetSelectRect(Rectangle rect)
        {
            rect.Intersect(this.DisplayRectangle);
            if (rect.IsEmpty) return;
            rect.Width--; rect.Height--;
            if (this.selectedRectangle == rect) return;
            this.selectedRectangle = rect;
            this.Invalidate();
        }


        /// <summary>
        /// 手动设置信息显示的位置
        /// </summary>
        /// <param name="x">要显示位置的x坐标</param>
        /// <param name="y">要显示位置的y坐标</param>
        public void SetInfoPoint(int x, int y)
        {
            if (m_ptCurrent.X == x && m_ptCurrent.Y == y) return;
            m_ptCurrent.X = x;
            m_ptCurrent.Y = y;
            m_bMouseEnter = true;
            this.Invalidate();
        }


        /// <summary>
        /// 获取操作框内的图像
        /// </summary>
        /// <returns>结果图像</returns>
        public Bitmap GetResultBmp()
        {
            if (this.baseImage == null) return null;
            Bitmap bmp = new Bitmap(this.selectedRectangle.Width + 1, this.selectedRectangle.Height + 1);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.SmoothingMode = SmoothingMode.None;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                g.DrawImage(this.baseImage, -this.selectedRectangle.X, -this.selectedRectangle.Y);
            }
            return bmp;
        }
    }
}
