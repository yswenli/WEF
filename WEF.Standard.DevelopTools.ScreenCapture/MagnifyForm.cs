using System;
using System.Drawing;
using System.Windows.Forms;

namespace WEF.Standard.DevelopTools.ScreenCapture
{
    /// <summary>
    /// 图片放大信息窗体
    /// </summary>
    public partial class MagnifyForm : Form
    {
        int _left;
        int _top;
        string _position, _color;

        public MagnifyForm()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.KeyPreview = true;
        }

        /// <summary>
        /// 设置放大信息
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="color"></param>
        public void SetMagnifyInfo(Bitmap bitmap, int left, int top, string color)
        {
            _left = left;
            _top = top;
            _position = $"{left},{top}";
            _color = color;
            label3.Text = _position;
            label4.Text = _color;
            pictureBox1.Image = bitmap;
        }

        /// <summary>
        /// 处理快捷键
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 处理 Ctrl+C 快捷键
            if (keyData == (Keys.Control | Keys.C))
            {
                CopyColorToClipboard();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 复制颜色值到剪贴板
        /// </summary>
        private void CopyColorToClipboard()
        {
            try
            {
                if (!string.IsNullOrEmpty(_color))
                {
                    // 复制颜色值到剪贴板
                    Clipboard.SetText(_color);

                    // 可选：显示一个短暂的提示（比如改变标签颜色或添加其他视觉反馈）
                    ShowCopyFeedback();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"复制颜色值失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示复制反馈
        /// </summary>
        private void ShowCopyFeedback()
        {
            try
            {
                // 保存原始颜色
                Color originalColor = label4.ForeColor;

                // 改变颜色显示已复制
                label4.ForeColor = Color.Lime;

                // 使用Timer来恢复原始颜色
                Timer feedbackTimer = new Timer();
                feedbackTimer.Interval = 500; // 500毫秒后恢复
                feedbackTimer.Tick += (sender, e) =>
                {
                    label4.ForeColor = originalColor;
                    feedbackTimer.Stop();
                    feedbackTimer.Dispose();
                };
                feedbackTimer.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示复制反馈失败: {ex.Message}");
            }
        }
    }
}
